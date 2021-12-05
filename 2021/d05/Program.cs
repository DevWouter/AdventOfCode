using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace d05
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").Select(Line.Parse).ToArray();
            var points = lines.SelectMany(GetPoints).ToList();
            var unsafePoints = points.GroupBy(x => new { x.X, x.Y }).Where(x => x.Count() >= 2);
            Console.WriteLine("Part 1");
            Console.WriteLine($"Total points:  {points.Count(),8}");
            Console.WriteLine($"Unsafe points: {unsafePoints.Count(),8}");

            points.AddRange(lines.SelectMany(GetDiagonalPoints));
            unsafePoints = points.GroupBy(x => new { x.X, x.Y }).Where(x => x.Count() >= 2);
            Console.WriteLine("Part 2");
            Console.WriteLine($"Total points:  {points.Count(),8}");
            Console.WriteLine($"Unsafe points: {unsafePoints.Count(),8}");
        }

        static IEnumerable<Vec2> GetDiagonalPoints(Line line)
        {
            if (line.IsDiagonal)
            {
                var dx = line.V1.X < line.V2.X ? 1 : -1;
                var dy = line.V1.Y < line.V2.Y ? 1 : -1;

                var x = line.V1.X;
                var y = line.V1.Y;
                while (x != line.V2.X)
                {
                    yield return new Vec2 { X = x, Y = y };
                    x += dx;
                    y += dy;
                }

                yield return new Vec2 { X = x, Y = y };
            }
        }

        static IEnumerable<Vec2> GetPoints(Line line)
        {
            if (line.IsHorizontal)
            {

                var lowerX = Math.Min(line.V1.X, line.V2.X);
                var upperX = Math.Max(line.V1.X, line.V2.X);
                for (var x = lowerX; x <= upperX; ++x)
                {
                    yield return new Vec2 { X = x, Y = line.V1.Y };
                }
            }
            if (line.IsVertical)
            {
                var lowerY = Math.Min(line.V1.Y, line.V2.Y);
                var upperY = Math.Max(line.V1.Y, line.V2.Y);
                for (var y = lowerY; y <= upperY; ++y)
                {
                    yield return new Vec2 { X = line.V1.X, Y = y };
                }
            }
        }
    }

    public record Vec2
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Line
    {
        public Vec2 V1 { get; set; }
        public Vec2 V2 { get; set; }

        public override string ToString()
        {
            return $"{V1.X,4},{V1.Y,4} -> {V2.X,4},{V2.Y,4}";
        }

        public bool IsDiagonal => Math.Abs(V2.X - V1.X) == Math.Abs(V2.Y - V1.Y);
        public bool IsHorizontal => V1.Y == V2.Y;
        public bool IsVertical => V1.X == V2.X;

        public static Line Parse(string src)
        {
            var regex = new Regex(@"^(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)");
            var result = regex.Match(src);
            var x1 = int.Parse(result.Groups["x1"].Value);
            var y1 = int.Parse(result.Groups["y1"].Value);
            var x2 = int.Parse(result.Groups["x2"].Value);
            var y2 = int.Parse(result.Groups["y2"].Value);

            return new Line
            {
                V1 = new Vec2() { X = x1, Y = y1 },
                V2 = new Vec2() { X = x2, Y = y2 },
            };
        }
    }
}
