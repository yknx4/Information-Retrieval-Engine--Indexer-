using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Tools.Structures
{
    public class ObjectPool<T>
    {
        protected ConcurrentBag<T> _objects;
        protected Func<T> _objectGenerator;
        protected int limit;

        public ObjectPool(Func<T> objectGenerator)
        {
            if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator;
        }
        
        public T GetObject()
        {
            T item;
            if (_objects.TryTake(out item)) return item;
            return _objectGenerator();
        }

        public void PutObject(T item)
        {
            _objects.Add(item);
        }
    }

    public class LimitedObjectPool<T> : ObjectPool<T>
    {
        protected int Limit;
        public LimitedObjectPool(Func<T> objectGenerator, int limit) : base(objectGenerator)
        {
            Limit = limit;
        }
    }
}
