namespace AoC2025.Problems
{
    internal class Problem5a : IProblem
    {
        private readonly record struct FreshRange(long Start, long End);

        public async Task Run()
        {
            var dataFile = "Data/5a.txt";
            var freshAvailableIngredients = 0;
            
            using (var reader = new StreamReader(dataFile)) 
            {
                var freshRanges = await FillRangesAsync(reader);

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();    
                        
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var id = long.Parse(line);

                    if (freshRanges.Any(range => range.Start <= id && range.End >= id))
                    {
                        freshAvailableIngredients += 1;
                    }
                }
            }

            Console.WriteLine(freshAvailableIngredients);
        }

        private async Task<List<FreshRange>> FillRangesAsync(StreamReader reader)
        {
            var ranges = new List<FreshRange>();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                {
                    return ranges;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    // In this problem we only need the fresh ids
                    break;
                }

                var (start, end) = ParseLine(line);
                ranges.Add(new FreshRange(start, end));
            }

            return ranges;
        }

        private (long start, long end) ParseLine(string line)
        {
            var separatorIndex = line.IndexOf('-');

            if (separatorIndex == -1)
            {
                var id = int.Parse(line);
                return (id, id);
            }
            else
            {
                var rangeStart = long.Parse(line.AsSpan().Slice(0, separatorIndex));
                var rangeEnd = long.Parse(line.AsSpan().Slice(separatorIndex + 1));

                return (rangeStart, rangeEnd);
            }
        }
    }
}
