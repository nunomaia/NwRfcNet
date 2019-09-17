using System;
using System.Collections.Generic;

namespace NwRfcNet.Util
{
    /// <summary>
    /// Dictionary Extensions 
    /// </summary>
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// if dictonary don't contains an element with the specified key, obtain value from funcValue
        /// and add to dictonary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="funcValue"></param>
        /// <returns></returns>
        internal static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> funcValue)
        {
            if (!dict.ContainsKey(key))
            {
                var value = funcValue.Invoke();
                dict[key] = value;
                return value;
            }

            return dict[key];
        }
    }
}
