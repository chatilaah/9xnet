using System.Drawing;

namespace W9xNET.User32.Interfaces
{
    public interface IRendererWin9x<EnumType>
    {
        /// <summary>
        /// Renders the element.
        /// </summary>
        void Render(Rectangle rect, EnumType enumType);
    }
}