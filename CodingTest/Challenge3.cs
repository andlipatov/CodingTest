using System.Text.RegularExpressions;

namespace CodingTest
{
    public static class Challenge3
    {
        private const string _INPUT_LOG_PATH = @"..\..\..\Logs\input.log";
        private const string _OUTPUT_LOG_PATH = @"..\..\..\Logs\output.log";
        private const string _PROBLEMS_LOG_PATH = @"..\..\..\Logs\problems.log";

        private const string _EMPTY_LINE = "";

        private const string _DATE_FORMAT_1 = "dd.mm.yyyy";
        private const string _DATE_FORMAT_2 = "yyyy-mm-dd";

        private const string _DEFAULT_METHOD = "_DEFAULT";

        //Format1: 10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'
        private const string _PATTERN_1 = @"^(\d{2}\.\d{2}\.\d{4})\s+(\d{2}:\d{2}:\d{2}\.\d+)\s+(\w+)\s+(.+)$";

        //Format2: 2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'
        private const string _PATTERN_2 = @"^(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2}\.\d+)\|\s+(\w+)\|(\d+)\|(.+?)\|\s+(.+)$";

        //OutputFormat: DD-MM-YYYY dd.mm.yyyy Level Message 

        public static void Execute()
        {
            try
            {
                using (StreamReader inputReader = new(_INPUT_LOG_PATH))
                using (StreamWriter outputWriter = new(_OUTPUT_LOG_PATH))
                using (StreamWriter problemsWriter = new(_PROBLEMS_LOG_PATH))
                {
                    string? inputLine;

                    while ((inputLine = inputReader.ReadLine()) != null)
                    {
                        string? outputLine = ProcessLine(inputLine);

                        if (outputLine != null)
                        {
                            outputWriter.WriteLine(outputLine);
                        }
                        else
                        {
                            problemsWriter.WriteLine(inputLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        private static string? ProcessLine(string line)
        {
            Match match1 = Regex.Match(line, _PATTERN_1);
            Match match2 = Regex.Match(line, _PATTERN_2);

            if (match1.Success)
            {
                string date = DateTime.ParseExact(match1.Groups[1].Value, _DATE_FORMAT_1, null).ToString(_DATE_FORMAT_2);
                string time = match1.Groups[2].Value;
                string level = ConvertLevel(match1.Groups[3].Value);
                string message = match1.Groups[4].Value;

                return CreateLine(date, time, level, _DEFAULT_METHOD, message);
            }
            else if (match2.Success)
            {
                string date = match2.Groups[1].Value;
                string time = match2.Groups[2].Value;
                string level = ConvertLevel(match2.Groups[3].Value);
                string method = match2.Groups[5].Value;
                string message = match2.Groups[6].Value;

                return CreateLine(date, time, level, method, message);
            }

            return null;
        }

        static string ConvertLevel(string level)
        {
            string result;

            switch (level)
            {
                case "INFORMATION":
                case "INFO":
                {
                    result = "INFO";
                    break;
                }
                case "WARNING":
                case "WARN":
                {
                    result = "WARN";
                    break;
                }
                case "ERROR":
                {
                    result = "ERROR";
                    break;
                }
                case "DEBUG":
                {
                    result = "DEBUG";
                    break;
                }
                default:
                {
                    result = _EMPTY_LINE;
                    break;
                }
            }

            return result;
        }

        static string CreateLine(string date, string time, string level, string method, string message)
        {
            return $"{date}\t{time}\t{level}\t{method}\t{message}";
        }
    }
}