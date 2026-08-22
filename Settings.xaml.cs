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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using static QR_Code_Generator.MainWindow;

namespace QR_Code_Generator
{
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
            VersionTextBlock.Text = GetAppVersion();
        }

        public class SettingData
        {
            private readonly ApplicationDataContainer _local =
                ApplicationData.Current.LocalSettings;

            // ---- Default values ----
            private const bool DefaultWifi = true;
            private const bool DefaultPhone = true;
            private const bool DefaultLink = true;

            //Experiments
            private const bool DefaultShowMassQR = false;

            // ---- Publicly readable settings ----
            public bool Wifi { get; private set; }
            public bool Phone { get; private set; }
            public bool Link { get; private set; }
            public bool ShowMassQR { get; private set; }

            // ---- Global access across the whole project ----
            public static SettingData Current { get; } = new SettingData();

            private SettingData()
            {
                Load();
            }

            // ---- Load settings (defaults + saved values) ----
            private void Load()
            {
                Wifi = _local.Values["Wifi"] as bool? ?? DefaultWifi;
                Phone = _local.Values["Phone"] as bool? ?? DefaultPhone;
                Link = _local.Values["Link"] as bool? ?? DefaultLink;
                ShowMassQR = _local.Values["ShowMassQR"] as bool? ?? DefaultShowMassQR;
            }

            // ---- Save updated values ----
            public void Update(string key, bool value)
            {
                _local.Values[key] = value;

                switch (key)
                {
                    case "Wifi": Wifi = value; break;
                    case "Phone": Phone = value; break;
                    case "Link": Link = value; break;
                    case "ShowMassQR": ShowMassQR = value; break;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WifiCheck.IsChecked = SettingData.Current.Wifi;
            PhoneCheck.IsChecked = SettingData.Current.Phone;
            LinkCheck.IsChecked = SettingData.Current.Link;
            //ShowMassQR.IsOn = SettingData.Current.ShowMassQR;
        }

        private void Checkbox1(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            bool isChecked = checkbox.IsChecked == true;

            switch (checkbox.Name)
            {
                case "WifiCheck":
                    SettingData.Current.Update("Wifi", isChecked);
                    break;

                case "PhoneCheck":
                    SettingData.Current.Update("Phone", isChecked);
                    break;

                case "LinkCheck":
                    SettingData.Current.Update("Link", isChecked);
                    break;
            }

        }

        //Experiments
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var toggle = (ToggleSwitch)sender;
            bool isToggled = toggle.IsOn;

            SettingData.Current.Update(toggle.Name, isToggled);

            if (toggle.Name == "ShowMassQR")
            {
                // Call MainWindow to update the NavigationView
                MainWindow.Instance?.RefreshNavigationView();
            }

        }


        public static string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private async void OpenIssueLink_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(
                new Uri("https://github.com/Geomedge/QR-Code-Generator/issues/new/choose")
            );
        }


    }
}
