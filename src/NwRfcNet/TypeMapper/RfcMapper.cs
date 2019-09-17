using System;
using System.Collections.Generic;
using NwRfcNet.Util;

namespace NwRfcNet.TypeMapper
{
    /// <summary>
    /// Mapping between RFC parameters and .NET types
    /// </summary>
    public class RfcMapper
    {
        /// <summary>
        /// Default Mapper 
        /// </summary>
        public static RfcMapper DefaultMapper { get; } = new RfcMapper();

        // mapping between types and rfc functions
        private readonly Dictionary<string, PropertyInternalStorage> _entityMaps = new Dictionary<string, PropertyInternalStorage>();

        /// <summary>
        /// Creates a new RFC Mapper
        /// </summary>
        public RfcMapper() {}

        /// <summary>
        /// Add a .NET type to mapper
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public ParameterTypeBuilder<TEntity> Parameter<TEntity>()
        {
            string entitKey = GetEntityKey<TEntity>();
            var entity = _entityMaps.GetOrAdd(entitKey, () => new PropertyInternalStorage());
            var entityBuilder = new ParameterTypeBuilder<TEntity>(entity);
            return entityBuilder;
        }

        internal PropertyInternalStorage GetTypeMapping(Type t)
        {
            var key = GetEntityKey(t);
            if (!_entityMaps.TryGetValue(key, out var storage))
                throw new RfcException($"Type {t.Name} is not mapped");

            return storage;
        }

        internal PropertyInternalStorage this[Type t] => GetTypeMapping(t);

        private static string GetEntityKey<TEntity>() => GetEntityKey(typeof(TEntity));

        private static string GetEntityKey(Type t) => t.FullName;
    }

    internal class PropertyInternalStorage : Dictionary<string, PropertyMap> { }
}
