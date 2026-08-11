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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using static QR_Code_Generator.MainWindow;

namespace QR_Code_Generator
{
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            Checkboxes();
        }

        public void SavedSettings()
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["DarkMode"] = true;
            bool darkMode = (bool)(settings.Values["DarkMode"] ?? false);
        }


        public void Checkboxes()
        {
            WifiCheckBox.IsChecked = AppState.elementHide[0];
            LinkCheckBox.IsChecked = AppState.elementHide[1];
            PhoneCheckBox.IsChecked = AppState.elementHide[2];
        }


    }
}
