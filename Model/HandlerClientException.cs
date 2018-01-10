namespace MarkdocsService.Model
{
    internal class HandlerClientException : HandlerException
    {
        public HandlerClientException() : this("Client error happens while handling http context")
        {
        }

        public HandlerClientException(string message) : base(message)
        {
        }
    }
}