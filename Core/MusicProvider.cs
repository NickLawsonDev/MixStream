using Core.Models;

namespace Core;

public abstract class MusicProvider : IMusicProvider
{
    protected static HttpClient HttpClient { get; set; } = new HttpClient();

    public abstract Task<Album> GetAlbumById(string id);
    public abstract Task<Artist> GetArtistById(string id);
    public abstract Task<Playlist> GetPlaylistById(string id);
    public abstract Task<Song> GetSongById(string id);
    public abstract Task<IEnumerable<Playlist>> GetUserPlaylists();
}