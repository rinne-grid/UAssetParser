using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace UAsset2Json
{
	public class IgnoreDataBinding : DefaultContractResolver
	{
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);
			jsonProperty.Ignored = false;
			return jsonProperty;
		}
	}
}
