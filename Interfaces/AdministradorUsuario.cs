using System;
using System.Collections.Generic;
using System.Text;
using Examen.Interfaces.Mostradores;
using Examen.Interfaces.Registradores;

namespace Examen.Interfaces
{
    internal class AdministradorUsuario
    {
        public static void Menu(string rutaArchivo)
        {
            int subMode = 0;

            do
            {
                Console.WriteLine();
                Console.WriteLine("--- ADMINISTRADOR DE USUARIO ---");
                Console.WriteLine("1. Galeria de libros actuales (disponibles y prestados)");
                Console.WriteLine("2. Prestamos y devoluciones");
                Console.WriteLine("3. Guardar informacion");
                Console.WriteLine("0. Volver");
                subMode = Interfaces.Menu.ReadUserInput(0, 3);

                switch (subMode)
                {
                    case 1: // GALERÍA DE LIBROS ACTUALES
                        Interfaces.Menu.EjecutarSeguro(() =>
                        {
                            MostradorBibliotecaPrincipal.Mostrar(rutaArchivo);
                            MostradorBibliotecaPersonal.Mostrar(rutaArchivo);
                        });
                        break;
                    case 2: // PRESTAMOS Y DEVOLUCIONES
                        MenuPrestamosYDevoluciones(rutaArchivo);
                        break;
                    case 3: // GUARDAR INFORMACIÓN
                        Interfaces.Menu.EjecutarSeguro(() => GuardarInformacion(rutaArchivo));
                        break;
                    case 0: // VOLVER
                        Console.WriteLine("Volviendo...");
                        break;
                }
            } while (subMode > 0);
        }

        private static void MenuPrestamosYDevoluciones(string rutaArchivo)
        {
            int opcion;

            do
            {
                Console.WriteLine();
                Console.WriteLine("--- PRESTAMOS Y DEVOLUCIONES ---");
                Console.WriteLine("1. Registrar prestamo");
                Console.WriteLine("2. Registrar devolucion");
                Console.WriteLine("0. Volver");
                opcion = Interfaces.Menu.ReadUserInput(0, 2);

                switch (opcion)
                {
                    case 1:
                        Interfaces.Menu.EjecutarSeguro(() => RegistradorPrestamo.Registrar(rutaArchivo));
                        break;
                    case 2:
                        Interfaces.Menu.EjecutarSeguro(() => RegistradorDevolucion.Registrar(rutaArchivo));
                        break;
                    case 0:
                        Console.WriteLine("Volviendo...");
                        break;
                }
            } while (opcion > 0);
        }

        // El catálogo ya se guarda solo en cada cambio; aquí además se genera un reporte de texto
        private static void GuardarInformacion(string rutaArchivo)
        {
            string reporte = Consultas.Consultas.guardarReporte(rutaArchivo);
            Console.WriteLine("Informacion guardada.");
            Console.WriteLine($"Catalogo (JSON): {rutaArchivo}");
            Console.WriteLine($"Reporte (texto): {reporte}");
        }
    }
}
