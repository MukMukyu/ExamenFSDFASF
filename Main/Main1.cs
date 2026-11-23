using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Examen.Main
{
    internal class Main1
    {
        public static void leerLista(List<string> lista)
        {
            if (lista == null || lista.Count == 0)
            {
                Console.WriteLine("No hay elementos para mostrar.");
                return;
            }

            foreach (string item in lista)
            {
                Console.WriteLine(item);
            }
        }


        public static void Main(string[] args)
        {
            string rutaArchivo = "Consultas/catalogo.json";


            string id = "90";
            string nombre = "1981";
            string anio = "2012";
            string autor = "Jorge Orwell";

            Consultas.Consultas.setLibros("1", "El Principito", "1943", "Antoine de Saint-Exupéry", rutaArchivo);

            //Obtener y mostrar la lista de libros
            List<string> catalogo = Consultas.Consultas.getCatalogo(rutaArchivo);

            //Inserta libros nuevos
            List<string> insertar = Consultas.Consultas.setLibros(id, nombre, anio, autor, rutaArchivo);


            //Busca libros por su nombre
            List<string> buscar = Consultas.Consultas.searchLibro(nombre, rutaArchivo);

            // Registrar un préstamo y mostrar la lista actualizada
            List<string> catalogoTrasPrestamo = Consultas.Consultas.setPrestamos(rutaArchivo);

            // Registrar una devolución y mostrar la lista actualizada
            List<string> catalogoTrasDevolucion = Consultas.Consultas.setDevoluciones(rutaArchivo);
        }
    }
}
