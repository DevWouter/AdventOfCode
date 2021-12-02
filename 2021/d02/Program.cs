using System;
using System.IO;
using System.Linq;

namespace d02
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = File.ReadAllLines("input.txt").Select(x => new SubmarineCommand(x)).ToArray();
            Part1(commands);
            Part2(commands);
        }

        private static void Part2(SubmarineCommand[] commands)
        {
            var horizontal = 0;
            var depth = 0;
            var aim = 0;

            foreach (var command in commands)
            {
                if (command.Direction == "forward")
                {
                    horizontal += command.Amount;
                    depth += aim * command.Amount;
                }
                else if (command.Direction == "down")
                {
                    aim += command.Amount;
                }
                else if (command.Direction == "up")
                {
                    aim -= command.Amount;
                }
                else
                {
                    throw new Exception("Unknown direction:" + command.Direction);
                }
            }

            Console.WriteLine($"Part 2: horizontal({horizontal}) * depth({depth}) = {horizontal * depth}");
        }

        private static void Part1(SubmarineCommand[] commands)
        {
            var horizontal = 0;
            var depth = 0;

            foreach (var command in commands)
            {
                if (command.Direction == "forward")
                {
                    horizontal += command.Amount;
                }
                else if (command.Direction == "down")
                {
                    depth += command.Amount;
                }
                else if (command.Direction == "up")
                {
                    depth -= command.Amount;
                }
                else
                {
                    throw new Exception("Unknown direction:" + command.Direction);
                }
            }

            Console.WriteLine($"Part 1: horizontal({horizontal}) * depth({depth}) = {horizontal * depth}");
        }
    }

    class SubmarineCommand
    {
        public string Direction { get; }
        public int Amount { get; }
        public SubmarineCommand(string src)
        {
            var splitted = src.Split(" ");
            Direction = splitted[0];
            Amount = int.Parse(splitted[1]);

        }
    }
}
