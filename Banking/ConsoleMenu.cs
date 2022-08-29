﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking
{
    internal class ConsoleMenu
    {
        #region Properties

        private string menuTitle = "";
        private string[] menuItems = new string[0];
        private string lastBit = "";
        private ConsoleColor bgc = ConsoleColor.Black; //Background Color
        private ConsoleColor fgc = ConsoleColor.White; //Foreground Color
        private ConsoleColor specialbg = ConsoleColor.White;
        private ConsoleColor specialfg = ConsoleColor.Black;
        private int menuSelectorIndex = 0;
        private string infoTxt = "";
        private string input = "";

        #endregion

        #region Methods

        public int ShowMenu()
        {
            bool menuDone = true;
            bool write = true;
            ConsoleKeyInfo key;
            do
            {
                if (write)
                {
                    WriteMenu();
                }
                write = true;
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.DownArrow)
                {
                    if (menuSelectorIndex + 1 < menuItems.Length)
                    {
                        menuSelectorIndex++;
                        continue;
                    }
                    else
                    {
                        write = false;
                    }
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    if (menuSelectorIndex - 1 >= 0)
                    {
                        menuSelectorIndex--;
                        continue;
                    }
                    else
                    {
                        write = false;
                    }
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    menuDone = false;
                    return menuSelectorIndex;
                }
            } while (menuDone);

            Console.ReadKey();
            return menuSelectorIndex;
        }

        public void ShowInfo()
        {
            WriteInfo();
            Console.ReadKey();
        }

        public string ShowPrompt(string inp="")
        {
            bool menuDone = true;
            input = inp;
            if (input != "" && infoTxt.Length > 9)
            {
                if (infoTxt.Substring(0, 10) == "Password: ")
                {
                    infoTxt += String.Concat(Enumerable.Repeat("*", input.Length));
                }
                else
                {
                    infoTxt += input;
                }
            }
            else
            {
                infoTxt += input;
            }
            ConsoleKeyInfo key;
            string validInput =
                "\'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890`-=[]\\;,./~!@#$%^&*()_+{}|:\"<>? ";
            if (infoTxt == "")
            {
                infoTxt = "> ";
            }
            do
            {
                WriteInfo();
                key = Console.ReadKey(true);
                
                if (key.Key == ConsoleKey.Enter)
                {
                    if (input != "")
                    {
                        return input;
                    }
                }
                else if (key.Key == ConsoleKey.Backspace && input != "")
                {
                    input = input.Remove(input.Length - 1);
                    infoTxt = infoTxt.Remove(infoTxt.Length - 1);
                }
                else if (validInput.IndexOf(key.KeyChar.ToString()) > 0)
                {
                    input += key.KeyChar.ToString();
                    if (infoTxt.Length > 9)
                    {
                        if (infoTxt.Substring(0, 10) == "Password: ")
                        {
                            infoTxt += "*";
                        }
                        else
                        {
                            infoTxt += key.KeyChar.ToString();
                        }
                    }
                    else
                    {
                        infoTxt += key.KeyChar.ToString();
                    }
                }
                else
                {
                    continue;
                }
            } while (menuDone);

            Console.ReadKey();
            return "";
        }
        public ConsoleMenu(string title, string[] items, string info = "", 
            ConsoleColor bg = ConsoleColor.Black, ConsoleColor fg = ConsoleColor.Cyan)
        {
            menuTitle = title;
            if (menuTitle.Length > Console.WindowWidth - 28)
            {
                menuTitle = menuTitle.Substring(0, Console.WindowWidth - 28);
                menuTitle += "..";
            }
            
            foreach (string item in items)
            {
                if (item.Length > Console.WindowWidth - 28)
                {
                    string newitem = "";
                    foreach (string word in item.Split(" "))
                    {
                        if (newitem.Length + word.Length >= Console.WindowWidth - 28)
                        {
                            menuItems = menuItems.Concat(new string[] { newitem }).ToArray();
                            newitem = "" + word + " ";
                        }
                        else
                        {
                            newitem += word + " ";
                        }
                    }
                    menuItems = menuItems.Concat(new string[] { newitem }).ToArray();

                }
                else
                {
                    menuItems = menuItems.Concat(new string[] { item }).ToArray();
                }
            }
            
            bgc = bg;
            fgc = fg;
            specialbg = fg;
            specialfg = bg;
            infoTxt = info;
            if (menuTitle.Length % 2 == 0)
            {
                lastBit = "! ";
            }
            else
            {
                lastBit = "!";
            }
        }

        private void WriteMenu()
        {
            Console.Clear();

            WriteTitle();
            WriteEmptyLine(4);

            for (int i = 0; i<menuItems.Length; i++)
            {
                Console.BackgroundColor = bgc;
                Console.ForegroundColor = fgc;
                Console.Write(" !");

                if (i == menuSelectorIndex)
                {
                    Console.Write("          ");
                    Console.BackgroundColor = specialbg;
                    Console.ForegroundColor = specialfg;
                    Console.Write(String.Concat("> ", menuItems[i]));
                    Console.BackgroundColor = bgc;
                    Console.ForegroundColor = fgc;
                    Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth - menuItems[i].Length - 14
                        -lastBit.Length)));
                    Console.Write(lastBit);
                }
                else
                {
                    Console.Write("            ");
                    Console.Write(menuItems[i]);
                    Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth - menuItems[i].Length - 14
                        - lastBit.Length)));
                    Console.Write(lastBit);
                }
            }

            WriteEmptyLine(Console.WindowHeight - menuItems.Length - 10);

            if (infoTxt == "")
            {
                WriteEmptyLine(4);
            }
            else
            {
                WriteInfoBox(infoTxt);
                WriteEmptyLine();
            }

            Console.Write(String.Concat(" !", string.Concat(Enumerable.Repeat("~", (Console.WindowWidth - 2 - lastBit.Length)))));
            Console.Write(lastBit);

        }

        private void WriteInfo()
        {
            Console.Clear();

            WriteTitle();
            WriteEmptyLine(4);

            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.BackgroundColor = bgc;
                Console.ForegroundColor = fgc;
                Console.Write(" !");

                Console.Write("            ");
                Console.Write(menuItems[i]);
                Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth - menuItems[i].Length - 14
                    - lastBit.Length)));
                Console.Write(lastBit);
            }

            WriteEmptyLine(Console.WindowHeight - menuItems.Length - 10);

            if (infoTxt == "")
            {
                WriteEmptyLine(4);
            }
            else
            {
                WriteInfoBox(infoTxt);
                WriteEmptyLine();
            }

            Console.Write(String.Concat(" !", string.Concat(Enumerable.Repeat("~", (Console.WindowWidth - 2 - lastBit.Length)))));
            Console.Write(lastBit);
        }

        private void WriteTitle()
        {
            Console.BackgroundColor = bgc;
            Console.ForegroundColor = fgc;
            Console.Write(string.Concat(" !", string.Concat(Enumerable.Repeat("~", (Console.WindowWidth / 2) -
                (menuTitle.Length / 2) - 2))));
            Console.BackgroundColor = specialbg;
            Console.ForegroundColor = specialfg;
            Console.Write(menuTitle);
            Console.BackgroundColor = bgc;
            Console.ForegroundColor = fgc;
            Console.Write(string.Concat(Enumerable.Repeat("~", (Console.WindowWidth / 2) -
                (menuTitle.Length / 2) - 2)));
            Console.WriteLine(lastBit);
        }
        private void WriteInfoBox(string infoMsg = "")
        {
            if (infoMsg == "")
            {
                WriteEmptyLine(3);
                return;
            }
            else
            {
                Console.BackgroundColor = bgc;
                Console.ForegroundColor = fgc;
                Console.Write(" !");
                Console.Write("            +");
                Console.Write(String.Concat(Enumerable.Repeat("-", infoMsg.Length+4)));
                Console.Write("+");
                Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth-(infoMsg.Length + 20 + 
                    lastBit.Length))));
                Console.WriteLine(lastBit);

                Console.Write(" !");
                Console.Write("            |  ");
                Console.BackgroundColor = specialbg;
                Console.ForegroundColor = specialfg;
                Console.Write(infoMsg);
                Console.BackgroundColor = bgc;
                Console.ForegroundColor = fgc;
                Console.Write("  |");
                Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth - (infoMsg.Length + 20 +
                    lastBit.Length))));
                Console.WriteLine(lastBit);

                Console.Write(" !");
                Console.Write("            +");
                Console.Write(String.Concat(Enumerable.Repeat("-", infoMsg.Length + 4)));
                Console.Write("+");
                Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth - (infoMsg.Length + 20 +
                    lastBit.Length))));
                Console.WriteLine(lastBit);
            }
        }

        private void WriteEmptyLine(int lineCount = 1)
        {
            for (int i = 0; i < lineCount; i++)
            {
                Console.BackgroundColor = bgc;
                Console.ForegroundColor = fgc;
                Console.Write(String.Concat(" !", string.Concat(Enumerable.Repeat(" ", (Console.WindowWidth - 2 - lastBit.Length)))));
                Console.WriteLine(lastBit);
            }
            
        }

        #endregion
    }
}
