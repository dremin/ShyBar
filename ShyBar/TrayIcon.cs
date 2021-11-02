using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ShyBar
{
    class TrayIcon : IDisposable
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenuStrip _contextMenu;

        public TrayIcon()
        {
            Container container = new Container();

            _contextMenu = createContextMenu(container);
            _notifyIcon = new NotifyIcon(container);

            _notifyIcon.ContextMenuStrip = _contextMenu;
            _notifyIcon.Icon = SystemIcons.Information;
            _notifyIcon.Text = "ShyBar";
            _notifyIcon.Visible = true;
        }

        private ContextMenuStrip createContextMenu(Container container)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip(container);

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit ShyBar", null, (sender, e) =>
            {
                Application.Exit();
            });

            contextMenu.Items.Add(exitItem);

            return contextMenu;
        }

        public void Dispose()
        {
            _notifyIcon.Visible = false;
        }
    }
}
