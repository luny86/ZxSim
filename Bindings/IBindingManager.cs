
namespace Bindings
{
    /// <summary>
    /// Interface describing a binding manager.
    /// </summary>
    /// <remarks>
    /// This is the main interface used by external modules.
    /// Users can create a bindable object and bind to it.
    /// Each object should have a unique name.
    /// <remarks>
    public interface IBindingManager
    {
        /// <summary>
        /// Create a bindable object.
        /// </summary>
        /// <typeparam name="T">Type of value stored.</typeparam>
        /// <param name="name">Name of bindable object.</param>
        /// <param name="value">Initial value of type T.</param>
        /// <returns>Bindable object created, so that it can be owned by the creator.</returns>
        IBoundObject<T> CreateObject<T>(string name, T value);

        /// <summary>
        /// Bind to an existing object.
        /// </summary>
        /// <param name="name">Name of object</param>
        /// <param name="valueChangedHandler">delegate used to receive any value changes.</param>
        /// <remarks>
        /// Allows none owning instances to watch and receive value changes.
        /// Method should throw an exception to indicate if a name is not found.
        /// </remarks>
        void Bind(string name, ValueChangedHandler valueChangedHandler);

        /// <summary>
        /// Attempts to get a binding object based on its name.
        /// </summary>
        /// <typeparam name="T">Type binding object uses as a value.</typeparam>
        /// <param name="name">Name of binding object.</param>
        /// <returns>Reference to the object if found.</returns>
        IBoundObject<T> GetObject<T>(string name);

    }
}