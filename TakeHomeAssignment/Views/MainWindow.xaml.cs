using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace TakeHomeAssignment
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var windowId = Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(this));
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Resize(new Windows.Graphics.SizeInt32(600, 400));
        }
    }
}
