// 11.Сгенерировать массив из 300 строк, содержащих случайные 200000 цифр.
// Посчитать число вхождений четных и нечетных цифр в каждой строке, используя N
// потоков. Измерить время программы для N = 1, 2, 4, 10. Измерить работы время
// программы в каждом случае.

using lab_002;

const int NumberOfStrings = 300;
const int StringLength = 200000;
var N = new int[]{ 1, 2, 4, 10 };


var array = Enumerable.Range(0, NumberOfStrings)
    .Select(_ => StringGenerator.GenerateNumericString(StringLength))
    .ToArray();

Console.WriteLine(array);
