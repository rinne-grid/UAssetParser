using JetBrains.Annotations;

namespace UObject.JSON
{
    [PublicAPI]
    public interface IValueType<T>
    {
        public T Value { get; set; }
    }
}
