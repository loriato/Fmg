using Newtonsoft.Json;
using NHibernate.Proxy;
using System;

namespace Europa.Data
{
    public class NHibernateProxyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteNull();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(INHibernateProxy).IsAssignableFrom(objectType);
        }
    }
}
