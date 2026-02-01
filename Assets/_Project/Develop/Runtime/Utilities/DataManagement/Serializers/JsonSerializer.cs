using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Develop.Runtime.Utilities.DataManagement.Serializers
{
    public class JsonSerializer : IDataSerializer
    {
        public string Serialize<TData>(TData data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public TData Deserialize<TData>(string serializedData)
        {
            return JsonConvert.DeserializeObject<TData>(serializedData, new JsonSerializerSettings
            {
                Formatting = Application.isEditor ? Formatting.Indented : Formatting.None,
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}
