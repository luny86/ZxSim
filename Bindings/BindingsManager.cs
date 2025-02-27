
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("ZX.Tests")]

namespace Bindings
{
    /// <summary>
    /// Main class for handling data bindings.
    /// </summary>
    /// <remarks>
    /// Allows users to create, get and bind to value objects.
    /// Object creation if generic so any type can be used for
    /// a value.
    /// </remarks>
    internal class BindingsManager : IBindingManager
    {
        private Dictionary<string, IBoundObjectStore> _bindingValues;

        public BindingsManager()
        {
            _bindingValues = new Dictionary<string, IBoundObjectStore>();
        }

        public IBoundObject<T> GetObject<T>(string name)
        {
            if(_bindingValues.TryGetValue(name, out IBoundObjectStore? value))
            {
                try
                {
                    return (BoundObject<T>)Convert.ChangeType(value, typeof(BoundObject<T>));
                }
                
                catch(InvalidCastException)
                {
                    string typeName = typeof(T).Name;
                    throw new InvalidCastException($"Unable to convert object '{name}' to a Boundbject<{typeName}>.");
                }
            }
            
            throw new InvalidOperationException($"Unable to find bound object '{name}'.");
        } 

        /// <summary>
        /// Create a new binding, or update is it exists.
        /// </summary>
        public IBoundObject<T> CreateObject<T>(string name, T value)
        {
            IBoundObject<T> item;

            if (_bindingValues.TryGetValue(name, out IBoundObjectStore? stored) &&
                stored is not null)
            {
                item = (IBoundObject<T>)stored;
                item.Value = value;
            }
            else
            {
                item = new BoundObject<T>(name, value);
                _bindingValues.Add(name, (IBoundObjectStore)item);
            }

            return item;
        }

        public void Bind(string name, ValueChangedHandler valueChangedHandler)
        {
            if(_bindingValues.TryGetValue(name, out IBoundObjectStore? item) &&
                item is not null)
            {
                item.ValueChanged += valueChangedHandler;
            }
            else
            {
                throw new ArgumentException($"'{name}' not found in bindings.");
            }
        }
    }

    public delegate void ValueChangedHandler(string name, Type  type, object? value);
}