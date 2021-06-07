using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class PlayerMenu : AbstractMenu
    {
        public PlayerMenu()
        {
            options = new string[]{ "Choose song", "Play/Pause", "Stop", "Loop", "Shuffle playlist", "Save current playlist" };
            posX = MainMenu.MAXLENGTH+6+2;
            posY = 11+4;
        }

        public override void playerChoice(int current, int length)
        {
            Player player = Player.getInstance();
            switch (current)
            {
                case (0):
                    {
                        if(player.getPlaylist().Count!=0)
                        {
                            Console.SetCursorPosition(2, length - 2);
                            Console.CursorVisible = true;
                            Console.WriteLine("INSERT NUMBER OF SONG");
                            int result;
                            string choice;
                            Console.SetCursorPosition(2, length - 1);
                            choice = Console.ReadLine();
                            bool parse= Int32.TryParse(choice, out result);
                            if(parse && (result-1< player.getPlaylistSize() && result-1>=0) )
                            {
                                player.currentSong = result - 1;
                                Console.SetCursorPosition(2, length - 2);
                                Console.Write(new string(' ', Console.WindowWidth - 1));
                                Console.SetCursorPosition(2, length - 1);
                                Console.Write(new string(' ', Console.WindowWidth - 1));
                                Console.CursorVisible = false;
                                player.choosePlay();
                            }
                            else 
                            {

                                Console.SetCursorPosition(2, length - 2);
                                Console.WriteLine("WRONG NUMBER, PRESS ANY KEY TO CONTINUE");
                                Console.SetCursorPosition(2, length - 1);
                                Console.Write(new string(' ', Console.WindowWidth - 1));
                                Console.CursorVisible = false;
                                Console.SetCursorPosition(2, length - 2);
                                Console.ReadKey(true);
                                Console.Write(new string(' ', Console.WindowWidth - 1));
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(2, length-2);
                            Console.WriteLine("THERE IS NOTHING TO SELECT, PRESS ANY KEY TO CONTINUE");
                            Console.ReadKey(true);
                            Console.SetCursorPosition(2, length-2);
                            Console.Write(new string(' ', Console.WindowWidth - 1));

                        }
                        
                        break;

                    }
                case (1):
                    {
                        if(player.getPlaylist().Count == 0)
                        {
                            Console.SetCursorPosition(2, length - 2);
                            Console.WriteLine("THERE IS NOTHING TO SELECT, PRESS ANY KEY TO CONTINUE");
                            Console.ReadKey(true);
                            Console.SetCursorPosition(2, length - 2);
                            Console.Write(new string(' ', Console.WindowWidth - 1));
                            return;
                        }
                        player.Play();

                        break;

                    }
                case (2):
                    {
                        player.Stop();
                        break;
                    }
                case (3):
                    {
                        player.isLooped = (!player.isLooped);
                        break;

                    }
                case (4):
                    {
                        player.shufflePlaylist();

                        break;

                    }
                case (5):
                    {
                        if(player.getPlaylist().Count==0)
                        {
                            if (player.getPlaylist().Count == 0)
                            {
                                Console.SetCursorPosition(2, length - 2);
                                Console.WriteLine("THERE IS NOTHING TO SAVE, PRESS ANY KEY TO CONTINUE");
                                Console.ReadKey(true);
                                Console.SetCursorPosition(2, length - 2);
                                Console.Write(new string(' ', Console.WindowWidth));
                                return;
                            }
                        }
                        Console.SetCursorPosition(0, length);
                        Console.CursorVisible = true;
                        Console.WriteLine("INSERT NAME OF PLAYLIST");
                        player.savePlaylist(Console.ReadLine());
                        Console.SetCursorPosition(0, length);
                        Console.WriteLine("PLAYLIST SAVED");
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.CursorVisible = false;
                        Console.ReadKey(true);
                        Console.SetCursorPosition(0, length);
                        Console.Write(new string(' ', Console.WindowWidth));
                        break;

                    }
            }

        }

        public override int Choice()
        {
            inMain = false;
            int current = 0;
            ConsoleKey key;
            Console.CursorVisible = false;
            int length = options.Length + posY + 3;
            do
            {
                showMenu(current);
                key = Console.ReadKey(true).Key;
                menuController(ref current, length, key);

            } while(key != ConsoleKey.Escape);


            return current;
        }
    }
}
