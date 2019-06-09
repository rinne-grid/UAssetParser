using System;

namespace UAssetParser
{
    public class SizeAttribute : Attribute
    {
        public SizeAttribute(int size)
        {
            Size = size;
        }

        public int Size { get; }
    }
}
