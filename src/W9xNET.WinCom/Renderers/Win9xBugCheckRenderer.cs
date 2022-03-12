using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using W9xNET.WinCom.Interfaces;
using W9xNET.WinCom.Properties;

namespace W9xNET.WinCom.Renderers
{
    /// <summary>
    /// Windows 95, 98, and Millennium Edition bugcheck renderer.
    /// </summary>
    internal class Win9xBugCheckRenderer : BugCheckRenderer
    {
        int messageHeight = 0;
        SizeF titleTextSize;
        SizeF footerTextSize;
        SizeF bodySize;

        internal Win9xBugCheckRenderer(IBugCheckWindow bcw) : base(bcw, Types.BugCheckStyle.Win9xStyle)
        {
            // do nothing.
        }

        protected override void InitResources()
        {
            BugCheckHandle.BackgroundColor = Properties.Settings.Default.BugCheckBackColor;
            BugCheckHandle.TextColor = Properties.Settings.Default.BugCheckTextColor;

            LoadFont(Properties.Resources.PerfectDosVga437);
        }

        public override void Render(PaintEventArgs e)
        {
            SolidBrush bgColor = new(BugCheckHandle.BackgroundColor);
            SolidBrush txColor = new(BugCheckHandle.TextColor);

            //
            // Prepare the text
            //

            string titleText = Resources.AppName;

            IReadOnlyList<string> body = new List<string>(new string[]{
                BugCheckHandle.Message,
                "Press any key to return.",
                "Press CTRL+ALT+DEL to show the Windows security options screen. You will not lose unsaved information in all programs that are running."
            });
            string footerText = Resources.PressAnyKeyToContinue;

            //
            // Measure the buffer
            //

            const int padding = 28;

            if (titleTextSize.IsEmpty)
            {
                titleTextSize = e.Graphics.MeasureString(titleText, SelectedFont);
            }

            if (footerTextSize.IsEmpty)
            {
                footerTextSize = e.Graphics.MeasureString(footerText, SelectedFont);
            }

            if (bodySize.IsEmpty)
            {
                bodySize = new SizeF(1024 - padding, 0);
            }

            if (messageHeight == 0)
            {
                for (int i = 0; i < body.Count; i++)
                {
                    var curr = body[i];

                    var szI = e.Graphics.MeasureString(curr, SelectedFont, (int)bodySize.Width);
                    bodySize.Height += szI.Height;

                    if (i == 0)
                    {
                        messageHeight = (int)bodySize.Height;
                    }
                }
            }

            //
            // Draw the header
            //

            var hdX = (e.ClipRectangle.Width - (int)titleTextSize.Width) / 2;
            var hdY = (float)((e.ClipRectangle.Height * 0.5) - bodySize.Height);

            e.Graphics.FillRectangle(txColor,
                hdX,
                hdY - 1,
                (int)titleTextSize.Width,
                (int)titleTextSize.Height
             );

            e.Graphics.DrawString(titleText, SelectedFont, bgColor, hdX, hdY);

            //
            // Draw the body
            //

            float startX = (float)((e.ClipRectangle.Width * 0.5) - bodySize.Width * 0.5);
            float startY = (float)((e.ClipRectangle.Height * 0.5) - (bodySize.Height * 0.5));

            for (int i = 0; i < body.Count; i++)
            {
                var toPrint = body[i];

                e.Graphics.DrawString(toPrint, SelectedFont, txColor, new RectangleF(startX, startY, bodySize.Width, bodySize.Height), StringFormat.GenericDefault);

                if (i == 0)
                {
                    startY += messageHeight + 24 /* Some padding */;
                    startX += padding /* Some padding */;
                }
                else
                {
                    e.Graphics.DrawString("*", SelectedFont, txColor, startX - padding, startY);
                    startY += titleTextSize.Height;
                }
            }

            //
            // Draw the footer
            //

            var ftX = (e.ClipRectangle.Width - (int)footerTextSize.Width) / 2;
            var ftY = (float)((e.ClipRectangle.Height * 0.5) + bodySize.Height) + padding;

            e.Graphics.DrawString(footerText, SelectedFont, txColor, ftX, ftY);
        }
    }
}