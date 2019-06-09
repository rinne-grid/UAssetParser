using System.Runtime.Serialization;

namespace UAssetParser.Formats
{
    [DataContract]
    public class FPackageIndex
    {
        [DataMember]
        public int Index { get; set; }

        [IgnoreDataMember]
        public object ObjectResource { get; private set; }

        public object Ref(FObjectExport[] exports, FObjectImport[] imports)
        {
            if (ObjectResource != null) return ObjectResource;
            var importIndex = 0 - Index - 1;
            var exportIndex = Index - 1;
            if (IsImport && imports?.Length > importIndex) ObjectResource = imports[importIndex];
            if (IsExport && exports?.Length > exportIndex) ObjectResource = exports[exportIndex];
            return ObjectResource;
        }

        public object Ref(FPackageFileSummary summary)
        {
            if (summary == null) return ObjectResource;
            return Ref(summary.Exports, summary.Imports);
        }

        public bool IsImport => Index < 0;

        public bool IsExport => Index > 0;

        public bool IsNull => Index == 0;

        public string Name {
            get {
                if (ObjectResource is FObjectExport export) return export.ObjectName;
                else if (ObjectResource is FObjectImport import) return import.ObjectName;
                return null;
            }
        }

        public string FullName {
            get {
                if (ObjectResource is FObjectExport export) return export.ObjectName;
                else if (ObjectResource is FObjectImport import) return import.PackageRef.IsNull ? import.ObjectName : import.PackageRef.Name;
                return null;
            }
        }
    }
}
