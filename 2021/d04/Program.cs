using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace d04
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = new ParsedFile("input.txt");
            var remainingBoards = file.Boards.ToArray();
            var foundWinningBoard = false;

            foreach (var n in file.Numbers)
            {
                remainingBoards = remainingBoards.Where(x => !x.HasMarkedRowsOrColumns()).ToArray();
                if (remainingBoards.Length == 1)
                {
                    var lastBoard = remainingBoards.Single();
                    lastBoard.Mark(n);
                    if (lastBoard.HasMarkedRowsOrColumns())
                    {
                        Console.WriteLine("Part 2");
                        Console.WriteLine(new string('=', Console.BufferWidth));
                        Console.WriteLine("Last number: " + n);

                        foreach (var board in remainingBoards)
                        {
                            board.Print();
                            var values = board.GetUnmarkedNumbers();
                            var sum = values.Sum();
                            var score = sum * n;
                            Console.WriteLine($"Sum ({sum}) * n ({n}) = {sum * n}");
                        }
                        break;
                    }

                }
                foreach (var board in file.Boards)
                {
                    board.Mark(n);
                }

                if (!foundWinningBoard)
                {
                    var completeBoards = file.Boards.Where(x => x.HasMarkedRowsOrColumns()).ToArray();
                    if (completeBoards.Any())
                    {
                        Console.WriteLine("Part 1");
                        Console.WriteLine(new string('=', Console.BufferWidth));
                        Console.WriteLine("Last number: " + n);

                        foreach (var board in completeBoards)
                        {
                            board.Print();
                            var values = board.GetUnmarkedNumbers();
                            var sum = values.Sum();
                            var score = sum * n;
                            Console.WriteLine($"Sum ({sum}) * n ({n}) = {sum * n}");
                        }

                        foundWinningBoard = true;
                    }
                }
            }

        }
    }

    public class BoardCell
    {
        public int Value { get; init; }
        public bool Marked { get; set; } = false;
    }

    public class Board
    {
        public BoardCell[] Cells { get; set; }
        public Board(int[] values)
        {
            this.Cells = values.Select(x => new BoardCell { Value = x }).ToArray();
        }

        public void Mark(int value)
        {
            foreach (var cell in Cells)
            {
                if (cell.Value == value) cell.Marked = true;
            }
        }

        public BoardCell[] GetRow(int row)
        {
            return this.Cells.Skip(row * 5).Take(5).ToArray();
        }
        public BoardCell[] GetColumn(int column)
        {
            return this.Cells.Where((cell, index) => (index % 5) == column).ToArray();
        }

        public void Print()
        {
            for (var i = 0; i < Cells.Length; ++i)
            {

                Console.ResetColor();
                if (!Cells[i].Marked)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[{0, 2}] ", Cells[i].Value);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("({0, 2}) ", Cells[i].Value);
                }
                if (i % 5 == 4) Console.WriteLine();

            }
            Console.ResetColor();
        }

        public bool HasMarkedRowsOrColumns()
        {
            for (var i = 0; i < 5; ++i)
            {
                if (GetRow(i).All(x => x.Marked)) return true;
                if (GetColumn(i).All(x => x.Marked)) return true;
            }

            return false;
        }

        public int[] GetUnmarkedNumbers()
        {
            return Cells.Where(x => !x.Marked).Select(x => x.Value).ToArray();
        }

        public int[] GetValuesOfMarkedRowOrColumn()
        {
            for (var i = 0; i < 5; ++i)
            {
                if (GetRow(i).All(x => x.Marked)) return GetRow(i).Select(x => x.Value).ToArray();
                if (GetColumn(i).All(x => x.Marked)) return GetColumn(i).Select(x => x.Value).ToArray();
            }

            throw new Exception("No marked row or column found");
        }

    }

    public class ParsedFile
    {
        public int[] Numbers { get; }
        public List<Board> Boards { get; } = new();
        public ParsedFile(string filename)
        {
            var lines = File.ReadAllLines("input.txt").Where(x => !string.IsNullOrWhiteSpace(x));
            Numbers = lines.First().Split(",").Select(int.Parse).ToArray();
            lines = lines.Skip(1);
            while (lines.Any())
            {
                var values = lines.Take(5).SelectMany(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries)).Select(int.Parse).ToArray();
                var board = new Board(values);
                Boards.Add(board);
                lines = lines.Skip(5);
            }
        }
    }
}
