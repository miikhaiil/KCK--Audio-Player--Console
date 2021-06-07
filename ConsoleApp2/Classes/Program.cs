using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace ConsoleApp2
{
    class Program
    {
        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();


        static void Main(string[] args)
        {
            
            Console.WindowHeight = 30;
            Console.WindowWidth = 100;

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);

            Console.WriteLine(@" _________________________________________________________");
            Console.WriteLine(@"|                                                         |");
            Console.WriteLine(@"|                                                         |");
            Console.WriteLine(@"| _______  _        _______ _________ _______  _______    |");
            Console.WriteLine(@"| (  ____ )( |      (  ____ \\__    _/(  ____ \(  ____ )  |");
            Console.WriteLine(@"| | (    )|| (      | (    \/   )  (  | (    \/| (    )|  |");
            Console.WriteLine(@"| | (____)|| |      | (__       |  |  | (__    | (____)|  |");
            Console.WriteLine(@"| |  _____)| |      |  __)      |  |  |  __)   |     __)  |");
            Console.WriteLine(@"| | (      | |      | (         |  |  | (      | (\ (     |");
            Console.WriteLine(@"| | )      | (____/\| (____/\|\_)  )  | (____/\| ) \ \__  |");
            Console.WriteLine(@"| |/       (_______/(_______/(____/   (_______/|/   \__/  |");
            Console.WriteLine(@"|                                                         |");
            Console.WriteLine(@"|                                                         |");
            Console.WriteLine( "|_________________________________________________________|");
            MainMenu menu = new MainMenu();
            menu.startAutoPlay();
            int selected = menu.Choice();
            
            //TO DO
            //POPRAWA paskow EQUALIZERA
            //ogarniecie minimalizacji
        }
    }
}
