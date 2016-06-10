using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Common
{
    [Serializable]
    public class LookupDictionery<TKey, TValue>
    {
        private readonly Dictionary<TKey, List<TValue>> _keyValues;

        public LookupDictionery()
            : this(null)
        {
        }

        public LookupDictionery(IEqualityComparer<TKey> comparer)
        {
            _keyValues = new Dictionary<TKey, List<TValue>>(comparer);
        }

        public IEnumerable<TKey> Keys
        {
            get { return _keyValues.Keys; }
        }

        public IEnumerable<TValue> this[TKey key]
        {
            get { return Find(key); }
        }

        public bool ContainsKey(TKey key)
        {
            return _keyValues.ContainsKey(key);
        }

        public bool ContainsKeyValue(TKey key, TValue value)
        {
            List<TValue> values;
            return _keyValues.TryGetValue(key, out values) && values.Contains(value);
        }

        public IEnumerable<TValue> Find(TKey key)
        {
            List<TValue> values;
            return !_keyValues.TryGetValue(key, out values) ? Enumerable.Empty<TValue>() : values;
        }

        public TValue FindFirst(TKey key)
        {
            List<TValue> values;
            return !_keyValues.TryGetValue(key, out values) ? default(TValue) : values[0];
        }

        public TValue FindLast(TKey key)
        {
            List<TValue> values;
            return !_keyValues.TryGetValue(key, out values) ? default(TValue) : values[values.Count - 1];
        }

        public bool TryGetFirst(TKey key, out TValue result)
        {
            List<TValue> values;
            if (!_keyValues.TryGetValue(key, out values))
            {
                result = default(TValue);
                return false;
            }

            result = values[0];
            return true;
        }

        public bool TryGetLast(TKey key, out TValue result)
        {
            List<TValue> values;
            if (!_keyValues.TryGetValue(key, out values))
            {
                result = default(TValue);
                return false;
            }

            result = values[values.Count - 1];
            return true;
        }

        public void Add(TKey key, TValue value)
        {
            List<TValue> values;
            if (!_keyValues.TryGetValue(key, out values))
                values = _keyValues[key] = new List<TValue>();
            else
                values.Remove(value);

            values.Add(value);
        }

        public void AddFirst(TKey key, TValue value)
        {
            List<TValue> values;
            if (!_keyValues.TryGetValue(key, out values))
                values = _keyValues[key] = new List<TValue>();
            else
                values.Remove(value);

            values.Insert(0, value);
        }

        public bool Remove(TKey key)
        {
            return _keyValues.Remove(key);
        }

        public bool Remove(TKey key, TValue value)
        {
            List<TValue> values;
            if (!_keyValues.TryGetValue(key, out values)) return false;
            if (!values.Remove(value)) return false;
            if (values.Count == 0)
                _keyValues.Remove(key);
            return true;
        }

        public void Clear()
        {
            _keyValues.Clear();
        }
    }
}