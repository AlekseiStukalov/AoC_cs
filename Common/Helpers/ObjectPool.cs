namespace Common.Helpers
{
    public interface IRestorable
    {
        abstract static IRestorable Create();
        void Initialize();
        void SetObjectPool(object objectPool);
    }
    public interface IRestorable<Q>
    {
        abstract static IRestorable<Q> Create(Q data);
        void Initialize(Q data);
        void SetObjectPool(object objectPool);
    }

    public interface IRestorable<Q, W>
    {
        abstract static IRestorable<Q, W> Create(Q data1, W data2);
        void Initialize(Q data1, W data2);
        void SetObjectPool(object objectPool);
    }

    public interface IRestorable<Q, W, E>
    {
        abstract static IRestorable<Q, W, E> Create(Q data1, W data2, E data3);
        void Initialize(Q data1, W data2, E data3);
        void SetObjectPool(object objectPool);
    }


    public class ObjectPool<T> where T : IRestorable
    {
        private readonly Stack<T> _pool = new();

        public IRestorable GetObject()
        {
            if (_pool.TryPop(out var obj))
            {
                obj.Initialize();
                obj.SetObjectPool(this);
                return obj;
            }
            
            IRestorable newObj = T.Create();
            newObj.SetObjectPool(this);
            return newObj;
        }

        public void ReturnObject(T obj) => _pool.Push(obj);
    }

    public class ObjectPool<T, Q> where T : IRestorable<Q>
    {
        private readonly Stack<T> _pool = new();

        public IRestorable<Q> GetObject(Q data1)
        {
            if (_pool.TryPop(out var obj))
            {
                obj.Initialize(data1);
                obj.SetObjectPool(this);
                return obj;
            }
            
            IRestorable<Q> newObj = T.Create(data1);
            newObj.SetObjectPool(this);
            return newObj;
        }

        public void ReturnObject(T obj) => _pool.Push(obj);
    }

    public class ObjectPool<T, Q, W> where T : IRestorable<Q, W>
    {
        private readonly Stack<T> _pool = new();

        public IRestorable<Q, W> GetObject(Q data1, W data2)
        {
            if (_pool.TryPop(out var obj))
            {
                obj.Initialize(data1, data2);
                obj.SetObjectPool(this);
                return obj;
            }
            
            IRestorable<Q, W> newObj = T.Create(data1, data2);
            newObj.SetObjectPool(this);
            return newObj;
        }

        public void ReturnObject(T obj) => _pool.Push(obj);
    }

    public class ObjectPool<T, Q, W, E> where T : IRestorable<Q, W, E>
    {
        public int Restored = 0;
        private readonly Stack<T> _pool = new();

        public IRestorable<Q, W, E> GetObject(Q data1, W data2, E data3)
        {
            if (_pool.TryPop(out var obj))
            {
                obj.Initialize(data1, data2, data3);
                obj.SetObjectPool(this);
                Restored++;

                return obj;
            }

            IRestorable<Q, W, E> newObj = T.Create(data1, data2, data3);
            newObj.SetObjectPool(this);
            return newObj;
        }

        public void ReturnObject(T obj) => _pool.Push(obj);
    }
}