namespace MarkdocsService
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using MarkdocsService.Handler;
    using MarkdocsService.Model;

    internal class MarkdownHttpServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private IHttpHandler _handler;
        private ManualResetEvent _processing = new ManualResetEvent(false);
        private int _status;

        public MarkdownHttpServer(IHttpHandler handler, string host, int port)
        {
            var urlPrefix = $"http://{host}:{port}/";
            _listener.Prefixes.Add(urlPrefix);
            _handler = handler;
        }

        public void Start()
        {
            var originalStatus = Interlocked.CompareExchange(ref _status, 1, 0);
            if (originalStatus != 0)
            {
                return;
            }

            _listener.Start();
            _processing.Reset();
            RunServerCore();
        }

        public void Stop()
        {
            var originalStatus = Interlocked.CompareExchange(ref _status, 0, 1);
            if (originalStatus != 1)
            {
                return;
            }

            _listener.Stop();
            _processing.Set();
        }

        public void WaitForExit()
        {
            _processing.WaitOne();
            _listener.Close();
            _processing = null;
        }

        private void RunServerCore()
        {
            _listener.BeginGetContext(async ar => await Task.Run(() => 
            {
                try
                {
                    var httpContext = _listener.EndGetContext(ar);
                    RunServerCore();
                    try
                    {
                        var context = new ServiceContext { HttpContext = httpContext, Server = this };
                        _handler.Handle(context);
                    }
                    catch (HandlerClientException ex)
                    {
                        Utility.ReplyClientErrorResponse(httpContext, $"Error occurs while handling context, {ex.Message}");
                    }
                    catch (HandlerServerException ex)
                    {
                        Utility.ReplyServerErrorResponse(httpContext, $"Error occurs while handling context, {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Utility.ReplyServerErrorResponse(httpContext, $"Error occurs, {ex.ToString()}");
                    }
                }
                catch (HttpListenerException)
                {
                }
            }), null);
        }
    }
}
