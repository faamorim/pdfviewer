using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFReader
{
    public class ManagerViewModel : ViewModelBase
    {
        DocumentManager _manager;
        Document _currentDocument;

        private DocumentManager Manager
        {
            get => _manager;
            set
            {
                if (Manager != null)
                {
                    Documents.CollectionChanged -= Documents_CollectionChanged;
                }
                _manager = value;
                if (Manager != null)
                {
                    Documents.CollectionChanged += Documents_CollectionChanged;
                }
            }
        }
        public Document CurrentDocument
        {
            get => _currentDocument;
            set
            {
                _currentDocument = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Document> Documents
        {
            get => Manager?.Documents;
        }


        public ManagerViewModel()
        {
            Manager = new DocumentManager();
        }

        ~ManagerViewModel()
        {
            Manager = null;
        }

        private void Documents_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    CurrentDocument = e.NewItems.Cast<object>().LastOrDefault(item => item is Document) as Document;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if (e.OldItems.Contains(CurrentDocument))
                    {
                        CurrentDocument = Documents.First();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    CurrentDocument = null;
                    break;
            }
            OnPropertyChanged(nameof(Documents));
        }
    }
}
