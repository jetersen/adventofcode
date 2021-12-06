namespace AdventOfCode.Year2021;

public sealed class Day05 : BaseDay
{
    private List<(int x1, int y1, int x2, int y2, int xDirection, int yDirection, int x2Direction, int y2Direction, bool diagonal)> _lines = new();

    public override async Task LoadInput()
    {
        /*
         * Input examples:
         * 961,238 -> 240,959
         * 383,133 -> 383,503
         * 691,442 -> 616,442
         */
        var raw = await File.ReadAllLinesAsync(InputFilePath.FullPath);
        var list = raw
            .Select(line =>
            {
                var pairs = line.Split(", ->".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                var x1 = int.Parse(pairs[0]);
                var y1 = int.Parse(pairs[1]);
                var x2 = int.Parse(pairs[2]);
                var y2 = int.Parse(pairs[3]);
                var xDirection = Math.Sign(x2 - x1);
                var yDirection = Math.Sign(y2 - y1);
                var x2Direction = (x2 + xDirection);
                var y2Direction = (y2 + yDirection);
                var diagonal = (x1 != x2 && y1 != y2);
                return (
                    x1,
                    y1,
                    x2,
                    y2,
                    xDirection,
                    yDirection,
                    x2Direction,
                    y2Direction,
                    diagonal
                );
            });
        _lines.AddRange(list);
    }

    public override ValueTask<string> Part1()
    {
        var numberOfIntersections = NumberOfIntersections(true);

        return new($"{numberOfIntersections}");
    }

    private int NumberOfIntersections(bool skipDiagonals)
    {
        var intersections = new Dictionary<(int x, int y), int>();
        foreach (var (x1, y1, x2, y2, xDirection, yDirection, x2Direction, y2Direction, diagonal) in _lines)
        {
            if (skipDiagonals && diagonal) continue;

            for (int x = x1, y = y1; x != x2Direction || y != y2Direction; x += xDirection, y += yDirection)
                intersections[(x, y)] = intersections.GetValueOrDefault((x, y)) + 1;
        }

        var numberOfIntersections = intersections.Count(kvp => kvp.Value > 1);
        return numberOfIntersections;
    }

    public override ValueTask<string> Part2()
    {
        var numberOfIntersections = NumberOfIntersections(false);

        return new($"{numberOfIntersections}");
    }
}
