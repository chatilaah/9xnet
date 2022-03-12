using LegalMitigation.Utils;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LegalMitigation.Forms
{
    internal partial class FrmPickSource : Form
    {
        internal IEnumerable<WinSource> Sources { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public int SelectedIndex { get; set; } = -1;
        
        public FrmPickSource()
        {
            InitializeComponent();
        }
    }
}