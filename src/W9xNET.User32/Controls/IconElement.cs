using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using W9xNET.User32.Gfx;

namespace W9xNET.User32.Controls
{
    public sealed class IconElement : IDisposable
    {
        internal enum IconStyle
        {
            Normal,
            Highlight,
            Dragging,
            Dropped
        }

        /// <summary>
        /// Data structure that holds the icon elemnet's caption information.
        /// </summary>
        public struct CaptionData
        {
            Size _textSize = Size.Empty;
            string _text = "";


            /// <summary>
            /// The display text of the element.
            /// </summary>
            public string Text => _text;

            public void SetText(string value, IntPtr handle)
            {
                var size = Graphics.FromHwnd(handle).MeasureString(value,
                    IconFont,
                    MaxSize.Width,
                    _sf);

                Size = new Size((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
                _text = value;
            }

            /// <summary>
            /// Used for positioning the text.
            /// </summary>
            /// <returns></returns>
            public RectangleF GetRect() => new(
                    0,
                    (float)(IconDefaultSize.Height + (Height * 0.5)) + SpaceBetweenIconAndText,
                    Width,
                    0);

            /// <summary>
            /// Used for positioning the text background rectangle.
            /// </summary>
            /// <returns></returns>
            public Rectangle GetRectForTextBackground() => new((int)((Width * 0.5) - (Width * 0.5)),
                    IconDefaultSize.Height + SpaceBetweenIconAndText,
                    Width,
                    Height);

            /// <summary>
            /// The width of the measured string.
            /// </summary>
            public int Width => Size.Width;

            /// <summary>
            /// The height of the measured string.
            /// </summary>
            public int Height => Size.Height;

            /// <summary>
            /// The size of the measured string.
            /// </summary>
            public Size Size { get => _textSize; private set => _textSize = value; }
        }

        public readonly string Path;

        const int SpaceBetweenIconAndText = 4;

        internal IconStyle CurrentStyle { get; private set; } = IconStyle.Normal;

        public ContextMenu? ContextMenu { get; set; }

        /// <summary>
        /// Gets the user-specified font.
        /// </summary>
        public static Font IconFont => User32.Properties.Settings.Default.Icon_Font;

        readonly DirectBitmap _mainBuffer;
        readonly DirectBitmap _highlightBuffer;
        readonly DirectBitmap _dragBuffer;

        bool DidSnap
        {
            get => _diffX > -1 && _diffY > -1;
            set
            {
                if (value) return;
                _diffX = -1;
                _diffY = -1;
            }
        }

        public bool IsDragging
        {
            get => (_newX > 0 || _newY > 0);
            private set
            {
                if (value) return;

                _tempX = _tempY =
                    _newX = _newY = 0;
            }
        }

        int _tempX = 0, _tempY = 0;
        int _newX = 0, _newY = 0;
        int _diffX = -1, _diffY = -1;

        /// <summary>
        /// The maximum size of an Icon Element size.
        /// </summary>
        public static Size MaxSize => new(74, 74);

        /// <summary>
        /// Caption of the icon element
        /// </summary>
        public CaptionData Caption = new();

        /// <summary>
        /// Position of the element
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Position of the element on the Y-axis
        /// </summary>
        public int Top
        {
            get => Location.Y;
            set => Location = new Point(Left, value);
        }

        /// <summary>
        /// Position of the element on the X-axis
        /// </summary>
        public int Left
        {
            get => Location.X;
            set => Location = new Point(value, Top);
        }

        public int Right => Location.X + Width;

        public int Bottom => Location.Y + Height;

        /// <summary>
        /// Gets the size of the icon element.
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Gets or sets the Parent control that holds the icon.
        /// </summary>
        public Control Parent { get; set; }

        /// <summary>
        /// Gets the width of the icon element.
        /// </summary>
        public int Width
        {
            get
            {
                if (Caption.Width > IconDefaultSize.Width)
                {
                    return Caption.Width;
                }

                return IconDefaultSize.Width;
            }

            private set => Size = new Size(value, Height);
        }

        /// <summary>
        /// Gets the height of the icon element.
        /// </summary>
        public int Height
        {
            get => IconDefaultSize.Height + 6 + Caption.Height;
            private set => Size = new Size(Width, value);
        }

        /// <summary>
        /// Gets the user-specified Icon size.
        /// </summary>
        public static Size IconDefaultSize => User32.Properties.Settings.Default.Icon_ItemSize;

        private static readonly StringFormat _sf = new()
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };

        /// <summary>
        /// The display icon of the element.
        /// </summary>
        public readonly Icon Icon;

        /// <summary>
        /// Initializes the IconElement based on the specified path.
        /// </summary>
        /// <param name="path">The absolute path to the file/folder</param>
        public IconElement(Control parent, string path)
        {
            this.Parent = parent;

            Path = path;
            Size = MaxSize;

            _mainBuffer = GenerateBuffer(IconStyle.Normal);
            _highlightBuffer = GenerateBuffer(IconStyle.Highlight);
            _dragBuffer = GenerateBuffer(IconStyle.Dragging);
        }

        /// <summary>
        /// Initializes the IconElement by specifying the Icon and the caption.
        /// </summary>
        /// <param name="icon">The icon to be shown</param>
        /// <param name="caption">The caption to be shown</param>
        /// <param name="path">The absolute path to the file/folder</param>
        public IconElement(Control parent, Icon icon, string caption, string path)
        {
            this.Parent = parent;
            this.Icon = icon;
            this.Caption.SetText(caption, Parent.Handle);
            this.Path = path;
            Size = MaxSize;

            _mainBuffer = GenerateBuffer(IconStyle.Normal);
            _highlightBuffer = GenerateBuffer(IconStyle.Highlight);
            _dragBuffer = GenerateBuffer(IconStyle.Dragging);
        }

        /// <summary>
        /// Selects the item and then invalidates the graphics context.
        /// </summary>
        public void Select()
        {
            CurrentStyle = IconStyle.Highlight;
            Parent.Invalidate();
        }

        /// <summary>
        /// Unselects the item and then invalidates the graphics context.
        /// </summary>
        public void Unselect()
        {
            CurrentStyle = IconStyle.Normal;
            Parent.Invalidate();
        }

        /// <summary>
        /// Sets the Style to "Highlight"
        /// The caller function is expected to Invalidate the graphics context immediately after completing this call.
        /// </summary>
        internal void OnMouseDown(MouseEventArgs e)
        {
            CurrentStyle = IconStyle.Highlight;

            _tempX = e.X;
            _tempY = e.Y;
        }

        /// <summary>
        /// Sets the Style to "Dropped".
        /// The caller function is expected to Invalidate the graphics context immediately after completing this call.
        /// </summary>
        internal void OnMouseUp(MouseEventArgs e)
        {
            CurrentStyle = IconStyle.Dropped;

            if (IsDragging)
            {
                Location = new Point(e.X - _diffX, e.Y - _diffY);
                IsDragging = false;
            }

            DidSnap = false;
        }

        /// <summary>
        /// Sets the Style to "Dragging".
        /// The caller function is expected to Invalidate the graphics context immediately after completing this call.
        /// </summary>
        internal void OnMouseMove(MouseEventArgs e)
        {
            CurrentStyle = IconStyle.Dragging;

            if (_tempX != e.X || _tempY != e.Y)
            {
                _newX = e.X;
                _newY = e.Y;

                if (!DidSnap)
                {
                    _diffX = _newX - Left;
                    _diffY = _newY - Top;
                }
            }
        }

        internal void OnPaint(PaintEventArgs e)
        {
            switch (CurrentStyle)
            {
                case IconStyle.Normal:
                    e.Graphics.DrawImage(_mainBuffer.Bitmap, Left, Top);
                    break;
                case IconStyle.Highlight:
                    e.Graphics.DrawImage(_highlightBuffer.Bitmap, Left, Top);
                    break;
                case IconStyle.Dragging:
                    e.Graphics.DrawImage(_highlightBuffer.Bitmap, Left, Top);
                    if (IsDragging)
                    {
                        e.Graphics.DrawImage(_dragBuffer.Bitmap, _newX - _diffX, _newY - _diffY);
                    }
                    break;
                case IconStyle.Dropped:
                    e.Graphics.DrawImage(_highlightBuffer.Bitmap, Left, Top);
                    break;
            }
        }

        private void GenerateHighlight(Graphics gr, ref Rectangle iconRect, Icon icon, ref DirectBitmap buffer)
        {
            Debug.Assert(_mainBuffer.Bitmap.Size != Size.Empty, "_mainBuffer should be filled before generating highlight bitmap!");

            NxnControl.InternalSetUserGraphics(gr);

            gr.DrawIcon(icon, iconRect);

            gr.FillRectangle(new SolidBrush(Properties.Settings.Default.SelectedItems_ItemColor),
            Caption.GetRectForTextBackground());

            gr.DrawString(Caption.Text,
                IconFont,
                new SolidBrush(Color.White),
                Caption.GetRect(),
                _sf);

            using var p = new Pen(Properties.Settings.Default.SelectedItems_ItemColor);
            p.DashStyle = DashStyle.Dot;

            for (int y = 0; y < iconRect.Height; y++)
            {
                gr.DrawLine(p,
                    (y % 2) + iconRect.X, y,
                    iconRect.X + IconDefaultSize.Width, y
                );

                // Apply the cut-out here.
                for (int x = iconRect.X; x < iconRect.X + IconDefaultSize.Width; x++)
                {
                    if (_mainBuffer.GetPixel(x, y) == Color.FromArgb(0))
                    {
                        buffer.SetPixel(x, y, Color.Transparent);
                    }
                }
            }
        }

        private void GenerateNormal(Graphics gr, ref Rectangle iconRect, Icon icon)
        {
            NxnControl.InternalSetUserGraphics(gr);

            gr.DrawIcon(icon, iconRect);

            gr.FillRectangle(new SolidBrush(Properties.Settings.Default.Desktop_ItemColor),
                Caption.GetRectForTextBackground());

            gr.DrawString(Caption.Text,
                IconFont,
                new SolidBrush(Color.White),
                Caption.GetRect(),
                _sf);
        }

        private void GenerateDragging(Graphics gr, ref Rectangle iconRect, Icon icon, out Color keyColor)
        {
            gr.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            gr.SmoothingMode = SmoothingMode.HighSpeed;
            gr.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            keyColor = Color.Fuchsia;

            gr.DrawIcon(icon, iconRect);

            gr.DrawString(Caption.Text,
                IconFont,
                new SolidBrush(Color.Black),
                Caption.GetRect(),
                _sf);


            using var p = new Pen(keyColor);
            p.DashStyle = DashStyle.Dot;

            bool inv = true;

            //TODO: WTF, For some reason, GraphicsPath incorrectly aligns the added lines!!
            for (int y = 0; y < IconDefaultSize.Height; y++)
            {
                gr.DrawLine(p, inv ? 1 : 0 /*y % 2*/, y, Width, y);
                inv = !inv;
            }
        }

        private DirectBitmap GenerateBuffer(IconStyle style)
        {
            var buffer = new DirectBitmap(Size.Width, Size.Height);
            using (var gr = Graphics.FromImage(buffer.Bitmap))
            {
                int iconX = (int)((Width * 0.5) - (IconDefaultSize.Width * 0.5));

                // Prepare the icon rect
                Rectangle iconRect = new(iconX, 0, IconDefaultSize.Width, IconDefaultSize.Height);

                switch (style)
                {
                    case IconStyle.Normal:
                        GenerateNormal(gr, ref iconRect, Icon);
                        break;
                    case IconStyle.Highlight:
                        GenerateHighlight(gr, ref iconRect, Icon, ref buffer);
                        break;
                    case IconStyle.Dragging:
                        GenerateDragging(gr, ref iconRect, Icon, out Color keyColor);
                        buffer.Bitmap.MakeTransparent(keyColor);
                        break;
                    case IconStyle.Dropped:
                        //TODO: handle dropped case
                        break;
                }
            }

            return buffer;
        }

        public void Dispose()
        {
            _mainBuffer.Dispose();
            _highlightBuffer.Dispose();
            _dragBuffer.Dispose();
        }
    }
}