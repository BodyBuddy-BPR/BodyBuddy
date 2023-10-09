using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Helpers
{
    public class FileLogger
    {
        public static void Log(string text)
        {
            try
            {
                // Create or open the log file in append mode
                using (StreamWriter writer = new StreamWriter("Log.txt", true))
                {
                    // Write a log message
                    writer.WriteLine($"Debug: [{DateTime.Now}] - {text}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred while writing to the log file: " + ex.Message);
            }
        }

        public static void Debugging(string text)
        {
            try
            {
                // Create or open the log file in append mode
                using (StreamWriter writer = new StreamWriter(@"C:\Logs\Debug.txt", true))
                {
                    // Write a log message
                    writer.WriteLine($"Debug: [{DateTime.Now}] - {text}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred while writing to the log file: " + ex.Message);
                Debug.WriteLine("Error occurred while writing to the log file: " + ex.StackTrace);
            }
        }

        public static void PerformanceTest(string text)
        {
            try
            {
                // Create or open the log file in append mode
                using (StreamWriter writer = new StreamWriter("PerformanceTest.txt", true))
                {
                    // Write a log message
                    writer.WriteLine($"Debug: [{DateTime.Now}] - {text}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred while writing to the log file: " + ex.Message);
            }
        }
    }
}
