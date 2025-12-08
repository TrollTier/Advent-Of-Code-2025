using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Problems
{
    internal class Problem8b : IProblem
    {
        private record JunctionBox(int X, int Y, int Z)
        {
            public int CircuitIndex = 0;

            public long SquaredDistance(JunctionBox other)
            {
                long xDiff = other.X - X;
                long yDiff = other.Y - Y;
                long zDiff = other.Z - Z;

                return (xDiff * xDiff) + (yDiff * yDiff) + (zDiff * zDiff);
            }
        }

        private readonly struct Circuit
        {
            public readonly List<JunctionBox> Boxes = new List<JunctionBox>();

            public Circuit()
            {
            }
        }

        private readonly record struct BoxPair(
            JunctionBox A,
            JunctionBox B, 
            long SquaredDistance);

        public async Task Run()
        {
            var dataFile = "Data/8a.txt";

            var (junctionBoxes, circuits) = await ParseDataFileAsync(dataFile);

            var pairs = new List<BoxPair>();
            
            for (int i = 0; i < junctionBoxes.Count - 1; i++)
            {
                for (int k = i + 1; k < junctionBoxes.Count; k++)
                {
                    var boxA = junctionBoxes[i];
                    var boxB = junctionBoxes[k];

                    pairs.Add(new BoxPair(boxA, boxB, boxA.SquaredDistance(boxB)));
                }
            }

            pairs.Sort((a, b) => a.SquaredDistance.CompareTo(b.SquaredDistance));

            var allCombined = false;
            BoxPair lastUsedPair = pairs[0];

            for (int i = 0; i < pairs.Count && !allCombined; i++)
            {
                var pair = pairs[i];

                if (pair.A.CircuitIndex == pair.B.CircuitIndex)
                {
                    continue;
                }

                var circuitA = circuits[pair.A.CircuitIndex];
                var circuitB = circuits[pair.B.CircuitIndex];
                var circuitBIndex = pair.B.CircuitIndex;

                circuitA.Boxes.AddRange(circuitB.Boxes);
                
                foreach (var box in circuitB.Boxes)
                {
                    box.CircuitIndex = pair.A.CircuitIndex;
                }

                circuitB.Boxes.Clear();

                allCombined = circuitA.Boxes.Count == junctionBoxes.Count;
                lastUsedPair = pair;
            }

            Console.WriteLine((long)lastUsedPair.A.X * (long)lastUsedPair.B.X);
            Console.ReadLine();
        }

        private static async Task<(List<JunctionBox> JunctionBoxes, List<Circuit> Circuits)>
            ParseDataFileAsync(string dataFile)
        {
            var junctionBoxes = new List<JunctionBox>();
            var circuits = new List<Circuit>();

            using (var reader = new StreamReader(dataFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var lineSpan = line.AsSpan();
                    var xySplit = lineSpan.IndexOf(',');
                    var xSpan = lineSpan.Slice(0, xySplit);

                    var yzSpan = lineSpan.Slice(xySplit + 1);
                    var yzSplit = yzSpan.IndexOf(',');
                    var ySpan = yzSpan.Slice(0, yzSplit);
                    var zSpan = yzSpan.Slice(yzSplit + 1);

                    var box = new JunctionBox(int.Parse(xSpan), int.Parse(ySpan), int.Parse(zSpan)) { CircuitIndex = circuits.Count };
                    junctionBoxes.Add(box);

                    var circuit = new Circuit();
                    circuit.Boxes.Add(box);
                    circuits.Add(circuit);
                }
            }

            return (junctionBoxes, circuits);
        }
    }
}
