using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces.Registradores
{
    internal class RegistradorPrestamo
    {
        // Muestra los libros disponibles y registra el préstamo del que elija el usuario
        public static void Registrar(string rutaArchivo)
        {
            Console.WriteLine();
            Console.WriteLine("--- REGISTRAR PRESTAMO ---");

            List<string> disponibles = Consultas.Consultas.getDisponibles(rutaArchivo);
            if (disponibles.Count == 0)
            {
                Console.WriteLine("No hay libros disponibles para prestar.");
                return;
            }

            Console.WriteLine("Libros disponibles:");
            Main.Main1.leerLista(disponibles);

            List<string> catalogoActualizado = Consultas.Consultas.setPrestamos(rutaArchivo);

            Console.WriteLine("Catalogo actualizado:");
            Main.Main1.leerLista(catalogoActualizado);
        }
    }
}
