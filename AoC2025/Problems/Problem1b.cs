using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025
{
    internal class Problem1b : IProblem
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
                var rotationsToDo = int.Parse(line.AsSpan().Slice(1));

                int addition = rotation switch
                {
                    'L' => 99, // Additive inverse mod 100
                    _ => 1
                };

                var rotationsDone = 0;
                
                // Every 100 is a safe +1
                password += rotationsToDo / 100;

                // If we have i.e. 726 rotations our end value is the same as 26.
                // Just that we have rotated around the dial 700 times before.
                rotationsToDo %= 100;

                while (rotationsDone < rotationsToDo)
                {
                    currentRotation = (currentRotation + addition) % 100;

                    if (currentRotation == 0)
                    {
                        password += 1;
                    }
                    
                    rotationsDone += 1;
                }
            }

            Console.WriteLine(password);
        }
    }
}
