using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Europa.Fmg.Domain.Dto
{
    public class JExcelOptions
    {
        [JsonProperty("nestedHeaders")]
        public List<List<Header>> NestedHeaders { get; set; }
        [JsonProperty("columns")]
        public List<Column> Columns { get; set; }

        [JsonProperty("data")]
        public List<List<Object>> Data { get; set; }
    }
}