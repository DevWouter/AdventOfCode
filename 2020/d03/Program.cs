using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace d03
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new Map("input.txt");
            var x1y1 = Travel(map, 1, 1);
            Console.WriteLine($"x1y1: {x1y1}");
            var x3y1 = Travel(map, 3, 1);
            Console.WriteLine($"x3y1: {x3y1}");
            var x5y1 = Travel(map, 5, 1);
            Console.WriteLine($"x5y1: {x5y1}");
            var x7y1 = Travel(map, 7, 1);
            Console.WriteLine($"x7y1: {x7y1}");
            var x1y2 = Travel(map, 1, 2);
            Console.WriteLine($"x1y2: {x1y2}");

            var r = x1y1.Trees * x3y1.Trees * x5y1.Trees * x7y1.Trees * x1y2.Trees;
            Console.WriteLine("AllTrees multiplied: " + r);


        }

        static Counter Travel(Map map, int dx, int dy)
        {
            var pos = new { x = 0, y = 0 };
            var result = new Counter();
            while (map.At(pos.x, pos.y) != null)
            {
                if (map.At(pos.x, pos.y) == MapSymbol.Open) result.Open++;
                if (map.At(pos.x, pos.y) == MapSymbol.Tree) result.Trees++;
                pos = new { x = pos.x + dx, y = pos.y + dy };
            }
            return result;
        }
    }

    class Counter
    {
        public Int64 Open { get; set; }
        public Int64 Trees { get; set; }
        public override string ToString()
        {
            return $"Spaces: {this.Open}, Trees: {this.Trees}";
        }
    }

    class Map
    {
        private MapSymbol[][] _tiles;
        private int _width;
        private int _height;
        public Map(string path)
        {
            var lines = File.ReadAllLines(path);
            _height = lines.Length;
            _width = lines[0].Length;
            _tiles = new MapSymbol[_height][];
            for (var y = 0; y < _height; ++y)
            {
                var line = lines[y];
                _tiles[y] = new MapSymbol[_width];
                for (var x = 0; x < _width; ++x)
                {
                    var c = line[x];
                    var symbol =
                        c == '.' ? MapSymbol.Open :
                        c == '#' ? MapSymbol.Tree :
                        throw new Exception($"Invalid character {c} at {y + 1}:{x + 1}")
                    ;

                    _tiles[y][x] = symbol;
                }
            }
        }

        public MapSymbol? At(int x, int y)
        {
            x = x % _width;
            if (y >= _height) return null;
            return _tiles[y][x];
        }
    }

    class MapTile
    {
        public MapSymbol Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    enum MapSymbol
    {
        Open,
        Tree
    }
}
