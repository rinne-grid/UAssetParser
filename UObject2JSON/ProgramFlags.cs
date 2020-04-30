using System.Collections.Generic;
using DragonLib.CLI;
using JetBrains.Annotations;

namespace UObject2JSON
{
    [PublicAPI]
    public class ProgramFlags : ICLIFlags
    {
        [UsedImplicitly]
        [CLIFlag("paths", Positional = 0, Category = "Program Arguments", Help = "Paths to load")]
        public List<string> Paths { get; set; } = new List<string>();

        [CLIFlag("output", Aliases = new[] { "o" }, Category = "Program Arguments", Help = "Folder to output JSON files to")]
        public string? OutputFolder { get; set; }
    }
}
