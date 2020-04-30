using System;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DragonLib.IO;
using JetBrains.Annotations;
using UObject.Asset;
using UObject.Generics;
using UObject.ObjectModel;

namespace UObject.Properties
{
    [PublicAPI]
    public class StructProperty : AbstractProperty
    {
        public Name StructName { get; set; } = new Name();
        public Guid StructGuid { get; set; } = System.Guid.Empty;

        [JsonIgnore]
        public PropertyGuid Guid { get; set; } = new PropertyGuid();

        public object? Value { get; set; }

        public override void Deserialize(Span<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Deserialize(buffer, asset, ref cursor, false);
            StructName.Deserialize(buffer, asset, ref cursor);
            StructGuid = SpanHelper.ReadStruct<Guid>(buffer, ref cursor);
            Guid.Deserialize(buffer, asset, ref cursor);
            if (!isArray) Value = ObjectSerializer.DeserializeStruct(buffer, asset, StructName, ref cursor);
        }

        public override void Serialize(ref Memory<byte> buffer, AssetFile asset, ref int cursor, bool isArray)
        {
            base.Serialize(ref buffer, asset, ref cursor);
            StructName.Serialize(ref buffer, asset, ref cursor);
            SpanHelper.WriteStruct(ref buffer, StructGuid, ref cursor);
            Guid.Serialize(ref buffer, asset, ref cursor);
            if (!isArray)
            {
                if (Value is UnrealObject uobject)
                    uobject.Serialize(ref buffer, asset, ref cursor);
                else if (Value != null)
                    unsafe
                    {
                        SpanHelper.EnsureSpace(ref buffer, cursor + Marshal.SizeOf(Value.GetType()));
                        fixed (byte* pin = &buffer.Span.Slice(cursor).GetPinnableReference())
                        {
                            var addr = (IntPtr) pin;
                            Marshal.StructureToPtr(Value, addr, true);
                        }
                    }
            }
        }
    }
}
