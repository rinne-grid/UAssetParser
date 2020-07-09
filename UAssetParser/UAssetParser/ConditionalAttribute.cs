using System;

namespace UAssetParser
{
	public class ConditionalAttribute : Attribute
	{
		public string Condition
		{
			get;
		}

		public ConditionalAttribute(string condition)
		{
			Condition = condition;
		}
	}
}
