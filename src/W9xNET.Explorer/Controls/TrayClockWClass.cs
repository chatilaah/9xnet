using W9xNET.User32.Controls;

namespace W9xNET.Explorer.Controls
{
    internal class TrayClockWClass : NxnControl
    {
        internal delegate void OnSysTrayDoubleClickEventHandler(object? sender, MouseEventArgs e);
        System.Threading.Timer _timer;

        SizeF TextSize = SizeF.Empty;

        Font _font;
        string _currentTime = "";

        public TrayClockWClass()
        {
            _font = User32.Properties.Settings.Default.ActiveTitleBar_Font;
            _font = new Font(_font.FontFamily.Name, _font.Size);

            //_timer = new(TimerCallback, this, 0, 1000);

            //BackColor = Color.AliceBlue;

            DoRefreshTime();
        }

        private static void TimerCallback(object? o)
        {
            var clazz = ((TrayClockWClass)o!);
            clazz.DoRefreshTime();
        }

        private void DoRefreshTime()
        {
            var time = DateTime.Now.ToString("h:mm tt");
            if (time != _currentTime)
            {
                _currentTime = time;

                TextSize = Graphics.FromHwnd(Handle).MeasureString(_currentTime, _font);

                Invoke(delegate
                {
                    Invalidate();
                });
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            float xPos = (Width * 0.5f) - (TextSize.Width * 0.5f);
            float yPos = (Height * 0.5f) - (TextSize.Height * 0.5f);

            e.Graphics.DrawString(_currentTime, _font, new SolidBrush(User32.Properties.Settings.Default.ThreeDObjects_FontColor),
                xPos, yPos);
        }
    }
}