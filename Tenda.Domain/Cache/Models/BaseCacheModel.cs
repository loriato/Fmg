using System;

namespace Europa.Fmg.Domain.Cache.Models
{
    public class BaseCacheModel
    {
        public string CacheId { get; set; }
        public DateTime LastUpdate { get; set; }
        public object Cache { get; set; }
    }
}
