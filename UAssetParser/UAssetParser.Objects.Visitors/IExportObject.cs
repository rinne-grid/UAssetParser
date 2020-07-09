using System.Collections;
using System.IO;
using UAssetParser.Formats;

namespace UAssetParser.Objects.Visitors
{
	public interface IExportObject : IEnumerable
	{
		object Serialize();

		void BSerialize(BinaryWriter writer, FPackageFileSummary summary);

		void UpdateFromJSON(object jdata, FPackageFileSummary summary);

		void UpdateIndex(string[] names, FPackageFileSummary summary);
	}
}
