namespace AdventOfCode.Lib;

public interface IAdventClient
{
    Task FetchInput(uint year, uint day, FilePath path);
}

public class AdventClient : IAdventClient
{
    private readonly HttpClient _client;

    public AdventClient(HttpClient client)
    {
        _client = client;
    }

    private static string InputUrl(uint year, uint day) => $"{year}/day/{day}/input";

    public async Task FetchInput(uint year, uint day, FilePath path)
    {
        if (Constants.SessionCookie is "") return;
        if (File.Exists(path.FullPath)) return;

        var directory = path.GetDirectory();
        if (!Directory.Exists(directory.FullPath)) Directory.CreateDirectory(directory.FullPath);

        var stream = await _client.GetStreamAsync(InputUrl(year, day)).ConfigureAwait(false);
        var fileStream = File.OpenWrite(path.FullPath);
        await using var _ = fileStream.ConfigureAwait(false);
        await stream.CopyToAsync(fileStream).ConfigureAwait(false);
    }
}
