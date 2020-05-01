using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

// TODO: Move to DragonLib
namespace UObject.JSON
{
    [PublicAPI]
    public class GenericTypelessDictionaryConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType) return false;

            return typeToConvert.GetGenericTypeDefinition() == typeof(Dictionary<,>) && typeToConvert.GetGenericArguments()[0].IsEquivalentTo(typeof(string));
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) => (JsonConverter) (Activator.CreateInstance(typeof(GenericTypelessDictionaryConverter<>).MakeGenericType(typeToConvert.GetGenericArguments()[1])) ?? throw new JsonException());
    }
}
