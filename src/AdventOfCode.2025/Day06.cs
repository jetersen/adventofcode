using System.Buffers;

namespace aoc;

public partial class Day06
{
    public enum Operations
    {
        Add,
        Multiply,
    }

    static readonly SearchValues<char> OperationChars = SearchValues.Create('+', '*');

    internal partial class Part1
    {
        private readonly Example _example1 = new(
            """
            123 328  51 64 
             45 64  387 23 
              6 98  215 314
            *   +   *   +  
            """, 4277556ul);

        public (List<List<ulong>>, List<Operations> operations) Parse(string input)
        {
            var lines = input.AsSpan().TrimEnd('\n');
            var operationsLineIndex = lines.LastIndexOf('\n');
            var operationLine = lines[(operationsLineIndex + 1)..];
            var numberLines = lines[..operationsLineIndex].TrimEnd('\n');
            var operationCount = operationLine.CountAny(OperationChars);

            // Pre-split lines into ranges for efficient access without allocation
            Span<Range> lineRanges = stackalloc Range[32]; // Allocate stack space for up to 32 lines
            int lineCount = numberLines.Split(lineRanges, '\n', StringSplitOptions.RemoveEmptyEntries);
            var result = new List<List<ulong>>(lineCount);

            var operations = new List<Operations>(operationCount);
            var currentIndex = 0;

            while (operationLine.IsEmpty == false)
            {
                int opIndex = operationLine.IndexOfAny(OperationChars);
                if (opIndex == -1)
                {
                    break;
                }

                char opChar = operationLine[opIndex];
                operations.Add(opChar == '+' ? Operations.Add : Operations.Multiply);

                // Peek ahead to find the next digit to determine the column width
                var remainingLine = operationLine[(opIndex + 1)..];
                int nextOperationChar = remainingLine.IndexOfAny(OperationChars);

                int columnWidth;
                if (nextOperationChar == -1)
                {
                    columnWidth = remainingLine.Length + 1;
                }
                else
                {
                    columnWidth = nextOperationChar;
                }

                var numbers = new List<ulong>(operationCount);
                for (int i = 0; i < lineCount; i++)
                {
                    var line = numberLines[lineRanges[i]];
                    var startIndex = currentIndex + opIndex;
                    var segment = line.Slice(startIndex, columnWidth);
                    numbers.Add(ulong.Parse(segment));
                }

                result.Add(numbers);

                currentIndex += opIndex + 1;
                operationLine = remainingLine;
            }

            return (result, operations);
        }

        public ulong Solve((List<List<ulong>>, List<Operations> operations) input)
        {
            var (problems, operations) = input;
            ulong grandTotal = 0;

            // Single loop instead of nested loops
            for (int i = 0; i < problems.Count; i++)
            {
                var problem = problems[i];
                ulong result = problem[0];

                if (operations[i] == Operations.Multiply)
                {
                    for (int j = 1; j < problem.Count; j++)
                        result *= problem[j];
                }
                else
                {
                    for (int j = 1; j < problem.Count; j++)
                        result += problem[j];
                }

                grandTotal += result;
            }

            return grandTotal;
        }
    }

    internal partial class Part2
    {
        private readonly Example _example1 = new(
            """
            123 328  51 64 
             45 64  387 23 
              6 98  215 314
            *   +   *   +  
            """, 3263827ul);

        public (List<List<ulong>>, List<Operations> operations) Parse(string input)
        {
            var result = new List<List<ulong>>();
            var lines = input.AsSpan().TrimEnd('\n');
            var operationsLineIndex = lines.LastIndexOf('\n');
            var operationLine = lines[(operationsLineIndex + 1)..];
            var numberLines = lines[..operationsLineIndex].TrimEnd('\n');
            var operationCount = operationLine.CountAny(OperationChars);

            // Pre-split lines into ranges for efficient access without allocation
            Span<Range> lineRanges = stackalloc Range[32];
            int lineCount = numberLines.Split(lineRanges, '\n', StringSplitOptions.RemoveEmptyEntries);

            var operations = new List<Operations>(operationCount);
            var currentIndex = 0;

            // Stack allocate buffer once for all operations
            Span<char> digitBuffer = stackalloc char[32];

            while (operationLine.IsEmpty == false)
            {
                int opIndex = operationLine.IndexOfAny(OperationChars);
                if (opIndex == -1)
                {
                    break;
                }

                char opChar = operationLine[opIndex];
                operations.Add(opChar == '+' ? Operations.Add : Operations.Multiply);

                var remainingLine = operationLine[(opIndex + 1)..];
                int nextOperationChar = remainingLine.IndexOfAny(OperationChars);

                int columnWidth;
                if (nextOperationChar == -1)
                {
                    columnWidth = remainingLine.Length + 1;
                }
                else
                {
                    columnWidth = nextOperationChar;
                }

                // Read each column as a separate number (top to bottom)
                var numbers = new List<ulong>();
                var startIndex = currentIndex + opIndex;

                // Split the column into individual number columns
                for (int col = 0; col < columnWidth; col++)
                {
                    int digitCount = 0;
                    for (int i = 0; i < lineCount; i++)
                    {
                        var line = numberLines[lineRanges[i]];
                        int charIndex = startIndex + col;
                        if (charIndex < line.Length)
                        {
                            char c = line[charIndex];
                            if (char.IsDigit(c))
                            {
                                digitBuffer[digitCount++] = c;
                            }
                            else if (digitCount > 0)
                            {
                                // We've finished reading a number
                                break;
                            }
                        }
                    }

                    if (digitCount > 0)
                    {
                        numbers.Add(ulong.Parse(digitBuffer[..digitCount]));
                    }
                }

                result.Add(numbers);

                currentIndex += opIndex + 1;
                operationLine = remainingLine;
            }

            return (result, operations);
        }


        public ulong Solve((List<List<ulong>>, List<Operations> operations) input)
        {
            var (problems, operations) = input;
            ulong grandTotal = 0;

            for (int i = 0; i < problems.Count; i++)
            {
                var problem = problems[i];
                ulong result = problem[0];

                if (operations[i] == Operations.Multiply)
                {
                    for (int j = 1; j < problem.Count; j++)
                        result *= problem[j];
                }
                else
                {
                    for (int j = 1; j < problem.Count; j++)
                        result += problem[j];
                }

                grandTotal += result;
            }

            return grandTotal;
        }
    }
}
