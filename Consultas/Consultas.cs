using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Examen.Consultas
{
    internal class Consultas
    {
        // Opciones compartidas para guardar el JSON legible y con tildes
        private static readonly JsonSerializerOptions opcionesJson = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private static string hoy() => DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        // Lee y valida el archivo del catálogo
        private static Catalogo cargarCatalogo(String rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException($"No se encontró el archivo: {rutaArchivo}");
            }

            String content = File.ReadAllText(rutaArchivo);

            Catalogo? catalogo = JsonSerializer.Deserialize<Catalogo>(content);

            if (catalogo?.Libros == null)
            {
                throw new InvalidOperationException("No se encontraron libros en el catálogo");
            }

            return catalogo;
        }

        private static string formatearLibro(Libro l) =>
            $"[{l.Id}] {l.Nombre} ({l.Anio}) - {l.Autor} | {l.Prestamo.Observaciones}";

        // Quita tildes y mayúsculas para comparar textos ("garcia" encuentra "García")
        private static string normalizar(string texto)
        {
            var sb = new StringBuilder();
            foreach (char c in texto.Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }

        // Crea el archivo con un catálogo vacío si todavía no existe
        public static void asegurarArchivo(String rutaArchivo)
        {
            if (File.Exists(rutaArchivo))
            {
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(rutaArchivo))!);
            var vacio = new Catalogo
            {
                Biblioteca = "Catálogo General de Libros",
                FechaActualizacion = hoy(),
                TotalLibros = 0
            };
            File.WriteAllText(rutaArchivo, JsonSerializer.Serialize(vacio, opcionesJson));
        }

        public static List<string> getCatalogo(String rutaArchivo)
        {
            return cargarCatalogo(rutaArchivo).Libros.Select(formatearLibro).ToList();
        }

        public static void setLibros(string id, string nombre, string anio, string autor, string rutaArchivo)
        {
            id = id?.Trim() ?? string.Empty;
            nombre = nombre?.Trim() ?? string.Empty;
            autor = autor?.Trim() ?? string.Empty;

            if (id.Length == 0 || nombre.Length == 0 || autor.Length == 0)
            {
                throw new ArgumentException("El ID, el título y el autor son obligatorios.");
            }

            // 1. Leer el catálogo con getCatalogo para validar el archivo y evitar IDs repetidos
            List<string> librosActuales = getCatalogo(rutaArchivo);

            if (librosActuales.Any(item => item.StartsWith($"[{id}]", StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Ya existe un libro con el ID {id}.");
            }

            if (!int.TryParse(anio, out int anioNumerico))
            {
                throw new ArgumentException("El año debe ser un valor numérico.", nameof(anio));
            }

            // 2. Deserializar el catálogo completo para conservar todos los campos al guardar
            Catalogo catalogo = cargarCatalogo(rutaArchivo);

            // 3. Crear el nuevo libro con su estado inicial disponible
            Libro nuevoLibro = new Libro
            {
                Id = id,
                Nombre = nombre,
                Anio = anioNumerico,
                Autor = autor,
                Prestamo = new Prestamo
                {
                    Disponible = true,
                    EnPrestamo = false,
                    FechaPrestamo = null,
                    FechaLimiteDevolucion = null,
                    RequiereDevolucion = false,
                    Observaciones = "Disponible para préstamo"
                }
            };

            // 4. Agregar el libro y actualizar los metadatos del catálogo
            catalogo.Libros.Add(nuevoLibro);
            catalogo.TotalLibros = catalogo.Libros.Count;
            catalogo.FechaActualizacion = hoy();

            // 5. Serializar con formato legible y guardar con System.IO
            File.WriteAllText(rutaArchivo, JsonSerializer.Serialize(catalogo, opcionesJson));

            Console.WriteLine("Registro exitoso");
        }

        // Siguiente ID sugerido con el formato LIB-001, LIB-002...
        public static string getSiguienteId(String rutaArchivo)
        {
            int max = 0;
            foreach (Libro l in cargarCatalogo(rutaArchivo).Libros)
            {
                if (l.Id.StartsWith("LIB-", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(l.Id.AsSpan(4), NumberStyles.None, CultureInfo.InvariantCulture, out int n) &&
                    n > max)
                {
                    max = n;
                }
            }
            return $"LIB-{max + 1:000}";
        }

        // 1. Buscar libros por título o autor (sin distinguir mayúsculas ni tildes).
        // Todas las palabras escritas deben aparecer en el título o en el autor.
        public static List<string> searchLibro(String nombre, String rutaArchivo)
        {
            string[] palabras = normalizar(nombre ?? string.Empty)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return cargarCatalogo(rutaArchivo).Libros
                .Where(l =>
                {
                    string texto = normalizar($"{l.Nombre} {l.Autor}");
                    return palabras.All(p => texto.Contains(p));
                })
                .Select(formatearLibro)
                .ToList();
        }

        // Disponibles: se pueden prestar ahora
        public static List<string> getDisponibles(String rutaArchivo) =>
            disponibles(cargarCatalogo(rutaArchivo)).Select(formatearLibro).ToList();

        // Prestados: incluye fecha de préstamo y fecha límite
        public static List<string> getPrestados(String rutaArchivo) =>
            prestados(cargarCatalogo(rutaArchivo)).Select(formatearPrestado).ToList();

        // Libros que no se prestan (solo consulta en sala)
        public static List<string> getSoloConsulta(String rutaArchivo) =>
            soloConsulta(cargarCatalogo(rutaArchivo)).Select(formatearLibro).ToList();

        private static IEnumerable<Libro> prestados(Catalogo c) => c.Libros.Where(l => l.Prestamo.EnPrestamo);
        private static IEnumerable<Libro> disponibles(Catalogo c) => c.Libros.Where(l => !l.Prestamo.EnPrestamo && l.Prestamo.Disponible);
        private static IEnumerable<Libro> soloConsulta(Catalogo c) => c.Libros.Where(l => !l.Prestamo.EnPrestamo && !l.Prestamo.Disponible);

        private static string formatearPrestado(Libro l) =>
            $"{formatearLibro(l)} | Prestado: {l.Prestamo.FechaPrestamo} | Límite: {l.Prestamo.FechaLimiteDevolucion}";

        // Guarda un reporte de texto con el estado actual; devuelve la ruta del archivo creado
        public static string guardarReporte(String rutaArchivo)
        {
            Catalogo c = cargarCatalogo(rutaArchivo);
            string rutaReporte = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(rutaArchivo))!, "reporte_biblioteca.txt");

            var lineas = new List<string>
            {
                $"REPORTE DE BIBLIOTECA - {c.Biblioteca}",
                $"Generado: {DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}",
                $"Total de libros: {c.Libros.Count}",
                ""
            };

            void seccion(string titulo, IEnumerable<string> items)
            {
                List<string> lista = items.ToList();
                lineas.Add($"{titulo} ({lista.Count})");
                lineas.AddRange(lista.Select(i => "  " + i));
                lineas.Add("");
            }

            seccion("LIBROS DISPONIBLES", disponibles(c).Select(formatearLibro));
            seccion("LIBROS PRESTADOS", prestados(c).Select(formatearPrestado));
            seccion("SOLO CONSULTA EN SALA", soloConsulta(c).Select(formatearLibro));

            File.WriteAllLines(rutaReporte, lineas);
            return rutaReporte;
        }

        public static List<string> setPrestamos(String archivo)
        {
            // 1. Valida que el archivo exista y tenga libros usando getCatalogo
            getCatalogo(archivo);

            Console.Write("Ingresa el ID del libro a prestar (ej. LIB-001): ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            // 2. Deserializa el archivo en tus clases modelo
            Catalogo catalogo = cargarCatalogo(archivo);

            Libro? libro = catalogo.Libros
                .FirstOrDefault(l => l.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (libro == null)
            {
                throw new InvalidOperationException($"No se encontró ningún libro con el ID '{id}'.");
            }

            if (!libro.Prestamo.Disponible || libro.Prestamo.EnPrestamo)
            {
                throw new InvalidOperationException($"El libro '{libro.Nombre}' no está disponible para préstamo.");
            }

            // 3. Actualiza las propiedades del préstamo
            DateTime ahora = DateTime.Now;
            libro.Prestamo.Disponible = false;
            libro.Prestamo.EnPrestamo = true;
            libro.Prestamo.FechaPrestamo = ahora.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            libro.Prestamo.FechaLimiteDevolucion = ahora.AddDays(14).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            libro.Prestamo.RequiereDevolucion = false;
            libro.Prestamo.Observaciones = "En préstamo dentro del plazo vigente";

            catalogo.TotalLibros = catalogo.Libros.Count;
            catalogo.FechaActualizacion = hoy();

            // 4. Guarda los cambios en la ruta recibida y retorna el catálogo actualizado
            File.WriteAllText(archivo, JsonSerializer.Serialize(catalogo, opcionesJson));
            Console.WriteLine($"Préstamo registrado. Fecha límite: {libro.Prestamo.FechaLimiteDevolucion}");

            return getCatalogo(archivo);
        }

        public static List<string> setDevoluciones(String archivo)
        {
            // 1. Valida que el archivo exista y tenga libros usando getCatalogo
            getCatalogo(archivo);

            Console.Write("Ingresa el ID del libro a devolver (ej. LIB-002): ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            // 2. Deserializa el archivo en tus clases modelo
            Catalogo catalogo = cargarCatalogo(archivo);

            Libro? libro = catalogo.Libros
                .FirstOrDefault(l => l.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (libro == null)
            {
                throw new InvalidOperationException($"No se encontró ningún libro con el ID '{id}'.");
            }

            if (!libro.Prestamo.EnPrestamo)
            {
                throw new InvalidOperationException($"El libro '{libro.Nombre}' no se encuentra en préstamo actualmente.");
            }

            // 3. Libera el préstamo y limpia las fechas
            libro.Prestamo.Disponible = true;
            libro.Prestamo.EnPrestamo = false;
            libro.Prestamo.FechaPrestamo = null;
            libro.Prestamo.FechaLimiteDevolucion = null;
            libro.Prestamo.RequiereDevolucion = false;
            libro.Prestamo.Observaciones = "Disponible para préstamo";

            catalogo.TotalLibros = catalogo.Libros.Count;
            catalogo.FechaActualizacion = hoy();

            // 4. Guarda los cambios en la ruta recibida y retorna el catálogo actualizado
            File.WriteAllText(archivo, JsonSerializer.Serialize(catalogo, opcionesJson));
            Console.WriteLine($"Devolución registrada para '{libro.Nombre}'.");

            return getCatalogo(archivo);
        }
    }
}
