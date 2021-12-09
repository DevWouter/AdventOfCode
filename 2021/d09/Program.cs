using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace d09
{
    class Program
    {
        static void Main(string[] args)
        {
            var heightMap = new HeightMap(File.ReadAllLines("input.txt"));
            var dangerValues = new List<Danger>();
            for (var y = 0; y < heightMap.Height; ++y)
            {
                for (var x = 0; x < heightMap.Width; ++x)
                {
                    var danger = heightMap.GetDanger(x, y);
                    if (danger == null) continue;
                    dangerValues.Add(new Danger { x = x, y = y, danger = danger.Value });
                }
            }
            Console.WriteLine("Part 1: " + dangerValues.Sum(x => x.danger));

            var basins = new List<Basin>();

            foreach (var danger in dangerValues)
            {
                var basin = heightMap.GetBasin(danger);
                basins.Add(basin);
            }

            var biggestBasins = new List<Basin>();
            foreach (var b in basins.OrderByDescending(x => x.locations.Count()))
            {
                var overlap = false;
                foreach (var bb in biggestBasins)
                {
                    overlap = bb.locations.Any(bbpos => b.locations.Any(bpos => bbpos.x == bpos.x && bbpos.y == bpos.y));
                    if (overlap) break;
                }
                if (!overlap)
                {
                    Console.WriteLine(b);
                    biggestBasins.Add(b);
                }

                if (biggestBasins.Count() >= 3) break;
            }

            var part2 = biggestBasins.OrderByDescending(x => x.locations.Count()).Take(3).Select(x => x.locations.Count()).Aggregate((p, c) => p * c);
            Console.WriteLine("Part 2: " + part2);


            for (var y = 0; y < heightMap.Height; ++y)
            {
                for (var x = 0; x < heightMap.Width; ++x)
                {
                    var partOfBasin = basins.SelectMany(x => x.locations).Any(o => o.x == x && o.y == y);
                    if (partOfBasin)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;


                    var height = heightMap.GetHeight(x, y);
                    if (height == 9)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.Write(height);
                    Console.ResetColor();
                }
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.ResetColor();
        }

        public record Vec2
        {
            public int x;
            public int y;
        }

        public record Basin : Vec2
        {
            public List<Vec2> locations;
        }

        public record Danger : Vec2
        {
            public int danger;
        }

        public class HeightMap
        {
            public int[][] Numbers;
            public int Height;
            public int Width;
            public HeightMap(string[] lines)
            {
                Numbers = lines.Select(x => x.ToCharArray().Select(x => x.ToString()).Select(int.Parse).ToArray()).ToArray();
                Height = Numbers.Length;
                Width = Numbers[0].Length;
            }

            public int? GetHeight(int x, int y)
            {
                if (x < 0 || y < 0) return null;
                if (x >= Width || y >= Height) return null;
                return Numbers[y][x];
            }



            public int? GetDanger(int x, int y)
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height) throw new Exception("Invalid coordinates");
                var ownHeight = GetHeight(x, y).Value;
                var heightLeft = GetHeight(x - 1, y);
                var heightRight = GetHeight(x + 1, y);
                var heightUp = GetHeight(x, y + 1);
                var heightDown = GetHeight(x, y - 1);
                if (heightLeft <= ownHeight) return null;
                if (heightRight <= ownHeight) return null;
                if (heightUp <= ownHeight) return null;
                if (heightDown <= ownHeight) return null;

                return ownHeight + 1;
            }

            internal Basin GetBasin(Vec2 pos)
            {
                List<Vec2> result = new List<Vec2>();
                Queue<Vec2> queue = new Queue<Vec2>(new[] { pos });
                while (queue.Any())
                {
                    var cpos = queue.Dequeue();
                    var x = cpos.x;
                    var y = cpos.y;
                    if (result.Any(o => o.x == x && o.y == y)) continue;
                    result.Add(cpos);
                    var ownHeight = GetHeight(x, y);
                    var heightLeft = GetHeight(x - 1, y);
                    var heightRight = GetHeight(x + 1, y);
                    var heightUp = GetHeight(x, y + 1);
                    var heightDown = GetHeight(x, y - 1);
                    if (heightLeft != 9 && heightLeft.HasValue)
                        queue.Enqueue(new Danger { x = x - 1, y = y, danger = heightLeft.Value });
                    if (heightRight != 9 && heightRight.HasValue)
                        queue.Enqueue(new Danger { x = x + 1, y = y, danger = heightRight.Value });
                    if (heightUp != 9 && heightUp.HasValue)
                        queue.Enqueue(new Danger { x = x, y = y + 1, danger = heightUp.Value });
                    if (heightDown != 9 && heightDown.HasValue)
                        queue.Enqueue(new Danger { x = x, y = y - 1, danger = heightDown.Value });
                }

                return new Basin() { x = pos.x, y = pos.y, locations = result };
            }
        }
    }
}
