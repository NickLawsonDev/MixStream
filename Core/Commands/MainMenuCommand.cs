namespace Core.Commands;

public class MainMenuCommand : ICommand
{
    public string CommandName => "menu";
    public bool RequiresParameter => false;

    public async Task ExecuteCommand(SpotifyProvider provider, Dictionary<string, string> args)
    {
        var playlists = await provider.GetUserPlaylists();
        foreach (var p in playlists) Console.WriteLine(p.Name);
        Console.WriteLine($"Total Count of Playlists: {playlists.Count()}");
    }
}