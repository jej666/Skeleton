﻿using System.Collections.Generic;

namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    public class PeekableCollection<T>
    {
        private readonly List<T> _list;

        public PeekableCollection()
        {
            _list = new List<T>();
        }

        public PeekableCollection(IEnumerable<T> initialItems)
        {
            _list = new List<T>(initialItems);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public IEnumerable<T> Items
        {
            get { return _list.ToArray(); }
        }

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