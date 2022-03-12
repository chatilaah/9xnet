using System.Diagnostics;
using System.Drawing.Drawing2D;
using W9xNET.RcReader.Icons;
using W9xNET.User32.Controls;

namespace W9xNET.Explorer.Controls
{
    /// <summary>
    /// System tray/notification area control is responsible for displaying time, program notification icons.
    /// </summary>
    internal sealed class TrayNotifyWnd : NxnControl
    {
        public TrayNotifyIconCollection Items { get; } = new();

        readonly TrayClockWClass Clock = new();

        const int MinWidth = 64;

        public TrayNotifyWnd()
        {
            Height = ShellTrayWnd.DefaultChildHeight;

            Clock.Parent = this;
            Clock.Size = new(61, 20);
            Clock.SendToBack();
            Controls.Add(Clock);

            Items.Add(new IconInfo(Shell32.Program.Instance.RC.IconAt(1)).Images[1], "Hello2");
            Items.Add(new IconInfo(Shell32.Program.Instance.RC.IconAt(0)).Images[1], "Hello2");
            Items.Add(new IconInfo(Shell32.Program.Instance.RC.IconAt(2)).Images[1], "Hello2");
            Items.Add(new IconInfo(Shell32.Program.Instance.RC.IconAt(3)).Images[1], "Hello2");
            Items.Add(new IconInfo(Shell32.Program.Instance.RC.IconAt(4)).Images[1], "Hello2");
            Items.Add(new IconInfo(Shell32.Program.Instance.RC.IconAt(5)).Images[1], "Hello2");
        }

        public new void Refresh()
        {
            Width = MinWidth + (16 * Items.Count);
            Clock.Location = new(Width - Clock.Width - 1, 1);
            Clock.SendToBack();
        }

        protected override void OnShowContextMenu()
        {
            Debug.Assert(ContextMenu != null);
            ContextMenu.Items[0].Visible = true;
            ContextMenu.Reload();

            base.OnShowContextMenu();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            GraphicsPath path = new();
            using (Pen p = new(Color.Gray))
            {
                path.AddLine(
                    x1: 0, x2: Width,
                    y1: 0, y2: 0);

                path.AddLine(
                    x1: 0, x2: 0,
                    y1: 0, y2: Height);

                e.Graphics.DrawPath(p, path);
            }

            using (Pen p = new(Color.White))
            {
                path.Reset();

                path.AddLine(
                    x1: Width - 1, x2: Width - 1,
                    y1: 0, y2: Height);

                path.AddLine(
                    x1: 0, x2: Width - 1,
                    y1: Height - 1, y2: Height - 1);

                e.Graphics.DrawPath(p, path);
            }

            int startX = 0;
            float y = (ClientSize.Height * 0.5f) - (16 * 0.5f);
            foreach (var item in Items)
            {
                e.Graphics.DrawIcon(item.Icon, startX, (int)y);

                startX += 16;
            }
        }
    }
}