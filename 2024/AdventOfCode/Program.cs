using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode;

internal class Program
{
    static void Main(string[] args)
    {
        using (var fileStream = File.OpenRead("input2.txt"))
        using (var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
        {
            dayTwoPartTwo(streamReader);
        }
    }

    static void dayOnePartOne(StreamReader streamReader)
    {
        List<int> firstColumn = new List<int>();
        List<int> secondColumn = new List<int>();
        string line;
        while((line = streamReader.ReadLine()) != null)
        {
            string[] numbers = line.Split("   ");
            if (numbers.Length > 1)
            {
                firstColumn.Add(int.Parse(numbers[0]));
                secondColumn.Add(int.Parse(numbers[1]));
            }
        }

        firstColumn.Sort();
        secondColumn.Sort();
        int totalDistance = 0;

        for (int i = 0; i < firstColumn.Count; i++)
        {
            int distance = firstColumn[i] - secondColumn[i];
            distance = Math.Abs(distance);
            totalDistance += distance;
        }

        Console.WriteLine(totalDistance);

        dayOnePartTwo(firstColumn, secondColumn);
    }

    static void dayOnePartTwo(List<int> firstColumn, List<int> secondColumn)
    {
        for (int i = 0; i < firstColumn.Count; i++)
        {
            int num = firstColumn[i];
            int count = 0;
            foreach (int num2 in secondColumn)
            {
                if (num == num2) count++;
            }
            firstColumn[i] *= count;
        }

        int sum = firstColumn.Sum();
        Console.WriteLine(sum);
    }

    static void dayTwoPartOne(StreamReader reader) 
    {
        //int safeReports = 0;
        string line;
        List<List<int>> safeReports = new List<List<int>>();
        List<List<int>> badReports = new List<List<int>>();
        while ((line = reader.ReadLine()) != null)
        {
            List<int> numbers = new List<int>();
            string[] strings = line.Split(' ');
            foreach (string s in strings) 
            {
                numbers.Add(int.Parse(s));
            }
            if (numbers.Count != numbers.Distinct().Count()) continue;
            if (((!numbers.Order().SequenceEqual(numbers)) && (!numbers.OrderDescending().SequenceEqual(numbers)))) continue;
            bool safe = true;
            for (int i = 0; i < numbers.Count-1; i++)
            {
                if (Math.Abs(numbers[i] - numbers[i+1]) > 3) safe = false;
            }
            if (!safe) continue;
            safeReports.Add(numbers);
        }

        Console.WriteLine(safeReports.Count);
    }

    static void dayTwoPartTwo(StreamReader reader)
    {
        string line;
        List<List<int>> safeReports = new List<List<int>>();
        List<List<int>> badReports = new List<List<int>>();
        while ((line = reader.ReadLine()) != null)
        {
            List<int> numbers = new List<int>();
            string[] strings = line.Split(' ');
            foreach (string s in strings)
            {
                numbers.Add(int.Parse(s));
            }
            if (!isSafe(numbers)) 
            { 
                numbers = tryMakeSafe(numbers);
                if (isSafe(numbers)) safeReports.Add(numbers);
                continue; 
            }
            safeReports.Add(numbers);
        }

        Console.WriteLine(safeReports.Count);
    }

    private static List<int> tryMakeSafe(List<int> numbers)
    {
        if (numbers.Count < 2) return numbers;
        bool isIncreasing = numbers[0] < numbers[1];
        for (int i = 0; i < numbers.Count; i++)
        {
            //if (!isIncreasing)
            //{
            //    if (numbers[i] < numbers[i + 1] || numbers[i] - numbers[i + 1] > 3)
            //    {
            //        numbers.RemoveAt(i); 
            //        break;
            //    }
            //}
            //else
            //{
            //    if (numbers[i] > numbers[i + 1] || numbers[i+1] - numbers[i] > 3)
            //    {
            //        numbers.RemoveAt(i); 
            //        break;
            //    }
            //}
            List<int> trySafeNumbers = numbers.ToList();
            trySafeNumbers.RemoveAt(i);
            if (isSafe(trySafeNumbers)) return trySafeNumbers;
        }
        return numbers;
    }

    static bool isSafe(List<int> numbers) 
    {
        bool isSafe = true;
        if (numbers.Count != numbers.Distinct().Count()) { isSafe = false; }
        if (((!numbers.Order().SequenceEqual(numbers)) && (!numbers.OrderDescending().SequenceEqual(numbers)))) { isSafe = false; }
        for (int i = 0; i < numbers.Count - 1; i++)
        {
            if (Math.Abs(numbers[i] - numbers[i + 1]) > 3) { isSafe = false; break; }
        }
        return isSafe;
    }
}
