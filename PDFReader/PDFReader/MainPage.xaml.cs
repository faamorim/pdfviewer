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
            pdftron.PDFNet.Initialize();
            Setup();
            MyPDFViewCtrl = new pdftron.PDF.PDFViewCtrl();
            PDFViewBorder.Child = MyPDFViewCtrl;
            OpenButton.Click += OpenButton_Click;
        }

        async void Setup()
        {
            StorageFolder localFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var assets = await localFolder.GetFolderAsync("Assets");
            pdftron.PDFNet.AddResourceSearchPath(assets.Path);
        }

        async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Get a file from the file picker.
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.List;
            fileOpenPicker.FileTypeFilter.Add(".pdf");
            fileOpenPicker.FileTypeFilter.Add(".doc");
            fileOpenPicker.FileTypeFilter.Add(".docx");
            fileOpenPicker.FileTypeFilter.Add(".ppt");
            fileOpenPicker.FileTypeFilter.Add(".pptx");
            fileOpenPicker.FileTypeFilter.Add(".xls");
            fileOpenPicker.FileTypeFilter.Add(".xlsx");
            StorageFile file = await fileOpenPicker.PickSingleFileAsync();

            // Create a PDFDocument and use it as the source for the PDFViewCtrl
            if (file != null)
            {
                pdftron.PDF.PDFDoc doc;
                Windows.Storage.Streams.IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                using (pdftron.Filters.RandomAccessStreamFilter filter = new pdftron.Filters.RandomAccessStreamFilter(stream))
                {
                    //pdftron.Filters.FilterReader reader = new pdftron.Filters.FilterReader(filter);

                    if (file.FileType == ".pdf")
                    {
                        doc = new pdftron.PDF.PDFDoc(file);
                    }
                    else
                    {
                        doc = new pdftron.PDF.PDFDoc();
                        pdftron.PDF.Convert.OfficeToPDF(doc, filter, null);
                    }
                }
                MyPDFViewCtrl.SetDoc(doc);
            }
        }
    }
}
