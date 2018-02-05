namespace MarkdocsService
{
    using System;
    using MarkdocsService.Handler;

    internal class Program
    {
        private static readonly string HOST = "127.0.0.1";
        private static readonly int PORT = 4462;

        public static void Main(string[] args)
        {
            HandleRequest();
        }

        private static void HandleRequest()
        {
            var handler = new CompositeHandler(
                    new MarkdownPreviewHandler(),
                    new ExitHandler(),
                    new PingHandler()
                    );

            var service = new MarkdownHttpServer(handler, HOST, PORT);
            service.Start();
            Console.WriteLine("Start...");
            service.WaitForExit();
            Console.WriteLine("Exit.");
        }
    }
}
