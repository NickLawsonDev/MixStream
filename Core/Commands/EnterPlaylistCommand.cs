namespace Core.Commands;

public class EnterPlaylistCommand : ICommand
{
    public string CommandName => "playlist";

    public bool RequiresParameter => true;

    public async Task ExecuteCommand(SpotifyProvider provider, string argument)
    {
        var playlistName = argument;

        var playlists = await provider.GetUserPlaylists();
        var playlist = playlists.First(x => x.Name.ToLower() == playlistName.ToLower());
        var songs = await provider.GetSongsFromPlaylistId(playlist.Id);
        foreach (var s in songs)
            Console.WriteLine($"{s.Title} - {string.Join(", ", s.Artists.Select(x => x.Name))} - {s.Id}");

        Console.WriteLine($"Number of songs in this playlist is {songs.Count()}");
    }
}