using LegalMitigation;
using LegalMitigation.Types;

namespace W9xNET.Explorer
{
    /// <summary>
    /// Program main entry point for "W9xNET.Explorer"
    /// </summary>
    public sealed class Program : W9xNETApp
    {
        internal const string KEY = "explorer.exe";

        public static Program Instance => (Program)m_instances[KEY];

        public static bool IsInitial { get; private set; } = true;

        #region Constructor(s)

        public Program() : base(KEY, new Dictionary<WinSourceType, string> {
            { WinSourceType.Win95, "WIN95_17.CAB" }
        })
        {
            // do nothing.
        }

        #endregion

        public override void OnInvoke(dynamic sender, params string[] args) => sender.Run(new Forms.FrmMain(args));

        public override string DisplayName => Properties.Resources.Explorer;

        public override Icon? DisplayIcon => RC?.IconAt(0);

        public override Form? FormEntry
        {
            get
            {
                if (!IsInitial) throw new NotSupportedException("More than one instance of this object is not supported");

                // In this step, we load the primary libraries before going any deeper.
                if (StartupHelper.ResolveFromGuiMode(
                    this, new Shell32.Program()))
                {
                    IsInitial = false;
                    return new Explorer.Forms.DesktopHost /* TestWindow */();
                }

                return null;
            }
        }
    }
}