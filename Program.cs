using System.Drawing;
using static Prototype.ConsoleGraphics;

namespace Prototype
{
    internal class Program
    {
        internal static Size Window = new Size(30, 12);

        static string input = "";
        static Point currentReadOutput = new Point(14, 8);
        internal static readonly object consoleLock = new object();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            ConsoleBuffer myBuffer = new ConsoleBuffer();
            GfxEgg myEgg = new(new(5, 2));

            char[] currentRead = new char[Window.Width - 1];

            Thread inputThread = new Thread(() => {
                while(true) {
                    ConsoleKeyInfo keyInfo;
                    int count = 0;
                    do
                    {
                        keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Backspace || keyInfo.Key == ConsoleKey.Delete)
                        {
                            if (count <= 0) continue;
                            count--;
                            input = input.Substring(0, count);
                            myBuffer.EraseFromBuf(currentReadOutput.X + count, currentReadOutput.Y, 1);
                        }
                        else
                        {
                            if (keyInfo.Key != ConsoleKey.Enter)
                            {
                                input += keyInfo.KeyChar;
                                myBuffer.WriteToBuf(currentReadOutput.X + count, currentReadOutput.Y, keyInfo.KeyChar.ToString());
                                count++;
                            }
                        }
                    } while (keyInfo.Key != ConsoleKey.Enter);
                    myBuffer.EraseFromBuf(currentReadOutput.X, currentReadOutput.Y, count);
                    DigestInput();
                }
            });

            Thread renderThread = new Thread(() => {
                while(true)
                {
                    myBuffer.PrintBuf();
                    Thread.Sleep(12);
                }
            });

            myBuffer.ClearBuf();

            inputThread.Start();
            renderThread.Start();

            while (true)
            {
                myBuffer.WriteToBuf(10, 1, "It begins");
                myBuffer.PrintFrameToBuf(myEgg.Animation);

                if (myEgg.Animation.CurrentFrame == 0)
                {
                    myEgg.Animation.CurrentFrame = 1;
                }
                else if (myEgg.Animation.CurrentFrame == 1)
                {
                    myEgg.Animation.CurrentFrame = 0;
                }

                
                Thread.Sleep(250);
            }
        }

        static void DigestInput()
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, 12);
                Console.Write($"                               ");
                Console.SetCursorPosition(0, 12);
                Console.WriteLine($"Was typed: {input}");
                input = "";
                currentReadOutput = new Point(14, 8);
                Console.SetCursorPosition(0, 0);
            }
        }
    }

    internal class ConsoleBuffer
    {
        private char[,] Buffer { get; }

        public ConsoleBuffer()
        {
            Buffer = new char[Program.Window.Width, Program.Window.Height];
        }

        public void WriteToBuf(int x, int y, string str)
        {
            for (int i = 0 ; i < str.Length ; i++)
            {
                if (x + i >= Buffer.GetLength(0)) break;
                Buffer[x + i, y] = str[i];
            }
        }

        public void EraseFromBuf(int x, int y, int count)
        {
            for (int i = 0 ; i < count ; i++)
            {
                if (x + i >= Buffer.GetLength(0)) break;
                Buffer[x + i, y] = '.';
            }
        }

        public void ClearBuf()
        {
            for (int y = 0 ; y < Buffer.GetLength(1) ; y++)
            {
                for (int x = 0 ; x < Buffer.GetLength(0) ; x++)
                {
                    Buffer[x, y] = '.';
                }
            }
        }

        public void PrintBuf()
        {
            lock (Program.consoleLock)
            {
                Console.SetCursorPosition(0, 0);
                for (int y = 0 ; y < Buffer.GetLength(1) ; y++)
                {
                    for (int x = 0 ; x < Buffer.GetLength(0) ; x++)
                    {
                        Console.Write(Buffer[x, y]);
                    }
                    Console.WriteLine();
                }
            }
        }

        public void PrintFrameToBuf(Animation animation, int offset_x = 0, int offset_y = 0)
        {
            int[,] current = animation.Frames[animation.CurrentFrame];
            for (int j = 0 ; j < current.GetLength(1) ; j++)
            {
                for (int i = 0 ; i < current.GetLength(0) ; i++)
                {
                    if (current[i, j] == 1)
                    {
                        Buffer[j + animation.Origin.X + offset_x, i + animation.Origin.Y + offset_y] = '@';
                    }
                    else
                    {
                        Buffer[j + animation.Origin.X + offset_x, i + animation.Origin.Y + offset_y] = ' ';
                    }
                }
            }
        }
    }
}