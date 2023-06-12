namespace Core.Commands;

public class NextPageCommand : ICommand
{
    public string CommandName => "nextpage";
    public bool RequiresParameter => false;

    public async Task ExecuteCommand(SpotifyProvider provider, Dictionary<string, string> args)
    {
        var currentPage = Cache.GetCurrentPage();
        Cache.UpdateCurrentPage(currentPage++);
    }
}
