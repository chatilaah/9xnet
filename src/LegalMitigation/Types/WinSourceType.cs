namespace LegalMitigation.Types
{
    public enum WinSourceType
    {
        /// <summary>
        /// The source is unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// The source is Windows 95
        /// </summary>
        Win95,

        /// <summary>
        /// The source is Windows 98
        /// </summary>
        Win98,

        /// <summary>
        /// The source is Windows Millennium Edition
        /// </summary>
        WinMe
    }

    public static class WinSourceTypeExtensions
    {
        /// <summary>
        /// ToString (alternative)
        /// </summary>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public static string ToStringAlt(this WinSourceType sourceType)
        {
            switch (sourceType)
            {
                case WinSourceType.Win95: return "Win95";
                case WinSourceType.Win98: return "Win98";
                case WinSourceType.WinMe: return "WinME";
            }

            throw new System.NotSupportedException("The specified source type is unsupported.");
        }
    }
}