using System.Collections.Generic;

namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    public class PeekableCollection<T>
    {
        private readonly List<T> _list;

        public PeekableCollection()
        {
            _list = new List<T>();
        }

        public int Count => _list.Count;

        public IEnumerable<T> Items => _list.ToArray();

        public T Peek(int depth)
        {
            var index = _list.Count - 1 - depth;

            return index < 0 ? default(T) : _list[index];
        }

        public T Pop()
        {
            var index = _list.Count - 1;
            var ret = _list[index];
            _list.RemoveAt(index);
            return ret;
        }

        public void Push(T obj)
        {
            _list.Add(obj);
        }
    }
}