namespace NwRfcNet
{
    /// <summary>
    /// Currently loaded sapnwrfc library
    /// </summary>
    public class RfcLibVersion
    {
        #region Constructors
        internal RfcLibVersion(uint majorVersion, uint minorVersion, uint patchLevel)
        {
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            PatchLevel = patchLevel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Library Major Version
        /// </summary>
        public uint MajorVersion { get; }

        /// <summary>
        /// Library Minor Version
        /// </summary>
        public uint MinorVersion { get; }

        /// <summary>
        /// Library Patch Level
        /// </summary>
        public uint PatchLevel { get; }

        #endregion
    }
}
