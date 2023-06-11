using System.Text.Json;
using Core.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using static SpotifyAPI.Web.Scopes;

namespace Core;

public class SpotifyProvider : MusicProvider
{
    private static string _clientId { get; set; } = "";
    private static string _clientSecret { get; set; } = "";
    private const string _credentialsPath = "credentials.json";
    private static EmbedIOAuthServer _server = new(new Uri("http://localhost:5543/callback"), 5543);
    private SpotifyClient _spotify = new SpotifyClient("");

    public bool IsInitialized = false;

    public SpotifyProvider()
    {
        _clientId = Environment.GetEnvironmentVariable("MixStreamSpotifyClientId") ?? "";
        _clientSecret = Environment.GetEnvironmentVariable("MixStreamSpotifyClientSecret") ?? "";
    }

    public async Task Initialize()
    {
        if (File.Exists(_credentialsPath))
        {
            Start();
        }
        else
        {
            await Authentication();
        }
    }

    public void Start()
    {
        var json = File.ReadAllText(_credentialsPath);
        var token = JsonSerializer.Deserialize<PKCETokenResponse>(json);

        var authenticator = new PKCEAuthenticator(_clientId, token!);
        authenticator.TokenRefreshed += (sender, token) => File.WriteAllText(_credentialsPath, JsonSerializer.Serialize(token));

        var config = SpotifyClientConfig.CreateDefault()
          .WithAuthenticator(authenticator);

        _spotify = new SpotifyClient(config);
        _server.Dispose();
        IsInitialized = true;
    }

    public async Task Authentication()
    {
        var (verifier, challenge) = PKCEUtil.GenerateCodes();

        await _server.Start();

        _server.AuthorizationCodeReceived += async (sender, response) =>
        {
            await _server.Stop();
            var token = await new OAuthClient().RequestToken(
                new PKCETokenRequest(_clientId!, response.Code, _server.BaseUri, verifier)
            );

            await File.WriteAllTextAsync(_credentialsPath, JsonSerializer.Serialize(token));
            Start();
        };

        var request = new LoginRequest(_server.BaseUri, _clientId, LoginRequest.ResponseType.Code)
        {
            CodeChallenge = challenge,
            CodeChallengeMethod = "S256",
            Scope = new List<string>
            {
                UserReadEmail,
                UserReadPrivate,
                PlaylistReadPrivate,
                PlaylistReadCollaborative,
                AppRemoteControl,
                Streaming,
                PlaylistReadPrivate,
                UserReadCurrentlyPlaying,
                UserReadPlaybackState,
                UserReadRecentlyPlayed
            }
        };

        var uri = request.ToUri();
        try
        {
            BrowserUtil.Open(uri);
        }
        catch (Exception)
        {
            Console.WriteLine("Unable to open URL, manually open: {0}", uri);
        }
    }

    public override async Task<Album> GetAlbumById(string id)
    {
        Guards.IsNotNullOrEmpty(id);
        var album = await _spotify.Albums.Get(id);
        Guards.IsNotNull(album);

        return new(album.Id, album.Name, album.TotalTracks, album.ReleaseDate);
    }

    public override async Task<Artist> GetArtistById(string id)
    {
        Guards.IsNotNullOrEmpty(id);
        var artist = await _spotify.Artists.Get(id);
        Guards.IsNotNull(artist);

        return new(artist.Id, artist.Name);
    }

    public override async Task<Playlist> GetPlaylistById(string id)
    {
        Guards.IsNotNullOrEmpty(id);
        var playlist = await _spotify.Playlists.Get(id);
        Guards.IsNotNull(playlist);
        return new(playlist.Id ?? Guid.NewGuid().ToString(), playlist.Name ?? "");
    }

    public override async Task<Song> GetSongById(string id)
    {
        Guards.IsNotNullOrEmpty(id);
        var track = await _spotify.Tracks.Get(id);
        Guards.IsNotNull(track);

        return new(track.Id, track.Name, track.TrackNumber)
        {
            Artists = track.Artists.Select(x => new Artist(x.Id, x.Name)),
            Album = new(track.Album.Id, track.Album.Name, track.Album.TotalTracks, track.Album.ReleaseDate)
        };
    }

    public override async Task<IEnumerable<Playlist>> GetUserPlaylists()
    {
        var page = await _spotify.Playlists.CurrentUsers();
        var playlists = ((await _spotify.PaginateAll(page)).ToList())
                    .Select(x => new Playlist(x.Id ?? Guid.NewGuid().ToString(), x.Name ?? ""));

        Cache.UpdatePlaylists(playlists.ToList());
        return playlists;
    }

    public async Task<IEnumerable<Song>> GetSongsFromPlaylistId(string id)
    {
        Guards.IsNotNullOrEmpty(id);
        var playlist = await _spotify.Playlists.Get(id);
        Guards.IsNotNull(playlist);

        var songs = (await _spotify.PaginateAll
                        (await _spotify.Playlists.GetItems(playlist.Id!)))
                        .Where(x => x.Track.Type == ItemType.Track)
                        .ToList()
                        .Select(x => new Song(
                            ((FullTrack)x.Track).Id,
                            ((FullTrack)x.Track).Name,
                            ((FullTrack)x.Track).TrackNumber)
                        {
                            Artists = ((FullTrack)x.Track).Artists.Select(x => new Artist(x.Id, x.Name)),
                            Album = new Album(
                                ((FullTrack)x.Track).Id,
                                ((FullTrack)x.Track).Album.Name,
                                ((FullTrack)x.Track).Album.TotalTracks,
                                ((FullTrack)x.Track).Album.ReleaseDate),
                        })
                        .ToList();

        var p = Cache.GetPlaylist(playlist.Name!);
        Guards.IsNotNull(p!);
        p.Songs = songs;

        return songs;
    }

    public async Task PlaySong(string id)
    {
        Guards.IsNotNullOrEmpty(id);
        var track = await _spotify.Tracks.Get(id);
        Guards.IsNotNull(track);

        await _spotify.Player.ResumePlayback(new PlayerResumePlaybackRequest
        {
            Uris = new List<string>() { track.Uri },
        });
    }

    public async Task Pause()
    {
        await _spotify.Player.PausePlayback();
    }

    public async Task<CurrentlyPlayingContext> GetCurrentStatus()
    {
        return await _spotify.Player.GetCurrentPlayback();
    }
}