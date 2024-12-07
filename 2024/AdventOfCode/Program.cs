using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Program
{
    static void Main(string[] args)
    {
        using (var fileStream = File.OpenRead("input6.txt"))
        using (var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
        {
            daySixPartTwo(streamReader);
        }
    }

    #region dayOne
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

    #endregion dayOne
    #region dayTwo

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

    #endregion dayTwo

    #region dayThree
    static void dayThreePartOne(StreamReader reader)
    {
        int total = 0;
        string line;
        string regex = @"mul\((\d+),(\d+)\)";
        while ((line = reader.ReadLine()) != null)
        {
            var matches = Regex.Matches(line, regex);
            foreach (Match match in matches) 
            {
                total += (int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
            }
        }
        Console.WriteLine(total);
    }

    static void dayThreePartTwo(StreamReader reader) 
    { 
        int total = 0; 
        string line;
        string regex = @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)";
        bool doMul = true;
        while ((line = reader.ReadLine()) != null)
        {
            var matches = Regex.Matches(line, regex);
            foreach (Match match in matches)
            {
                if (match.Value == "do()" || match.Value == "don't()") doMul = match.Value == "do()";
                else if (doMul) total += (int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
            }
        }
        Console.WriteLine(total);
    }

    #endregion dayThree

    #region dayFour

    enum Direction
    {
        Bottom,
        Top,
        Right,
        Left,
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft
    }

    static void dayFourPartOne(StreamReader reader)
    {
        List<List<char>> crossword = new List<List<char>>();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            crossword.Add(line.ToList());
        }

        int total = 0;

        for (int i = 0; i < crossword.Count; i++)
        {
            for (int j = 0; j < crossword[i].Count; j++)
            {
                if (crossword[i][j] == 'X')
                {
                    string word = "XMAS";
                    //if (HasRight(crossword, i, j) || HasBottom(crossword, i, j) || HasLeft(crossword, i, j) || HasTop(crossword, i, j) || HasTopRight(crossword, i, j) || HasBottomRight(crossword, i, j) || HasBottomLeft(crossword, i, j) || HosTopLeft(crossword, i, j)) total++;
                    if (hasWord(crossword, i, j, Direction.Right, word)) total++;
                    if (hasWord(crossword, i, j, Direction.Left, word)) total++;
                    if (hasWord(crossword, i, j, Direction.Top, word)) total++;
                    if (hasWord(crossword, i, j, Direction.Bottom, word)) total++;
                    if (hasWord(crossword, i, j, Direction.TopRight, word)) total++;
                    if (hasWord(crossword, i, j, Direction.BottomRight, word)) total++;
                    if (hasWord(crossword, i, j, Direction.BottomLeft, word)) total++;
                    if (hasWord(crossword, i, j, Direction.TopLeft, word)) total++;
                    //if (HasTop(crossword, i, j)) total++;
                    //if (HasBottom(crossword, i, j)) total++;
                    //if (HasTopRight(crossword, i, j)) total++;
                    //if (HasBottomRight(crossword, i, j)) total++;
                    //if (HasBottomLeft(crossword, i, j)) total++;
                    //if (HasTopLeft(crossword, i, j)) total++;
                }
            }
        }

        Console.WriteLine(total);
    }

    private static bool hasWord(List<List<char>> crossword, int row, int column, Direction direction, string word) 
    {
        List<char> potentialWord;
        switch (direction)
        {
            case Direction.Bottom:
                if (row + ( word.Length - 1 ) >= crossword.Count) return false;
                potentialWord = new List<char>();
                for (int i = 0; i < word.Length; i++)
                {
                    potentialWord.Add(crossword[row + i][column]);
                }
                return new String(potentialWord.ToArray()) == word;
            case Direction.Top:
                if (row - ( word.Length - 1 ) < 0) return false;
                potentialWord = new List<char>();
                for (int i = 0; i < word.Length; i++)
                {
                    potentialWord.Add(crossword[row - i][column]);
                }
                return new String(potentialWord.ToArray()) == word;
            case Direction.Right:
                if (column + ( word.Length - 1 ) >= crossword[row].Count) return false;
                potentialWord = new List<char>();
                for (int i = 0; i < word.Length; i++)
                {
                    potentialWord.Add(crossword[row][column + i]);
                }
                return new String(potentialWord.ToArray()) == word;
            case Direction.Left:
                if (column - ( word.Length - 1 ) < 0) return false;
                potentialWord = new List<char>();
                for (int i = 0; i < word.Length; i++)
                {
                    potentialWord.Add(crossword[row][column - i]);
                }
                return new String(potentialWord.ToArray()) == word;
            case Direction.TopRight:
                if (row - ( word.Length - 1 ) < 0 || column + ( word.Length - 1 ) >= crossword[row].Count) return false;
                potentialWord = new List<char>();
                for (int i = 0; i < word.Length; i++)
                {
                    potentialWord.Add(crossword[row - i][column + i]);
                }
                return new String(potentialWord.ToArray()) == word;
            case Direction.BottomRight:
                if (row + ( word.Length - 1 ) >= crossword.Count || column + ( word.Length - 1 ) >= crossword[row].Count) return false;
                potentialWord = new List<char>();
                for (int i = 0; i < word.Length; i++)
                {
                    potentialWord.Add(crossword[row + i][column + i]);
                }
                return new String(potentialWord.ToArray()) == word;
            case Direction.BottomLeft:
                if (row + ( word.Length - 1 ) >= crossword.Count || column - ( word.Length - 1 ) < 0) return false;
                potentialWord = new List<char>();
                for (int i = 0; i < word.Length; i++)
                {
                    potentialWord.Add(crossword[row + i][column - i]);
                }
                return new String(potentialWord.ToArray()) == word;
            case Direction.TopLeft:
                if (row - ( word.Length - 1 ) < 0 || column - ( word.Length - 1 ) < 0) return false;
                potentialWord = new List<char>();
                for (int i = 0; i < word.Length; i++)
                {
                    potentialWord.Add(crossword[row - i][column - i]);
                }
                return new String(potentialWord.ToArray()) == word;
            default:
                return false;
        }
    }

    class MasMatch
    {
        private int topLeftRow;
        private int topLeftColumn;
        private int bottomRightRow;
        private int bottomRightColumn;
        public MasMatch(int topLeftRow, int topLeftColumn, int bottomRightRow, int bottomRightColumn) 
        {
            this.topLeftRow = topLeftRow;
            this.topLeftColumn = topLeftColumn;
            this.bottomRightRow = bottomRightRow;
            this.bottomRightColumn = bottomRightColumn;
        }

        public override bool Equals(object? obj)
        {
            if (obj is MasMatch match) 
            {
                return topLeftColumn == match.topLeftColumn && topLeftRow == match.topLeftRow && bottomRightColumn == match.bottomRightColumn && bottomRightRow == match.bottomRightRow;
            }
            else
            {
                return false;
            }
        }
    }

    static void dayFourPartTwo(StreamReader reader)
    {
        List<List<char>> crossword = new List<List<char>>();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            crossword.Add(line.ToList());
        }

        int total = 0;
        List<MasMatch> matches = new List<MasMatch>();
        for (int i = 0; i < crossword.Count; i++)
        {
            for (int j = 0; j < crossword[i].Count; j++)
            {
                if (crossword[i][j] == 'M')
                {
                    string word = "MAS";
                    if (hasWord(crossword, i, j, Direction.BottomLeft, word)) 
                    {
                        if (hasWord(crossword, i, j - (word.Length - 1), Direction.BottomRight, word))
                        {
                            MasMatch match = new MasMatch(i, j - (word.Length - 1), i + (word.Length - 1), j);
                            if (!matches.Contains(match)) matches.Add(match);
                        }
                        if (hasWord(crossword, i + (word.Length - 1), j, Direction.TopLeft, word)) 
                        {
                            MasMatch match = new MasMatch(i, j - (word.Length - 1), i + (word.Length - 1), j);
                            if (!matches.Contains(match)) matches.Add(match);
                        }
                    }
                    if (hasWord(crossword, i, j, Direction.BottomRight, word))
                    {
                        if (hasWord(crossword, i - (word.Length - 1), j, Direction.TopRight, word))
                        {
                            MasMatch match = new MasMatch(i, j, i + (word.Length - 1), j + (word.Length - 1));
                            if (!matches.Contains(match)) matches.Add(match);
                        }
                        if (hasWord(crossword, i, j + (word.Length - 1), Direction.BottomLeft, word))
                        {
                            MasMatch match = new MasMatch(i, j, i + (word.Length - 1), j + (word.Length - 1));
                            if (!matches.Contains(match)) matches.Add(match);
                        }
                    }
                    if (hasWord(crossword, i, j, Direction.TopLeft, word))
                    {
                        if (hasWord(crossword, i, j + (word.Length - 1), Direction.TopRight, word))
                        {
                            MasMatch match = new MasMatch(i - (word.Length - 1), j - (word.Length - 1), i, j);
                            if (!matches.Contains(match)) matches.Add(match);
                        }
                        if (hasWord(crossword, i - (word.Length - 1), j, Direction.BottomLeft, word))
                        {
                            MasMatch match = new MasMatch(i - (word.Length - 1), j - (word.Length - 1), i, j);
                            if (!matches.Contains(match)) matches.Add(match);
                        }
                    }
                    if (hasWord(crossword, i, j, Direction.TopRight, word))
                    {
                        if (hasWord(crossword, i, j + (word.Length - 1), Direction.TopLeft, word))
                        {
                            MasMatch match = new MasMatch(i - (word.Length - 1), j, i, j + (word.Length - 1));
                            if (!matches.Contains(match)) matches.Add(match);
                        }
                        if (hasWord(crossword, i - (word.Length - 1), j, Direction.BottomRight, word))
                        {
                            MasMatch match = new MasMatch(i - (word.Length - 1), j, i, j + (word.Length - 1));
                            if (!matches.Contains(match)) matches.Add(match);
                        }
                    }
                }
            }
        }
        Console.WriteLine(matches.Count);
    }

    #endregion dayFour

    #region dayFive

    static void dayFivePartOne(StreamReader reader) 
    {
        string line;
        List<Tuple<int,int>> rules = new List<Tuple<int,int>>();
        List<List<int>> manuals = new List<List<int>>();
        bool readingManuals = false;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Length <= 0) 
            { 
                readingManuals = true;
                continue;
            }
            if (!readingManuals)
            {
                string[] numbers = line.Split('|');
                if (numbers.Length < 2) continue;
                rules.Add(new Tuple<int, int>(int.Parse(numbers[0]), int.Parse(numbers[1])));
            }
            else
            {
                string[] numbers = line.Split(',');
                List<int> manual = new List<int>();
                foreach (string number in numbers)
                {
                    manual.Add(int.Parse(number));
                }
                manuals.Add(manual);
            }
        }

        List<List<int>> correctManuals = new List<List<int>>();
        List<List<int>> incorrectManuals = new List<List<int>>();

        foreach (List<int> manual in manuals) 
        {
            if (IsManualCorrect(manual, rules)) correctManuals.Add(manual);
            else incorrectManuals.Add(manual);
        }

        int total = 0;
        foreach (List<int> manual in correctManuals)
        {
            total += manual[manual.Count / 2];
        }

        Console.WriteLine(total);

        dayFivePartTwo(rules, incorrectManuals);
    }

    static void dayFivePartTwo(List<Tuple<int, int>> rules, List<List<int>> incorrectManuals)
    {
        List<List<int>> correctedManuals = new List<List<int>>();
        foreach (List<int> incorrectManual in incorrectManuals)
        {
            correctedManuals.Add(CorrectList(incorrectManual, rules, 0));
        }

        int total = 0;
        foreach (List<int> manual in correctedManuals)
        {
            total += manual[manual.Count / 2];
        }

        Console.WriteLine(total);
    }

    private static List<int> CorrectList(List<int> incorrectManual, List<Tuple<int, int>> rules, int currentNumber)
    {
        List<int> correctedManual = new List<int>();
        foreach(int number in incorrectManual)
        {
            List<int> testManual = correctedManual.ToList();
            int index = 0;
            do
            {
                testManual = correctedManual.ToList();
                testManual.Insert(index++, number);
            }
            while (!IsManualCorrect(testManual, rules));
            correctedManual = testManual.ToList();
        }
        return correctedManual;
    }

    private static bool IsManualCorrect(List<int> manual, List<Tuple<int, int>> rules)
    {
        bool isCorrect = true;
        for (int i = 0; i < manual.Count; i++)
        {
            int currentNum = manual[i];
            rules.Where(x => x.Item1 == currentNum || x.Item2 == currentNum).ToList().ForEach(x => {
                if (x.Item1 == currentNum) 
                {
                    int index = manual.IndexOf(x.Item2);
                    if (index >= 0 && index <= i) isCorrect = false;
                }
                else
                {
                    int index = manual.IndexOf(x.Item1);
                    if (index >= 0 && index >= i) isCorrect = false;
                }
            });
        }
        return isCorrect;
    }

    #endregion dayFive

    #region daySix

    static void daySixPartOne(StreamReader reader)
    {
        string line;
        List<string> map = new List<string>();
        while ((line = reader.ReadLine()) != null)
        {
            map.Add(line);
        }

        List<string> completedMaze = WalkMaze(map, 0);

        int total = 0;
        completedMaze.ForEach(x => { total += x.Count(character => character == 'X'); });

        Console.WriteLine(total);
    }

    private static List<string> WalkMaze(List<string> map, int iterationCount)
    {
        iterationCount++;
        if (iterationCount > map.Count * map[0].Length) 
        { 
            return new List<string>();
        }
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                switch (map[i][j]) 
                {
                    case '<':
                        if (j - 1 < 0)
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = 'X';
                            map[i] = sb.ToString();
                            return map;
                        }
                        else if (map[i][j-1] == '#')
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = '^';
                            map[i] = sb.ToString();
                            return WalkMaze(map, iterationCount);
                        }
                        else /*if (map[i][j] == '.')*/
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = 'X';
                            sb[j - 1] = '<';
                            map[i] = sb.ToString();
                            return WalkMaze(map, iterationCount);
                        }
                        break;
                    case '^':
                        if (i - 1 < 0)
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = 'X';
                            map[i] = sb.ToString();
                            return map;
                        }
                        else if (map[i - 1][j] == '#')
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = '>';
                            map[i] = sb.ToString();
                            return WalkMaze(map, iterationCount);
                        }
                        else /*if (map[i - 1][j] == '.')*/
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = 'X';
                            map[i] = sb.ToString();
                            sb = new StringBuilder(map[i-1]);
                            sb[j] = '^';
                            map[i-1] = sb.ToString();
                            return WalkMaze(map, iterationCount);
                        }
                        break;
                    case '>':
                        if (j + 1 >= map[i].Length)
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = 'X';
                            map[i] = sb.ToString();
                            return map;
                        }
                        else if (map[i][j+1] == '#')
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = 'v';
                            map[i] = sb.ToString();
                            return WalkMaze(map, iterationCount);
                        }
                        else /*if (map[i][j+1] == '.')*/
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = 'X';
                            sb[j + 1] = '>';
                            map[i] = sb.ToString();
                            return WalkMaze(map, iterationCount);
                        }
                        break;
                    case 'v':
                        if (i + 1 >= map.Count) 
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = 'X';
                            map[i] = sb.ToString();
                            return map;
                        }
                        else if (map[i + 1][j] == '#')
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = '<';
                            map[i] = sb.ToString();
                            return WalkMaze(map, iterationCount);
                        }
                        else /*if (map[i + 1][j] == '.')*/
                        {
                            StringBuilder sb = new StringBuilder(map[i]);
                            sb[j] = 'X';
                            map[i] = sb.ToString();
                            sb = new StringBuilder(map[i+1]);
                            sb[j] = 'v';
                            map[i+1] = sb.ToString();
                            return WalkMaze(map, iterationCount);
                        }
                        break;
                }
            }
        }
        return map;
    }

    static void daySixPartTwo(StreamReader reader)
    {
        string line;
        List<string> map = new List<string>();
        while ((line = reader.ReadLine()) != null)
        {
            map.Add(line);
        }

        int total = 0;

        for (int i = 0; i < map.Count; i++)
        {
            Console.WriteLine("currently on line " + i);
            for (int j = 0; j < map[i].Length; j++)
            {
                List<string> newMap = map.ToList();
                if (map[i][j] == '.')
                {
                    StringBuilder sb = new StringBuilder(map[i]);
                    sb[j] = '#';
                    newMap[i] = sb.ToString();
                    if (WalkMaze(newMap, 0).Count == 0) total++;
                }
            }
        }

        Console.WriteLine(total);
        Console.ReadLine();
    }

    #endregion daySix
}
