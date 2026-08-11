using CommunityToolkit.Common;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic.FileIO;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using WinRT.Interop;
using static QR_Code_Generator.MainWindow;
using static QRCoder.PayloadGenerator;
using static QRCoder.PayloadGenerator.WiFi;
using static System.Net.Mime.MediaTypeNames;


namespace QR_Code_Generator
{
    public sealed partial class Wifi : Page
    {
        public Wifi()
        {
            this.InitializeComponent();
        }

        //--------------------------------------------------------------------------------------------
        //Global Variables
        Authentication auth;
        Bitmap? localBitmap;

        //Local folder for saving QR code images
        private StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        //--------------------------------------------------------------------------------------------


        //QR Code Generators
        // WiFi (Combo + 2 inputs) - SSID and Password
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Null Check
            if (e?.AddedItems?.FirstOrDefault() is ComboBoxItem comboBoxItem && comboBoxItem.Content is string selectedSecurity)
            {
                switch (selectedSecurity)
                {
                    case "WPA":  auth = Authentication.WPA;  break;
                    case "WEP":  auth = Authentication.WEP;  break;
                    case "WPA2": auth = Authentication.WPA2; break;
                    default: break;
                }
            }
        }

        private void GenerateWifiButton_Click(object sender, RoutedEventArgs e)
        {
            //Null check
            if (string.IsNullOrWhiteSpace(SSID.Text) || string.IsNullOrWhiteSpace(Password.Password)) { return; }

            WiFi generator = new WiFi(SSID.Text, Password.Password, auth);
            string payload = generator.ToString();

            SaveQRCode(payload);
        }

        // URL (1 Input) - URLTextBox
        private void GenerateURLButton_Click(object sender, RoutedEventArgs e)
        {
            //Null check
            if (string.IsNullOrWhiteSpace(URLTextBox.Text)) { return; }

            Url generator = new Url(URLTextBox.Text);
            string payload = generator.ToString();

            SaveQRCode(payload);
        }

        // Phone Number (1 Input) - PhoneTextBox
        private void GeneratePhoneButton_Click(object sender, RoutedEventArgs e)
        {
            //Null check
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text)) { return; }

            PhoneNumber generator = new PhoneNumber(PhoneTextBox.Text);
            string payload = generator.ToString();
            
            SaveQRCode(payload);
        }


        //QR Saving to Windows Local Folder and Displaying in the UI
        //For Local storage check : Download QR Code Dropdown Menu
        private async void SaveQRCode(string payload)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(20);


            //Enables the 2 buttons when a QR code is generated
            ClearButton.IsEnabled = true;
            DownloadButton.IsEnabled = true;

            //Stores the generated QR code bitmap in a variable for later use
            localBitmap = qrCodeAsBitmap;


            // Save the bitmap to Windows local folder
            var file = await localFolder.CreateFileAsync("qrCode.jpg", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
            {
                qrCodeAsBitmap.Save(stream.AsStreamForWrite(), System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            // Load the saved image into the UI with cache busting
            var bitmapImage = new Microsoft.UI.Xaml.Media.Imaging.BitmapImage();
            var uri = new Uri(file.Path + "?t=" + DateTime.Now.Ticks); // cache buster
            bitmapImage.UriSource = uri;

            qrCodeImage.Source = null; // clear current image
            qrCodeImage.Source = bitmapImage; // load new image
        }


        //Clear and Download Buttons
        //Clear QR Code Button
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SSID.Text = string.Empty;
            Password.Password = string.Empty;
            qrCodeImage.Source = null; // Clear the displayed QR code

            //Disables the buttons when No QR Code
            ClearButton.IsEnabled = false;
            DownloadButton.IsEnabled = false;
        }

        //Download QR Code Dropdown Menu
        // For 800x800
        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e) { download(800); }
        //For 1000x1000
        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e) { download(1000); }
        //For 1200x1200
        private void MenuFlyoutItem_Click_3(object sender, RoutedEventArgs e) { download(1200); }

        //Download QR Code Function
        private async void download(int size)
        {
            if (localBitmap == null) { return; }
            Bitmap resizedBitmap = new Bitmap(localBitmap, new System.Drawing.Size(size, size));
            await AppState.Download(resizedBitmap);
        }

    }
}
