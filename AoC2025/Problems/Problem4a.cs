namespace AoC2025.Problems
{
    internal class Problem4a : IProblem
    {
        private sealed record Scenario(string File, int Columns, int Rows);

        private static readonly Scenario[] s_scenarios = 
        [
            new(Path.Combine("Data", "TestData4a.txt"), 10, 10),
            new(Path.Combine("Data", "4a.txt"), 137, 137)
        ];

        public async Task Run()
        {
            var scenario = s_scenarios[1];
            var paperConfiguration = await GetPaperConfiguration(scenario);

            var liftablePaperRolls = 0;

            for (int row = 0; row < scenario.Rows; row++)
            {
                for (int column = 0; column < scenario.Columns; column++)
                {
                    if (GetValueAt(scenario, paperConfiguration, row, column) != 1)
                    {
                        continue;
                    }

                    var sum =
                        GetValueAt(scenario, paperConfiguration, row - 1, column - 1) +
                        GetValueAt(scenario, paperConfiguration, row - 1, column) +
                        GetValueAt(scenario, paperConfiguration, row - 1, column + 1) +
                        GetValueAt(scenario, paperConfiguration, row, column - 1) +
                        GetValueAt(scenario, paperConfiguration, row, column + 1) +
                        GetValueAt(scenario, paperConfiguration, row + 1, column - 1) +
                        GetValueAt(scenario, paperConfiguration, row + 1, column) +
                        GetValueAt(scenario, paperConfiguration, row + 1, column + 1);

                    if (sum < 4)
                    {
                        liftablePaperRolls += 1;
                    }
                }
            }

            Console.WriteLine(liftablePaperRolls);
        }

        private static async Task<byte[]> GetPaperConfiguration(Scenario scenario)
        {
            var paperConfiguration = new byte[scenario.Columns * scenario.Rows];

            using (var reader = new StreamReader(scenario.File))
            {
                var row = 0;

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    for (int column = 0; column < scenario.Columns; column++)
                    {
                        paperConfiguration[row * scenario.Columns + column] = line[column] switch
                        {
                            '@' => 1,
                            _ => 0
                        };
                    }

                    row += 1;
                }
            }

            return paperConfiguration;
        }

        private static byte GetValueAt(Scenario scenario, byte[] paperConfiguration, int row, int column)
        {
            if (row < 0 || row >= scenario.Rows || column < 0 || column >= scenario.Columns)
            {
                return 0;
            }

            return paperConfiguration[row *  scenario.Columns + column];    
        }
    }
}
