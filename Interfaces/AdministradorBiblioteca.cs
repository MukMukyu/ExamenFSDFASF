using System;
using System.Collections.Generic;
using System.Text;
using Examen.Interfaces.Consultadores;
using Examen.Interfaces.Registradores;

namespace Examen.Interfaces
{
    internal class AdministradorBiblioteca
    {
        public static void Menu(string rutaArchivo)
        {
            int subMode = 0;

            do
            {
                Console.WriteLine();
                Console.WriteLine("--- ADMINISTRADOR DE BIBLIOTECA ---");
                Console.WriteLine("1. Buscar libros por autor o titulo");
                Console.WriteLine("2. Consultar catalogo");
                Console.WriteLine("3. Registrar libro");
                Console.WriteLine("0. Volver");
                subMode = Interfaces.Menu.ReadUserInput(0, 3);

                switch (subMode)
                {
                    case 1: // BUSCAR LIBROS POR AUTOR/TITULO
                        Interfaces.Menu.EjecutarSeguro(() => BuscadorLibros.Buscar(rutaArchivo));
                        break;
                    case 2: // CONSULTAR CATÁLOGO
                        Interfaces.Menu.EjecutarSeguro(() => BuscardorCatalogos.Consultar(rutaArchivo));
                        break;
                    case 3: // REGISTRAR LIBROS
                        Interfaces.Menu.EjecutarSeguro(() => RegistradorLibro.Registrar(rutaArchivo));
                        break;
                    case 0: // VOLVER
                        Console.WriteLine("Volviendo...");
                        break;
                }
            } while (subMode > 0);
        }
    }
}
