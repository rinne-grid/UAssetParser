using JetBrains.Annotations;

namespace UObject.Enum
{
    [PublicAPI]
    public enum DynamicType : uint
    {
        NotDynamicExport,
        DynamicType,
        ClassDefaultObject
    }
}
