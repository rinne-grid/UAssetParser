using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UAssetParser;
using UAssetParser.Attributes;
using UAssetParser.Formats;
using UAssetParser.Objects;
using UAssetParser.Objects.Visitors;
using UAssetParser.Objects.Visitors.Unreal;

namespace UAsset2Json
{
	public class Program
	{
		private const string version = "\nCurrent version: v0.2.a (17 Aug 2019) KZ";

		private const string gameVer = "\nSupported game version: v1.05 (PC)";

		private const string helpMsg = "\nUSAGE:\nUAsset2Json <action1> [action2] [...] [path or file]\n\nEXAMPLE:\nUAsset2Json -tojson -dumpnames d:\\assets\\my.uasset\nACTIONS:\n -tojson     \tdeserialize uasset(s) into JSON\n -tobin      \tupdates .uasset(s) with changes from JSON\n -dumpnames  \tdumps all names from uasset into .names file\n -dumpmeta   \tdumps some meta from uasset into .meta file\n -instnums   \tadds instance numbers > 0 to FName values in JSON\n -fullinst   \tadds instance numbers > 0 to every FName\n -newentries \tallow new entries (experimental, unstable)\n -verbose    \tprint warnings for some actions\n -force      \tforces updating of complex arrays from JSON\n";

		private static string reply = string.Empty;

		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (args == null || args.Length < 2)
			{
				Console.Write("No proper arguments were specified or input file not found.\nUSAGE:\nUAsset2Json <action1> [action2] [...] [path or file]\n\nEXAMPLE:\nUAsset2Json -tojson -dumpnames d:\\assets\\my.uasset\nACTIONS:\n -tojson     \tdeserialize uasset(s) into JSON\n -tobin      \tupdates .uasset(s) with changes from JSON\n -dumpnames  \tdumps all names from uasset into .names file\n -dumpmeta   \tdumps some meta from uasset into .meta file\n -instnums   \tadds instance numbers > 0 to FName values in JSON\n -fullinst   \tadds instance numbers > 0 to every FName\n -newentries \tallow new entries (experimental, unstable)\n -verbose    \tprint warnings for some actions\n -force      \tforces updating of complex arrays from JSON\n\nCurrent version: v0.2.a (17 Aug 2019) KZ\nSupported game version: v1.05 (PC)");
				return;
			}
			string[] actions = (from x in RuntimeHelpers.GetSubArray(args, new Range(0, new Index(1, fromEnd: true)))
				select x.ToLower()).Distinct().ToArray();
			string text = args[new Index(1, fromEnd: true).GetOffset(args.Length)].Trim('"');
			if (File.Exists(text))
			{
				ExportDir(actions, new string[1]
				{
					text
				});
			}
			else if (Directory.Exists(text))
			{
				ExportDir(actions, Directory.GetFiles(text, "*.uasset", SearchOption.AllDirectories));
			}
			Console.Write("Done");
		}

		private static void ExportDir(string[] actions, string[] files)
		{
			VisitorFactory.LoadVisitors();
			foreach (string filepath in files)
			{
				ProcessFile(actions, filepath);
			}
		}

		private static void ProcessFile(string[] actions, string filepath)
		{
			Console.WriteLine(filepath);
			string text = Path.ChangeExtension(filepath, "uasset");
			if (!File.Exists(text))
			{
				Console.WriteLine("Unable to open " + text);
				return;
			}
			UAsset.Options = new UAParserOptions(actions.Contains("-verbose"), actions.Contains("-force"), actions.Contains("-instnums"), actions.Contains("-fullinst"), actions.Contains("-newentries"));
			using (FileStream uassetStream = File.OpenRead(text))
			{
				UAsset uAsset = new UAsset(uassetStream);
				for (int i = 0; i < actions.Length; i++)
				{
					switch (actions[i])
					{
					case "-tojson":
					{
						Dictionary<string, object>[] array = (from x in uAsset.Summary.Exports.SelectMany((FObjectExport x) => x.Objects)
							select x.ToDictionary()).ToArray();
						File.WriteAllText(Path.ChangeExtension(filepath, "json"), JsonConvert.SerializeObject((array.Length >= 2) ? array : (uAsset.Summary.Exports.FirstOrDefault()?.Objects.FirstOrDefault()?.ObjectData?.Serialize() ?? array), Formatting.Indented, new JsonSerializerSettings
						{
							ContractResolver = new IgnoreDataBinding()
						}));
						break;
					}
					case "-dumpmeta":
						DumpMeta(uAsset, filepath);
						break;
					case "-dumpnames":
						File.WriteAllLines(Path.ChangeExtension(filepath, "names"), uAsset.Summary.Names.Select((FNameEntry x) => x.Name));
						break;
					case "-tobin":
					{
						if (uAsset.Summary.ExportCount > 1)
						{
							throw new NotImplementedException("Export Count > 1 is not supported!");
						}
						string text2 = Path.ChangeExtension(filepath, "json");
						if (!File.Exists(text2))
						{
							Console.WriteLine("Unable to open " + text2);
							break;
						}
						new UAParserOptions(actions.Contains("-verbose"), actions.Contains("-force"));
						uAsset.UpdateFromJSON(File.ReadAllText(text2, Encoding.UTF8));
						uAsset.SerializeToBinary(text2);
						break;
					}
					}
				}
			}
		}

		private static void DumpMeta(UAsset asset, string filepath)
		{
			PropertyInfo[] array = (from x in asset.Summary.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where x.GetCustomAttribute<TextDumpable>() != null
				select (x)).ToArray();
			using (FileStream stream = new FileStream(Path.ChangeExtension(filepath, "meta"), FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
				{
					PropertyInfo[] array2 = array;
					foreach (PropertyInfo propertyInfo in array2)
					{
						object value = propertyInfo.GetValue(asset.Summary);
						object obj;
						if (!propertyInfo.PropertyType.IsArray)
						{
							obj = value;
						}
						else
						{
							object[] obj2 = value as object[];
							obj = ((obj2 != null) ? obj2.Length : 0);
						}
						value = obj;
						streamWriter.WriteLine($"{propertyInfo.Name}: {value}");
					}
					FObjectExport[] exports = asset.Summary.Exports;
					foreach (FObjectExport fObjectExport in exports)
					{
						streamWriter.WriteLine("\n");
						streamWriter.WriteLine("TYPE DATA FOR " + fObjectExport.ObjectName.Name + ":");
						if (!(fObjectExport.ClassIndex.Name == "DataTable"))
						{
							continue;
						}
						foreach (FPropertyTag item in ((UDataTable)(fObjectExport.Objects?.FirstOrDefault().ObjectData)).FirstOrDefault().Value)
						{
							string text = (item.Type.Name == "ArrayProperty") ? ("[" + (item as UArrayProperty).ArrayType.Name + "]") : string.Empty;
							streamWriter.WriteLine($"{item.Name.Name} ({item.Name.Index}|{item.Name.InNumber}) -> {item.Type.Name} ({item.Type.Index}|{item.Type.InNumber}) {text}");
						}
					}
				}
			}
		}
	}
}
