using Newtonsoft.Json;

namespace Europa.Fmg.Domain.Dto
{
    public class Header
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("colspan")]
        public int Colspan { get; set; }
        [JsonProperty("rowspan")]
        public int Rowspan { get; set; }
        [JsonProperty("headerId")]
        public long Id { get; set; }
    }
}
