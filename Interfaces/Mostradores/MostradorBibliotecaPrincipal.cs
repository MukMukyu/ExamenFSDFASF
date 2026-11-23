using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces.Mostradores
{
    internal class MostradorBibliotecaPrincipal
    {
        // Libros que están en la biblioteca y se pueden prestar
        public static void Mostrar(string rutaArchivo)
        {
            List<string> disponibles = Consultas.Consultas.getDisponibles(rutaArchivo);

            Console.WriteLine();
            Console.WriteLine($"--- LIBROS DISPONIBLES ({disponibles.Count}) ---");
            Main.Main1.leerLista(disponibles);

            // Libros que no se prestan, para que el total cuadre con el catálogo
            List<string> soloConsulta = Consultas.Consultas.getSoloConsulta(rutaArchivo);
            if (soloConsulta.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"--- SOLO CONSULTA EN SALA ({soloConsulta.Count}) ---");
                Main.Main1.leerLista(soloConsulta);
            }
        }
    }
}
