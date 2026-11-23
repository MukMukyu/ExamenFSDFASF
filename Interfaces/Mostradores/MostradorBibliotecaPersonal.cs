using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces.Mostradores
{
    internal class MostradorBibliotecaPersonal
    {
        // Libros que están prestados, con fecha de préstamo y fecha límite
        public static void Mostrar(string rutaArchivo)
        {
            List<string> prestados = Consultas.Consultas.getPrestados(rutaArchivo);

            Console.WriteLine();
            Console.WriteLine($"--- LIBROS PRESTADOS ({prestados.Count}) ---");
            Main.Main1.leerLista(prestados);
        }
    }
}
