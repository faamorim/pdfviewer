using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PDFReader
{
    public class DocumentManager
    {
        public ObservableCollection<Document> Documents = new ObservableCollection<Document>();

        public void Load(StorageFile file)
        {
            var doc = Document.Load(file);
            doc.ContinueWith((task) =>
            {
                if (task.IsCompleted)
                {
                    Documents.Add(task.Result);
                }
            }
            );
        }
    }
}
