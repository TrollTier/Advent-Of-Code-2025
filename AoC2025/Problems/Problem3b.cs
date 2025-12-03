using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Problems
{
    internal class Problem3b : IProblem
    {
        public async Task Run()
        {
            var data = await File.ReadAllLinesAsync("Data/3a.txt");

            List<byte> digits = new List<byte>(100);

            long maxJoltageInSum = 0;
            Span<byte> maxBatteryJoltages = stackalloc byte[12];
            Span<byte> usedBatteryJoltages = stackalloc byte[12];

            foreach (var line in data)
            {
                var asSpan = line.AsSpan();
                digits.Clear();
                maxBatteryJoltages.Clear();

                for (int i = 0; i < asSpan.Length; i++)
                {
                    digits.Add(ToByte(asSpan[i]));
                }

                // lets say we have 14 digits
                // digits.count = 14
                // possible index range = 0 - 13
                // digits.count - 12 = 2
                // 12 digits = 0-11 indices
                // So max index for first position = 2
                // Then max position for second place = 3 and so on

                for (int i = 0; i < digits.Count - 12; i++)
                {
                    usedBatteryJoltages.Clear();

                    var currentPlace = 0;
                    var lastUsedIndex = i - 1;
                    var isMax = true;

                    while (currentPlace < 12 && isMax)
                    {
                        var maxIndex = (asSpan.Length - 12) + currentPlace;
                        byte maxForPlace = 0;
                        int maxForPlaceIndex = 0;
                        
                        for (int k = lastUsedIndex + 1; k <= maxIndex; k++)
                        {
                            if (maxForPlace < digits[k])
                            {
                                maxForPlace = digits[k];
                                maxForPlaceIndex = k;
                            }
                        }

                        usedBatteryJoltages[currentPlace] = maxForPlace;
                        lastUsedIndex = maxForPlaceIndex;
                        isMax = maxBatteryJoltages[currentPlace] <= maxForPlace;

                        currentPlace += 1;
                    }

                    if (isMax)
                    {
                        usedBatteryJoltages.CopyTo(maxBatteryJoltages);
                    }
                }

                for (int i = 0; i < maxBatteryJoltages.Length; i++)
                {
                    maxJoltageInSum += maxBatteryJoltages[i] * (long)Math.Pow(10, 11 - i);
                }
            }

            Console.WriteLine(maxJoltageInSum);
        }

        private static byte ToByte(char input)
        {
            return (byte)((byte)input - 48);
        }
    }
}
