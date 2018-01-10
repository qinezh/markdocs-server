namespace MarkdocsService.Model
{
    internal class HandlerServerException : HandlerException
    {
        public HandlerServerException() : this("Server error happens while handling http context")
        {
        }

        public HandlerServerException(string message) : base(message)
        {
        }
    }
}