using System.Collections.Generic;
using JetBrains.Annotations;
using UObject.Properties;

namespace UObject.ObjectModel
{
    [PublicAPI]
    public interface IObject : IObjectProperty
    {
        public Dictionary<string, object> KeyValues { get; set; }
    }
}
