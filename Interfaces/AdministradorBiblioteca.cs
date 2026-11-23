using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces
{
    internal class AdministradorBiblioteca
    {
        public static void Menu()
        {
            int subMode = 0;

            do
            {
                subMode = Examen.Interfaces.Menu.ReadUserInput(4);


                switch (subMode)
                {
                    case 1: // BUSCAR LIBROS POR AUTOR/TITULO
                        break;
                    case 2: // CONSULTAR CATÁLOGO
                        ;
                        break;
                    case 3: // REGISTRAR LIBROS
                        break;
                    case 0: // VOLVER
                        break;
                }
            } while (subMode >= 0);
        }
    }
}
