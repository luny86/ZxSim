
using System;
using System.Collections.Generic;

namespace ZX.Util.Observing
{
    /// <summary>
    /// Handles all of the subscriptions for <see cref="IObserver{T}"/>
    /// where T is of the same type for all subscribing.
    /// </summary>
    /// <typeparam name="T">Argument used when invoking an update.</typeparam>
    /// <remarks>
    /// The <see cref="Subscriber{T}"/> is generally owned by the 
    /// instance implementing <see cref="IObservable{T}"/>. It is this
    /// instance which should invokes <see cref="Subscriber{T}.OnChanged(T)"/>. 
    public class Subscriber<T> : IUnsubscriber<T>
    {
        private List<IObserver<T>> _observers = new List<IObserver<T>>();

        private IObservable<T> _observable = null!;


        public Subscriber(IObservable<T> observable)
        {
            _observable = observable;
        }

        public void Subscribe(IObserver<T> observer)
        {
            _observers.Add(observer);
        }

        void IUnsubscriber<T>.Release(IObserver<T> observer)
        {
            _observers.Remove(observer);
        }

        public void OnChanged(T args)
        {
            foreach(var observer in _observers)
            {
                observer.ObserableChanged(args);
            }
        }
    }
}