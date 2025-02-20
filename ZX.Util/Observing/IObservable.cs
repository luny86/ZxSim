
namespace ZX.Util.Observing
{
    /// <summary>
    /// Describes an object which can be watched by an
    /// <see cref="ZX.Util.Observing.IObserver"/> 
    /// </summary>
    /// <typeparam name="T">
    /// Class or struct holding arguments
    /// to be sent when <see cref="Subscriber.OnChanged(T)"/>
    /// is invoked.
    /// </typeparam>
    /// <remarks>
    /// The best design is for the <see cref="IObservable{T}"/>
    /// implmentation to own an instance of the
    /// <see cref="Subscriber"/> object and to return
    /// it as the unsubscriber. The implemtation should
    /// call <see cref="Subscriber.OnChanged(T)"/> when something
    /// needs to be updated.
    /// </remarks>
    public interface IObservable<T>
    {
        IUnsubscriber<T> Subscribe(IObserver<T> observer);
    }
}