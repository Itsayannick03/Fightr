public static class Debug
{
    public static bool Enabled = false;
    public static bool Detailed = false;
    public static bool StepThrough = false;

    private static void Write(string message)
    {
        if (!Enabled) return;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [DEBUG] {message}");
        Console.ResetColor();
    }

    public static void Log(string message)
    {
        Write(message);
    }

    public static void LogDetail(string message)
    {
        if (!Detailed) return;
        Write(message);
    }
}
