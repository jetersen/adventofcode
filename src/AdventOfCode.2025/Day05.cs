namespace aoc;

public partial class Day05
{
    private static List<(ulong start, ulong end)> MergeRanges(List<(ulong start, ulong end)> ranges)
    {
        if (ranges.Count == 0) return [];

        // Sort ranges in-place by start value
        ranges.Sort((a, b) => a.start.CompareTo(b.start));

        var merged = new List<(ulong start, ulong end)>(ranges.Count) { ranges[0] };

        for (int i = 1; i < ranges.Count; i++)
        {
            var range = ranges[i];
            var last = merged[^1];

            // Check if ranges overlap or are adjacent
            if (range.start <= last.end)
            {
                // Merge by extending the end if necessary
                var newEnd = Math.Max(last.end, range.end);
                merged[^1] = (last.start, newEnd);
            }
            else
            {
                merged.Add(range);
            }
        }

        return merged;
    }

    public ((ulong start, ulong end)[] ranges, ulong[] numbers) Parse(string input)
    {
        var inputSpan = input.AsSpan().Trim();
        var indexOfDoubleNewline = inputSpan.IndexOf("\n\n");
        var rangesSpan = inputSpan[..indexOfDoubleNewline];
        var numbersSpan = inputSpan[(indexOfDoubleNewline + 2)..];
        var rangesList = new List<(ulong start, ulong end)>();
        int start = 0;
        while (start < rangesSpan.Length)
        {
            int end = rangesSpan[start..].IndexOf('\n');
            if (end == -1) end = rangesSpan.Length - start;
            var line = rangesSpan.Slice(start, end);
            var dashIndex = line.IndexOf('-');
            var rangeStart = ulong.Parse(line[..dashIndex]);
            var rangeEnd = ulong.Parse(line[(dashIndex + 1)..]);
            rangesList.Add((rangeStart, rangeEnd + 1)); // Range end is exclusive
            start += end + 1;
        }
        var numbersList = new List<ulong>();
        start = 0;
        while (start < numbersSpan.Length)
        {
            int end = numbersSpan[start..].IndexOf('\n');
            if (end == -1) end = numbersSpan.Length - start;
            var line = numbersSpan.Slice(start, end);
            var number = ulong.Parse(line);
            numbersList.Add(number);
            start += end + 1;
        }

        return (MergeRanges(rangesList).ToArray(), numbersList.ToArray());
    }

    internal partial class Part1
    {
        private readonly Example _example1 = new(
        """
        3-5
        10-14
        16-20
        12-18
        
        1
        5
        8
        11
        17
        32
        """, 3);

        public int Solve(((ulong start, ulong end)[] ranges, ulong[] numbers) input)
        {
            var (ranges, numbers) = input;

            // Count numbers that fall within any merged range using binary search
            return numbers.Count(number => IsInRanges(ranges, number));
        }

        private static bool IsInRanges((ulong start, ulong end)[] ranges, ulong number)
        {
            // Binary search to find the range that might contain the number
            int left = 0;
            int right = ranges.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                var range = ranges[mid];

                if (number < range.start)
                {
                    right = mid - 1;
                }
                else if (number >= range.end)
                {
                    left = mid + 1;
                }
                else
                {
                    // number >= range.start && number < range.end
                    return true;
                }
            }

            return false;
        }



    }

    internal partial class Part2
    {
        private readonly Example _example1 = new(
            """
            3-5
            10-14
            16-20
            12-18
            
            1
            5
            8
            11
            17
            32
            """, 14);

        public ulong Solve(((ulong start, ulong end)[] ranges, ulong[] numbers) input)
        {
            var (ranges, _) = input;

            return ranges.Aggregate(0UL, (sum, range) => sum + (range.end - range.start));
        }
    }
}
