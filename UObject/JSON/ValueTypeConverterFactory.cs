using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace UObject.JSON
{
    [PublicAPI]
    public class ValueTypeConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValueType<>));
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var valueTypeInterface = typeToConvert.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValueType<>));
            return (JsonConverter) (Activator.CreateInstance(typeof(ValueTypeConverter<>).MakeGenericType(valueTypeInterface.GetGenericArguments()[0])) ?? throw new JsonException());
        }
    }
}
