using NewsDump.Lib.Operations.Sites;
using NewsDump.Lib.Operations.Sites.Interface;
using NewsDump.Lib.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsDump.Lib.Operations
{
    public static class NewsHandler
    {
        public static void Run()
        {
            var services = RegisterServices();

            services.ForEach(x => x.RunAndSave());

            EventBus.Notify("عملیات با موفقیت به پایان رسید", "DoneOperation");
        }
        public static void RunParallel(bool silent)
        {
            var services = RegisterServices();

            services.AsParallel().ForAll(x => x.RunAndSave());

            if (!silent)
            {
                EventBus.Notify("عملیات با موفقیت به پایان رسید", "DoneOperation");
            }

        }
        public static async Task RunAsync() => await Task.Run(Run);
        public static async Task RunParallelAsync(bool silent) => await Task.Run(() => RunParallel(silent));
        private static List<IDumper> RegisterServices()
        {
            var serviceList = new List<IDumper>();
            serviceList.Add(new FarsnewsDumper());
            serviceList.Add(new IribDumper());
            serviceList.Add(new SookDumper());
            serviceList.Add(new IrnaDumper());
            serviceList.Add(new MehrnewsDumper());
            serviceList.Add(new TasnimnewsDumper());
            serviceList.Add(new YjcDumper());
            serviceList.Add(new PgnewsDumper());
            return serviceList;
        }
    }
}
