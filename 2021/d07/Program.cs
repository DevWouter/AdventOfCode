using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace d07
{
    public record FuelCostLine(int pos, int cost);
    class Program
    {
        static void Main(string[] args)
        {
            var positions = File.ReadAllText("input.txt")
                .Split(",").Select(int.Parse).ToArray();

            // Brute force the attempt
            List<FuelCostLine> part1fuelCostTable = new();
            for (var i = positions.Min(); i < positions.Max(); ++i)
            {
                part1fuelCostTable.Add(Part1FuelCost(i, positions));
            }

            var part1 = part1fuelCostTable.OrderBy(x => x.cost).First();
            Console.WriteLine($"Part 1: {part1.cost}");

            List<FuelCostLine> part2fuelCostTable = new();
            for (var i = positions.Min(); i < positions.Max(); ++i)
            {
                part2fuelCostTable.Add(Part2FuelCost(i, positions));
            }

            var part2 = part2fuelCostTable.OrderBy(x => x.cost).First();
            Console.WriteLine($"Part 2: {part2.cost}");
        }

        private static FuelCostLine Part1FuelCost(int i, int[] positions)
        {
            return new FuelCostLine(i, positions.Select(x => Math.Abs(x - i)).Sum());
        }

        private static FuelCostLine Part2FuelCost(int i, int[] positions)
        {
            return new FuelCostLine(i, positions.Select(x => FuelCostP2(i, x)).Sum());
        }

        private static int FuelCostP2(int start, int target)
        {
            var distance = Math.Abs(target - start);
            var result = 0;

            // Ugh, I know this can be done faster.
            for (var i = 0; i <= distance; i++)
            {
                result += i;
            }

            return result;
        }
    }
}
