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

    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

    [Flags]
    public enum EXECUTION_STATE : uint
    {
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    const int KEYEVENTF_KEYUP = 0x0002;
    const byte VK_SHIFT = 0x10;

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

        // Prevent the system from entering sleep or turning off the display
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);

        while (!token.IsCancellationRequested)
        {
            int x = random.Next(0, screenWidth);
            int y = random.Next(0, screenHeight);

            SetCursorPos(x, y);

            // Simulate a key press to ensure Teams stays active
            keybd_event(VK_SHIFT, 0, 0, UIntPtr.Zero);
            keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);

            Console.WriteLine($"{DateTime.Now}: 😊 El ratón se ha movido a la posición ({x}, {y})");

            await Task.Delay(1000); // Move the mouse every second
        }

        // Clear the execution state flags when the task is cancelled
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
    }
}