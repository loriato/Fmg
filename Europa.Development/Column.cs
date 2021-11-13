using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Europa.Development
{
    public class Column
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("readOnly")]
        public bool ReadOnly { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("source")]
        public List<Object> Source { get; set; }
        [JsonProperty("url")]
        public String Url { get; set; }
    }
}
