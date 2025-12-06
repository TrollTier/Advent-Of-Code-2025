using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Problems
{
    internal class Problem6b : IProblem
    {
        private struct ProblemSet
        {
            public List<int> Numbers;
            public char Operator;

            public ProblemSet()
            {
                Numbers = new List<int>();
            }
        }

        public async Task Run()
        {
            var file = "Data/6a.txt";

            var columns = new List<List<char>>();
            
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    var line = (await reader.ReadLineAsync());

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (columns.Count < i + 1)
                        {
                            columns.Add(new List<char>());
                        }

                        columns[i].Add(line[i]);
                    }
                }
            }

            var problemSets = new List<ProblemSet>();
            var currentProblemSet = new ProblemSet();

            foreach (var data in columns)
            {
                var operatorField = data[data.Count - 1];

                if (operatorField != ' ')
                {
                    currentProblemSet = new ProblemSet { Operator = operatorField };
                }

                var currentExponent = 0;
                var number = 0;

                for (int i = data.Count - 1; i >= 0; i--)
                {
                    if (data[i] < 48)
                    {
                        continue;
                    }

                    var digit = (byte)(data[i] - 48) * (int)Math.Pow(10, currentExponent);
                    number += digit;
                    currentExponent += 1;
                }

                // Separator column
                if (number != 0)
                {
                    currentProblemSet.Numbers.Add(number);
                }
                else
                {
                    problemSets.Add(currentProblemSet);
                }
            }

            problemSets.Add(currentProblemSet);

            long sumOfAll = 0;

            foreach (var set in problemSets)
            {
                Func<long, int, long> operatorFunc = set.Operator == '+'
                    ? (a, b) => a + b
                    : (a, b) => a * b;

                long setTotal = set.Numbers[0];
                for (int i = 1; i < set.Numbers.Count; i++)
                {
                    setTotal = operatorFunc(setTotal, set.Numbers[i]);
                }

                sumOfAll += setTotal;
            }

            Console.WriteLine(sumOfAll);
        }
    }
}
