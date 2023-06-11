using Core;
using Core.Models;
using SpotifyAPI.Web;

var spotify = new SpotifyProvider();
spotify.Initialize();
var commandHandler = new CommandHandler(spotify);
var end = false;

while (!end)
{
    if (!spotify.IsInitialized) continue;
    Console.Clear();
    var status = await spotify.GetCurrentStatus();
    if (status.IsPlaying)
        ConsoleEx.WriteColoredLine(
            $"Currently Playing {((FullTrack)status.Item).Name} - {string.Join(", ", ((FullTrack)status.Item).Artists.Select(x => x.Name))}",
            background: ConsoleColor.Gray,
            foreground: ConsoleColor.Green);

    ConsoleEx.WriteColoredLine("Please enter command name", foreground: ConsoleColor.Blue);
    var input = Console.ReadLine();
    if (input.IsNullOrEmpty()) continue;
    await commandHandler.HandleCommand(input);
}

Console.WriteLine("Press any key to escape");
Console.ReadLine();