using System;
using JetBrains.Annotations;

namespace UObject.Package
{
    [PublicAPI]
    public struct CustomVersion
    {
        public Guid Key { get; set; }
        public int Version { get; set; }
    }
}
