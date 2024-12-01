namespace AdventOfCode;

internal class Program
{
    static void Main(string[] args)
    {
        using (var fileStream = File.OpenRead("input.txt"))
        using (var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
        {
            dayOnePartOne(streamReader);
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
}
