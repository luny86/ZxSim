
namespace Bindings
{
    /// <summary>
    /// Class for holding a value that can be bound to several other objects.
    /// </summary>
    internal class BoundObject<T> : IBoundObject<T>, IBoundObjectStore
    {
        public event ValueChangedHandler? ValueChanged;

        private T _value;

        public BoundObject(string name, T value)
        {
            Name = name;
            _value = value;
        }

        public string Name
        {
            get;
        }

        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                if(value == null || !value.Equals(_value))
                {
                    _value = value;

                    OnValueChanged<T>(value);
                }
            }
        }

        protected void OnValueChanged<C>(C value)
        {
            ValueChanged?.Invoke(Name, typeof(T), value);
        }
    }
}