namespace Core.Commands;

public class EnterPlaylistCommand : ICommand
{
    public string CommandName => "playlist";
    public bool RequiresParameter => true;

    public async Task ExecuteCommand(SpotifyProvider provider, string argument)
    {
        var playlistName = argument;

        var playlist = Cache.GetPlaylist(playlistName);
        if (playlist == null) (await provider.GetUserPlaylists()).First(x => x.Name.ToLower() == playlistName.ToLower());
        Guards.IsNotNull(playlist!);
        var songs = await provider.GetSongsFromPlaylistId(playlist!.Id);
        foreach (var s in songs)
            Console.WriteLine($"{s.Title} - {string.Join(", ", s.Artists.Select(x => x.Name))} - {s.Id}");

        Console.WriteLine($"Number of songs in this playlist is {songs.Count()}");
    }
}