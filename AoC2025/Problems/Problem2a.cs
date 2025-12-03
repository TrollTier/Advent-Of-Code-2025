using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Problems
{
    internal class Problem2a : IProblem
    {
        public async Task Run()
        {
            var data = await File.ReadAllTextAsync("Data/2a.txt");

            var asSpan = data.AsSpan();

            var nextComma = asSpan.IndexOf(','); 
            var lastComma = -1;

            long sumOfInvalidIds = 0;
            var digits = new List<int>();

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

        private long GetSumOfInvalidIdsInRange(ReadOnlySpan<char> currentSpan, List<int> digits)
        {
            var separator = currentSpan.IndexOf('-');

            var rangeStart = long.Parse(currentSpan.Slice(0, separator));
            var rangeEnd = long.Parse(currentSpan.Slice(separator + 1, currentSpan.Length - separator - 1));

            long sumOfInvalidIds = 0;

            for (long checkedNumber = rangeStart; checkedNumber <= rangeEnd; checkedNumber++)
            {
                FillDigits(checkedNumber, digits);

                // Numbers with an uneven amount of digits can not be 
                // created by a repeated pattern
                if (digits.Count % 2 != 0)
                {
                    continue;
                }

                var isInvalidId = true;
                var middle = digits.Count / 2;

                // 123 123
                // middle = 3
                // n1 = 0-2
                // n2 = 3-5
                for (int i = 0; i < middle && isInvalidId; i++)
                {
                    isInvalidId = digits[i] == digits[i + middle];
                }

                if (isInvalidId)
                {
                    sumOfInvalidIds += checkedNumber;
                }
            }

            return sumOfInvalidIds;
        }

        private void FillDigits(long number, List<int> digits)
        {
            digits.Clear();

            while (number > 0)
            {
                digits.Add((int)(number % 10));
                number /= 10;
            }
        }
    }
}
