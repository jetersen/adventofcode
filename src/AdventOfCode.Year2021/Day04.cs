using System.Text;

namespace AdventOfCode.Year2021;

public sealed class Day04 : BaseDay
{
    private List<int> _numbers = new();
    private readonly List<BingoBoard> _boards = new();

    public override async Task LoadInput()
    {
        var raw = await File.ReadAllTextAsync(InputFilePath.FullPath);
        var splits = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var draws = splits[0].Split(',').Select(Int32.Parse).ToArray();
        var boards = splits[1..].Chunk(5).Select(ParseBoard).ToHashSet();
        _numbers.AddRange(draws);
        _boards.AddRange(boards);
    }

    private static BingoBoard ParseBoard(string[] arg) => new(string.Join(' ', arg).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray());

    public override ValueTask<string> Solve_1()
    {
        foreach (var number in _numbers)
        {
            foreach (var board in _boards)
            {
                if (board.CallNumber(number))
                {
                    return new($"{board.UnmarkedSum()}");
                }
            }
        }

        return new();
    }

    public override ValueTask<string> Solve_2()
    {
        var lastWinner = 0;
        foreach (var number in _numbers)
        {
            foreach (var board in _boards)
            {
                if (board.CallNumber(number))
                {
                    lastWinner = board.UnmarkedSum();
                }
            }
        }

        return new($"{lastWinner}");
    }
}

class BingoBoard
{
    public int[,] Numbers { get; }
    public bool[,] Marked { get; }

    public BingoBoard(int[] board)
    {
        Numbers = new int[5, 5];
        Marked = new bool[5, 5];
        for (int i = 0; i < 25; i++)
        {
            Numbers[i / 5, i % 5] = board[i];
        }
    }

    public bool CallNumber(int number)
    {
        if (Finished) { return false; }

        for (var i = 0; i < 5; i++)
        {
            int totalRow = 0;
            int totalColumn = 0;

            for (var j = 0; j < 5; j++)
            {
                if (Numbers[i, j] == number) {
                    Marked[i, j] = true;
                } else if (Numbers[j, i] == number) {
                    Marked[j, i] = true;
                }

                if (Marked[i, j]) {
                    totalRow++;
                }
                if (Marked[j, i]) {
                    totalColumn++;
                }
            }

            if (totalRow != 5 && totalColumn != 5) continue;
            LastNumber = number;
            Finished = true;
            return true;
        }

        return false;
    }

    public int LastNumber { get; private set; }

    public int UnmarkedSum() {
        int sum = 0;
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                if (!Marked[i, j]) {
                    sum += Numbers[i, j];
                }
            }
        }
        return sum * LastNumber;
    }

    public bool Finished { get; private set; }
}
