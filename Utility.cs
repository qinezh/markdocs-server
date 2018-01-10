namespace MarkdocsService
{
    using System.Net;
    using System.Text;


    internal class Utility
    {
        public static void ReplySuccessfulResponse(HttpListenerContext context, string content, string contentType)
        {
            var response = context.Response;
            var buffer = Encoding.UTF8.GetBytes(content);
            response.ContentLength64 = buffer.Length;
            response.ContentType = contentType;
            using (var write = response.OutputStream)
            {
                write.Write(buffer, 0, buffer.Length);
            }
            response.Close();
        }

        public static void ReplyClientErrorResponse(HttpListenerContext context, string message)
        {
            ReplyResponse(context, HttpStatusCode.BadRequest, message);
        }

        public static void ReplyServerErrorResponse(HttpListenerContext context, string message)
        {
            ReplyResponse(context, HttpStatusCode.InternalServerError, message);
        }

        public static void ReplyNoContentResponse(HttpListenerContext context, string message)
        {
            ReplyResponse(context, HttpStatusCode.NoContent, message);
        }

        public static void ReplyResponse(HttpListenerContext context, HttpStatusCode statusCode, string message)
        {
            var response = context.Response;
            response.StatusCode = (int)statusCode;
            response.StatusDescription = message;
            response.Close();
        }
    }
}
