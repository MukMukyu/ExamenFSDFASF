using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces.Registradores
{
    internal class RegistradorDevolucion
    {
        // Muestra los libros prestados y registra la devolución del que elija el usuario
        public static void Registrar(string rutaArchivo)
        {
            Console.WriteLine();
            Console.WriteLine("--- REGISTRAR DEVOLUCION ---");

            List<string> prestados = Consultas.Consultas.getPrestados(rutaArchivo);
            if (prestados.Count == 0)
            {
                Console.WriteLine("No hay libros prestados para devolver.");
                return;
            }

            Console.WriteLine("Libros prestados:");
            Main.Main1.leerLista(prestados);

            List<string> catalogoActualizado = Consultas.Consultas.setDevoluciones(rutaArchivo);

            Console.WriteLine("Catalogo actualizado:");
            Main.Main1.leerLista(catalogoActualizado);
        }
    }
}
