using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automatisatie_csv_s.Services.IOServices
{
    public class LogService
    {
        public static bool LogToFile = true;
        public static bool LogOutputToConsole = false;
        public static bool LogErrorsToConsole = true;

        public static void LogEvent(string logEvent, string TypeEvent)
        {
            if (LogToFile)
            {
                LogEventTOCSV(logEvent, TypeEvent);
            }
            if (LogOutputToConsole)
            {
                Console.WriteLine(TypeEvent+ ": " + logEvent);
            }
            
        }

        private static void LogEventTOCSV(string logEvent, string TypeEvent)
        {
            // Path to the CSV file
            string csvFilePath = "logfile.csv";


            // Check if the CSV file exists, create it if not
            if (!File.Exists(csvFilePath))
            {
                // Create the CSV file and write header
                File.WriteAllText(csvFilePath, "LogItem,Category\n");
            }

            // Append log item and category to the CSV file
            AppendToCsv(csvFilePath, logEvent, TypeEvent);
        }

        private static void AppendToCsv(string filePath, string logItem, string category)
        {
            // Append the log item and category to the CSV file
            File.AppendAllText(filePath, $"{logItem},{category}\n");
        }


        public static void LogError(string error)
        {
            LogErrorToCSV(error);

            if (LogErrorsToConsole)
            {
                Console.WriteLine(error);
            }
        }

        private static void LogErrorToCSV(string error)
        {
            // Path to the CSV file
            string csvFilePath = "errors.csv";


            // Check if the CSV file exists, create it if not
            if (!File.Exists(csvFilePath))
            {

                File.WriteAllText(csvFilePath, "Errors\n");
            }

            File.AppendAllText(csvFilePath, error + "\n");
        }
    }


}

