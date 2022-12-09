using Newtonsoft.Json;

namespace bbt.framework.common.Helper
{
    public class JsonHelper<T>
    {
        JsonSerializerSettings settings;
        public JsonHelper()
        {
            settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        public T DeserializeObject(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        public string SerializeObject(T value)
        {
            return JsonConvert.SerializeObject(value, Formatting.None, settings);
        }
    }
}
