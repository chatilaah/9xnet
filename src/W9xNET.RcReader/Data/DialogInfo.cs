using System.Collections.Generic;
using DWORD = System.Int32;
using WORD = System.Int16;

namespace W9xNET.RcReader.Data
{
    public sealed class DialogInfo : DialogControlElement
    {
        public DialogInfo(DWORD style, short x, short y, short width, short height, DWORD menuId, string caption, string fontFamily, WORD fontSize, IEnumerable<DialogControlElement> controls, byte[] rc) 
            : base(style, caption, null, x, y, width, height, 0)
        {
            MenuID = menuId;
            FontFamily = fontFamily;
            FontSize = fontSize;
            Controls = controls;
            RcData = rc;
        }

        public new string Caption => (string)base.GetCaption<string>();

        public readonly DWORD MenuID;
        public readonly string FontFamily;
        public readonly WORD FontSize;
        public readonly IEnumerable<DialogControlElement> Controls;
        public readonly ReadOnlyMemory<Byte> RcData;
    }
}