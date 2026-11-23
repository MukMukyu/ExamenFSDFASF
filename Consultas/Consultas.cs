using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Examen.Consultas
{
    internal class Consultas
    {

        public static List<string> getCatalogo(String rutaArchivo)
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

            return catalogo.Libros.Select(l => $"[{l.Id}] {l.Nombre} ({l.Anio}) - {l.Autor} | {l.Prestamo.Observaciones}")
            .ToList();
        }

        public static void setLibros(string id, string nombre, string anio, string autor, string rutaArchivo)
        {
            // 1. Leer el catálogo con getCatalogo para validar el archivo y evitar IDs repetidos
            List<string> librosActuales = getCatalogo(rutaArchivo);

            if (librosActuales.Any(item => item.StartsWith($"[{id}]")))
            {
                throw new InvalidOperationException($"Ya existe un libro con el ID {id}.");
            }

            if (!int.TryParse(anio, out int anioNumerico))
            {
                throw new ArgumentException("El año debe ser un valor numérico.", nameof(anio));
            }

            // 2. Deserializar el catálogo completo para conservar todos los campos al guardar
            string content = File.ReadAllText(rutaArchivo);
            Catalogo catalogo = JsonSerializer.Deserialize<Catalogo>(content)!;

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
            catalogo.FechaActualizacion = DateTime.Now.ToString("yyyy-MM-dd");

            // 5. Serializar con formato legible y guardar con System.IO
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string jsonActualizado = JsonSerializer.Serialize(catalogo, opciones);
            File.WriteAllText(rutaArchivo, jsonActualizado);

            Console.WriteLine("Registro exitoso");
        }

        // 1. Buscar libros por nombre (coincidencia parcial, sin distinguir mayúsculas)
        public static List<string> searchLibro(String nombre, String rutaArchivo)
        {
            return getCatalogo(rutaArchivo)
                .Where(libro => libro.Contains(nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public static List<string> setPrestamos(String archivo)
        {
            // 1. Valida que el archivo exista y tenga libros usando getCatalogo
            getCatalogo(archivo);

            Console.Write("Ingresa el ID del libro a prestar (ej. LIB-001): ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            // 2. Deserializa el archivo en tus clases modelo
            string content = File.ReadAllText(archivo);
            Catalogo catalogo = JsonSerializer.Deserialize<Catalogo>(content)!;

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
            DateTime hoy = DateTime.Now;
            libro.Prestamo.Disponible = false;
            libro.Prestamo.EnPrestamo = true;
            libro.Prestamo.FechaPrestamo = hoy.ToString("yyyy-MM-dd");
            libro.Prestamo.FechaLimiteDevolucion = hoy.AddDays(14).ToString("yyyy-MM-dd");
            libro.Prestamo.RequiereDevolucion = false;
            libro.Prestamo.Observaciones = "En préstamo dentro del plazo vigente";

            catalogo.TotalLibros = catalogo.Libros.Count;
            catalogo.FechaActualizacion = hoy.ToString("yyyy-MM-dd");

            // 4. Guarda los cambios en la ruta recibida y retorna el catálogo actualizado
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            File.WriteAllText(archivo, JsonSerializer.Serialize(catalogo, opciones));
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
            string content = File.ReadAllText(archivo);
            Catalogo catalogo = JsonSerializer.Deserialize<Catalogo>(content)!;

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
            catalogo.FechaActualizacion = DateTime.Now.ToString("yyyy-MM-dd");

            // 4. Guarda los cambios en la ruta recibida y retorna el catálogo actualizado
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            File.WriteAllText(archivo, JsonSerializer.Serialize(catalogo, opciones));
            Console.WriteLine($"Devolución registrada para '{libro.Nombre}'.");

            return getCatalogo(archivo);
        }
    }
}