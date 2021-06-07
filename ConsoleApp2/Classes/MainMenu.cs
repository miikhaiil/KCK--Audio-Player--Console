using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ConsoleApp2.Classes;

namespace ConsoleApp2
{
    class MainMenu : AbstractMenu
    {
        public static int MAXLENGTH=14;
        Player player = Player.getInstance();
        public MainMenu()
        {
            options = new string[] { "Files", "Player options", "Equalizer", "Exit"};
            posX = 2;
            posY = 11+4;
        }

       public override void mainChoice(int current, int length)
       {
            int maxLenght = 0;
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].Length > maxLenght)
                    maxLenght = options[i].Length;
            }
            switch (current)
            {
                case (0):
                    {
                        FilesMenu menu = new FilesMenu();
                        int files = menu.Choice();
                        break;
                    }
                case (1):
                    {
                        PlayerMenu plrMenu = new PlayerMenu();
                        int plrOpt = plrMenu.Choice();
                        break;
                    }
                case (2):
                    {
                        EqualizerMenu eqMenu = new EqualizerMenu();
                        int eqOpt = eqMenu.Choice();
                        break;
                    }
                case (3):
                    {
                        player.Stop();
                        System.Environment.Exit(0);
                        break;
                    }
            }
        }

       

       public override int Choice()
        {
            inMain = true;
            int current = 0;
            int yPositon = posY;
            ConsoleKey key;
            Console.CursorVisible = false;

            do
            {
                showMenu(current);
                key = Console.ReadKey(true).Key;
                menuController(ref current, 0,key);

            } while (key != ConsoleKey.Escape);

            return current;
        }
    }
}
