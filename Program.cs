﻿namespace MarkdocsService
{
    using System;
    using MarkdocsService.Handler;

    internal class Program
    {
        private static readonly string HOST = "localhost";
        private static readonly int PORT = 4462;

        public static void Main(string[] args)
        {
            HandleRequest();
        }

        private static void HandleRequest()
        {
            var handler = new CompositeHandler(
                    new MarkdownPreviewHandler(),
                    new ExitHandler()
                    );

            var service = new MarkdownHttpServer(handler, HOST, PORT);
            service.Start();
            Console.WriteLine("Start...");
            service.WaitForExit();
            Console.WriteLine("Exit.");
        }
    }
}
