using DragonAge2CameraTools.ViewLogic.Settings.Interfaces;
using Newtonsoft.Json;

namespace DragonAge2CameraTools.ViewLogic.Settings
{
    public class JsonSerializer : IStringSerializer
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public JsonSerializer(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
        }
        
        public string Serialize(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, _jsonSerializerSettings);
        }

        public T Deserialize<T>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject, _jsonSerializerSettings);
        }
    }
}