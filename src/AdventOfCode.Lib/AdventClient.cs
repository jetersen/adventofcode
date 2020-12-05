namespace AdventOfCode.Lib
{
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class AdventClient
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
            if (Directory.Exists(directory) == false) Directory.CreateDirectory(directory);

            var stream = await _client.GetStreamAsync(InputUrl(year, day));
            await using var fileStream = File.Create(path);
            await stream.CopyToAsync(fileStream);
        }
    }
}
