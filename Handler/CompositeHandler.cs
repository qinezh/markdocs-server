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
                    try
                    {
                        handler.Handle(context);
                        return;
                    }
                    catch (HandlerClientException)
                    {
                        throw;
                    }
                    catch (HandlerServerException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Utility.ReplyServerErrorResponse(context.HttpContext, $"{handler.GetType().Assembly.GetName()} failed, {ex.Message}");
                        return;
                    }
                }
            }

            Utility.ReplyClientErrorResponse(context.HttpContext, "No handler can processes the request");
        }
    }
}
