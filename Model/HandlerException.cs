namespace MarkdocsService.Model
{
    using System;

    internal abstract class HandlerException : Exception
    {
        public HandlerException() : this("Error happens while handling http context")
        {
        }

        public HandlerException(string message) : base(message)
        {
        }
    }
}