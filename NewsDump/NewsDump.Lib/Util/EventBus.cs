using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Lib.Util
{
    public static class EventBus
    {
        public delegate void MessageEventHandler(MessageArgs message);

        public static event MessageEventHandler OnMessageFired;

        internal static void Notify(string str ,string type)
        {
            OnMessageFired?.Invoke(new MessageArgs { Message = str, Type = type });
        }
    }
    public class MessageArgs
    {
        public string Message { get; set; }
        public string Type { get; set; }

        public override string ToString() => $"{Type}: {Message}";
    }
}
