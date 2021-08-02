class Program
{
    public static void Main()
    {
        SendKeyInput s = new SendKeyInput();
        Controller c = new Controller(s);
        InterceptKeys i = new InterceptKeys(c);
    }
}
