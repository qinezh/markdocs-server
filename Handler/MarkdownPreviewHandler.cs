namespace MarkdocsService.Handler
{
    using System.IO;
    using System.Net.Http;
    using System.Collections.Generic;
    using MarkdocsService.Model;
    using Newtonsoft.Json;
    using Microsoft.DocAsCode.MarkdigEngine;
    using Microsoft.DocAsCode.MarkdigEngine.Extensions;
    using Microsoft.DocAsCode.Plugins;
    using Microsoft.DocAsCode.Common;

    /// <summary>
    /// POST {content: <markdown>}
    /// </summary>
    internal class MarkdownPreviewHandler : IHttpHandler
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

            if (request.HttpMethod != HttpMethod.Post.ToString())
            {
                return false;
            }

            if (!request.HasEntityBody)
            {
                throw new HandlerClientException("No body in this request");
            }

            return true;
        }

        public void Handle(ServiceContext context)
        {
            var request = context.HttpContext.Request;

            string content;
            using (var body = request.InputStream)
            {
                using (var reader = new StreamReader(body, request.ContentEncoding))
                {
                    content = reader.ReadToEnd();
                }
            }

            RequestEntity entity;
            try
            {
                entity = JsonConvert.DeserializeObject<RequestEntity>(content);
            }
            catch (JsonException ex)
            {
                throw new HandlerClientException($"Can't parse request body, {ex.Message}");
            }

            var result = Markup(entity);
            Utility.ReplySuccessfulResponse(context.HttpContext, result, ContentType.Json);
        }

        private string Markup(RequestEntity entity)
        {
            var responseEntity = new ResponseEntity { Content = Markup(entity.Content, entity.FilePath, entity.BasePath) };
            return ResponseEntity.Serialize(responseEntity);
        }

        private string Markup(string markdown, string filePath = "topic.md", string basePath = ".")
        {
            EnvironmentContext.FileAbstractLayerImpl = FileAbstractLayerBuilder.Default.ReadFromRealFileSystem(basePath).WriteToRealFileSystem(basePath).Create();
            var parameter = new MarkdownServiceParameters
            {
                BasePath = basePath,
                Extensions = new Dictionary<string, object>
                {
                    { LineNumberExtension.EnableSourceInfo, true }
                }
            };
            var markupService = new MarkdigMarkdownService(parameter);
            return markupService.Markup(markdown, filePath).Html;
        }
    }
}