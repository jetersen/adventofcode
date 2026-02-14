namespace aoc;

partial class Day02
{
    public ulong[] Parse(string input)
    {
        var inputSpan = input.AsSpan();
        var parts = new List<ulong>();
        var start = 0;

        while (start < inputSpan.Length)
        {
            var end = inputSpan[start..].IndexOf(',');
            if (end == -1) end = inputSpan.Length - start;
            var segment = inputSpan.Slice(start, end);

            var dashIndex = segment.IndexOf('-');
            if (dashIndex != -1)
            {
                var firstPart = segment[..dashIndex];
                var secondPart = segment[(dashIndex + 1)..];
                var startNumber = ulong.Parse(firstPart);
                var endNumber = ulong.Parse(secondPart);
                // Add the range of numbers to the list (inclusive)
                for (var num = startNumber; num <= endNumber; num++)
                {
                    parts.Add(num);
                }
            }

            start += end + 1;
        }

        return parts.ToArray();
    }

    internal partial class Part1
    {
        private readonly Example _example1 = new(
        """
        11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124
        """, 1227775554u);

        public ulong Solve(ulong[] input)
        {
            // Look for any number that is made only of some sequence of digits repeated twice. So, 55, 6464, 123123 are all valid.
            // And sum them up
            var sum = 0ul;
            foreach (var number in input)
            {
                var places = (uint)(Math.Log10(number) + 1) / 2;
                var hundos = (uint)Math.Pow(10, places);
                if (number / hundos == number % hundos)
                {
                    sum += number;
                }
            }

            return sum;
        }
    }

    internal partial class Part2
    {
        private readonly Example _example1 = new(
            """
            11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124
            """, 4174379265);

        public ulong Solve(ulong[] input)
        {
            var sum = 0ul;

            foreach (var number in input)
            {
                var digitCount = (int)(Math.Log10(number) + 1);

                for (var patternLength = 1; patternLength <= digitCount / 2; patternLength++)
                {
                    if (digitCount % patternLength != 0)
                        continue;

                    var divisor = (ulong)Math.Pow(10, patternLength);
                    var repetitions = digitCount / patternLength;

                    // Calculate what the number would be if it's just the pattern repeated
                    // For example: pattern 12, repetitions 3 -> 12 * (1 + 100 + 10000) = 121212
                    var multiplier = ((ulong)Math.Pow(divisor, repetitions) - 1) / (divisor - 1);
                    var pattern = number % divisor;

                    if (pattern * multiplier != number) continue;
                    sum += number;
                    break;
                }
            }

            return sum;
        }
    }
}
