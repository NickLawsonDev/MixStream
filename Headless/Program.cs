using Core;
using SpotifyAPI.Web;

var spotify = new SpotifyProvider();
await spotify.Initialize();
var commandHandler = new CommandHandler(spotify);
var end = false;

while (!end)
{
    if (!spotify.IsInitialized)
    {
        Console.Clear();
        continue;
    }
    ConsoleEx.WriteColoredLine("Please enter command name", foreground: ConsoleColor.Blue);
    var input = Console.ReadLine();
    if (input.IsNullOrEmpty()) continue;
    Console.Clear();

    var status = await spotify.GetCurrentStatus();
    if (status != null && status.IsPlaying)
        ConsoleEx.WriteColoredLine(
            $"Currently Playing {((FullTrack)status.Item).Name} - {string.Join(", ", ((FullTrack)status.Item).Artists.Select(x => x.Name))}",
            background: ConsoleColor.Gray,
            foreground: ConsoleColor.Green);
    else
        ConsoleEx.WriteColoredLine(
            $"Not Currently Playing Anything",
            background: ConsoleColor.Gray,
            foreground: ConsoleColor.Green);

    await commandHandler.HandleCommand(input);
}

Console.WriteLine("Press any key to escape");
Console.ReadLine();