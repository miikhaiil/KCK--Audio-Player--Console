using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp2
{
    abstract class AbstractMenu
    {
        Player player = Player.getInstance();
        public string[] options;
        public int posX;
        public int posY;
        public int frameWidth;
        public bool equalizer = false;
        public bool inMain = false;
        public int pom = 20;
        
        public void printFrame(int width, int heigth, int x, int y)
        {
            frameWidth = width;
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(i +x + 1, y);
                Console.WriteLine("_");

                Console.SetCursorPosition(i + x + 1, y + heigth);
                Console.WriteLine("_");
            }

            for (int j = 0; j < heigth; j++)
            {
                Console.SetCursorPosition(0+x, y +j+1);
                Console.WriteLine("|");

                Console.SetCursorPosition(width+1+x, y +j+1);
                Console.WriteLine("|");
            }

        }

        public void showMenu(int current)
        {
            printFrame(Console.WindowWidth-2,14,0,posY-1);
            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(posX, posY + i);
                if (i == current)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("> " + options[i] + " <  " + "\n");
                }
                else if(options[i]=="Loop" && player.isLooped)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(options[i] + "     " + "\n");
                }
                else
                {
                    Console.Write(options[i] + "     " + "\n");
                }
                Console.ResetColor();
            }
            showPlaylist();
        }

        public void showEqualizer(int current)
        {
            equalizer = true;
            printFrame(Console.WindowWidth - 2, 14, 0, posY - 1);
            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(posX + i * 8, posY);
                if (i == current)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    if (i == options.Length - 1)
                    {
                        Console.SetCursorPosition(posX + (i - 1) * 8, posY + 12);
                        Console.Write("> " + options[i]);
                        Console.ResetColor();
                        break;
                    }
                    else
                        Console.Write("> " + options[i]);

                    for (int j = 0; j < 10; j++)
                    {
                        int counter = (int)(player.getGain(i) + 20) / 4;
                        Console.SetCursorPosition(posX + i * 8 + 2, posY + 10 - j);
                        if (j < counter)
                        {
                            Console.WriteLine("|");
                        }
                        else
                            Console.WriteLine(" ");
                    }

                    if (player.getGain(i) >= 0)
                    {
                        Console.SetCursorPosition(posX + i * 8 + 1, posY + 1 + 10);
                        Console.WriteLine(" " + player.getGain(i) + "dB ");
                    }
                    else
                    {
                        Console.SetCursorPosition(posX + i * 8 + 1, posY + 1 + 10);
                        Console.WriteLine(player.getGain(i) + "dB ");
                    }

                }
                else
                {
                    if (i == options.Length - 1)
                    {
                        Console.SetCursorPosition(posX + (i - 1) * 8, posY + 12);
                        Console.Write("  " + options[i] + "  ");
                        break;
                    }
                    else
                        Console.Write("  " + options[i] + "  ");
                    for (int j = 0; j < 10; j++)
                    {
                        int counter = (int)(player.getGain(i) + 20) / 4;
                        Console.SetCursorPosition(posX + i * 8 + 2, posY + 10 - j);
                        if (j < counter)
                        {
                            Console.WriteLine("|");
                        }
                        else
                            Console.WriteLine(" ");
                    }


                    if (player.getGain(i) >= 0)
                    {
                        Console.SetCursorPosition(posX + i * 8 + 1, posY + 1 + 10);
                        Console.WriteLine(" " + player.getGain(i) + "dB ");
                    }
                    else
                    {
                        Console.SetCursorPosition(posX + i * 8 + 1, posY + 1 + 10);
                        Console.WriteLine(player.getGain(i) + "dB ");
                    }
                }
                Console.ResetColor();
            }

            showPlaylist();

        }

        public void showPlaylist()
        {
            printFrame(38, 13,60,0);
            if (player.getPlaylist().Count > 0)
            {
                List<string>song = player.getPlaylist();
                int songs = song.Count();
                
                int starta = player.currentSong / 10;
                int start = starta * 10;
                Console.SetCursorPosition(62, 2);
                Console.WriteLine("PLAYLIST OUT OF " + player.getPlaylist().Count + " SONGS");
                for (int j = 0; j <10; j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(62, j + 3);
                    Console.Write(new string(' ', 36));
                }
                for (int i=0;i<10;i++)
                {
                    if(start==songs-(songs%10) && i>(songs%10)-1)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    if (player.isPlaying == true && i == player.currentSong%10)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(62, i + 3);
                        if (Path.GetFileName(song[start + i]).Length > 20)
                        {
                            Console.WriteLine(start + i + 1 + ". " + Path.GetFileName(song[start + i]).Substring(0, 17) + "... .mp3");
                        }
                        else
                            Console.WriteLine(start + i + 1 + ". " + Path.GetFileName(song[start + i]));
                        Console.SetCursorPosition(Console.WindowWidth - 7, i + 3);
                        Console.WriteLine(player.getSongTime((start + i)).ToString().Substring(3,5));
                        if (i == 9)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                    }
                    else if(player.isPlaying == false && i == player.currentSong % 10)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(62, i + 3);
                        if (Path.GetFileName(song[start + i]).Length > 20)
                        {
                            Console.WriteLine(start + i + 1 + ". " + Path.GetFileName(song[start + i]).Substring(0, 17) + "... .mp3");
                        }
                        else
                            Console.WriteLine(start + i + 1 + ". " + Path.GetFileName(song[start + i]));
                        Console.SetCursorPosition(Console.WindowWidth - 7, i + 3);
                        Console.WriteLine(player.getSongTime(start + i).ToString().Substring(3,5));
                        if (i ==9)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(62, i + 3);
                        if (Path.GetFileName(song[start + i]).Length > 20)
                        {
                            Console.WriteLine(start + i + 1 + ". " + Path.GetFileName(song[start + i]).Substring(0, 17) + "... .mp3");
                        }
                        else
                            Console.WriteLine(start + i + 1 + ". " + Path.GetFileName(song[start + i]));
                        Console.SetCursorPosition(Console.WindowWidth - 7, i + 3);
                        Console.WriteLine(player.getSongTime(start + i).ToString().Substring(3,5));
                    }
                 
                }
                
            }
            else
            {
                Console.SetCursorPosition(62, 2);
                Console.WriteLine("NO SONGS LOADED");
            }

        }

        public void startAutoPlay()
        {
            Thread thread = new Thread(() => autoPlay());
            thread.Start();
        }

        public void autoPlay()
        {
            while (true)
            {
                if (player.flag == true)
                {
                    if (player.checkTime())
                    {
                        showPlaylist();
                    }
                }
                Thread.Sleep(100);
            }
        }


        public void menuController(ref int current, int length, ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.OemPlus:
                case ConsoleKey.Add:
                    {
                        player.volumeUp();
                        break;
                    }
                case ConsoleKey.Subtract:
                case ConsoleKey.OemMinus:
                    {
                        player.volumeDown();
                        break;
                    }

                case ConsoleKey.PageDown:
                    {
                        if (player.getPlaylistSize() > 0)
                            player.playPrevious();
                        break;
                    }
                case ConsoleKey.PageUp:
                    {
                        if (player.getPlaylistSize() > 0)
                            player.playNext();
                        break;
                    }

                case ConsoleKey.DownArrow:
                    {
                        if (current < options.Length && equalizer==false)
                        {
                            current++;
                            if (current == options.Length)
                                current = 0;
                        }
                        if(equalizer==true && current!=options.Length-1)
                        {
                            player.gainDown(current);

                        }
                        break;
                    }

                case ConsoleKey.RightArrow:
                    {
                        if(equalizer==true)
                        {
                            if (current < options.Length)
                            {
                                current++;
                                if (current == options.Length)
                                    current = 0;
                            }
                        }
                        else
                            player.FastForward();
                        break;
                    }

                case ConsoleKey.LeftArrow:
                    {
                        if(equalizer==true)
                        {

                            if (current >= 0)
                            {
                                if (current == 0)
                                {
                                    current = options.Length;
                                }
                                current--;
                            }
                        }
                        else
                            player.Rewind();
                        break;
                    }

                case ConsoleKey.UpArrow:
                    {
                        if (current >= 0 && equalizer==false)
                        {
                            if (current == 0)
                            {
                                current = options.Length;
                            }
                            current--;
                        }
                        if (equalizer == true && current!= options.Length-1)
                        {
                            player.gainUp(current);
                        }
                        break;
                    }
                case ConsoleKey.Enter:
                    {
                        filesChoice(current, length);
                        mainChoice(current,length);
                        playerChoice(current,length);
                        equalizerChoice(current);
                        break;
                    }
                case ConsoleKey.Escape:
                    {
                        if(!inMain)
                        {
                            for (int i = 0; i < options.Length + 4; i++)
                            {
                                Console.SetCursorPosition(posX, posY + i);
                                Console.Write(new string(' ', Console.WindowWidth - 1));
                            }
                        }
                        if(inMain)
                        {
                            System.Environment.Exit(0);
                        }
                        break;
                    }
            }
        }

        public virtual void filesChoice(int current, int lenght) { }
        public virtual void mainChoice(int current, int length) { }
        public virtual void playerChoice(int current,int length) { }
        public virtual void equalizerChoice(int current) { }
        public abstract int Choice();
    }
}
