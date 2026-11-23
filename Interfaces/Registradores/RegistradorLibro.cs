using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces.Registradores
{
    internal class RegistradorLibro
    {
        // Pide los datos del libro y lo guarda en el catálogo
        public static void Registrar(string rutaArchivo)
        {
            Console.WriteLine();
            Console.WriteLine("--- REGISTRAR LIBRO ---");

            string sugerido = Consultas.Consultas.getSiguienteId(rutaArchivo);
            string id = Menu.LeerTexto($"ID del libro [{sugerido}] (Enter para usar el sugerido): ", sugerido);
            string nombre = Menu.LeerTexto("Titulo: ");
            int anio = Menu.LeerEntero("Anio de publicacion: ", 1, DateTime.Now.Year);
            string autor = Menu.LeerTexto("Autor: ");

            Consultas.Consultas.setLibros(id, nombre, anio.ToString(), autor, rutaArchivo);
        }
    }
}
