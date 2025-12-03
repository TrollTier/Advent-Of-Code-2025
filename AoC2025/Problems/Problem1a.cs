using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Problems
{
    internal class Problem1a : IProblem
    {
        public async Task Run()
        {
            var data = await File.ReadAllLinesAsync("Data/1a.txt");

            var password = 0;
            var currentRotation = 50;

            foreach (var line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var rotation = line[0];
                var number = int.Parse(line.AsSpan().Slice(1));

                int addition = rotation switch
                {
                    'L' => 100 - number % 100,
                    _ => number
                };

                currentRotation = (currentRotation + addition) % 100;

                if (currentRotation == 0)
                {
                    password += 1;
                }
            }

            Console.WriteLine(password);
        }
    }
}
