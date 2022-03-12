using System.Drawing.Text;
using W9xNET.WinCom.Interfaces;
using W9xNET.WinCom.Types;

namespace W9xNET.WinCom.Renderers
{
    internal class BugCheckRenderer
    {
        #region Properties

        protected readonly IBugCheckWindow BugCheckHandle;

        readonly PrivateFontCollection _fonts = new();

        protected Font SelectedFont { get; private set; }

        public readonly BugCheckStyle Style;

        #endregion

        #region Constructor(s)

        public BugCheckRenderer(IBugCheckWindow bcw, BugCheckStyle bcs)
        {
            BugCheckHandle = bcw;
            Style = bcs;

            InitResources();
        }

        #endregion

        protected virtual void InitResources()
        {
            // do nothing.
        }

        /// <summary>
        /// Loads a font from the embedded resource project.
        /// </summary>
        /// <param name="fontFromResources"></param>
        protected void LoadFont(byte[] fontFromResources)
        {
            byte[] fontData = fontFromResources;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            _fonts.AddMemoryFont(fontPtr, Properties.Resources.PerfectDosVga437.Length);
            Interop.Win32.Gdi32.AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.PerfectDosVga437.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
            SelectedFont = new Font(_fonts.Families[0], 16.0F);
        }

        /// <summary>
        /// Performs the rendering sub-routine
        /// </summary>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Render(PaintEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
