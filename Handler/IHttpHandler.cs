namespace MarkdocsService.Handler
{
    using MarkdocsService.Model;
    
    internal interface IHttpHandler
    {
        bool CanHandle(ServiceContext context);
        void Handle(ServiceContext context);
    }
}