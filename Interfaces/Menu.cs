using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Interfaces
{
    internal class Menu
    {
        public int mode = 0;
        public int numOptions = 4;

        public void SetMenuPrincipal()
        {
            do
            {
                ReadUserInput();


                switch (mode)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 0:
                        break;
                }
            } while (mode < 4);
        }

        private void ReadUserInput()
        {
            do
            {
                Console.WriteLine("Qué quiere hacer ahora?");
                mode = Console.Read();
            } while (mode > numOptions);
        }
    }
}
