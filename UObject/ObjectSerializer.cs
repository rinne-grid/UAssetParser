using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.ObjectModel;
using UObject.Package;
using UObject.Properties;
using UObject.Structs;

namespace UObject
{
    [PublicAPI]
    public static class ObjectSerializer
    {
        public static Dictionary<string, Type> PropertyTypes { get; } = new Dictionary<string, Type>
        {
            { nameof(StructProperty), typeof(StructProperty) }
        };

        public static Dictionary<string, Type> StructTypes { get; } = new Dictionary<string, Type>
        {
            { nameof(Box), typeof(Box) },
            { nameof(Box2D), typeof(Box2D) },
            { nameof(Color), typeof(Color) },
            { nameof(IntPoint), typeof(IntPoint) },
            { nameof(LinearColor), typeof(LinearColor) },
            { nameof(Rotator), typeof(Rotator) },
            { nameof(Vector), typeof(Vector) },
            { nameof(Vector2D), typeof(Vector2D) }
        };

        public static Dictionary<string, Type> ExportTypes { get; } = new Dictionary<string, Type>
        {
            { nameof(DataTable), typeof(DataTable) },
            { nameof(StringTable), typeof(StringTable) }
        };

        public static AssetFile Deserialize(Span<byte> uasset, Span<byte> uexp) => new AssetFile(uasset, uexp);

        public static Span<byte> SerializeExports(ref PackageFileSummary summary, List<UnrealObject> uexp) => throw new NotImplementedException();

        public static Span<byte> SerializeSummary(PackageFileSummary summary, List<UnrealObject> uasset) => throw new NotImplementedException();

        public static string DeserializeString(Span<byte> buffer) => throw new NotImplementedException();

        public static Span<byte> SerializeString(Span<byte> buffer) => throw new NotImplementedException();
    }
}
