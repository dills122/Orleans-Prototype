using BusinessLogic.Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace SiloCluster
{
    class Program
    {
        private static ISiloHost silo;
        private static ConsoleColor color = ConsoleColor.Red;
        private static readonly ManualResetEvent siloStopped = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            silo = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "nexsys-prototype-deployment1";
                    options.ServiceId = "NexsysOrleansPrototype";
                })
                //.Configure<MultiClusterOptions>(options =>
                //{
                //    options.HasMultiClusterNetwork = true;
                //    options.DefaultMultiCluster = new[] { "us1", "eu1", "us2" };
                //    options.BackgroundGossipInterval = TimeSpan.FromSeconds(30);
                //    options.UseGlobalSingleInstanceByDefault = false;
                //    options.GlobalSingleInstanceRetryInterval = TimeSpan.FromSeconds(30);
                //    options.GlobalSingleInstanceNumberRetries = 3;
                //    options.MaxMultiClusterGateways = 10;
                //})
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(UserGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .UseDashboard(options => { })
                .Build();

            Task.Run(StartSilo);

            AssemblyLoadContext.Default.Unloading += context =>
            {
                Task.Run(StopSilo);
                siloStopped.WaitOne();
            };
            siloStopped.WaitOne();
        }

        private static async Task StartSilo()
        {
            await silo.StartAsync();
            Console.ForegroundColor = color;
            Console.WriteLine("Silo Started at {0}", DateTime.Now);
            Console.ResetColor();
        }

        private static async Task StopSilo()
        {
            await silo.StopAsync();
            Console.ForegroundColor = color;
            Console.WriteLine("Silo Stopped at {0}", DateTime.Now);
            Console.ResetColor();
        }
    }
}
