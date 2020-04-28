using JetBrains.Annotations;

namespace UObject.Package
{
    [PublicAPI]
    public class GenerationInfo
    {
        public int ExportCount { get; set; }
        public int NameCount { get; set; }
    }
}
