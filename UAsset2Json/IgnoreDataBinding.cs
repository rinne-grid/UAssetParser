using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace UAsset2Json
{
    public class IgnoreDataBinding : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.Ignored = false;
            return property;
        }
    }
}