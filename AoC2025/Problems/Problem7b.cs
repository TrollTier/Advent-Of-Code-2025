namespace AoC2025.Problems
{
    internal class Problem7b : IProblem
    {
        private sealed class SplitterNode
        {
            public int Column { get; init; }
            public int Row { get; init; }
            public long PossiblePathsCount { get; set; }
            public List<SplitterNode> NextSplitters { get; } = new List<SplitterNode>();
            public List<SplitterNode> PreviousSplitters { get; } = new List<SplitterNode>();
        }

        public async Task Run()
        {
            var dataFile = "Data/7a.txt";
            var lines = await File.ReadAllLinesAsync(dataFile);

            var splittersByRow = new Dictionary<int, List<SplitterNode>>();
            var splitters = new List<SplitterNode>();

            for (int row = 0; row < lines.Length; row++) 
            {
                var line = lines[row];

                if (!splittersByRow.ContainsKey(row))
                {
                    splittersByRow.Add(row, new List<SplitterNode>());
                }

                for (int column = 0; column < line.Length; column++)
                {
                    if (line[column] == '^')
                    {
                        var splitter = new SplitterNode { Column = column, Row = row };

                        var blockingSplitter = false;
                        var parentRow = row - 1;

                        while (!blockingSplitter && parentRow >= 0)
                        {
                            foreach (var possibleParentSplitter in splittersByRow[parentRow])
                            {
                                if (possibleParentSplitter.Column == column)
                                {
                                    blockingSplitter = true;
                                    break;
                                }

                                if (possibleParentSplitter.Column == column - 1)
                                {
                                    if (!possibleParentSplitter.NextSplitters.Any(ns => ns.Column == column))
                                    {
                                        possibleParentSplitter.NextSplitters.Add(splitter);
                                        splitter.PreviousSplitters.Add(possibleParentSplitter);
                                    }
                                }
                                else if (possibleParentSplitter.Column == column + 1)
                                {
                                    if (!possibleParentSplitter.NextSplitters.Any(ns => ns.Column == column))
                                    {
                                        possibleParentSplitter.NextSplitters.Add(splitter);
                                        splitter.PreviousSplitters.Add(possibleParentSplitter);
                                    }
                                
                                }
                            }

                            parentRow--;
                        }

                        splitters.Add(splitter);
                        splittersByRow[row].Add(splitter);
                    }
                }
            }

            // Sum up everything breath-first bottom to top.
            var leafSplitters = splitters.Where(s => s.NextSplitters.Count == 0);
            var currentSplitters = new List<SplitterNode>(leafSplitters);
            var nextEvaluated = new HashSet<SplitterNode>(currentSplitters.Count);

            while (currentSplitters.Count > 0)
            {
                foreach (var splitter in currentSplitters)
                {
                    splitter.PossiblePathsCount =
                        (2 - splitter.NextSplitters.Count) +
                        splitter.NextSplitters.Sum(ns => ns.PossiblePathsCount);

                    foreach (var next in splitter.PreviousSplitters)
                    {
                        nextEvaluated.Add(next);
                    }
                }

                currentSplitters.Clear();
                currentSplitters.AddRange(nextEvaluated);
                nextEvaluated.Clear();
            }

            var rootNode = splitters.Single(r => r.Row == 2);
            Console.WriteLine(rootNode.PossiblePathsCount);
            Console.ReadLine();
        }
    }
}
