namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    using System.Collections.Generic;

    public class PeekableCollection<T>
    {
        private readonly List<T> list;

        public PeekableCollection()
        {
            list = new List<T>();
        }

        public PeekableCollection(IEnumerable<T> initialItems)
        {
            list = new List<T>(initialItems);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public IEnumerable<T> Items
        {
            get { return list.ToArray(); }
        }

        public T Peek(int depth)
        {
            var index = list.Count - 1 - depth;

            if (index < 0)
                return default(T);

            return list[index];
        }

        public T Pop()
        {
            var index = list.Count - 1;
            T ret = list[index];
            list.RemoveAt(index);
            return ret;
        }

        public void Push(T obj)
        {
            list.Add(obj);
        }
    }
}