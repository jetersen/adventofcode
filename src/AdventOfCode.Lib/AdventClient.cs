namespace AdventOfCode.Lib;

public interface IAdventClient
{
    Task FetchInput(uint year, uint day, FilePath path);
}

public class AdventClient : IAdventClient
{
    private readonly HttpClient _client;
    private readonly IEnvironment _environment;
    private readonly IFileSystem _fileSystem;

    public AdventClient(HttpClient client, IEnvironment environment, IFileSystem fileSystem)
    {
        _client = client;
        _environment = environment;
        _fileSystem = fileSystem;
    }

    private static string InputUrl(uint year, uint day) => $"{year}/day/{day}/input";

    public async Task FetchInput(uint year, uint day, FilePath path)
    {
        var file = _fileSystem.GetFile(path);
        if (file.Exists) return;

        var directory = _fileSystem.GetDirectory(path.GetDirectory());
        if (!directory.Exists) directory.Create();

        var stream = await _client.GetStreamAsync(InputUrl(year, day)).ConfigureAwait(false);
        var fileStream = file.OpenWrite();
        await using var _ = fileStream.ConfigureAwait(false);
        await stream.CopyToAsync(fileStream).ConfigureAwait(false);
    }
}
