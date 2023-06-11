namespace Core;

using Core.Models;

public interface IMusicProvider
{
    Task<Song> GetSongById(string id);
    Task<Album> GetAlbumById(string id);
    Task<Artist> GetArtistById(string id);
    Task<Playlist> GetPlaylistById(string id);
    Task<IEnumerable<Playlist>> GetUserPlaylists();
}