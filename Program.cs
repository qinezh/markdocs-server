using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace DocsPreviewService
{
    public class Program
    {
        private static readonly CancellationTokenSource s_cancelTokenSource = new CancellationTokenSource();

        public static async Task Main(string[] args)
        {
            var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseStartup<Startup>()
                        .UseUrls("http://localhost:4462")
                        .Build();
            try
            {
                await host.RunAsync(s_cancelTokenSource.Token);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Shutdown()
        {
            s_cancelTokenSource.Cancel();
        }
    }
}
