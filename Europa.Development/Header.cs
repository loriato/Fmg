using Newtonsoft.Json;

namespace Europa.Development
{
    public class Header
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("colspan")]
        public int Colspan { get; set; }
    }
}
