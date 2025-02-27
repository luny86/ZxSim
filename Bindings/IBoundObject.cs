
namespace Bindings
{

    /// <summary>
    /// Base object for storing and dealing with genericallwithout a type.
    /// </summary>
    internal interface IBoundObjectStore
    {
        event ValueChangedHandler ValueChanged;
    }

    /// <summary>
    /// Public interface for a bound object.
    /// </summary>
    /// <typeparam name="T">Type used as a value.</typeparam>
    public interface IBoundObject<T>
    {
        string Name { get; }
        T Value { get; set; }
    }
}