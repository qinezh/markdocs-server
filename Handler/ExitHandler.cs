namespace MarkdocsService.Handler
{
    using System;
    using System.Net.Http;
    using MarkdocsService.Model;

    /// <summary>
    /// GET ?command=exit
    /// </summary>
    internal class ExitHandler : IHttpHandler
    {
        public bool CanHandle(ServiceContext context)
        {
            if (context == null)
            {
                throw new HandlerServerException($"{nameof(context)} can't be null");
            }

            var request = context.HttpContext?.Request;
            if (request == null)
            {
                throw new HandlerServerException($"{nameof(request)} can't be null");
            }

            if (request.HttpMethod != HttpMethod.Get.ToString())
            {
                return false;
            }

            var command = request.QueryString.Get("command");
            if (string.Equals(command, "exit", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public void Handle(ServiceContext context)
        {
            Utility.ReplyNoContentResponse(context.HttpContext, "Markdown http service exits.");
            context.Server.Stop();
        }
    }
}
