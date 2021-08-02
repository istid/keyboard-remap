/** This whole class is created by Stephen Toub: https://docs.microsoft.com/en-us/archive/blogs/toub/low-level-keyboard-hook-in-c
 * Which I have only modified the function "HookCallback" in it.
*/

using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

class InterceptKeys
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

    private static Controller controller;

    public InterceptKeys(Controller c)
    {
        controller = c;
        _hookID = SetHook(_proc);
        Application.Run();
        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

    //Modified
    private static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN) || wParam == (IntPtr)WM_KEYUP)     //Check key and if button is pressed or released
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Keys input = (Keys)vkCode;


            if (input == Keys.D1 && Keys.Control != Control.ModifierKeys && Keys.Shift != Control.ModifierKeys) //only accept '1' without shift or Ctr modifiers
            {
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    Console.WriteLine("1 is pressed");
                    controller.KeyInput('1', true);
                }
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    Console.WriteLine("1 is depressed");
                    controller.KeyInput('1', false);
                }

                return wParam;      //Return here if you want to catch the keyboard input. If you only want to read it make sure to call "return CallNextHookEx()"
            }
            Console.WriteLine(input);
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}