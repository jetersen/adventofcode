namespace aoc;

public partial class Day03
{
    public int[][] Parse(string input)
    {
        var inputSpan = input.AsSpan().Trim();
        var lineCount = inputSpan.Count('\n') + 1;
        var result = new int[lineCount][];
        int start = 0, idx = 0;
        while (start < inputSpan.Length)
        {
            var end = inputSpan[start..].IndexOf('\n');
            if (end == -1) end = inputSpan.Length - start;
            var line = inputSpan.Slice(start, end);
            var numbers = new int[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                numbers[i] = line[i] - '0';  // Direct char to int conversion
            }
            result[idx++] = numbers;
            start += end + 1;
        }
        return result;
    }

    internal partial class Part1
    {
        private readonly Example _example1 = new(
        """
        987654321111111
        811111111111119
        234234234234278
        818181911112111
        """, 357);

        public int Solve(int[][] input)
        {
            // For each position, track the best first digit seen so far and best pair
            return input.Sum(row =>
            {
                var maxPair = -1;
                var maxFirst = row[0];  // Best first digit seen so far

                for (var i = 1; i < row.Length; i++)
                {
                    // Try pairing maxFirst with current digit
                    var currentPair = maxFirst * 10 + row[i];
                    if (currentPair > maxPair)
                    {
                        maxPair = currentPair;
                    }

                    // Update maxFirst if current is better
                    if (row[i] > maxFirst)
                    {
                        maxFirst = row[i];
                    }
                }

                return maxPair;
            });
        }
    }

    internal partial class Part2
    {
        private readonly Example _example1 = new(
            """
            987654321111111
            811111111111119
            234234234234278
            818181911112111
            """, 3121910778619);

        public long Solve(int[][] input)
        {
            // Greedy algorithm: pick 12 best digits maintaining order
            // For each position i (0..11), find the max digit in a window that leaves room for remaining digits
            return input.Sum(row =>
            {
                long result = 0;
                var currentIndex = 0;

                for (var i = 0; i < 12; i++)
                {
                    // Window end: must leave room for (11 - i) more digits
                    var windowEnd = row.Length - (11 - i);

                    // Find the maximum digit in the current window
                    var maxDigit = row[currentIndex];
                    var maxIndex = currentIndex;

                    for (var j = currentIndex + 1; j < windowEnd; j++)
                    {
                        if (row[j] <= maxDigit) continue;
                        maxDigit = row[j];
                        maxIndex = j;
                    }

                    result = result * 10 + maxDigit;
                    currentIndex = maxIndex + 1;
                }

                return result;
            });
        }
    }
}
