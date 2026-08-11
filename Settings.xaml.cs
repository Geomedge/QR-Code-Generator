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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
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
            this.InitializeComponent();
            LoadSettings();
        }

        private void Checkbox1(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            bool isChecked = checkbox.IsChecked == true;

            switch (checkbox.Name)
            {
                case "Wifi":
                    Save("Wifi", isChecked);
                    break;

                case "Phone":
                    Save("Phone", isChecked);
                    break;

                case "Link":
                    Save("Link", isChecked);
                    break;
            }
        }

        private void Save(string key, bool value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }

        private void LoadSettings()
        {
            WifiCheck.IsChecked = GetSetting("Wifi");
            PhoneCheck.IsChecked = GetSetting("Phone");
            LinkCheck.IsChecked = GetSetting("Link");
        }

        private bool GetSetting(string key)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                return (bool)ApplicationData.Current.LocalSettings.Values[key];
            }
            return false;
        }
    }
}
