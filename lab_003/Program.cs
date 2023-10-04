// 11.Сгенерировать массив из 300 строк, содержащих случайные 200000 цифр.
// Посчитать число вхождений четных и нечетных цифр в каждой строке, используя N
// потоков. Измерить время программы для N = 1, 2, 4, 10. Измерить работы время
// программы в каждом случае. OPenMP

using lab_003;
using System.Diagnostics;

const int NumberOfStrings = 300;
const int StringLength = 200000;
var N = new int[] { 1, 2, 4, 10 };

var array = ParallelEnumerable.Range(0, NumberOfStrings)
    .Select(_ => StringGenerator.GenerateNumericString(StringLength))
    .ToArray();

foreach (var numberOfThreads in N)
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var evenCounts = new int[NumberOfStrings];
    var oddCounts = new int[NumberOfStrings];

    Parallel.For(0, NumberOfStrings, new ParallelOptions { MaxDegreeOfParallelism = numberOfThreads }, lineIndex =>
    {
        for (var i = 0; i < StringLength; i++)
        {
            var digit = array[lineIndex][i] - '0';
            if (digit % 2 == 0)
                evenCounts[lineIndex]++;
            else
                oddCounts[lineIndex]++;
        }
    });

    var totalEvenCount = evenCounts.Sum();
    var totalOddCount = oddCounts.Sum();

    stopwatch.Stop();

    Console.WriteLine($"Threads: {numberOfThreads}");
    Console.WriteLine($"Total number of even: {totalEvenCount}");
    Console.WriteLine($"Total number of odd: {totalOddCount}");

    for (var i = 0; i < NumberOfStrings; i++)
    {
        Console.WriteLine($"Number of even for line {i}: {evenCounts[i]}");
        Console.WriteLine($"Number of odd for line {i}: {oddCounts[i]}");
    }

    Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");
    Console.WriteLine();
}