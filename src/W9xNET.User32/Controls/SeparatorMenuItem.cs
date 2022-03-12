namespace W9xNET.User32.Controls
{
    public sealed class SeparatorMenuItem : ContextMenuItem
    {
        private ContextMenu ContextMenuRef => (ContextMenu)this.Parent;

        static readonly Pen p1 = new(Color.FromArgb(128, 128, 128));
        static readonly Pen p2 = new(Color.FromArgb(255, 255, 255));

        public SeparatorMenuItem()
        {
            Height = 9;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            // do nothing.
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            // do nothing.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int begin = 2;

            if (!ContextMenuRef.IsLarge)
            {
                begin += 1;
            }

            e.Graphics.DrawLine(p1, 0, begin, ClientRectangle.Width, begin);
            e.Graphics.DrawLine(p2, 0, begin + 1, ClientRectangle.Width, begin + 1);
        }
    }
}