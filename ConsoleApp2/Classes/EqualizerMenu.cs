using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Classes
{
    class EqualizerMenu:AbstractMenu
    {
        Player player = Player.getInstance();
      
        public EqualizerMenu() : base()
        {

            options = new string[] { "31Hz", "62Hz", "125Hz", "250Hz", "500Hz", "1kHz", "2kHz", "4kHz", "8kHz", "16kHz","RESET" };
            posX = MainMenu.MAXLENGTH + 2+2;
            posY = 11+4;
        }

        public override void equalizerChoice(int current)
        {
            if(current==10)
            {
                player.resetEqualizer();
            }
        }

        public override int Choice()
        {
            inMain = false;
            int current = 0;
            ConsoleKey key;
            Console.CursorVisible = false;
            do
            {
                showEqualizer(current);
                key = Console.ReadKey(true).Key;
                menuController(ref current, 0, key);

            } while (key != ConsoleKey.Escape);
            return current;
        }
    }
}
