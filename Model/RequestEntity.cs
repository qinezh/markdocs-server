namespace MarkdocsService.Model
{
    using Newtonsoft.Json;

    internal class RequestEntity
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("basePath")]
        public string BasePath {get;set;}

        public static RequestEntity Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<RequestEntity>(content);
        }
    }
}
