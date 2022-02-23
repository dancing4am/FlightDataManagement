using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SimpleLogger
{
    public static class Log
    {
        private static readonly string DEFAULT_LOG_FILE_LOCATION = @".\log.txt";        
        private static FileInfo logFile;
        
        public static void SetLogFile(string value, ref FileInfo file)
        {
            FileInfo temp = (value != null) ? new FileInfo(value) : null;
            if (temp != null && (temp.Extension.Contains("log") || temp.Extension.Contains("txt")))
            {
                file = temp;
            }
        }

        private static string GetLogFile()
        {
            return (logFile != null) ? logFile.FullName : null;
        }

        public static void SetDefaultLogFileLocation()
        {
            SetLogFile(DEFAULT_LOG_FILE_LOCATION, ref logFile); 
        }
        
        public static void Debug(string message)
        {
            Write("DEBUG: [" + GetTS() + " (UTC)] - " + message);
        }

        public static void Error(string message)
        {
            Write("ERROR: [" + GetTS() + " (UTC)] - " + message);
        }

        public static void Info(string message)
        {
            Write("INFO: [" + GetTS() + " (UTC)] - " + message);
        }

        public static void Warning(string message)
        {
            Write("WARN: [" + GetTS() + " (UTC)] - " + message);
        }

        public static void Except(Exception ex)
        {
            Except(ex.Message);
        }

        public static void Except(string message)
        {
            Write("EXCEPTION: [" + GetTS() + " (UTC)] - " + message);
        }

        public static void Trace(string message)
        {
            Write("TRACE: [" + GetTS() + " (UTC)] - " + message);
        }

        private static string GetTS()
        {
            return DateTime.UtcNow.ToString();
        }

        private static void Write(params string[] ps)
        {
            try
            {
                using (FileStream fileStream = new FileStream(GetLogFile(), FileMode.Append))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        StringBuilder line = new StringBuilder();
                        foreach (string s in ps)
                        {
                            line.Append(s).Append(". ");
                        }
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Could not write to the log file. " + ex.Message);
            }
        }
    }

    public static class TelemetryLog
    {
        private static FileInfo telemetryLogFile;
        private static readonly string TELEMETRY_LOG_FILE_LOCATION = @".\telemetry_log.txt";

        public static void SetTelemetryLogFileLocation()
        {
            SetLogFile(TELEMETRY_LOG_FILE_LOCATION, ref telemetryLogFile);
        }

        public static void SetLogFile(string value, ref FileInfo file)
        {
            FileInfo temp = (value != null) ? new FileInfo(value) : null;
            if (temp != null && (temp.Extension.Contains("log") || temp.Extension.Contains("txt")))
            {
                file = temp;
            }
        }

        private static string GetTelemetryLogFile()
        {
            return (telemetryLogFile != null) ? telemetryLogFile.FullName : null;
        }

        public static void LogTelemetry(List<FlightDataLibrary.TelemetryRecord> records)
        {
            foreach (FlightDataLibrary.TelemetryRecord record in records)
            {
                LogTelemetry(record);
            }
        }

        public static void LogTelemetry(FlightDataLibrary.TelemetryRecord[] records)
        {
            foreach (FlightDataLibrary.TelemetryRecord record in records)
            {
                LogTelemetry(record);
            }
        }

        public static void LogTelemetry(FlightDataLibrary.TelemetryRecord tel)
        {
            WriteTelemetry(tel.Timestamp, tel.AccelX, tel.AccelY, tel.AccelZ, tel.Weight, tel.Altitude, tel.Pitch, tel.Bank);
        }

        private static void WriteTelemetry(params string[] ps)
        {
            try
            {
                using (FileStream fileStream = new FileStream(GetTelemetryLogFile(), FileMode.Append))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        StringBuilder line = new StringBuilder();
                        foreach (string s in ps)
                        {
                            line.Append(s).Append(", ");
                        }
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Could not write to the log file. " + ex.Message);
            }

        }
    }
}
