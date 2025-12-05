namespace AoC2025.Problems
{
    internal class Problem5b : IProblem
    {
        private readonly record struct FreshRange(long Start, long End)
        {
            public override string ToString()
            {
                return $"{Start} - {End}";
            }
        }

        public async Task Run()
        {
            var dataFile = "Data/5a.txt";
            
            List<FreshRange> freshRanges;
            using (var reader = new StreamReader(dataFile)) 
            {
                freshRanges = await FillRangesAsync(reader);
            }

            long freshIdsCount = 0;
            var uniqueRanges = GetMergedRanges(freshRanges);

            // Since we sometimes replace ranges that are completely covered by other ranges
            // those can then overlap with others that have been merged at some point. 
            // So we clean that up here.
            while (HasOverlaps(uniqueRanges))
            {
                uniqueRanges = GetMergedRanges(uniqueRanges);
            }

            foreach (var range in uniqueRanges)
            {
                freshIdsCount += ((range.End - range.Start) + 1);
            }

            Console.WriteLine(freshIdsCount);
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

        private List<FreshRange> GetMergedRanges(List<FreshRange> freshRanges)
        {
            var usedFreshIdRanges = new List<FreshRange>();

            foreach (var currentRange in freshRanges)
            {
                var alreadyExists = false;
                var currentStart = currentRange.Start;
                var currentEnd = currentRange.End;

                for (int i = 0; i < usedFreshIdRanges.Count && !alreadyExists; i++)
                {
                    var existingRange = usedFreshIdRanges[i];

                    // Existing range completely covers the current range
                    if (existingRange.Start <= currentStart && existingRange.End >= currentEnd)
                    {
                        alreadyExists = true;
                        break;
                    }

                    // Current range completely covers existing range and can replace it
                    if (currentStart <= existingRange.Start && currentEnd >= existingRange.End)
                    {
                        usedFreshIdRanges[i] = currentRange;
                        alreadyExists = true;
                        break;
                    }

                    if (currentEnd == existingRange.Start)
                    {
                        currentEnd -= 1;
                    }

                    if (currentStart == existingRange.End)
                    {
                        currentStart += 1;
                    }

                    // No overlap
                    if (currentStart > existingRange.End || currentEnd < existingRange.Start)
                    {
                        continue;
                    }

                    // At this point we know, we have overlap, but not enough, that range completely covers
                    // the other range. 
                    // Therefore it's either that currentStart > existingRange.Start or
                    // currentEnd >= existingRange.End but never both.
                    if (currentStart > existingRange.Start)
                    {
                        currentStart = existingRange.End + 1;
                    }
                    else 
                    {
                        currentEnd = existingRange.Start - 1;
                    }
                }

                if (!alreadyExists)
                {
                    usedFreshIdRanges.Add(new FreshRange(currentStart, currentEnd));
                }
            }

            return usedFreshIdRanges;
        }

        private bool HasOverlaps(List<FreshRange> usedFreshIdRanges)
        {
            var overlaps = 0;

            for (int i = 0; i < usedFreshIdRanges.Count; i++)
            {
                var range = usedFreshIdRanges[i];

                for (int j = i + 1; j < usedFreshIdRanges.Count; j++)
                {
                    var checkedRange = usedFreshIdRanges[j];

                    if (!(range.End < checkedRange.Start || range.Start > checkedRange.End))
                    {
                        overlaps += 1;
                    }
                }
            }

            return overlaps > 0;
        }
    }
}
