using NewsDump.Lib.Operations.Sites;
using NewsDump.Lib.Operations.Sites.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsDump.Lib.Operations
{
    public static class NewsHandler
    {
        public static void Run()
        {
            var services = RegisterServices();
            services.ForAll(s =>
            {
                s.RunAndSave();
            });
        }

        private static ParallelQuery<IDumper> RegisterServices()
        {
            var serviceList = new List<IDumper>();
            serviceList.Add(new IribDumper());

            return serviceList.AsParallel();
        }
    }
}
