namespace W9xNET.User32.Interfaces
{
    public interface IRenderer
    {
        /// <summary>
        /// Renders the element.
        /// </summary>
        void Render();

        /// <summary>
        /// Deletes all styling.
        /// </summary>
        void DeRender();

        void HandleMouseDown();

        void HandleMouseUp();
    }
}