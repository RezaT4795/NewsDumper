using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Lib.Util
{
    public static class EventBus
    {
        internal static void Notify(string str ,string type)
        {
            Console.WriteLine($"{type}: {str}");
        }
    }
}
