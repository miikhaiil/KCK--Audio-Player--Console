using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class FilesMenu : AbstractMenu
    {

        public FilesMenu():base()
        {
            
            options = new string[]{ "Open file", "Open folder", "Open playlist" };
            posX = MainMenu.MAXLENGTH+2+2;
            posY = 11+4;
        }



        #region METODY DO ODCZYTU PLIKÓW
        private static void checkFile(int length, Player player, int frameWidth)
        {
            string path;
            Console.SetCursorPosition(2, length);
            Console.CursorVisible = true;
            Console.WriteLine("INSERT PATH AND PRESS ENTER: ");
            Console.SetCursorPosition(2, length+1);
            path = Console.ReadLine();

            Console.CursorVisible = false;
            if (File.Exists(path) && Path.GetExtension(path)==".mp3")
            {
                player.addToPlaylist(path);
                Console.SetCursorPosition(2, length);
                Console.WriteLine("SONG ADDED TO PLAYLIST, PRESS ANY KEY TO CONTINUE");
                for (int i = 1; i < 6 - 1; i++)
                {
                    Console.SetCursorPosition(1, length + i);
                    Console.Write(new string(' ', Console.WindowWidth - 2));
                }
                Console.ReadKey(true);
                Console.Write(new string(' ', Console.WindowWidth - 2));

            }
            else if(File.Exists(path) && Path.GetExtension(path) != ".mp3")
            {
                Console.SetCursorPosition(2, length);
                Console.WriteLine("EXTENSION NOT SUPPORTED, PRESS ANY KEY TO CONTINUE");
                for (int i = 1; i < 6 - 1; i++)
                {
                    Console.SetCursorPosition(1, length + i);
                    Console.Write(new string(' ', Console.WindowWidth - 2));
                }
                Console.ReadKey(true);
                Console.Write(new string(' ', Console.WindowWidth - 2));
            }
            else
            {
                Console.SetCursorPosition(2, length);
                Console.WriteLine("WRONG PATH, PRESS ANY KEY TO CONTINUE");
                for (int i = 1; i < 6 - 1; i++)
                {
                    Console.SetCursorPosition(1, length + i);
                    Console.Write(new string(' ', Console.WindowWidth - 2));
                }
                Console.ReadKey(true);
                
            }
            Console.SetCursorPosition(2, length);
            Console.Write(new string(' ', Console.WindowWidth - 2));
        }

        private static List<String> GetFiles(string path, string pattern)
        {
            var files = new List<string>();
            var directories = new string[] { };

            try
            {
                files.AddRange(Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly));
                directories = Directory.GetDirectories(path);
            }
            catch (UnauthorizedAccessException) { }

            foreach (var directory in directories)
                try
                {
                    files.AddRange(GetFiles(directory, pattern));
                }
                catch (UnauthorizedAccessException) { }

            return files;
        }
        #endregion

        public override void filesChoice(int current,int length)
        {
            Player player = Player.getInstance();
            string path;
            List<string> filesPaths = new List<string>();
            switch (current)
            {
                case (0):
                    {
                        checkFile(length, player,frameWidth);
                        break;

                    }
                case (1):
                    {
                        Console.SetCursorPosition(2, length);
                        Console.CursorVisible = true;
                        Console.WriteLine("INSERT PATH AND PRESS ENTER: ");
                        Console.SetCursorPosition(2, length + 1);
                        path = Console.ReadLine();
                        Console.CursorVisible = false;
                        if (Directory.Exists(path))
                        {
                            Console.SetCursorPosition(2, length);
                            Console.WriteLine("LOOKING FOR FILES, PLEASE WAIT");
                            filesPaths = GetFiles(path, "*.mp3");
                            foreach (string file in filesPaths)
                            {
                                player.addToPlaylist(file);
                            }
                            if (filesPaths.Count == 0)
                            {
                                Console.SetCursorPosition(2, length);
                                Console.WriteLine("THERE IS NO FILE WITH SUPPORTED EXTENSIONS! PRESS ANY KEY TO CONTINUE");
                                for (int i = 1; i < 6 - 1; i++)
                                {
                                    Console.SetCursorPosition(1, length + i);
                                    Console.Write(new string(' ', Console.WindowWidth - 2));
                                }
                            }
                            else
                            {
                                Console.SetCursorPosition(2, length);
                                Console.WriteLine("SONGS ADDED TO PLAYLIST, LOADED: " + filesPaths.Count + " FILES, PRESS ANY KEY TO CONTINUE");
                                for (int i = 1; i < 6 - 1; i++)
                                {
                                    Console.SetCursorPosition(1, length + i);
                                    Console.Write(new string(' ', Console.WindowWidth - 2));
                                }
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(2, length);
                            Console.WriteLine("WRONG PATH, PRESS ANY KEY TO CONTINUE");
                            for (int i = 1; i < 6 - 1; i++)
                            {
                                Console.SetCursorPosition(1, length + i);
                                Console.Write(new string(' ', Console.WindowWidth - 2));
                            }
                            Console.ReadKey(true);
                            Console.Write(new string(' ', Console.WindowWidth-1));
                        }
                        Console.SetCursorPosition(2, length);
                        Console.Write(new string(' ', Console.WindowWidth - 2));
                        break;
                    }
                case (2):
                    {

                        Console.SetCursorPosition(2, length);
                        Console.WriteLine("INSERT PATH TO .PLR FILE");
                        Console.CursorVisible = true;
                        Console.SetCursorPosition(2, length + 1);
                        path = Console.ReadLine();
                        if (Path.GetExtension(path) == ".plr")
                        {
                            player.loadPlaylist(path);
                            Console.CursorVisible = false;
                            Console.SetCursorPosition(2, length);
                            Console.WriteLine("PLAYLIST LOADED, PRESS ANY KEY TO CONTINUE");
                            for (int i = 1; i < 6 - 1; i++)
                            {
                                Console.SetCursorPosition(1, length + i);
                                Console.Write(new string(' ', Console.WindowWidth - 2));
                            }
                            Console.ReadKey(true);
                        }
                        else
                        {
                            Console.SetCursorPosition(2, length);
                            Console.WriteLine("WRONG PATH, PRESS ANY KEY TO CONTINUE");
                            
                            for (int i = 1; i < 6 - 1; i++)
                            {
                                Console.SetCursorPosition(1, length + i);
                                Console.Write(new string(' ', Console.WindowWidth - 2));
                            }
                            Console.CursorVisible = false;
                            Console.ReadKey(true);
                        }
                        Console.SetCursorPosition(2, length);
                        Console.Write(new string(' ', Console.WindowWidth-1));

                        break;
                    }

            }
        }

        #region PORUSZANIE SIE PO MENU
        public override int Choice()
        {
            inMain = false;
            int length = options.Length +posY+ 4;
            int current = 0;
            ConsoleKey key;
            Console.CursorVisible = false;

            do
            {
                showMenu(current);
                key = Console.ReadKey(true).Key;

                menuController (ref current, length, key);

            } while (key != ConsoleKey.Escape);
            return current;
        }
        #endregion
    }
}
