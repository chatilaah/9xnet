using System.Drawing;
using System.Windows.Forms;
using W9xNET.RcReader.Enums;

namespace W9xNET.RcReader.Base
{
    /// <summary>
    /// A Win32-specific control object
    /// </summary>
    public class RcControlObject
    {
        /// <summary>
        /// The style of the Win32 control.
        /// </summary>
        public readonly RcControlStyle Style;

        /// <summary>
        /// The type of the Win32 control.
        /// </summary>
        public Win32ControlType ControlType;

        /// <summary>
        /// Main initializer of the control.
        /// Subclasses MUST implement the constructor.
        /// </summary>
        /// <param name="ct">The Win32ControlType value.</param>
        /// <param name="style">An instance to a subclass of the Win32Style class.</param>
        public RcControlObject(Win32ControlType ct, RcControlStyle style)
        {
            ControlType = ct;
            Style = style;
        }

        /// <summary>
        /// The name of the element.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The font of the element.
        /// </summary>
        public Font Font { get; set; } = new Font("MS Sans Serif", 8);

        /// <summary>
        /// The size of the element.
        /// </summary>
        public Size Size = Size.Empty;

        /// <summary>
        /// The location of the element.
        /// </summary>
        public Point Location = Point.Empty;

        public int Width { get => Size.Width; set => Size.Width = value; }

        public int Height { get => Size.Height; set => Size.Height = value; }

        public int Top { get => Location.Y; set => Location.Y = value; }

        public int Left { get => Location.X; set => Location.X = value; }


        /// <summary>
        /// Converts the Win32 control to a WinForms-compatible control.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual Control ToControl()
        {
            throw new System.NotImplementedException();
        }
    }
}