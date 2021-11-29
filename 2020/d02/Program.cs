using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace d02
{
    class Program
    {
        static void Main(string[] args)
        {
            var entries = File.ReadAllLines("input.txt").Select(PasswordEntry.Parse).ToArray();
            Console.WriteLine("Valid passwords according to rule 1: " + entries.Count(x => x.IsValidRule1()));
            Console.WriteLine("Valid passwords according to rule 2: " + entries.Count(x => x.IsValidRule2()));
        }
    }

    class PasswordEntry
    {
        public PasswordEntry(string input)
        {
            Input = input;
            var result = new Regex(@"^(?<min>\d+)-(?<max>\d+) (?<char>[a-zA-Z]): (?<password>[a-zA-Z]+)$").Match(input);
            if (!result.Success) throw new Exception($"The input doesn't match the regex: {input}");
            this.MinLength = int.Parse(result.Groups["min"].Value);
            this.MaxLength = int.Parse(result.Groups["max"].Value);
            this.Character = char.Parse(result.Groups["char"].Value);
            this.Password = result.Groups["password"].Value;
        }

        public string Input { get; }
        public int MinLength { get; }
        public int MaxLength { get; }

        public char Character { get; }
        public string Password { get; }

        static public PasswordEntry Parse(string input)
        {
            return new PasswordEntry(input);
        }

        public bool IsValidRule1()
        {
            var charCount = Password.Count(x => x == Character);
            return charCount >= MinLength && charCount <= MaxLength;
        }

        public bool IsValidRule2()
        {
            var a = Password.ElementAt(MinLength - 1);
            var b = Password.ElementAt(MaxLength - 1);
            
            return a!=b && (a == Character || b==Character);
        }
    }
}
