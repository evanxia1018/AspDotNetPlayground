using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AspDotNetPlayground.Playground
{
    public class DoAppDomainsShareTheSameThreadPool
    {
        public static void Run()
        {
            var domain0 = AppDomain.CreateDomain("domain 0", null, AppDomain.CurrentDomain.SetupInformation);
            var domain1 = AppDomain.CreateDomain("domain 1", null, AppDomain.CurrentDomain.SetupInformation);

            Task.Run(() => {
                domain0.DoCallBack(new CrossAppDomainDelegate(PrintDomainNameLoop));
            });
            Task.Run(() => {
                domain1.DoCallBack(new CrossAppDomainDelegate(PrintDomainNameLoop));
            });

            Thread.Sleep(5000);
            ThreadPool.GetMaxThreads(out int maxWorkerThreadCount, out _);
            ThreadPool.GetAvailableThreads(out int availableWorkerThreadCount, out _);
            int busyWorkerThreadCount = maxWorkerThreadCount - availableWorkerThreadCount;
            Console.WriteLine("There are [{0}] busy worker threads", busyWorkerThreadCount);
        }

        private static void PrintDomainNameLoop()
        {
            while (true)
            {
                Thread.Sleep(1000);
                System.Diagnostics.Debug.WriteLine("I'm an AppDomain. My name is {0}", AppDomain.CurrentDomain.FriendlyName);
            }
        }
    }
}