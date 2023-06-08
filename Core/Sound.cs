
using NAudio.Wave;

namespace Core;

public static class Sound
{
    private static string SONG_PATH => new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MediaFiles\SampleAudio_0.4mb.mp3")).ToString();
    private static IWavePlayer _player;

    public static void Play()
    {
        _player = new WaveOutEvent();

        using (var audioFile = new AudioFileReader(SONG_PATH))
        {
            _player.Init(audioFile);
            _player.Play();
        }
    }

    public static void Stop()
    {
        _player.Stop();
    }
}