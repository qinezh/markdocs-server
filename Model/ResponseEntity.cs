namespace MarkdocsService.Model
{
    using Newtonsoft.Json;

    internal class ResponseEntity
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        public static string Serialize(ResponseEntity entity)
        {
            return JsonConvert.SerializeObject(entity);
        }
    }
}
