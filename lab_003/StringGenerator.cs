using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_003
{
    public static class StringGenerator
    {
        private static readonly Random _random = new();

        public static string GenerateNumericString(int length)
        {
            var chars = new char[length];
            for(int i = 0; i < length; i++)
            {
                chars[i] = (char)_random.Next('0', '9' + 1);
            }
            return string.Join(string.Empty, chars);
        }
    }
}
