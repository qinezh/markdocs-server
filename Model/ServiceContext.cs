namespace MarkdocsService.Model
{
    using System.Net;
    using MarkdocsService;

    internal class ServiceContext
    {
        public HttpListenerContext HttpContext { get; set; }

        public MarkdownHttpServer Server {get;set;}
    }
}
