using System;
using System.Collections.Generic;
using UObject.ObjectModel;
using UObject.Package;
using UObject.Properties;
using UObject.Structs;

namespace UObject
{
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

        public static (PackageFileSummary summary, List<UnrealObject> exports) Deserialize(Span<byte> uasset, Span<byte> uexp)
        {
        }

        public static Span<byte> SerializeExports(ref PackageFileSummary summary, List<UnrealObject> uexp)
        {
        }

        public static Span<byte> SerializeSummary(PackageFileSummary summary, List<UnrealObject> uasset)
        {
        }
    }
}
