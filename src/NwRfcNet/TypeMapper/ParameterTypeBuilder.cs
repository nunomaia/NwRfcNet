using NwRfcNet.Util;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NwRfcNet.TypeMapper
{
    public class ParameterTypeBuilder<TEntity>
    {
        // 
        private readonly PropertyInternalStorage _internalStorage;

        internal ParameterTypeBuilder(PropertyInternalStorage internalStorage) 
            => _internalStorage = internalStorage;

        public PropertyBuilder Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression) 
            => new PropertyBuilder(GetMappingOrAdd(propertyExpression));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyLambda"></param>
        /// <returns></returns>
        private PropertyMap GetMappingOrAdd<TProperty>(Expression<Func<TEntity, TProperty>> propertyLambda)
        {
            var prop = GetPropertyInfo(propertyLambda);
            return _internalStorage.GetOrAdd(prop.Name,
                () => new PropertyMap()
                {
                    PropertyName = prop.Name,
                    RfcParameterName = prop.Name.ToUpper(),
                    ParameterType = null,
                    PropertyType = prop.PropertyType
                }
            );            
        }

        // ideia from : https://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
        /// <summary>
        /// Retive property name from a lambda expression
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyLambda"></param>
        /// <returns></returns>
        private PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            if (propertyLambda == null)
                throw new ArgumentNullException(nameof(propertyLambda));

            if (!(propertyLambda.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{propertyLambda.ToString()}' refers to a method, not a property.");

            PropertyInfo propInfo = member.Member as PropertyInfo
                ?? throw new ArgumentException($"Expression '{propertyLambda.ToString()}' refers to a field, not a property.");

            Type type = typeof(TEntity);
            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException($"Expression '{propertyLambda.ToString()}' refers to a property that is not from type {type}.");

            return propInfo;
        }
    } 
}
