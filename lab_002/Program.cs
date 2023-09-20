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

var locker = new object();
foreach (var numberOfThreads in N)
{
    int threadSliceSize = NumberOfStrings / numberOfThreads;
    int[] threadEvenCounts = new int[numberOfThreads];
    int[] threadOddCounts = new int[numberOfThreads];

    List<Thread> threads = new List<Thread>();

    for (int i = 0; i < numberOfThreads; i++)
    {
        int localIndex = i;
        Thread thread = new Thread(() =>
        {
            int start = localIndex * threadSliceSize;
            int end = (localIndex == numberOfThreads - 1) ? NumberOfStrings : start + threadSliceSize;

            for (int lineIndex = start; lineIndex < end; lineIndex++)
            {
                foreach (var c in array[lineIndex])
                {
                    if ((c - '0') % 2 == 0)
                    {
                        threadEvenCounts[localIndex]++;
                    }
                    else
                    {
                        threadOddCounts[localIndex]++;
                    }
                }
            }
        });
        threads.Add(thread);
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
    int totalEvenCount = threadEvenCounts.Sum();
    int totalOddCount = threadOddCounts.Sum();
    stopwatch.Stop();

    Console.WriteLine($"Number of even: {totalEvenCount}");
    Console.WriteLine($"Number of odd: {totalOddCount}");
    Console.WriteLine($"Threads: {numberOfThreads}");
    Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");
    Console.WriteLine();
}

