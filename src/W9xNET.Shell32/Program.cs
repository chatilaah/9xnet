using LegalMitigation;
using LegalMitigation.Types;

namespace W9xNET.Shell32
{
    /// <summary>
    /// Program main entry point for "W9xNET.Shell32"
    /// </summary>
    public sealed class Program : W9xNETApp
    {
        internal const string KEY = "shell32.dll";

        public static Program Instance => (Program)W9xNETApp.m_instances[KEY];

        #region Constructor(s)

        public Program() : base(KEY, new Dictionary<WinSourceType, string> {
            { WinSourceType.Win95, "WIN95_17.CAB" }
        })
        {
            // do nothing.
        }

        #endregion

        public override void OnInvoke(dynamic sender, params string[] args)
        {
            sender.Run(new Forms.FrmAbout());
            //throw new NotSupportedException("Shell32 does not have an entry point. Hence the operation is not supported.");
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public override Icon? DisplayIcon => null;

        /// <summary>
        /// Not used.
        /// </summary>
        public override string DisplayName => String.Empty;

        /// <summary>
        /// Not used.
        /// </summary>
        public override Form? FormEntry => null;
    }
}