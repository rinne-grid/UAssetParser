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
        public ValueTypeConverterFactory(bool aggressive) => Aggressive = aggressive;

        public bool Aggressive { get; set; }

        public override bool CanConvert(Type typeToConvert)
        {
            var test1 = typeToConvert.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition().IsEquivalentTo(typeof(IValueType<>)));
            var test2 = typeToConvert.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition().IsEquivalentTo(typeof(IArrayValueType<>)));

            if (!Aggressive && test2) return false;

            return test1;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var valueTypeInterface = typeToConvert.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition().IsEquivalentTo(typeof(IValueType<>)));
            return (JsonConverter) (Activator.CreateInstance(typeof(ValueTypeConverter<>).MakeGenericType(valueTypeInterface.GetGenericArguments()[0])) ?? throw new JsonException());
        }
    }
}
