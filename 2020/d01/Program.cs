using System.IO;
using System;
using System.Linq;
static class Program
{
    static void Main()
    {

        var input = File.ReadAllLines("input.txt").Select(x=>int.Parse(x)).ToArray();

        for (var i = 0; i < input.Length; ++i)
        {
            for (var j = (i + 1); j < input.Length; ++j)
            {
                var a = input[i];
                var b = input[j];
                if ((a + b) == 2020)
                {
                    Console.WriteLine($"{a}+{b} = {a + b}, {a}*{b}={a * b}");
                }

                for (var k = (j + 1); k < input.Length; ++k)
                {
                    var c = input[k];
                    if ((a + b + c) == 2020)
                    {
                        Console.WriteLine($"{a}+{b}+{c} = {a + b + c}, {a}*{b}*{c}={a * b * c}");
                    }
                }
            }
        }
    }
}

