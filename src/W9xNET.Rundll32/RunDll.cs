using LegalMitigation;
using System.Diagnostics;
using W9xNET.Interop.Win32;
using W9xNET.User32.Controls;
using W9xNET.User32.Controls.Interfaces;

namespace W9xNET.Rundll32
{
    /// <summary>
    /// A mocked Rundll32 simulation.
    /// </summary>
    public class RunDll
    {
        /// <summary>
        /// An event that occurs when a form was created and placed on the foreground.
        /// </summary>
        public delegate void OnFormSpawnedEventHandler();

        public delegate Dictionary<string, OnInvokedEventHandler> OnPathResolverStartupEventHandler();

        /// <summary>
        /// An event that occurs when a form should stick below the taskbar.
        /// </summary>
        public delegate void OnKeepFormBelowTaskbarEventHandler();

        [Obsolete("Eternal95 stub, don't use")]
        public event OnFormSpawnedEventHandler? OnFormSpawned;

        [Obsolete("Eternal95 stub, don't use")]
        public event OnKeepFormBelowTaskbarEventHandler? OnKeepFormBelowTaskbar;

        private readonly OnPathResolverStartupEventHandler OnPathResolverStartup;

        /// <summary>
        /// A routine that gets called everytime the user clicks on an icon element
        /// </summary>
        /// <param name="sender">The instance to the Rundll object</param>
        /// <param name="args">Command-line parameters passed each time an icon element is being invoked</param>
        public delegate void OnInvokedEventHandler(RunDll sender, params string[] args);

        /// <summary>
        /// The interface handle to the INxnForm that is retrieved from the constructor
        /// </summary>
        readonly INxnForm _nxnForm;

        /// <summary>
        /// A shortcut to retrieving the Form through the _nxnForm instance
        /// </summary>
        Form Parent => _nxnForm.GetForm();

        /// <summary>
        /// A shortcut to retrieving the Container through the _nxnForm instance
        /// </summary>
        Control Container => _nxnForm.GetContainer();

        /// <summary>
        /// Invoke listeners dictionary
        /// </summary>
        private Dictionary<string, OnInvokedEventHandler> _listeners;

        /// <summary>
        /// Initializes the RunDll instance by specifying a valid Parent object.
        /// Without the parent object, this class will be rendered as useless.
        /// </summary>
        /// <param name="parent"></param>
        public RunDll(INxnForm nxnForm, OnPathResolverStartupEventHandler callback)
        {
            _nxnForm = nxnForm;

            _listeners = callback.Invoke();

            if (_listeners.Count == 0)
            {
                throw new NotSupportedException("listener count shouldn't be zero");
            }

            OnPathResolverStartup = callback;
        }

        /// <summary>
        /// Runs a core W9xNET application.
        /// </summary>
        /// <param name="exe">The instance to the W9xNET-based application.</param>
        /// <param name="args">Command-line arguments to be passed upon launch.</param>
        /// <param name="parent">The parent control.</param>
        /// <returns>The newly created form.</returns>
        public Form Run(W9xNETApp exe, string args = "", Form? parent = null)
        {
            if (parent == null)
            {
                parent = _nxnForm.GetForm();
            }

            var form = exe.FormEntry;
            if (form == null)
            {
                throw new NullReferenceException("The specified application could not be run.");
            }

            form.TopLevel = false;
            //childForm.MdiParent = parentForm;

            form.BackColor = Color.FromArgb(192, 192, 192);
            parent.Controls.Add(form);
            form.Show();
            //form.Location = new Point((parentForm.Width - childForm.Width) / 2, (parentForm.Height - childForm.Height) / 4);
            form.BringToFront();

            MorphToClassic(form.Handle);

            return form;
        }

        private void MorphToClassic(IntPtr handle) => Uxtheme.SetWindowTheme(handle, string.Empty, string.Empty);

        /// <summary>
        /// Runs a program externally (i.e. an actual Microsoft Windows application executable.
        /// </summary>
        /// <param name="filename">Fully qualified path to the executable.</param>
        /// <param name="args">The Command-line arguments to be passed upon launch.</param>
        public void Run(string filename, string args = "")
        {
            //TOOD: Needs to be sorted out.
            IntPtr appWin1;

            var parent = Parent;

            ProcessStartInfo ps1 = new(filename);
            ps1.WindowStyle = ProcessWindowStyle.Minimized;
            var p1 = Process.Start(ps1);
            Thread.Sleep(5000); // Allow the process to open it's window
            appWin1 = p1.MainWindowHandle;
            // Put it into this form
            W9xNET.Interop.Win32.User32.SetParent(appWin1, parent.Handle);
            // Move the window to overlay it on this window
            W9xNET.Interop.Win32.User32.MoveWindow(appWin1, 0, 0, parent.ClientSize.Width / 2, parent.ClientSize.Height, true);
            //SetWindowTheme(appWin1, string.Empty, string.Empty);
        }

        public void Run(IconElement iconElement)
        {
            if (_listeners.ContainsKey(iconElement.Path))
            {

                _listeners[iconElement.Path].Invoke(this);
            }
            else
            {
                //TODO: implement the case where the user launches usermode applications (i.e. outside the 9Xnet environment)
                Debugger.Break();
            }
        }

        public void Run(Form form, int x = 0, int y = 0)
        {
            form.TopLevel = false;
            form.Location = new Point(x, y);

            MorphToClassic(form.Handle);

            Container.Controls.Add(form);
            form.Show();
        }
    }
}