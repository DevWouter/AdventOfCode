using System;
using System.Linq;
using System.IO;

namespace d06
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt")
                .Split(",")
                .Select(int.Parse)
                .ToArray();

            Console.WriteLine("Initial Count: " + input.Length);

            var simulator = new Simulator(input);
            long part1 = 0;
            long part2 = 0;
            for (var i = 1; i <= 256; ++i)
            {
                simulator.Step();
                long fishCount = simulator.GetFishCount();
                Console.WriteLine($"After {i,3} days: {fishCount}");
                switch(i){
                    case 80: part1 = fishCount; break;
                    case 256: part2 = fishCount; break;
                }
            }

            Console.WriteLine("Answer part 1: " + part1);
            Console.WriteLine("Answer part 2: " + part2);
        }
    }

    public class Simulator
    {
        private long[] AgeGroups;

        public Simulator(int[] ages)
        {
            AgeGroups = new long[9];
            for (var i = 0; i <= 8; ++i)
            {
                AgeGroups[i] = ages.Count(x => x == i);
            }
        }

        public long GetFishCount()
        {
            return AgeGroups.Sum();
        }

        public void Step()
        {
            var newAgeGroups = new long[9];
            // Changes related to age
            newAgeGroups[7] += AgeGroups[8];
            newAgeGroups[6] += AgeGroups[7];
            newAgeGroups[5] += AgeGroups[6];
            newAgeGroups[4] += AgeGroups[5];
            newAgeGroups[3] += AgeGroups[4];
            newAgeGroups[2] += AgeGroups[3];
            newAgeGroups[1] += AgeGroups[2];
            newAgeGroups[0] += AgeGroups[1];
            // Restart age cycle
            newAgeGroups[6] += AgeGroups[0];
            // Childs
            newAgeGroups[8] += AgeGroups[0];

            for (var i = 0; i < 9; ++i)
            {
                AgeGroups[i] = newAgeGroups[i];
            }
        }
    }
}
