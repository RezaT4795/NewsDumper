using Olive;
using System;
using System.IO;
using System.Threading;

namespace NewsDump.Lib.Util
{
    public static class EventBus
    {
        public delegate void MessageEventHandler(MessageArgs message);

        public static event MessageEventHandler OnMessageFired;

        public static void Notify(string str, string type)
        {
            OnMessageFired?.Invoke(new MessageArgs { Message = str, Type = type });
        }
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public static void Log(string str, string type)
        {
            var file = GetLoggingPath().AsFile();

            try
            {
                file.WriteToFileThreadSafe(Environment.NewLine + $"--{type}--{DateTime.UtcNow}--" + Environment.NewLine + str);
            }
            catch (Exception)
            {
                Notify(Environment.NewLine + $"--{type}--{DateTime.UtcNow}--" + Environment.NewLine + str, "Log");
            }


        }
        private static void WriteToFileThreadSafe(this FileInfo file, string text)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                // Append text to the file
                using (StreamWriter sw = File.AppendText(file.FullName))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }
        public static void TouchLogFile()
        {
            var file = GetLoggingPath();
            if (!File.Exists(file))
            {
                file.AsFile().WriteToFileThreadSafe("");
            }
        }
        public static void ClearLog()
        {
            GetLoggingPath().AsFile().DeleteIfExists();
            TouchLogFile();
        }
        public static string GetLoggingPath()
        {
            var appDomain = AppDomain.CurrentDomain;
            var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
            return Path.Combine(basePath, "events.log");
        }
    }
    public class MessageArgs
    {
        public string Message { get; set; }
        public string Type { get; set; }

        public override string ToString() => $"{Type}: {Message}";
    }
}
