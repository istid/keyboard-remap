using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

class Controller
{
    private SendKeyInput send;

    public Controller(SendKeyInput s)
    {
        send = s;
    }

    //ATTENTION! Currently catching keypress "1" and sending keypress "2", running this program will make writing "1" impossible, only "NumPad1"
    public void KeyInput(char key, bool pressed)
    {
        if (key == '1' && pressed)
        {
            send.CharPress('2', pressed);
        }
        else if (key == '1' && !pressed)    //depress key
        {
            send.CharPress('2', pressed);
        }
    }

    //If you only want to write to a certain process
    private void Focus()
    {
        Process p = Process.GetProcessesByName("notepad.exe").FirstOrDefault();
        if (p != null)
        {
            IntPtr h = p.MainWindowHandle;
            SetForegroundWindow(h);
        }
    }

    [DllImport("User32.dll")]
    static extern int SetForegroundWindow(IntPtr point);
}