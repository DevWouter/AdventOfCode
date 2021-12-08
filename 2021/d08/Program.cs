using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace d08
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").Select(Line.Parse).ToArray();

            var ones = lines.SelectMany(x => x.Outputs)
            .Where(x => x.NumericValue == 1 ||
            x.NumericValue == 4 ||
            x.NumericValue == 7 ||
            x.NumericValue == 8)
            ;

            Console.WriteLine("Part 1: " + ones.Count());
            Console.WriteLine("Part 2: " + lines.Sum(x => x.Output));
        }
    }

    public class SignalPattern
    {
        public Line Line { get; set; }
        public string Signal { get; set; }
        public SignalPattern(Line line, string signal)
        {
            Line = line;
            Signal = signal;
        }

        public int NumericValue => Line.Lookup[Signal];
    }

    public class Line
    {
        public Dictionary<string, int> Lookup { get; set; } = new();
        public SignalPattern[] Outputs { get; set; }
        public int Output =>
            Outputs[0].NumericValue * 1000 +
            Outputs[1].NumericValue * 100 +
            Outputs[2].NumericValue * 10 +
            Outputs[3].NumericValue * 1;

        public static Line Parse(string source)
        {
            var split = source.Split(" | ");
            Line line = new();
            line.Outputs = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SortCharacters)
                .Select(x => new SignalPattern(line, x))
                .ToArray();
            var inputs = split[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SortCharacters).ToArray();
            line.Solve(inputs);
            return line;
        }

        private static string SortCharacters(string x)
        {
            return string.Join("", x.OrderBy(y => y));
        }

        public void Solve(string[] inputs)
        {
            var remainingPatterns = inputs.ToList();

            var n1 = inputs.Single(x => x.Length == 2);
            var n7 = inputs.Single(x => x.Length == 3);
            var n4 = inputs.Single(x => x.Length == 4);
            var n8 = inputs.Single(x => x.Length == 7);

            var n0or6or9 = inputs.Where(x => x.Length == 6);
            var n2or3or5 = inputs.Where(x => x.Length == 5);
            var n9 = n0or6or9.Single(x => x.Intersect(n4).Count() == 4);
            var n3 = n2or3or5.Single(x => x.Intersect(n7).Count() == 3);

            var n0or6 = n0or6or9.Where(x => x != n9);
            var n2or5 = n2or3or5.Where(x => x != n3);
            var n0 = n0or6.Single(x => x.Intersect(n1).Count() == 2);
            var n5 = n2or5.Single(x => x.Contains(n4.Except(n3).Single()));

            var n6 = n0or6.Single(x => x != n0);
            var n2 = n2or5.Single(x => x != n5);

            Lookup[n0] = 0;
            Lookup[n1] = 1;
            Lookup[n2] = 2;
            Lookup[n3] = 3;
            Lookup[n4] = 4;
            Lookup[n5] = 5;
            Lookup[n6] = 6;
            Lookup[n7] = 7;
            Lookup[n8] = 8;
            Lookup[n9] = 9;

        }
    }}
