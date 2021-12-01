using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace d01
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File
                .ReadLines("input.txt")
                .Select(int.Parse)
                .ToArray();

            Part1(numbers);
            Part2(numbers);
        }

        private static void Part1(int[] numbers)
        {
            var lastDepth = numbers[0];
            var increaseMeasured = 0;
            for (var i = 0; i < numbers.Length; ++i)
            {
                var currentDepth = numbers[i];

                if (currentDepth > lastDepth) increaseMeasured++;

                lastDepth = currentDepth;
            }

            Console.WriteLine("Part 1: Increase measured: " + increaseMeasured);
        }

        private static void Part2(int[] numbers)
        {
            List<int[]> windows = new List<int[]>();
            for (var i = 0; i < (numbers.Length - 2); ++i)
            {
                var window = new[]{
                    numbers[i+0],
                    numbers[i+1],
                    numbers[i+2],
                };
                Console.WriteLine(numbers[i + 2]);
                windows.Add(window);
            }

            var lastDepth = windows[0].Sum();
            var increaseMeasured = 0;
            for (var i = 0; i < windows.Count; ++i)
            {
                var currentDepth = windows[i].Sum();

                if (currentDepth > lastDepth) increaseMeasured++;

                lastDepth = currentDepth;
            }

            Console.WriteLine("Part 2: Increase measured: " + increaseMeasured);
        }
    }
}
