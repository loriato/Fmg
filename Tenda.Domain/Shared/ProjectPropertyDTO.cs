using System;

namespace Tenda.Domain.Shared
{
    public class ProjectPropertyDto
    {
        public string Key { get; set; }
        public DateTime LastUpdate { get; set; }
        public object Value { get; set; }
        public int TimeToLive { get; set; }
    }

    //FIXME: Use Typed
    public class ProjectPropertyDTO<T> : ProjectPropertyDto
    {
        public T ConvertedValue { get; set; }
    }
}
