using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

// TODO: Move to DragonLib
namespace UObject.JSON
{
    [PublicAPI]
    public class GenericTypelessDictionaryConverter<T> : JsonConverter<Dictionary<string, T>>
    {
        public override Dictionary<string, T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, Dictionary<string, T> dict, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var (key, value) in dict)
            {
                writer.WritePropertyName(key);
                JsonSerializer.Serialize(writer, value, value?.GetType(), options);
            }

            writer.WriteEndObject();
        }
    }
}
