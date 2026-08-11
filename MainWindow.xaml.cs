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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WinRT.Interop;
using static QR_Code_Generator.Settings;

namespace QR_Code_Generator
{
    public sealed partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }
        public MainWindow()
        { 
            this.InitializeComponent();
            Instance = this;
            NavView.SelectedItem = NavView.MenuItems[0];
            ContentFrame.Navigate(typeof(Wifi));
            ExtendsContentIntoTitleBar = true;
            RefreshNavigationView();
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) // Handles the back button click event
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
                var currentPageType = ContentFrame.CurrentSourcePageType;

                foreach (var item in NavView.MenuItems.OfType<NavigationViewItem>())
                {
                    if (item.Tag.ToString() == currentPageType.Name)
                    {
                        NavView.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public static class AppState
        {
            public static bool[] elementHide = new bool[3] { true, true, true };

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



        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(Settings));
                return;
            }

            

            if (args.SelectedItem is NavigationViewItem item)
            {
                switch (item.Tag)
                {
                    case "Wifi":
                        ContentFrame.Navigate(typeof(Wifi));
                        break;
                    case "MassQRCode":
                        ContentFrame.Navigate(typeof(MassQRCode));
                        break;
                }
            }
        }

        public void RefreshNavigationView()
        {
            var massItem = NavView.MenuItems
        .OfType<NavigationViewItem>()
        .FirstOrDefault(i => (string)i.Tag == "MassQRCode");

            if (massItem != null)
            {
                massItem.Visibility = SettingData.Current.ShowMassQR
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

    }
}
