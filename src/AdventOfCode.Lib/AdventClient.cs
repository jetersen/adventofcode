namespace AdventOfCode.Lib;

public interface IAdventClient
{
    Task FetchInput(uint year, uint day, string path);
}

public class AdventClient : IAdventClient
{
    private readonly HttpClient _client;

    public AdventClient(HttpClient client)
    {
        _client = client;
    }

    private static string InputUrl(uint year, uint day) => $"{year}/day/{day}/input";

    public async Task FetchInput(uint year, uint day, string path)
    {
        if (File.Exists(path)) return;

        var directory = Path.GetDirectoryName(path) ?? string.Empty;
        Directory.CreateDirectory(directory);

        var stream = await _client.GetStreamAsync(InputUrl(year, day)).ConfigureAwait(false);
        var fileStream = File.Create(path);
        await using var _ = fileStream.ConfigureAwait(false);
        await stream.CopyToAsync(fileStream).ConfigureAwait(false);
    }
}
