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
        Console.Clear();
        var endOfCommand = input.IndexOf(' ');
        var commandName = input.Substring(0, endOfCommand == -1 ? input.Length : endOfCommand);
        var command = _commands.FirstOrDefault(x => x.CommandName.ToLower() == commandName.ToLower());

        if (command == null)
        {
            ConsoleEx.WriteErrorLine($"The command {input.ToLower()} didn't work. Please try again.");
            return;
        }

        var parameters = input.Substring(endOfCommand+1).Split('-').ToList();
        Guards.IsNotNull(parameters);
        parameters.RemoveAt(0);
        var args = new Dictionary<string, string>();
        foreach(var p in parameters) args.Add(p.Substring(0, 1), p.Substring(2).Trim());

        if (command.RequiresParameter && (endOfCommand == -1 || parameters.First().IsNullOrEmpty()))
        {
            ConsoleEx.WriteErrorLine($"Please enter a name of a playlist after the 'playlist' command.");
            return;
        }

        await command.ExecuteCommand(_spotify, args);
    }
}