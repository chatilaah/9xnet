namespace LegalMitigation.Utils
{
    /// <summary>
    /// Microsoft(R) Windows(R) Millennium Edition sources
    /// </summary>
    internal class WinSourceMe : WinSource
    {
        public WinSourceMe(string path = "") : base(path, Types.WinSourceType.WinMe)
        {
            // do nothing.
        }

        protected override string RelativeDirectory => "win9x";


        /// <summary>
        /// The expected Windows Millennium Edition installation files.
        /// </summary>
        protected override string[] InstallationFiles => new string[] {

        };
    }
}