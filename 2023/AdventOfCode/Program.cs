using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Program
{

    static void Main(string[] args)
    {
        using (var fileStream = File.OpenRead("input.txt"))
        using (var reader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
        {
            DayOnePartTwo(reader);
        }
    }

    static void DayOnePartOne(StreamReader reader)
    {
        char[] numbers = "0123456789".ToCharArray();

        string line;
        int total = 0;
        while((line = reader.ReadLine()) != null)
        {
            var lineArray = line.ToCharArray();
            string? first = null;
            string? last = null;

            ReplaceStringNumWithActualNum(line);

            for (int i = 0; i < lineArray.Length; i++)
            {
                char c = lineArray[i];
                if (numbers.Contains(c))
                {
                    last = c.ToString();
                    first = first != null ? first : c.ToString();
                }
            }
            if (first != null)
            {
                string num = first + last;
                total += int.Parse(num);
            }
        }
        Console.WriteLine(total.ToString());
    }

    private static string ReplaceStringNumWithActualNum(string line)
    {
        string[] numbersAsStrings = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        for (int i = 0; i < numbersAsStrings.Length; i++)
        {
            line = Regex.Replace(line, numbersAsStrings[i], i.ToString());
        }
        return line;
    }

    static void DayOnePartTwo(StreamReader reader)
    {
        //string[] matches = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string regex = @"zero|one|two|three|four|five|six|seven|eight|nine|\d";
        string line;
        int total = 0;
        while ((line = reader.ReadLine()) != null)
        {
            string? first = null;
            string? last = null;

            var matches = Regex.Matches(line, regex);

            first = int.Parse(ReplaceStringNumWithActualNum(matches.First().Value)).ToString();
            last = int.Parse(ReplaceStringNumWithActualNum(matches.Last().Value)).ToString();

            if (first != null)
            {
                string num = first + last;
                total += int.Parse(num);
            }
        }

        Console.WriteLine(total.ToString());
    }
}
