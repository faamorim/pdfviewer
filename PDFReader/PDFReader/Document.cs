using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PDFReader
{
    public class Document
    {
        public pdftron.PDF.PDFDoc PDF { get; private set; }
        public StorageFile File { get; private set; }
        public string Path => File.Path;
        public string Name => File.Name;
        public string FileType => File.FileType;

        private Document()
        {

        }

        public Document(pdftron.PDF.PDFDoc doc)
        {
            PDF = doc;
        }

        public static async Task<Document> Load(StorageFile file)
        {
            pdftron.PDF.PDFDoc pdf;
            if (file.FileType == ".pdf")
            {
                pdf = new pdftron.PDF.PDFDoc(file);
            }
            else if (PDFHelper.CanLoadOffice)
            {
                Windows.Storage.Streams.IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                using (pdftron.Filters.RandomAccessStreamFilter filter = new pdftron.Filters.RandomAccessStreamFilter(stream))
                {
                    pdf = new pdftron.PDF.PDFDoc();
                    pdftron.PDF.Convert.OfficeToPDF(pdf, filter, null);
                }
            }
            else
            {
                throw new FileLoadException("Unable to load file as PDF.", file.Path);
            }

            Document doc = new Document()
            {
                PDF = pdf,
                File = file
            };

            return doc;
        }

        public override string ToString()
        {
            return !String.IsNullOrEmpty(Name) ? Name : "PDF Document";
        }
    }
}
