using System;

namespace UAssetParser
{
    public class ConditionalAttribute : Attribute
    {
        public ConditionalAttribute(string condition)
        {
            Condition = condition;
        }

        public string Condition { get; }
    }
}
