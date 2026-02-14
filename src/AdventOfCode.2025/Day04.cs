namespace aoc;

public partial class Day04
{
    public readonly record struct Point2D(int X, int Y);

    public HashSet<Point2D> Parse(string input)
    {
        var points = new HashSet<Point2D>();
        var inputSpan = input.AsSpan().Trim();
        int start = 0, y = 0;
        while (start < inputSpan.Length)
        {
            int end = inputSpan[start..].IndexOf('\n');
            if (end == -1) end = inputSpan.Length - start;
            var line = inputSpan.Slice(start, end);
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '@')
                {
                    points.Add(new Point2D(x, y));
                }
            }
            start += end + 1;
            y++;
        }
        return points;
    }

    private static readonly (int dx, int dy)[] Directions =
    [
        (-1, -1), (0, -1), (1, -1),
        (-1, 0), (1, 0),
        (-1, 1), (0, 1), (1, 1),
    ];

    public static bool CountAdjacentNeighbors(Point2D point, HashSet<Point2D> allPoints, int threshold) =>
        GetNeighbors(point)
            .Where(allPoints.Contains)
            .Take(threshold)
            .Count() < threshold;

    public static IEnumerable<Point2D> GetNeighbors(Point2D point) =>
        Directions.Select(direction => new Point2D(point.X + direction.dx, point.Y + direction.dy));

    internal partial class Part1
    {
        private readonly Example _example1 = new(
        """
        ..@@.@@@@.
        @@@.@.@.@@
        @@@@@.@.@@
        @.@@@@..@.
        @@.@@@@.@@
        .@@@@@@@.@
        .@.@.@.@@@
        @.@@@.@@@@
        .@@@@@@@@.
        @.@.@@@.@.
        """, 13);

        public int Solve(HashSet<Point2D> input) =>
            input.Count(point => CountAdjacentNeighbors(point, input, threshold: 4));

    }

    internal partial class Part2
    {
        private readonly Example _example1 = new(
            """
            ..@@.@@@@.
            @@@.@.@.@@
            @@@@@.@.@@
            @.@@@@..@.
            @@.@@@@.@@
            .@@@@@@@.@
            .@.@.@.@@@
            @.@@@.@@@@
            .@@@@@@@@.
            @.@.@@@.@.
            """, 43);

        public int Solve(HashSet<Point2D> input)
        {
            // For Part 2 we want to repeatedly remove all rolls of paper that have less than 4 neighbors
            // until no more can be removed.
            var points = new HashSet<Point2D>(input);
            int removed;
            do
            {
                removed = points.RemoveWhere(p => CountAdjacentNeighbors(p, points, threshold: 4));
            } while (removed > 0);

            return input.Count - points.Count;
        }
    }
}
