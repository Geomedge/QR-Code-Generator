using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using WinRT.Interop;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace QR_Code_Generator
{
    public sealed partial class MainWindow : Window
    {
        public static class AppState
        {
            //Downloads QR Code image to user selected location
            public static async Task Download(Bitmap bitmap)
            {
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();

                var hwnd = GetActiveWindow();
                InitializeWithWindow.Initialize(savePicker, hwnd);

                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                savePicker.FileTypeChoices.Add("PNG Image", new List<string>() { ".png" });

                var storageFile = await savePicker.PickSaveFileAsync();
                if (storageFile != null)
                {
                    using (var outStream = await storageFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                    {
                        bitmap.Save(outStream.AsStreamForWrite(), ImageFormat.Png);
                    }
                }
            }
            [DllImport("user32.dll")]
            private static extern IntPtr GetActiveWindow();
        }

        //Initialises the MainWindow and loads pages
        public MainWindow()
        {
            this.InitializeComponent();
            // Pages Here
            WifiFrame.Navigate(typeof(Wifi));

            //Titlebar Fixes
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            if (appWindow != null)
            {
                var titleBar = appWindow.TitleBar;

                titleBar.ExtendsContentIntoTitleBar = true;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;
            }
        }
    }
}
