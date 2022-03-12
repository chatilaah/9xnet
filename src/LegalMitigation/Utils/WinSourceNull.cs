using System;

namespace LegalMitigation.Utils
{
    /// <summary>
    /// Microsoft(R) Windows(R) Millennium Edition sources
    /// </summary>
    internal class WinSourceNull : WinSource
    {
        public WinSourceNull(string path = "") : base(path, Types.WinSourceType.Unknown)
        {
            // do nothing.
        }

        /// <summary>
        /// The expected Windows Millennium Edition installation files.
        /// </summary>
        protected override string[] InstallationFiles
        {
            get { throw new NotSupportedException("Unsupported source type. Only Windows 95, 98, and Millennium Edition are currently supported."); }
        }
    }
}