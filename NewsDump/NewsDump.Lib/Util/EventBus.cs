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

            file.AppendAllText(Environment.NewLine + $"--{type}--" + Environment.NewLine + str);

        }
    }
    public class MessageArgs
    {
        public string Message { get; set; }
        public string Type { get; set; }

        public override string ToString() => $"{Type}: {Message}";
    }
}
