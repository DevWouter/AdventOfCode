using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace d10
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var related = new Dictionary<char, char>{
                { '(', ')'},
                { '<', '>'},
                { '{', '}'},
                { '[', ']'},
            };

            var pointsLookup = new Dictionary<char, long>
            {
                { ')', 3},
                { ']', 57},
                { '}', 1197},
                { '>', 25137},
            };

            var errorList = new List<Error>();
            var incompleteList = new List<long>();

            for (var li = 0; li < lines.Length; ++li)
            {
                var line = lines[li];
                Stack<char> stack = new Stack<char>();
                var good = true;
                for (var i = 0; i < line.Length; ++i)
                {
                    var c = line[i];
                    if (related.Keys.Contains(c))
                        stack.Push(c);
                    else
                    {
                        var pc = stack.Pop();
                        if (related[pc] != c)
                        {
                            good = false;
                            errorList.Add(new Error
                            {
                                line = li,
                                column = i,
                                actual = c,
                                expected = pc,
                            });
                            break;
                        }
                    }
                }

                if (good && stack.Any())
                {
                    incompleteList.Add(Part2(stack));
                }
            }

            var p1Score = errorList.Select(x => x.actual).Select(x => pointsLookup[x]).Sum();
            Console.WriteLine("Part 1: " + p1Score);
            var p2Score = incompleteList.OrderBy(x => x).ToArray()[(int)Math.Floor(incompleteList.Count() / 2.0)];
            Console.WriteLine("Part 2: " + p2Score);

        }

        public static long Part2(Stack<char> stack)
        {
            long a = 0;
            while (stack.Count > 0)
            {
                var c = stack.Pop();
                a = a * 5;
                if (c == '(') a += 1;
                if (c == '[') a += 2;
                if (c == '{') a += 3;
                if (c == '<') a += 4;
            }

            return a;
        }

    }

    class Error
    {
        public int line { get; init; }
        public int column { get; init; }
        public char actual { get; init; }
        public char expected { get; init; }
    }
}
