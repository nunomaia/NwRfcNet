using System;

namespace NwRfcNet.TypeMapper
{

    /// <summary>
    /// Map Properties to RFC function parameters
    /// </summary>
    public class PropertyMap
    {
        /// <summary>
        /// RFC Field Name
        /// </summary>
        public string RfcParameterName { get; internal set; }

        /// <summary>
        /// Property name
        /// </summary>
        public string PropertyName { get; internal set; }

        /// <summary>
        /// RFC Field Type
        /// </summary>
        public RfcFieldType? ParameterType { get; internal set; }

        /// <summary>
        /// Type of Property
        /// </summary>
        public Type PropertyType { get; internal set; }

        /// <summary>
        /// Define the RFC Field Length. 
        /// This is only required for some RFC Field Types
        /// </summary>
        public int Length { get; internal set; }

        /// <summary>
        /// Defines padding character for character type fields
        /// </summary>
        public char PaddingCharacter { get; internal set; } = ' ';

        /// <summary>
        /// Defines padding for character type fields
        /// </summary>
        public StringAlignment Alignment { get; internal set; } = StringAlignment.Right;
    }
}
