namespace Core.Commands;

public class PrevPageCommand : ICommand
{
    public string CommandName => "prevpage";
    public bool RequiresParameter => false;

    public async Task ExecuteCommand(SpotifyProvider provider, Dictionary<string, string> args)
    {
        var currentPage = Cache.GetCurrentPage();
        if (currentPage <= 0) return;
        Cache.UpdateCurrentPage(currentPage--);
    }
}