namespace Core.Commands;

public interface ICommand
{
    string CommandName { get; }
    bool RequiresParameter { get; }

    Task ExecuteCommand(SpotifyProvider provider, string args);
}