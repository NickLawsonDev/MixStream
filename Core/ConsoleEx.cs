namespace Core;

public static class ConsoleEx
{
    public static void WriteColoredLine(string input, ConsoleColor background = ConsoleColor.Black, ConsoleColor foreground = ConsoleColor.White)
    {
        Console.BackgroundColor = background;
        Console.ForegroundColor = foreground;
        Console.WriteLine(input);
        Console.ResetColor();
    }

    public static void WriteErrorLine(string input)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(input);
        Console.ResetColor();
    }
}