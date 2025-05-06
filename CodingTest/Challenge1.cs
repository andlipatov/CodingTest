using System.Text;
using System.Text.RegularExpressions;

namespace CodingTest
{
    public static class Challenge1
    {
        private const string _LOWERCASE_LATIN_PATTERN = "^[a-z]+$";
        private const int _DECIMAL_BASE = 10;
        private const char _CHAR_ZERO = '0';

        public static void Execute()
        {
            string input = "aaabbcccdde";

            if (Regex.IsMatch(input, _LOWERCASE_LATIN_PATTERN))
            {
                string compressed = Compress(input);
                string decompressed = Decompress(compressed);

                Console.WriteLine($"Исходная строка: {input}");
                Console.WriteLine($"Результат компрессии: {compressed}");
                Console.WriteLine($"Результат декомпрессии: {decompressed}");
            }
            else
            {
                Console.WriteLine("Ошибка: строка должна содержать только строчные буквы латинского алфавита");
            }

            Console.WriteLine("\n");
        }

        private static string Compress(string value)
        {
            StringBuilder result = new();
            int count = 1;

            if (string.IsNullOrEmpty(value))
            {
                Console.WriteLine("Ошибка: пустая строка");
                return result.ToString();
            }

            for (int i = 1; i < value.Length; i++)
            {
                if (value[i] == value[i - 1])
                {
                    count++;
                }
                else
                {
                    AppendCharacterWithCount(result, value[i - 1], count);
                    count = 1;
                }
            }

            AppendCharacterWithCount(result, value[^1], count);

            return result.ToString();

            static void AppendCharacterWithCount(StringBuilder stringBuilder, char character, int count)
            {
                stringBuilder.Append(character);

                if (count > 1)
                {
                    stringBuilder.Append(count);
                }
            }
        }

        private static string Decompress(string value)
        {
            StringBuilder result = new();

            if (string.IsNullOrEmpty(value))
            {
                Console.WriteLine("Ошибка: пустая строка");
                return result.ToString();
            }

            int i = 0;
            while (i < value.Length)
            {
                char character = value[i];
                int count = 0;

                i++;
                while (i < value.Length && char.IsDigit(value[i]))
                {
                    count = count * _DECIMAL_BASE + (value[i] - _CHAR_ZERO);
                    i++;
                }

                for (int j = 0; j < count; j++)
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }
    }
}