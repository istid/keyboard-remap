using System;
using System.Runtime.InteropServices;

class SendKeyInput  //TODO: Create enum Scancodes
{
    public void CharPress(char character, bool pressed)   //Scancodes https://www.win.tue.nl/~aeb/linux/kbd/scancodes-1.html
    {
        if (character == '1')
        {
            SendKey(0x02, pressed);  //'1'
        }
        else if (character == '2')
        {
            SendKey(0x03, pressed);  //'2'
        }
        else if (character == '3')
        {
            SendKey(0x04, pressed);  //'3'
        }
        else
        {
            SendKey(0x02, pressed);  //'1'
        }
    }

    private static void SendKey(ushort key, bool pressed)
    {
        Console.WriteLine("Inside sendkey");

        Input[] InputData = new Input[1];
        InputData[0].type = 1;
        InputData[0].u.ki.wScan = key;
        InputData[0].u.ki.time = 0;
        InputData[0].u.ki.dwExtraInfo = IntPtr.Zero;

        if (pressed)
        {
            InputData[0].u.ki.dwFlags = (uint)(KeyEventF.KeyDown | KeyEventF.Scancode);
        }
        else
        {
            InputData[0].u.ki.dwFlags = (uint)(KeyEventF.KeyUp | KeyEventF.Scancode);
        }

        SendInput(1, InputData, Marshal.SizeOf(typeof(Input)));
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

    [DllImport("user32.dll")]
    private static extern IntPtr GetMessageExtraInfo();

    [Flags]
    private enum InputType
    {
        Mouse = 0,
        Keyboard = 1,
        Hardware = 2
    }

    [Flags]
    private enum KeyEventF
    {
        KeyDown = 0x0000,
        ExtendedKey = 0x0001,
        KeyUp = 0x0002,
        Unicode = 0x0004,
        Scancode = 0x0008,
    }

    private struct Input
    {
        public int type;
        public InputUnion u;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion
    {
        [FieldOffset(0)] public readonly MouseInput mi;
        [FieldOffset(0)] public KeyboardInput ki;
        [FieldOffset(0)] public readonly HardwareInput hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MouseInput
    {
        public readonly int dx;
        public readonly int dy;
        public readonly uint mouseData;
        public readonly uint dwFlags;
        public readonly uint time;
        public readonly IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KeyboardInput
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HardwareInput
    {
        public readonly uint uMsg;
        public readonly ushort wParamL;
        public readonly ushort wParamH;
    }
}