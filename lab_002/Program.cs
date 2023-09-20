// 11.Сгенерировать массив из 300 строк, содержащих случайные 200000 цифр.
// Посчитать число вхождений четных и нечетных цифр в каждой строке, используя N
// потоков. Измерить время программы для N = 1, 2, 4, 10. Измерить работы время
// программы в каждом случае.

using lab_002;
using System.Diagnostics;

const int NumberOfStrings = 300;
const int StringLength = 200000;
var N = new int[]{ 1, 2, 4, 10 };

var array = Enumerable.Range(0, NumberOfStrings)
    .Select(_ => StringGenerator.GenerateNumericString(StringLength))
    .ToArray();

foreach (var numberOfThreads in N)
{
    var threadSliceSize = NumberOfStrings / numberOfThreads;
    var threadEvenCounts = new int[NumberOfStrings, numberOfThreads];
    var threadOddCounts = new int[NumberOfStrings, numberOfThreads];
    var evenCounts = new int[NumberOfStrings];
    var oddCounts = new int[NumberOfStrings];

    var threads = new Thread[numberOfThreads];

    for (int i = 0; i < numberOfThreads; i++)
    {
        var localIndex = i;
        var thread = new Thread(() =>
        {
            int start = localIndex * threadSliceSize;
            var end = (localIndex == numberOfThreads - 1) ? NumberOfStrings : start + threadSliceSize;

            for (int lineIndex = start; lineIndex < end; lineIndex++)
            {
                foreach (var c in array[lineIndex])
                {
                    if ((c - '0') % 2 == 0)
                    {
                        threadEvenCounts[lineIndex, localIndex]++;
                    }
                    else
                    {
                        threadOddCounts[lineIndex, localIndex]++;
                    }
                }
            }
        });
        threads[i] = thread;
    }

    var stopwatch = new Stopwatch();
    stopwatch.Start();
    foreach (var thread in threads)
    {
        thread.Start();
    }
    foreach (var thread in threads)
    {
        thread.Join();
    }
    for(int i = 0; i < NumberOfStrings; i++)
    {
        for(int j = 0; j < numberOfThreads; j++)
        {
            evenCounts[i] += threadEvenCounts[i, j];
            oddCounts[i] += threadOddCounts[i, j];
        }
    }
    stopwatch.Stop();

    var totalEvenCount = 0;
    var totalOddCount = 0;
    for (var lineIndex = 0;  lineIndex < NumberOfStrings; lineIndex++)
    {
        totalEvenCount += evenCounts[lineIndex];
        totalOddCount += oddCounts[lineIndex];
        Console.WriteLine($"Number of even for line {lineIndex}: {evenCounts[lineIndex]}");
        Console.WriteLine($"Number of odd for line {lineIndex}: {oddCounts[lineIndex]}");
    }

    Console.WriteLine($"Total number of even: {totalEvenCount}");
    Console.WriteLine($"Total number of odd: {totalOddCount}");
    Console.WriteLine($"Threads: {numberOfThreads}");
    Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");
    Console.WriteLine();
}

