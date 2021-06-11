using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace PDFReader
{
    public static class PDFHelper
    {
        public static bool CanLoadOffice
        {
            get; private set;
        }

        static bool init = false;
        public static void Init()
        {
            if (!init)
            {
                Setup();
                init = true;
            }
        }

        public static void Setup()
        {
            pdftron.PDFNet.Initialize();
            FindResources();
        }

        static async void FindResources()
        {
            StorageFolder localFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            if (await localFolder.TryGetItemAsync("Assets") is StorageFolder assets && await assets.TryGetItemAsync("Resources") is StorageFolder resources && await resources.TryGetItemAsync("pdftron_layout_resources.plugin") != null)
            {
                pdftron.PDFNet.AddResourceSearchPath(resources.Path);
                CanLoadOffice = true;
            }
        }

        public static void PrepareFileOpenPicker(FileOpenPicker fileOpenPicker)
        {
            fileOpenPicker.FileTypeFilter.Add(".pdf");
            if (CanLoadOffice)
            {
                fileOpenPicker.FileTypeFilter.Add(".doc");
                fileOpenPicker.FileTypeFilter.Add(".docx");
                fileOpenPicker.FileTypeFilter.Add(".ppt");
                fileOpenPicker.FileTypeFilter.Add(".pptx");
                fileOpenPicker.FileTypeFilter.Add(".xls");
                fileOpenPicker.FileTypeFilter.Add(".xlsx");
            }
        }

        public static async Task<pdftron.PDF.PDFDoc> LoadPDFFromFile(StorageFile file)
        {
            pdftron.PDF.PDFDoc doc;
            if (file.FileType == ".pdf")
            {
                return new pdftron.PDF.PDFDoc(file);
            }
            else
            {
                Windows.Storage.Streams.IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                using (pdftron.Filters.RandomAccessStreamFilter filter = new pdftron.Filters.RandomAccessStreamFilter(stream))
                {
                    doc = new pdftron.PDF.PDFDoc();
                    pdftron.PDF.Convert.OfficeToPDF(doc, filter, null);
                }
            }
            return doc;
        }
    }
}
