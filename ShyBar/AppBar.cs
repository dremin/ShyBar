using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShyBar
{
    public class AppBar : IDisposable
    {
        private readonly HookWindow _hookWin;
        private IntPtr _hwnd;
        private bool _isHidden;

        private static int WM_TASKBARCREATEDMESSAGE = -1;

        public AppBar()
        {
            _hookWin = new HookWindow();
            _hookWin.CreateHandle(new CreateParams());
            _hookWin.MessageReceived += hookWin_MessageReceived;

            _hwnd = findTrayHwnd();

            WM_TASKBARCREATEDMESSAGE = RegisterWindowMessage("TaskbarCreated");
        }

        private void hookWin_MessageReceived(Message m)
        {
            if (m.Msg == WM_TASKBARCREATEDMESSAGE)
            {
                // Update hwnd if new taskbar is created
                _hwnd = findTrayHwnd();
            }
        }

        public void Hide()
        {
            if (_isHidden)
            {
                return;
            }

            _isHidden = true;
            setState(TaskbarState.AutoHide);
        }

        public void Show()
        {
            if (!_isHidden)
            {
                return;
            }

            _isHidden = false;
            setState(TaskbarState.OnTop);
        }

        public void Dispose()
        {
            Show();
        }

        private void setState(TaskbarState state)
        {
            Task.Run(() =>
            {
                APPBARDATA abd = new APPBARDATA
                {
                    cbSize = Marshal.SizeOf(typeof(APPBARDATA)),
                    hWnd = _hwnd,
                    lParam = (IntPtr)state
                };

                SHAppBarMessage((int)ABMsg.ABM_SETSTATE, ref abd);
            });
        }

        private IntPtr findTrayHwnd()
        {
            return FindWindow("Shell_TrayWnd", "");
        }

        enum TaskbarState : int
        {
            OnTop = 0,
            AutoHide
        }

        [StructLayout(LayoutKind.Sequential)]
        struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public Rect rc;
            public IntPtr lParam;
        }

        enum ABMsg : int
        {
            ABM_NEW = 0,
            ABM_REMOVE,
            ABM_QUERYPOS,
            ABM_SETPOS,
            ABM_GETSTATE,
            ABM_GETTASKBARPOS,
            ABM_ACTIVATE,
            ABM_GETAUTOHIDEBAR,
            ABM_SETAUTOHIDEBAR,
            ABM_WINDOWPOSCHANGED,
            ABM_SETSTATE
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Rect
        {
            public Rect(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width => Right - Left;

            public int Height => Bottom - Top;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int RegisterWindowMessage(string msg);

        [DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);
    }
}
