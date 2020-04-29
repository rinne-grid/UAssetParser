using System.IO;
using System.Text.Json;
using DragonLib.JSON;
using JetBrains.Annotations;
using UObject;

namespace UObject2JSON
{
    [PublicAPI]
    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                var uasset = File.ReadAllBytes(arg + ".uasset");
                var uexp = File.ReadAllBytes(arg + ".uexp");
                var json = JsonSerializer.Serialize(ObjectSerializer.Deserialize(uasset, uexp).ExportObjects, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters =
                    {
                        new GenericDictionaryConverterFactory(),
                        new GenericListConverterFactory()
                    }
                });
                File.WriteAllText(arg + ".json", json);
            }
        }
    }
}
