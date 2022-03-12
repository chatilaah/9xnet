namespace W9xNET.User32.Controls.Interfaces
{
    /// <summary>
    /// A 9xNET-based Form interface
    /// </summary>
    public interface INxnForm
    {
        /// <summary>
        /// Brings the focus to the foreground of the form
        /// </summary>
        /// <param name="sender"></param>
        void OnRequestFocus(object? sender);

        /// <summary>
        /// Locks the cursor
        /// </summary>
        void LockCursor();

        /// <summary>
        /// Gets the form
        /// </summary>
        /// <returns></returns>
        Form GetForm();

        /// <summary>
        /// Gets the container
        /// </summary>
        /// <returns></returns>
        Control GetContainer();
    }
}