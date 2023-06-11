namespace Core.Commands;

public class PlaySong : ICommand
{
    public string CommandName => "play";

    public bool RequiresParameter => true;

    public async Task ExecuteCommand(SpotifyProvider provider, string argument)
    { 
        var songId = argument;
        await provider.PlaySong(songId);        
    }
}