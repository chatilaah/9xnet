using System.Diagnostics;
using System.Drawing;
using W9xNET.RcReader;
using W9xNET.RcReader.Data;
using static W9xNET.Interop.Win32.User32;

namespace LegalMitigation
{
    /// <summary>
    /// To avoid legal conflicts, this library was made to resolve the required resource files on runtime.
    /// Users should acquire a legitimate copy of a Windows 9x-based CD-ROM to utilize this software.
    /// </summary>
    public class RcObject
    {
        #region Properties

        private Extractor? _extractor;
        private readonly string m_path;

        #endregion

        public RcObject(string path)
        {
            m_path = path;
            Debug.WriteLine(m_path);
        }

        private void InitializeExtractorIfNeeded() => _extractor ??= new Extractor(m_path);

        public Icon IconAt(int i)
        {
            InitializeExtractorIfNeeded();
            return _extractor!.GetIconAt(i);
        }

        public DialogInfo DialogAt(int i)
        {
            InitializeExtractorIfNeeded();
            return _extractor!.GetDialogAt(i);
        }

        public List<ExMenuItem> MenuAt(int i)
        {
            InitializeExtractorIfNeeded();
            return _extractor!.GetMenuAt(i);
        }

        public Bitmap BmpAt(int i)
        {
            InitializeExtractorIfNeeded();
            return _extractor!.GetBmpAt(i);
        }
    }
}