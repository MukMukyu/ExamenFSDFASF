using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces
{
    internal class Menu
    {
        public void SetMenuPrincipal(string rutaArchivo)
        {
            int mode = 0;

            do
            {
                Console.WriteLine();
                Console.WriteLine("===== SISTEMA DE BIBLIOTECA =====");
                Console.WriteLine("1. Administrador de biblioteca (buscar, consultar y registrar libros)");
                Console.WriteLine("2. Administrador de usuario (galeria, prestamos y devoluciones)");
                Console.WriteLine("3. Salir");
                mode = ReadUserInput(1, 3);

                switch (mode)
                {
                    case 1:
                        AdministradorBiblioteca.Menu(rutaArchivo);
                        break;
                    case 2:
                        AdministradorUsuario.Menu(rutaArchivo);
                        break;
                    case 3:
                        Console.WriteLine("Hasta pronto usuario!");
                        break;
                }
            } while (mode < 3);
        }

        // Lee una línea; si ya no hay entrada (fin de archivo) se termina el programa
        private static string LeerLinea()
        {
            return Console.ReadLine() ?? throw new EndOfStreamException();
        }

        // Pide una opción numérica entre min y max (ambos incluidos)
        public static int ReadUserInput(int min, int max)
        {
            while (true)
            {
                Console.Write("¿Qué quiere hacer ahora? ");
                if (int.TryParse(LeerLinea().Trim(), out int opcion) && opcion >= min && opcion <= max)
                {
                    return opcion;
                }
                Console.WriteLine($"Opción no válida. Escriba un número entre {min} y {max}.");
            }
        }

        // Pide un texto obligatorio. Si hay valor por defecto, Enter lo acepta.
        public static string LeerTexto(string mensaje, string? porDefecto = null)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = LeerLinea().Trim();
                if (entrada.Length > 0)
                {
                    return entrada;
                }
                if (porDefecto != null)
                {
                    return porDefecto;
                }
                Console.WriteLine("Este dato es obligatorio.");
            }
        }

        public static int LeerEntero(string mensaje, int min, int max)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(LeerLinea().Trim(), out int valor) && valor >= min && valor <= max)
                {
                    return valor;
                }
                Console.WriteLine($"Escriba un número entre {min} y {max}.");
            }
        }

        // Ejecuta una acción y muestra los errores esperados sin cerrar el programa
        public static void EjecutarSeguro(Action accion)
        {
            try
            {
                accion();
            }
            catch (EndOfStreamException)
            {
                throw;
            }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException
                                          or IOException or System.Text.Json.JsonException
                                          or UnauthorizedAccessException)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
