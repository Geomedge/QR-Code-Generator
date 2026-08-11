using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.WindowsAppSDK.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QR_Code_Generator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MassQRCode : Page
    {
        public ObservableCollection<DataItem> MyData { get; set; }
        
        public MassQRCode()
        {
            InitializeComponent();

            MyData = new ObservableCollection<DataItem>
            {
                new DataItem { QRCodeData = "https://google.com", QRCodeType = "URL", QRCodeData1 = "Additional Data 1", QRCodeData2 = "Additional Data 2" },
                new DataItem { QRCodeData = "https://google.com", QRCodeType = "URL", QRCodeData1 = "Additional Data 1", QRCodeData2 = "Additional Data 2" },
                new DataItem { QRCodeData = "999", QRCodeType = "Phone", QRCodeData1 = "Additional Data 1", QRCodeData2 = "Additional Data 2" }
            };
            this.DataContext = this;
        }

        public class Data
        {  
            public string QRCodeData { get; set; } = string.Empty;
            public string QRCodeType { get; set; } = string.Empty;
            public string QRCodeData1 { get; set; } = string.Empty;
            public string QRCodeData2 { get; set; } = string.Empty;
            public Data()
            {
                QRCodeData = "Test";
                QRCodeData1 = "Test1";
                QRCodeData2 = "Test2";
                QRCodeType = "TestType";
            }

        }




        public class DataItem
        {
            public string QRCodeData { get; set; } = string.Empty;
            public string QRCodeType { get; set; } = string.Empty;
            public string QRCodeData1 { get; set; } = string.Empty;
            public string QRCodeData2 { get; set; } = string.Empty;
        }

        public void GenerateMassQRCodeButton_Click(object sender, RoutedEventArgs e)
        {
            new DataItem { QRCodeData = "999", QRCodeType = "Phone", QRCodeData1 = "Additional Data 1", QRCodeData2 = "Additional Data 2" };
        }
    }
}
