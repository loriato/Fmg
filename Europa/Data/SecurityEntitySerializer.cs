using Europa.Data.Model;
using Newtonsoft.Json;

namespace Europa.Data
{
    public static class SecurityEntitySerializer
    {
        private static JsonSerializerSettings _jsonSerializerSettings = null;

        public static string Serialize(BaseEntity entity)
        {
            return JsonConvert.SerializeObject(entity, Current());
        }

        public static JsonSerializerSettings Current()
        {
            if (_jsonSerializerSettings == null)
            {
                _jsonSerializerSettings = Default();
            }
            return _jsonSerializerSettings;
        }

        public static JsonSerializerSettings NewInstance()
        {
            return Default();
        }

        private static JsonSerializerSettings Default()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();

            // O que o Serializador vai fazer quando encontrar um loog na referência
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            // Grava a referência da memória do objeto na serialização
            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            settings.Formatting = Formatting.None;

            // Personalizações na geração do log
            settings.ContractResolver = new NHibernateContractResolver();
            // Não exportando valores nulos
            settings.NullValueHandling = NullValueHandling.Ignore;

            // Pega os atributos relevantes
            settings.Converters.Add(new DynamicProxyJsonConverter());

            return settings;
        }

    }
}
