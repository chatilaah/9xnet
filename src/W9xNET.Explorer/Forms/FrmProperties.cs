using System.ComponentModel;
using W9xNET.Explorer.Dialogs;

namespace W9xNET.Explorer.Forms
{
    internal class FrmProperties : Form
    {
        readonly DlgTaskbarOptions taskbarOptions;
        readonly DlgStartMenuPrograms startMenuPrograms;
        readonly Button btnApply;
        readonly Button btnOk;
        readonly Button btnCancel;
        readonly Button btnHelp;
        readonly TabControl tabControl;

        public FrmProperties()
        {
            //
            // taskbarOptions
            //
            this.taskbarOptions = new DlgTaskbarOptions
            {
                Name = "taskbarOptions",
                FormBorderStyle = FormBorderStyle.None,
                TopLevel = false,
                Visible = true
            };
            //
            // startMenuPrograms
            //
            this.startMenuPrograms = new DlgStartMenuPrograms
            {
                Name = "startMenuPrograms",
                FormBorderStyle = FormBorderStyle.None,
                TopLevel = false,
                Visible = true
            };

            //
            // btnApply
            //
            this.btnApply = new Button
            {
                Name = "btnApply",
                Size = new(75, 23),
                Location = new(257, 345),
                Visible = true,
                Enabled = false,
                Text = "Apply"
            };

            this.btnApply.Click += delegate (object? sender, EventArgs e)
            {
                throw new NotImplementedException();
            };

            //
            // btnOk
            //
            this.btnOk = new Button
            {
                Name = "btnOk",
                Size = new(75, 23),
                Location = new(95, 345),
                Visible = true,
                Text = "OK"
            };

            this.btnOk.Click += delegate (object? sender, EventArgs e)
            {
                throw new NotImplementedException();
            };

            //
            // btnCancel
            //
            this.btnCancel = new Button
            {
                Name = "btnCancel",
                Size = new(75, 23),
                Location = new(176, 345),
                Visible = true,
                Text = "Cancel"
            };

            btnCancel.Click += delegate (object? sender, EventArgs e)
            {
                Close();
            };

            //
            // btnHelp
            //
            this.btnHelp = new Button
            {
                Name = "btnHelp",
                Size = new(75, 23),
                Location = new(338, 345),
                Visible = false,
                Text = "Help"
            };

            //
            // tabControl
            //
            this.tabControl = new TabControl
            {
                Name = "tabControl",
                Size = new Size(326, 332),
                Location = new Point(6, 7),
            };
            this.tabControl.TabPages.Add(taskbarOptions.Text);
            this.tabControl.TabPages.Add(startMenuPrograms.Text);
            this.tabControl.TabPages[0].Controls.Add(taskbarOptions);
            this.tabControl.TabPages[1].Controls.Add(startMenuPrograms);
            this.taskbarOptions.Dock = DockStyle.Fill;
            this.startMenuPrograms.Dock = DockStyle.Fill;

            this.TopLevel = false;
            this.Name = "frmProperties";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Taskbar Properties";
            this.ClientSize = new Size(344, 375);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            Controls.AddRange(new Control[] {
                tabControl,
                btnOk,
                btnCancel,
                btnApply,
                btnHelp
            });
        }
    }
}
