namespace NwRfcNet.TypeMapper
{
    /// <summary>
    /// Utility Class to create a PropertyMap using fluent expressions
    /// </summary>
    public class PropertyBuilder
    {
        private readonly PropertyMap _propertyMap;

        /// <summary>
        /// Creates a new 
        /// </summary>
        /// <param name="propertyMap"></param>
        internal PropertyBuilder(PropertyMap propertyMap) => _propertyMap = propertyMap;

        /// <summary>
        /// RFC parameter name
        /// </summary>
        /// <param name="rfcParamType"></param>
        /// <returns></returns>
        public PropertyBuilder HasParameterType(RfcFieldType rfcParamType)
        {
            _propertyMap.ParameterType = rfcParamType;
            return this;
        }

        /// <summary>
        /// RFC parameter name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertyBuilder HasParameterName(string name)
        {
            _propertyMap.RfcParameterName = name;
            return this;
        }

        /// <summary>
        /// RFC Type max length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public PropertyBuilder MaxLength(int length)
        {
            _propertyMap.Length = length;
            return this;
        }

        /// <summary>
        /// Alignment type and padding character
        /// </summary>
        /// <param name="type">Type of padding</param>
        /// <param name="character">Character to be used for padding</param>
        /// <returns></returns>
        public PropertyBuilder HasAlignment(StringAlignment type, char character = ' ')
        {
            _propertyMap.Alignment = type;
            _propertyMap.PaddingCharacter = character;
            return this;
        }

        /// <summary>
        /// Sets no alignment
        /// </summary>
        /// <returns></returns>
        public PropertyBuilder HasNoAlignment()
        {
            _propertyMap.Alignment = StringAlignment.None;
            return this;
        }
    }
}
