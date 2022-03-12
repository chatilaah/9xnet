namespace W9xNET.User32.Controls
{
    public class ResizableControl : NxnControl
    {
        readonly NxnControl ChildControl;

        public new Size ClientSize => ChildControl.Size;

        public new Rectangle ClientRectangle => ChildControl.ClientRectangle;

        public bool IsLeadResizable { get; set; } = true;

        public bool IsTrailResizable { get; set; } = true;

        public bool IsTopResizable { get; set; } = true;

        public bool IsBottomResizable { get; set; } = true;

        public ResizableControl(NxnControl childControl)
        {
            ChildControl = childControl;
            Controls.Add(ChildControl);
        }

        static int ActiveWindowBorder => Properties.Settings.Default.ActiveTitleBar_ItemSize;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ChildControl.Location = new Point(ActiveWindowBorder);
        }
    }
}