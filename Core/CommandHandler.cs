using Core.Commands;

namespace Core;

public class CommandHandler
{
    private List<ICommand> _commands { get; set; } = new List<ICommand>();
    private SpotifyProvider _spotify { get; set; }

    public CommandHandler(SpotifyProvider spotify)
    {
        _spotify = spotify;

        var inteface = typeof(ICommand);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.GetInterfaces().Contains(typeof(ICommand)))
            .ToList();

        foreach (var type in types) _commands.Add((ICommand)Activator.CreateInstance(type)!);
    }

    public async Task HandleCommand(string input)
    {
        Guards.IsNotNullOrEmpty(input);
        var endOfCommand = input.IndexOf(' ');
        var commandName = input.Substring(0, endOfCommand == -1 ? input.Length : endOfCommand);
        var command = _commands.FirstOrDefault(x => x.CommandName.ToLower() == commandName.ToLower());

        if (command == null)
        {
            ConsoleEx.WriteErrorLine($"The command {input.ToLower()} didn't work. Please try again.");
            return;
        }

        var parameter = input.Substring(endOfCommand+1);
        if (command.RequiresParameter && (endOfCommand == -1 || parameter.IsNullOrEmpty()))
        {
            ConsoleEx.WriteErrorLine($"Please enter a name of a playlist after the 'playlist' command.");
            return;
        }

        await command.ExecuteCommand(_spotify, parameter);
    }
}