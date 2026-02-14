namespace aoc;

partial class Day01
{
    const int DIAL_SIZE = 100;
    const int START_POS = 50;

    private static int EuclideanMod(int dividend, int divisor) =>
        (dividend % divisor + divisor) % divisor;

    public int[] Parse(string input)
    {
        var inputSpan = input.AsSpan();
        var lineCount = inputSpan.Count('\n') + 1;
        var result = new int[lineCount];
        int start = 0, idx = 0;

        while (start < inputSpan.Length)
        {
            int end = inputSpan[start..].IndexOf('\n');
            if (end == -1) end = inputSpan.Length - start;
            var line = inputSpan.Slice(start, end);
            char direction = line[0];
            int number = int.Parse(line[1..]);
            result[idx++] = direction == 'L' ? -number : number;
            start += end + 1;
        }

        return result;
    }

    internal partial class Part1
    {
        private readonly Example _example1 = new(
        """
        L68
        L30
        R48
        L5
        R60
        L55
        L1
        L99
        R14
        L82
        """, 3);

        private static (int dial, int zeroCount) MoveDialState((int dial, int zeroCount) state, int direction)
        {
            state.dial = EuclideanMod(state.dial + direction, DIAL_SIZE);
            state.zeroCount += state.dial == 0 ? 1 : 0;
            return state;
        }

        public int Solve(int[] directions) =>
            directions.Aggregate((dial: START_POS, zeroCount: 0),
                MoveDialState).zeroCount;

    }

    internal partial class Part2
    {
        private readonly Example _example1 = new(
            """
            L68
            L30
            R48
            L5
            R60
            L55
            L1
            L99
            R14
            L82
            """, 6);

        private static (int newDial, int revolutions) MoveDialState((int dial, int revolutions) state, int move)
        {
            var dialMoved = state.dial + move;
            var revolutions = Math.Abs(dialMoved / DIAL_SIZE);
            var newDial = EuclideanMod(dialMoved, DIAL_SIZE);
            if (state.dial != 0 && dialMoved <= 0)
            {
                revolutions++;
            }

            state.dial = newDial;
            state.revolutions += revolutions;

            return state;
        }

        public int Solve(int[] directions) =>
            directions.Aggregate((dial: START_POS, revolutions: 0), MoveDialState).revolutions;

    }
}
