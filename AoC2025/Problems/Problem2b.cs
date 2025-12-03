namespace AoC2025.Problems
{
    internal class Problem2b : IProblem
    {
        public async Task Run()
        {
            var data = await File.ReadAllTextAsync("Data/2a.txt");

            var asSpan = data.AsSpan();

            var nextComma = asSpan.IndexOf(','); 
            var lastComma = -1;

            long sumOfInvalidIds = 0;
            var digits = new List<byte>();

            while (nextComma > 0)
            {
                var currentSpan = asSpan.Slice(0, nextComma);
                sumOfInvalidIds += GetSumOfInvalidIdsInRange(currentSpan, digits);

                lastComma = nextComma;

                asSpan = asSpan.Slice(nextComma + 1);
                nextComma = asSpan.IndexOf(",");
            } while (nextComma > 0);

            // Left over span that is not at the of the separators
            sumOfInvalidIds += GetSumOfInvalidIdsInRange(asSpan, digits);

            Console.WriteLine(sumOfInvalidIds);
        }

        private long GetSumOfInvalidIdsInRange(ReadOnlySpan<char> currentSpan, List<byte> digits)
        {
            var separator = currentSpan.IndexOf('-');

            var rangeStart = long.Parse(currentSpan.Slice(0, separator));
            var rangeEnd = long.Parse(currentSpan.Slice(separator + 1, currentSpan.Length - separator - 1));

            long sumOfInvalidIds = 0;
            
            for (long checkedNumber = rangeStart; checkedNumber <= rangeEnd; checkedNumber++)
            {
                FillDigits(checkedNumber, digits);

                if (IsInvalidId(digits))
                {
                    sumOfInvalidIds += checkedNumber;
                }
            }

            return sumOfInvalidIds;
        }

        private static void FillDigits(long number, List<byte> digits)
        {
            digits.Clear();

            while (number > 0)
            {
                digits.Add((byte)(number % 10));
                number /= 10;
            }

            digits.Reverse();
        }

        private static bool IsInvalidId(List<byte> digits)
        {
            var maxPatternSize = digits.Count / 2;

            for (int patternSize = 1; patternSize <= maxPatternSize; patternSize++)
            {
                if (digits.Count % patternSize != 0)
                {
                    continue;
                }

                var isInvalidPattern = true;

                for (int i = patternSize; i < digits.Count && isInvalidPattern; i++)
                {
                    isInvalidPattern = digits[i] == digits[i - patternSize];
                }

                if (isInvalidPattern)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
