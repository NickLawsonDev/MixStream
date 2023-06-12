namespace Core.Commands;

public class GoToPageCommand : ICommand
{
    public string CommandName => "gopage";
    public bool RequiresParameter => true;

    public async Task ExecuteCommand(SpotifyProvider provider, Dictionary<string, string> args)
    {
        Guards.IsNotNull(args);
        int.TryParse(args.First().Value, out var page);

        if (page < 0)
        {
            ConsoleEx.WriteErrorLine("The requested page cannot be below 0");
            return;
        }
        Cache.UpdateCurrentPage(page);
    }
}