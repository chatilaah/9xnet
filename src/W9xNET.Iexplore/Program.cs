using LegalMitigation.Types;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using W9xNET.Iexplore.Properties;
using W9xNET.LegalMitigation;

namespace W9xNET.Iexplore
{
    /// <summary>
    /// Program main entry point for "W9xNET.Iexplore"
    /// </summary>
    public sealed class Program : W9xNETApp
    {
        internal const string KEY = "iexplore.exe";

        public static Program Instance => (Program)W9xNETApp.m_instances[KEY];

        #region Constructor(s)

        public Program() : base(KEY, new Dictionary<WinSourceType, string> {
            { WinSourceType.Win95, "WIN95_20.CAB" }
        })
        {
            // do nothing.
        }

        #endregion

        public override string DisplayName => Properties.Resources.W9xNetInternetBrowser;

        public override Icon? DisplayIcon => RC?.IconAt(0);

        public override Form FormEntry => new Forms.FrmMain();
    }
}