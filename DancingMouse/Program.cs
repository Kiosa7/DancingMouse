using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

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

    static async Task Main(string[] args)
    {
        Console.WriteLine("Press Escape to close the application.");

        CancellationTokenSource cts = new CancellationTokenSource();
        Task moveMouseTask = MoveMouseAsync(cts.Token);

        while (true)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                cts.Cancel();
                break;
            }

            await Task.Delay(100); // Check for key press every 100ms
        }

        await moveMouseTask; // Wait for the mouse moving task to complete
    }

    static async Task MoveMouseAsync(CancellationToken token)
    {
        Random random = new Random();
        int screenWidth = Console.LargestWindowWidth;
        int screenHeight = Console.LargestWindowHeight;

        while (!token.IsCancellationRequested)
        {
            int x = random.Next(0, screenWidth);
            int y = random.Next(0, screenHeight);

            SetCursorPos(x, y);

            Console.WriteLine($"{DateTime.Now}: 😊 El ratón se ha movido a la posición ({x}, {y})");

            await Task.Delay(1000); // Move the mouse every second
        }
    }
}