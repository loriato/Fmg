using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tenda.EmpresaVenda.Domain.Dto
{
    public class Column
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("readOnly")]
        public bool? ReadOnly { get; set; }
        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public string Width { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("source")]
        public List<Object> Source { get; set; }
        [JsonProperty("url")]
        public String Url { get; set; }
        [JsonProperty("mask")]
        public String Mask { get; set; }
        [JsonProperty("decimal")]
        public String Decimal { get; set; }
        [JsonProperty("decimalcount")]
        public int DecimalCount { get; set; }
        [JsonProperty("options")]
        public ColumnOptions Options { get; set; }
        [JsonProperty("allowEmpty")]
        public bool AllowEmpty { get; set; }
        [JsonProperty("wordWrap")]
        public bool WordWrap { get; set; }
        [JsonProperty("regex")]
        public String Regex { get; set; }
    }

    public class ColumnOptions
    {
        [JsonProperty("reverse")]
        public bool? Reverse { get; set; }
        [JsonProperty("fixed")]
        public bool? Fixed { get; set; }
    }
}
