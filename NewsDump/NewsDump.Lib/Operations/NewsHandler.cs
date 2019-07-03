using NewsDump.Lib.Operations.Sites;
using NewsDump.Lib.Operations.Sites.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olive;

namespace NewsDump.Lib.Operations
{
    public static class NewsHandler
    {
        public static void Run()
        {
            var services = RegisterServices();

            services.ForEach(x => x.RunAndSave());

        }
        
        private static List<IDumper> RegisterServices()
        {
            var serviceList = new List<IDumper>();
            serviceList.Add(new FarsnewsDumper());
            serviceList.Add(new IribDumper());
            serviceList.Add(new SookDumper());
            serviceList.Add(new PgnewsDumper());
            return serviceList;
        }
    }
}
