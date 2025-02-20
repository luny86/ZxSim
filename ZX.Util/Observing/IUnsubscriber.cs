
namespace ZX.Util.Observing
{
    /// <summary>
    /// Describes the object that is able to unsubscribe
    /// an <see cref="IObserver{T}"/> 
    /// </summary>
    /// <typeparam name="T">Type used for the arguments being passed around.</typeparam>
    public interface IUnsubscriber<T>
    {
        /// <summary>
        /// Releases the <see cref="IObserver{T}"/> from the
        /// subscription.
        /// </summary>
        /// <param name="observer">Object to release.</param>
        void Release(IObserver<T> observer);
    }
}