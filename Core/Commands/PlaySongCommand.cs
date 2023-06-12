namespace Core.Commands;

public class PlaySong : ICommand
{
    public string CommandName => "play";
    public bool RequiresParameter => true;

    public async Task ExecuteCommand(SpotifyProvider provider, Dictionary<string, string> args)
    { 
        var songId = args.First().Value;
        await provider.PlaySong(songId);        
    }
}