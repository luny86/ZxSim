
using System.Text;

namespace Builder
{
    /// <summary>
    /// Holds information on all dependencies registered
    /// by all buildables.
    /// </summary>
    /// <remarks>
    /// Infomation is: {string: scope name, {Type, object} }
    /// </remarks>
    internal class DependencyPool : 
        IDependencyPool,
        IDependencies
    {
        private class InnerPool : Dictionary<Type, object>
        {
        }

        private readonly Dictionary<string, InnerPool> _pool;

        public DependencyPool()
        {
            _pool = new Dictionary<string, InnerPool>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"DependencyPool : {_pool.Count}");
            sb.AppendLine();
            foreach(var kvp in _pool)
            {
                foreach(var inner in kvp.Value)
                {
                    sb.AppendLine($" - {inner.Key} = {inner.Value}");
                }
            }
            return sb.ToString();
        }

        void IDependencyPool.Add(string scope, Type type, object instance)
        {
            if(string.IsNullOrWhiteSpace(scope))
            {
                throw new ArgumentNullException(nameof(scope));
            }

            if(instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if(!instance.GetType().IsAssignableTo(type))
            {
                throw new ArgumentException($"Instance given does not implement {type.FullName}");
            }

            if(!_pool.TryGetValue(scope, out InnerPool? inner))
            {
                inner = new InnerPool();
                _pool.Add(scope, inner);
            }

            inner.Add(type, instance);
        }

        object? IDependencies.TryGetInstance(string scope, Type type)
        {
            object? instance = null;

            if(!string.IsNullOrWhiteSpace(scope))
            {
                if(_pool.TryGetValue(scope, out InnerPool? inner))
                {
                    inner.TryGetValue(type, out instance);
                }
            }

            return instance;
        }

        T IDependencies.TryGetInstance<T>(string scope)
        {
            T instance;

            if(!string.IsNullOrWhiteSpace(scope))
            {
                if(_pool.TryGetValue(scope, out InnerPool? inner))
                {
                    inner.TryGetValue(typeof(T), out object? value);

                    if(value is null)
                    {
                        throw new InvalidOperationException($"Unable to get {typeof(T)} from dependency pool.");
                    }

                    instance = (T)value;
                }
                else
                {
                    throw new InvalidOperationException($"Unable to get {typeof(T)} from dependency pool.");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(scope));
            }

            return instance;
        }
    }
}