using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces
{
    internal class Menu
    {
        public void SetMenuPrincipal()
        {
            int mode = 0;

            do
            {
                mode = ReadUserInput(3);


                switch (mode)
                {
                    case 1:
                        AdministradorBiblioteca.Menu();
                        break;
                    case 2:
                        AdministradorUsuario.Menu();
                        break;
                    case 3:
                        Console.WriteLine("Hasta pronto usuario!");
                        break;
                }
            } while (mode < 3);
        }

        public static int ReadUserInput(int numOptions)
        {
            int _mode = 0;
            do
            {
                Console.WriteLine("Qué quiere hacer ahora?");
                _mode = Console.Read();
            } while (_mode > numOptions);

            return _mode;
        }
    }
}
