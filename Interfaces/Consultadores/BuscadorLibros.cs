using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces.Consultadores
{
    internal class BuscadorLibros
    {
        // Busca libros por título o autor
        public static void Buscar(string rutaArchivo)
        {
            Console.WriteLine();
            Console.WriteLine("--- BUSCAR LIBROS POR AUTOR O TITULO ---");
            string texto = Menu.LeerTexto("Texto a buscar: ");

            List<string> resultados = Consultas.Consultas.searchLibro(texto, rutaArchivo);

            if (resultados.Count == 0)
            {
                Console.WriteLine($"No se encontraron libros para '{texto}'.");
                return;
            }

            Console.WriteLine($"Resultados ({resultados.Count}):");
            Main.Main1.leerLista(resultados);
        }
    }
}
