using System;
using System.Runtime.InteropServices;
using System.Threading;

class DancingMouse
{
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Press Escape to close the application.");

        Random random = new Random();
        int screenWidth = Console.LargestWindowWidth;
        int screenHeight = Console.LargestWindowHeight;

        while (true)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                break;
            }

            int x = random.Next(0, screenWidth);
            int y = random.Next(0, screenHeight);

            SetCursorPos(x, y);

            Console.WriteLine($"{DateTime.Now}: 😊 El ratón se ha movido a la posición ({x}, {y})");

            Thread.Sleep(1000); // Move the mouse every second
        }
    }
}