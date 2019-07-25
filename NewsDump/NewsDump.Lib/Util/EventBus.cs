using Olive;
using System;
using System.IO;

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

        public static void Log(string str, string type)
        {
            var appDomain = AppDomain.CurrentDomain;
            var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
            var file = Path.Combine(basePath, "events.log").AsFile();

            try
            {
                file.AppendAllText(Environment.NewLine + $"--{type}--{DateTime.UtcNow}--" + Environment.NewLine + str);
            }
            catch (Exception)
            {
                Notify(Environment.NewLine + $"--{type}--{DateTime.UtcNow}--" + Environment.NewLine + str, "Log");
            }


        }
    }
    public class MessageArgs
    {
        public string Message { get; set; }
        public string Type { get; set; }

        public override string ToString() => $"{Type}: {Message}";
    }
}
