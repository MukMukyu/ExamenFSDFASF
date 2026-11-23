using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces.Consultadores
{
    internal class BuscardorCatalogos
    {
        // Muestra todo el catálogo de libros
        public static void Consultar(string rutaArchivo)
        {
            List<string> catalogo = Consultas.Consultas.getCatalogo(rutaArchivo);

            Console.WriteLine();
            Console.WriteLine($"--- CATALOGO DE LIBROS ({catalogo.Count}) ---");
            Main.Main1.leerLista(catalogo);
        }
    }
}
