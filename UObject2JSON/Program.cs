using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DragonLib.CLI;
using DragonLib.IO;
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
            Logger.PrintVersion("UAsset");
            var flags = CommandLineFlags.ParseFlags<ProgramFlags>(CommandLineFlags.PrintHelp, args);
            if (flags == null) return;
            var paths = new List<string>();
            // TODO: Move to DragonLib
            foreach (var path in flags.Paths)
            {
                if (Directory.Exists(path))
                    paths.AddRange(Directory.GetFiles(path, "*.uasset", SearchOption.AllDirectories));
                else if (File.Exists(path)) paths.Add(path);
            }

            foreach (var path in paths)
            {
                var arg = Path.Combine(Path.GetDirectoryName(path) ?? ".", Path.GetFileNameWithoutExtension(path));
                var uasset = File.ReadAllBytes(arg + ".uasset");
                var uexp = File.ReadAllBytes(arg + ".uexp");
                Logger.Info("UAsset", arg);
                var json = JsonSerializer.Serialize(ObjectSerializer.Deserialize(uasset, uexp).ExportObjects, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters =
                    {
                        new GenericDictionaryConverterFactory(),
                        new GenericListConverterFactory()
                    }
                });

                if (!string.IsNullOrWhiteSpace(flags.OutputFolder))
                {
                    arg = Path.Combine(flags.OutputFolder, Path.GetFileName(arg));
                    if (!Directory.Exists(flags.OutputFolder)) Directory.CreateDirectory(flags.OutputFolder);
                }

                File.WriteAllText(arg + ".json", json);
            }
        }
    }
}
