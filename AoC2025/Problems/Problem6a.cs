using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Problems
{
    internal class Problem6a : IProblem
    {
        public async Task Run()
        {
            var file = "Data/6a.txt";

            var columnData = new List<List<int>>();
            var operations = new List<char>();

            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    var line = (await reader.ReadLineAsync());

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var currentColumn = 0;
                    var rangeStart = 0;
                    var rangeEnd = 0;

                    while (rangeStart < line.Length && rangeEnd < line.Length)
                    {
                        while (rangeStart < line.Length && line[rangeStart] == ' ')
                        {
                            rangeStart++;
                        }

                        rangeEnd = rangeStart + 1;

                        while (rangeEnd < line.Length && line[rangeEnd] != ' ')
                        {
                            rangeEnd++;
                        }

                        if (rangeStart >= line.Length)
                        {
                            break;
                        }

                        var characters = line.AsSpan().Slice(rangeStart, rangeEnd - rangeStart);

                        if (characters.Length == 1 && characters[0] == '*' || characters[0] == '+')
                        {
                            operations.Add(characters[0]);
                        }
                        else
                        {
                            if (columnData.Count < currentColumn + 1)
                            {
                                columnData.Add(new List<int>());
                            }

                            var number = int.Parse(characters);
                            columnData[currentColumn].Add(number);
                        }

                        rangeStart = rangeEnd;
                        currentColumn += 1;
                    }
                }
            }

            long sumOfAll = 0;

            for (int i = 0; i < columnData.Count; i++)
            {
                Func<long, int, long> operatorFunction = operations[i] == '*'
                    ? (a, b) => a * b
                    : (a, b) => a + b;

                var columnValues = columnData[i];
                long columnTotal = columnValues[0];
                
                for (int k = 1; k < columnValues.Count; k++)
                {
                    columnTotal = operatorFunction(columnTotal, columnValues[k]);
                }                

                sumOfAll += columnTotal;
            }

            Console.WriteLine(sumOfAll);
        }
    }
}
