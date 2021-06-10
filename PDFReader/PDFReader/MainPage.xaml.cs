using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PDFReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        pdftron.PDF.PDFViewCtrl MyPDFViewCtrl;

        public MainPage()
        {
            this.InitializeComponent();
            pdftron.PDFNet.Initialize("your_key");
            MyPDFViewCtrl = new pdftron.PDF.PDFViewCtrl();
            PDFViewBorder.Child = MyPDFViewCtrl;
            OpenButton.Click += OpenButton_Click;
        }

        async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Get a file from the file picker.
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.List;
            fileOpenPicker.FileTypeFilter.Add(".pdf");
            StorageFile file = await fileOpenPicker.PickSingleFileAsync();

            // Create a PDFDocument and use it as the source for the PDFViewCtrl
            if (file != null)
            {
                Windows.Storage.Streams.IRandomAccessStream stream = await
                    file.OpenAsync(FileAccessMode.ReadWrite);
                pdftron.PDF.PDFDoc doc = new pdftron.PDF.PDFDoc(stream);
                MyPDFViewCtrl.SetDoc(doc);
            }
        }
    }
}
