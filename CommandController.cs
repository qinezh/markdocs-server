using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DocAsCode.Common;
using Microsoft.DocAsCode.MarkdigEngine;
using Microsoft.DocAsCode.Plugins;

namespace DocsPreviewService.Controllers
{
    [Route("")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        [HttpGet]
        public ActionResult RunCommand([FromQuery] string command)
        {
            if (string.Equals("exit", command, StringComparison.OrdinalIgnoreCase))
            {
                Program.Shutdown();
                return Ok();
            }

            if (string.Equals("ping", command, StringComparison.OrdinalIgnoreCase))
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<string> Markup([FromBody] MarkupRequest data)
        {
            return Markup(data.Content, data.FilePath, data.BasePath);
        }

        public class MarkupRequest
        {
            public string Content { get; set; }
            public string BasePath { get; set; }
            public string FilePath { get; set; }
        }

        private static string Markup(string markdown, string filePath, string basePath = ".")
        {
            EnvironmentContext.FileAbstractLayerImpl = FileAbstractLayerBuilder.Default.ReadFromRealFileSystem(basePath).WriteToRealFileSystem(basePath).Create();
            var parameter = new MarkdownServiceParameters
            {
                BasePath = basePath,
                Extensions = new Dictionary<string, object>
                {
                    { "EnableSourceInfo", true }
                }
            };
            var markupService = new MarkdigMarkdownService(parameter);
            return markupService.Markup(markdown, filePath ?? "topic.md").Html;
        }
    }
}
