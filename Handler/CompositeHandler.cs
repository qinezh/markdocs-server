namespace MarkdocsService.Handler
{
    using System;
    using MarkdocsService.Model;

    internal class CompositeHandler : IHttpHandler
    {
        private IHttpHandler[] _handlers;

        public CompositeHandler(params IHttpHandler[] handlers)
        {
            _handlers = handlers;
        }

        public bool CanHandle(ServiceContext context)
        {
            return true;
        }

        public void Handle(ServiceContext context)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(context))
                {
                    handler.Handle(context);
                    return;
                }
            }

            Utility.ReplyClientErrorResponse(context.HttpContext, "No handler can processes the request");
        }
    }
}
