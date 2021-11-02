using System.Windows.Forms;

namespace ShyBar
{
    public class HookWindow : NativeWindow
    {
        public delegate void MessageReceivedEventHandler(Message m);

        public event MessageReceivedEventHandler MessageReceived;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            MessageReceived?.Invoke(m);
        }
    }
}
