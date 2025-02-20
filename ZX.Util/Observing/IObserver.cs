
namespace ZX.Util.Observing
{
    /// <summary>
    /// Describes an observer object that can watch for a change
    /// in an <see cref="IObservable{T}"/> object.
    /// </summary>
    /// <typeparam name="T">Argument object to pass on when updating.</typeparam>
    public interface IObserver<T>
    {
        /// <summary>
        /// Invoked when the obserable object has
        /// a changed.
        /// </summary>
        void ObserableChanged(T arguments);
    }
}