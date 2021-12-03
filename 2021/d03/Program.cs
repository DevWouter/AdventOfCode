using System;
using System.Linq;
using System.IO;

namespace d03
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File
                .ReadAllLines("input.txt")
                .ToArray();
            var lineLength = lines.First().Length;
            Part1(lines, lineLength);
            Part2(lines, lineLength);
        }

        private static void Part2(string[] lines, int lineLength)
        {
            var oxygenRatings = lines;
            var co2Ratings = lines;

            for (var i = 0; i < lineLength; i++)
            {
                if (oxygenRatings.Length == 0) throw new Exception("No solution found");
                var grouped = oxygenRatings.GroupBy(x => x[i]);
                var amount0 = grouped.SingleOrDefault(x => x.Key == '0')?.Count() ?? 0;
                var amount1 = grouped.SingleOrDefault(x => x.Key == '1')?.Count() ?? 0;
                if (amount1 >= amount0)
                {
                    oxygenRatings = grouped.Single(x => x.Key == '1').ToArray();
                }
                else
                {
                    oxygenRatings = grouped.Single(x => x.Key == '0').ToArray();
                }
            }

            for (var i = 0; i < lineLength; i++)
            {
                if (co2Ratings.Length == 0) throw new Exception("No solution found");
                var grouped = co2Ratings.GroupBy(x => x[i]);
                var amount0 = grouped.SingleOrDefault(x => x.Key == '0')?.Count() ?? int.MaxValue;
                var amount1 = grouped.SingleOrDefault(x => x.Key == '1')?.Count() ?? int.MaxValue;
                if (amount0 <= amount1)
                {
                    co2Ratings = grouped.SingleOrDefault(x => x.Key == '0').ToArray();
                }
                else
                {
                    co2Ratings = grouped.SingleOrDefault(x => x.Key == '1').ToArray();
                }
            }

            string oxygenRatingBitString = oxygenRatings.Single();
            string co2RatingBitString = co2Ratings.Single();
            var oxygenRating = BinToInt(oxygenRatingBitString);
            var co2Rating = BinToInt(co2RatingBitString);
            var lifeSupport = oxygenRating * co2Rating;

            Console.WriteLine();
            Console.WriteLine("Part 2");
            Console.WriteLine(new string('=', Console.BufferWidth));
            Console.WriteLine($"oxygenRating (bits): {oxygenRatingBitString}");
            Console.WriteLine($"co2Rating (bits):    {co2RatingBitString}");
            Console.WriteLine($"oxygenRating (dec):  {oxygenRating}");
            Console.WriteLine($"co2Rating (dec):     {co2Rating}");
            Console.WriteLine($"lifeSupport (dec):   {lifeSupport}");

        }

        private static void Part1(string[] lines, int lineLength)
        {
            var gammaRateBitString = "";
            var epsilonRateBitString = "";
            for (var i = 0; i < lineLength; ++i)
            {
                var grouped = lines.GroupBy(x => x[i]);
                var amount0 = grouped.Single(x => x.Key == '0').Count();
                var amount1 = grouped.Single(x => x.Key == '1').Count();
                if (amount0 == amount1) throw new Exception($"No bit was more common or uncommon at index {i} ({amount0} == {amount1})");
                if (amount0 > amount1)
                {
                    gammaRateBitString += "0";
                    epsilonRateBitString += "1";
                }
                else
                {
                    gammaRateBitString += "1";
                    epsilonRateBitString += "0";
                }
            }

            var gammaRate = BinToInt(gammaRateBitString);
            var epsilonRate = BinToInt(epsilonRateBitString);


            Console.WriteLine();
            Console.WriteLine("Part 1");
            Console.WriteLine(new string('=', Console.BufferWidth));
            Console.WriteLine("Gamma rate (bits):        " + gammaRateBitString);
            Console.WriteLine("Epsilon rate (bits):      " + epsilonRateBitString);
            Console.WriteLine("Gamma rate (dec):         " + gammaRate);
            Console.WriteLine("Epsilon rate (dec):       " + epsilonRate);
            Console.WriteLine("Power (Gamma * Episolon): " + (gammaRate * epsilonRate));
        }

        private static int BinToInt(string source)
        {
            var result = 0;
            var length = source.Length - 1;
            for (var i = 0; i <= length; ++i)
            {
                if (source[i] == '1') result |= 1 << (length - i);
            }

            return result;
        }
    }
}
