using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Problems
{
    internal class Problem3a : IProblem
    {
        public async Task Run()
        {
            var data = await File.ReadAllLinesAsync("Data/3a.txt");

            var maxJoltageInSum = 0;
            List<byte> digits = new List<byte>(100);

            foreach (var line in data)
            {
                var asSpan = line.AsSpan();
                digits.Clear();

                for (int i = 0; i < asSpan.Length; i++)
                {
                    digits.Add(ToByte(asSpan[i]));
                }

                var maxJoltageInLine = 0;
                
                for (int i = 0; i < digits.Count - 1; i++)
                {
                    // i.e. something in the 80s will never beat something in the 90s
                    if (maxJoltageInLine / 10 > digits[i])
                    {
                        continue;
                    }

                    var currentJoltageStart = digits[i] * 10;

                    for (int k = i + 1; k < digits.Count; k++)
                    {
                        var currentJoltage = currentJoltageStart + digits[k];

                        if (currentJoltage > maxJoltageInLine)
                        {
                            maxJoltageInLine = currentJoltage;
                        }
                    }
                }

                maxJoltageInSum += maxJoltageInLine;
            }

            Console.WriteLine(maxJoltageInSum);
        }

        private static byte ToByte(char input)
        {
            return (byte)((byte)input - 48);
        }
    }
}
