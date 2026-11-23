using System;
using System.Collections.Generic;
using System.Text;

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
            // El catálogo se guarda en JSON/biblioteca.json junto al ejecutable
            // (el .csproj copia el archivo inicial al compilar)
            string rutaArchivo = Path.Combine(AppContext.BaseDirectory, "JSON", "biblioteca.json");

            try
            {
                Consultas.Consultas.asegurarArchivo(rutaArchivo);
                Console.WriteLine($"Archivo de datos: {rutaArchivo}");

                new Interfaces.Menu().SetMenuPrincipal(rutaArchivo);
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine();
                Console.WriteLine("Fin de la entrada. Hasta pronto usuario!");
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
            {
                Console.WriteLine($"Error con el archivo de datos: {ex.Message}");
            }
        }
    }
}
