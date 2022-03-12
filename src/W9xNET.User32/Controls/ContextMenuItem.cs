using System.Diagnostics;

namespace W9xNET.User32.Controls
{
    public class ContextMenuItem : NxnControl
    {
        public static Font MenuItemFont => User32.Properties.Settings.Default.Menu_Font;

        private static Font? _arrowFont;
        private static SizeF? _arrowSize;

        public static SizeF ArrowSize
        {
            private set => _arrowSize = value;
            get
            {
                if (_arrowSize == null)
                {
                    _arrowSize = Graphics.FromHwnd(IntPtr.Zero).MeasureString("4", ArrowFont);
                }

                return _arrowSize.Value;
            }
        }

        public static Font ArrowFont
        {
            private set => _arrowFont = value;
            get
            {
                if (_arrowFont == null)
                {
                    _arrowFont = new Font("Webdings", 9);
                }

                return _arrowFont;
            }
        }

        private static SolidBrush? _textNormalColor = null;
        private static SolidBrush? _textHighlightColor = null;
        private static SolidBrush? _backNormalColor = null;
        private static SolidBrush? _backHighlightColor = null;

        private bool _isHighlight = false;
        private float _labelY = 0;
        private float _iconY = 0;
        private float _arrowY = 0;
        private SizeF _textSize = SizeF.Empty;
        private bool _persistHighlight = false;

        internal void SetPersistHighlight(bool value)
        {
            _persistHighlight = value;
        }

        public int ContentWidth => (int)(_textSize.Width + IconWidth + (((ContextMenu)Parent).IsLarge ? 73 : 27));

        private SolidBrush CurrentTextColor => _isHighlight || _persistHighlight ?
            _textHighlightColor! : _textNormalColor!;

        private SolidBrush CurrentBackColor => _isHighlight || _persistHighlight ?
            _backHighlightColor! : _backNormalColor!;


        internal const int SmallMenuHeight = 17;
        internal const int LargeMenuHeight = 32;

        private ContextMenu SubMenu { get; set; } = new();

        public ContextMenuItemCollection Items => SubMenu.Items;

        #region Constructor(s)

        public ContextMenuItem()
        {
            // By default, we set the height to small.
            Height = SmallMenuHeight;

            InternalInitFixedObjectsIfNeeded();
        }

        #endregion

        private static void InternalInitFixedObjectsIfNeeded()
        {
            if (_backHighlightColor == null || _backNormalColor == null ||
                _textHighlightColor == null || _textNormalColor == null)
            {
                InitColors();
            }
        }

        private static void InitColors()
        {
            _backNormalColor = new(User32.Properties.Settings.Default.Menu_ItemColor);
            _backHighlightColor = new(User32.Properties.Settings.Default.SelectedItems_ItemColor);

            _textNormalColor = new(User32.Properties.Settings.Default.Menu_FontColor);
            _textHighlightColor = new(User32.Properties.Settings.Default.SelectedItems_FontColor);
        }

        public Icon? Icon { get; set; }

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                _textSize = Graphics.FromHwnd(IntPtr.Zero).MeasureString(value, MenuItemFont);
            }
        }

        private int IconWidth
        {
            get
            {
                if (((ContextMenu)Parent).IsLarge) return 32;
                return 16;
            }
        }

        private int LeadingSpace
        {
            get
            {
                if (((ContextMenu)Parent).IsLarge) return 10;
                return 4;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHighlight = true;
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHighlight = false;
            base.OnMouseLeave(e);
            //((ContextMenu)Parent).OnRemoveSubmenu(this, SubMenu);
            Invalidate();
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            ((ContextMenu)Parent).OnDisplaySubmenu(this, SubMenu);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(CurrentBackColor.Color);

            if (Icon != null)
            {
                if (_iconY == 0)
                {
                    _iconY = (float)((ClientSize.Height * 0.5) - (Icon!.Height * 0.5));
                }

                e.Graphics.DrawIcon(Icon, 5, (int)_iconY);
            }

            if (_labelY == 0)
            {
                _labelY = (float)((ClientSize.Height * 0.5) - (_textSize.Height * 0.5));
            }
            e.Graphics.DrawString(Text, MenuItemFont, CurrentTextColor, IconWidth + LeadingSpace, _labelY);


            if (SubMenu.Items.Count > 0)
            {
                if (_arrowY == 0)
                {
                    _arrowY = (float)((ClientSize.Height * 0.5) - (ArrowSize.Height * 0.5));
                }

                e.Graphics.DrawString("4", ArrowFont, CurrentTextColor, ClientSize.Width - ArrowSize.Width, _arrowY);
            }
        }
    }
}