using System.ComponentModel;
using System.Diagnostics;
using System.Security.Permissions;
using W9xNET.Interop.Win32;
using W9xNET.RcReader.Data;
using static LegalMitigation.Forms.NativeForm;

namespace LegalMitigation.Forms
{
    [DesignerCategory("")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class NativeForm : LegalForm
    {
        // Constant value was found in the "windows.h" header file.
        public const int WM_ACTIVATEAPP = 0x001C;
        public const int WM_INITDIALOG = 0x0110;
        public const int WM_COMMAND = 0x0111;

        public const int STM_SETIMAGE = 0x0172;

        private readonly MyNativeWindowListener m_nwl;
        private readonly MyNativeWindow m_nw;
        private readonly RcObject m_rc;
        public delegate void ButtonClick(object sender, EventArgs e);

        internal void ApplicationActivated(bool ApplicationActivated)
        {
            // The application has been activated or deactivated
            Debug.WriteLine("Application Active = " + ApplicationActivated.ToString());
        }

#pragma warning disable CS8618
        public NativeForm()
#pragma warning restore CS8618
        {
            if (!DesignMode)
            {
                throw new NotSupportedException();
            }
        }

        const int GWL_STYLE = -16;

        public NativeForm(RcObject rc, int i)
        {
            var di = rc.DialogAt(i);

            m_nwl = new MyNativeWindowListener(this);
            m_nw = new MyNativeWindow(this, ref di);
            m_rc = rc;

            this.ClientSize = new Size(di.Width, di.Height);
            this.Text = di.Caption ?? "W9xNET Window";
            this.Icon = SystemIcons.Application;
            this.Location = new Point(di.X, di.Y);
            this.Font = new Font(di.FontFamily, di.FontSize);

            User32.SetWindowLong32(Handle, GWL_STYLE, di.Style);
            Uxtheme.SetWindowTheme(Handle, string.Empty, string.Empty);
        }
    }

    // NativeWindow class to listen to operating system messages.
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    internal class MyNativeWindowListener : NativeWindow
    {
        private readonly NativeForm m_parent;

        public MyNativeWindowListener(NativeForm parent)
        {
            parent.HandleCreated += OnHandleCreated;
            parent.HandleDestroyed += OnHandleDestroyed;
            this.m_parent = parent;
        }

        // Listen for the control's window creation and then hook into it.
        internal void OnHandleCreated(object? sender, EventArgs e)
        {
            Debug.Assert(sender != null);

            // Window is now created, assign handle to NativeWindow.
            AssignHandle(((NativeForm)sender).Handle);
        }

        internal void OnHandleDestroyed(object? sender, EventArgs e)
        {
            // Window was destroyed, release hook.
            ReleaseHandle();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages
            switch (m.Msg)
            {
                case WM_INITDIALOG:
                    int a = 1;
                    a += 1;
                    break;

                case WM_ACTIVATEAPP:

                    // Notify the form that this message was received.
                    // Application is activated or deactivated, 
                    // based upon the WParam parameter.
                    m_parent.ApplicationActivated(((int)m.WParam != 0));

                    break;

                case WM_COMMAND:
                    //((NativeForm)m_parent).ButtonMap[(int)m.WParam].Invoke(this, new EventArgs());
                    break;
            }

            base.WndProc(ref m);
        }
    }

    // MyNativeWindow class to create a window given a class name.
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    internal class MyNativeWindow : NativeWindow
    {
        private int windowHandle;

        const int FF_DONTCARE = (0 << 4);
        const int DEFAULT_PITCH = 0;
        const int DEFAULT_QUALITY = 0;
        const int CLIP_DEFAULT_PRECIS = 0;
        const int OUT_TT_PRECIS = 4;
        const int FW_DONTCARE = 0;
        const int ANSI_CHARSET = 0;
        const int WM_SETFONT = 0x0030;
        const int GWLP_ID = -12;

        private static IntPtr CreateControl(DialogControlElement control, IntPtr hWndParent)
        {
            IntPtr handle = W9xNET.Interop.Win32.User32.CreateWindowEx(
                0,
                control.IDToPredefClass(),
                control.Caption.ToString(),
                control.Style,
                control.X, control.Y,
                control.Width, control.Height,
                hWndParent,
                IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            return handle;
        }

        public MyNativeWindow(Form parent, ref DialogInfo di)
        {
            //Win32.CreateDialogIndirectParamA(IntPtr.Zero, (DLGTEMPLATE)di.RcData, IntPtr.Zero, 0, IntPtr.Zero);

            foreach (var control in di.Controls)
            {
                IntPtr handle = CreateControl(control, parent.Handle);

                User32.SetWindowLong32(handle, GWLP_ID, control.ID);

                IntPtr hFont = Gdi32.CreateFont(di.FontSize, 0, 0, 0,
                    FW_DONTCARE, 0, 0, 0,
                    ANSI_CHARSET, OUT_TT_PRECIS, CLIP_DEFAULT_PRECIS, DEFAULT_QUALITY, DEFAULT_PITCH | FF_DONTCARE, di.FontFamily);

                User32.SendMessage(handle, WM_SETFONT, hFont, (nint)1 /* true */);
            }
        }

        // Listen to when the handle changes to keep the variable in sync
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void OnHandleChange()
        {
            windowHandle = (int)this.Handle;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Listen for messages that are sent to the button window. Some messages are sent
            // to the parent window instead of the button's window.

            base.WndProc(ref m);
        }
    }
}