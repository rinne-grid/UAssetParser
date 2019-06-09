using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UAssetParser;
using UAssetParser.Objects.Visitors;

namespace UAsset2Json
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ExportDir(args);
        }

        private static void ExportDir(string[] args)
        {
            foreach (var arg in args)
            {
                if (Directory.Exists(arg))
                {
                    ExportDir(Directory.GetFiles(arg, "*.uasset"));
                    ExportDir(Directory.GetDirectories(arg));
                }
                else Export(arg);
            }
        }

        private static void Export(string path)
        {
            Console.WriteLine(path);
            using var assetStream = File.OpenRead(Path.ChangeExtension(path, "uasset"));
            using var expStream = File.OpenRead(Path.ChangeExtension(path, "uexp"));

            VisitorFactory.LoadVisitors();
            VisitorFactory.SetEnumAsByte();
            var asset = new UAsset(assetStream, expStream);
            var arr = asset.Summary.Exports.SelectMany(x => x.Objects).Select(x => x.ToDictionary()).ToArray();

            File.WriteAllText(Path.ChangeExtension(path, "json"), JsonConvert.SerializeObject(arr.Length < 2 ? (asset.Summary.Exports.FirstOrDefault()?.Objects.FirstOrDefault()?.ObjectData?.Serialize() ?? arr) : arr, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new IgnoreDataBinding()
            }));
        }
    }
}
