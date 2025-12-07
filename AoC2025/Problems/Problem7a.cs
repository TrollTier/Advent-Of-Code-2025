using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Problems
{
    internal class Problem7a : IProblem
    {
        private enum FieldState
        {
            FreeToMove,
            TrachyonBeam,
            Splitter
        }

        private struct TrachyonBeam
        {
            public int Row;
            public int Column;
        }

        public async Task Run()
        {
            var dataFile = "Data/7a.txt";
            var lines = await File.ReadAllLinesAsync(dataFile);

            var fields = new FieldState[lines.Length, lines[0].Length];
            var beams = new Queue<TrachyonBeam>();
            
            for (int row = 0; row < lines.Length; row++) 
            {
                var line = lines[row];

                for (int column = 0; column < line.Length; column++)
                {
                    switch (line[column])
                    {
                        case 'S':
                            fields[row, column] = FieldState.TrachyonBeam;
                            beams.Enqueue(new TrachyonBeam { Row = row, Column = column });
                            break;

                        case '^':
                            fields[row, column] = FieldState.Splitter;
                            break;
                    }
                }
            }

            var splitTimes = 0;
            var splitColumns = new int[] { -1, 1 };

            while (beams.Count > 0)
            {
                var beam = beams.Dequeue();

                if (beam.Row == lines.Length - 1)
                {
                    continue;
                }

                var field = fields[beam.Row + 1, beam.Column];

                if (field != FieldState.Splitter)
                {
                    beam.Row += 1;
                    fields[beam.Row, beam.Column] = FieldState.TrachyonBeam;

                    beams.Enqueue(beam);
                    continue;
                }

                var hasSplit = false;

                foreach (var splitDir in splitColumns)
                {
                    var column = beam.Column + splitDir;

                    if (column < 0 || column >= fields.GetLength(1) || fields[beam.Row + 1, column] == FieldState.TrachyonBeam)
                    {
                        continue;
                    }

                    fields[beam.Row + 1, column] = FieldState.TrachyonBeam;
                    beams.Enqueue(new TrachyonBeam { Row = beam.Row + 1, Column = column });
                    hasSplit = true;
                }

                if (hasSplit)
                {
                    splitTimes += 1;
                }
            }

            Console.WriteLine(splitTimes);
        }
    }
}
