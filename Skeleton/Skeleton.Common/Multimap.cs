using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Common
{
    /// <summary>
    ///     A class to map a key to one or more values
    /// </summary>
    /// <typeparam name="TKey">Key to map values to</typeparam>
    /// <typeparam name="TValue">Value to map to key</typeparam>
    [Serializable]
    public class Multimap<TKey, TValue>
    {
        // All lists will contain at least one object of type Value.
        private readonly Dictionary<TKey, List<TValue>> _mKeyValues;

        /// <summary>
        ///     Constructor that uses the default IEqualityComparer for the type of the key
        /// </summary>
        public Multimap()
            : this(null)
        {
        }

        /// <summary>
        ///     Constructor that specifies the method to test for key equality
        /// </summary>
        /// <param name="comparer">
        ///     The comparer used to compare keys, or null to use the
        ///     default comparer for the type of the key
        /// </param>
        public Multimap(IEqualityComparer<TKey> comparer)
        {
            _mKeyValues = new Dictionary<TKey, List<TValue>>(comparer);
        }

        /// <summary>
        ///     Gets all keys in the map
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get { return _mKeyValues.Keys; }
        }

        /// <summary>
        ///     Indexer, returns collection of values for a given key
        /// </summary>
        public IEnumerable<TValue> this[TKey key]
        {
            get { return Find(key); }
        }

        /// <summary>
        ///     Checks if map contains a given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True iff map contains key</returns>
        public bool ContainsKey(TKey key)
        {
            return _mKeyValues.ContainsKey(key);
        }

        /// <summary>
        ///     Checks if map contains a given key/value pair
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True iff map contains key/value pair</returns>
        public bool ContainsKeyValue(TKey key, TValue value)
        {
            List<TValue> values;
            return _mKeyValues.TryGetValue(key, out values) && values.Contains(value);
        }

        /// <summary>
        ///     Finds the collection of values associated with the given key. If the key isn't found,
        ///     an empty collection is returned.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>A collection of 0 or more values</returns>
        public IEnumerable<TValue> Find(TKey key)
        {
            List<TValue> values;
            return !_mKeyValues.TryGetValue(key, out values) ? Enumerable.Empty<TValue>() : values;
        }

        /// <summary>
        ///     Finds the first value associated with the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The first value for the key, or null if key is not in map</returns>
        public TValue FindFirst(TKey key)
        {
            List<TValue> values;
            return !_mKeyValues.TryGetValue(key, out values) ? default(TValue) : values[0];
        }

        /// <summary>
        ///     Finds the last value associated with the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The last value for the key, or null if key is not in map</returns>
        public TValue FindLast(TKey key)
        {
            List<TValue> values;
            return !_mKeyValues.TryGetValue(key, out values) ? default(TValue) : values[values.Count - 1];
        }

        /// <summary>
        ///     Gets the first value associated with the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="result">The first value for the key, or default if key is not in map</param>
        /// <returns>True iff the key has an associated value</returns>
        public bool TryGetFirst(TKey key, out TValue result)
        {
            List<TValue> values;
            if (!_mKeyValues.TryGetValue(key, out values))
            {
                result = default(TValue);
                return false;
            }

            result = values[0];
            return true;
        }

        /// <summary>
        ///     Gets the last value associated with the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="result">The last value for the key, or default if key is not in map</param>
        /// <returns>True iff the key has an associated value</returns>
        public bool TryGetLast(TKey key, out TValue result)
        {
            List<TValue> values;
            if (!_mKeyValues.TryGetValue(key, out values))
            {
                result = default(TValue);
                return false;
            }

            result = values[values.Count - 1];
            return true;
        }

        /// <summary>
        ///     Adds a key/value pair to the map so that the value is last among possibly
        ///     many values that are already associated with this key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <remarks>
        ///     If the value is already present, it is removed and then added last,
        ///     so duplicate values aren't possible
        /// </remarks>
        public void Add(TKey key, TValue value)
        {
            List<TValue> values;
            if (!_mKeyValues.TryGetValue(key, out values))
                values = _mKeyValues[key] = new List<TValue>();
            else
                values.Remove(value);

            values.Add(value);
        }

        /// <summary>
        ///     Adds a key/value pair to the map so that the value is first among possibly
        ///     many values that are already associated with this key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <remarks>
        ///     If the value is already present, it is removed and then added first,
        ///     so duplicate values aren't possible.
        /// </remarks>
        public void AddFirst(TKey key, TValue value)
        {
            List<TValue> values;
            if (!_mKeyValues.TryGetValue(key, out values))
                values = _mKeyValues[key] = new List<TValue>();
            else
                values.Remove(value);

            values.Insert(0, value);
        }

        /// <summary>
        ///     Removes all values associated with the given key from the map
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True iff the value was removed</returns>
        public bool Remove(TKey key)
        {
            return _mKeyValues.Remove(key);
        }

        /// <summary>
        ///     Removes a value from the map
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True iff the value was removed</returns>
        public bool Remove(TKey key, TValue value)
        {
            List<TValue> values;
            if (!_mKeyValues.TryGetValue(key, out values)) return false;
            if (!values.Remove(value)) return false;
            if (values.Count == 0)
                _mKeyValues.Remove(key);
            return true;
        }

        /// <summary>
        ///     Clears the map
        /// </summary>
        public void Clear()
        {
            _mKeyValues.Clear();
        }
    }
}