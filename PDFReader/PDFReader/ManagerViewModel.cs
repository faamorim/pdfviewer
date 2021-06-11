using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using muxc = Microsoft.UI.Xaml.Controls;

namespace PDFReader
{
    public class ManagerViewModel : ViewModelBase
    {
        public Command OpenCommand {get; private set;}

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
                if(_currentDocument != value)
                {
                    _currentDocument = value;
                    OnPropertyChanged(nameof(SelectedTab));
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<muxc.TabViewItem> DocumentTabs
        {
            get; private set;
        }

        public muxc.TabViewItem SelectedTab
        {
            get => GetTabFromDocument(CurrentDocument);
            set
            {
                CurrentDocument = (value?.Tag as Document);
            }
        }

        public ObservableCollection<Document> Documents
        {
            get => Manager?.Documents;
        }


        public ManagerViewModel()
        {
            Manager = new DocumentManager();
            DocumentTabs = new ObservableCollection<muxc.TabViewItem>();
            OpenCommand = new Command(new Action(Open));
        }

        ~ManagerViewModel()
        {
            Manager = null;
        }

        muxc.TabViewItem GenerateTabFromDocument(Document document)
        {
            muxc.TabViewItem tab = new muxc.TabViewItem();
            tab.Tag = document;
            UpdateTab(tab);
            return tab;
        }

        muxc.TabViewItem GetTabFromDocument(Document document)
        {
            return DocumentTabs.FirstOrDefault(tab => tab.Tag == document) as muxc.TabViewItem;
        }

        void UpdateTab(Document document)
        {
            UpdateTab(GetTabFromDocument(document));
        }

        void UpdateTab(muxc.TabViewItem tab)
        {
            if (tab is null || !(tab.Tag is Document document))
            {
                return;
            }
            tab.Header = document.Name;
            string glyph = document.File.FileType == ".pdf" ? "\xEA90" : "\xE8A5";
            tab.IconSource = new muxc.FontIconSource() { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = glyph };
        }


        private void Documents_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (Document newItem in e.NewItems)
                    {
                        DocumentTabs.Add(GenerateTabFromDocument(newItem));
                    }
                    CurrentDocument = e.NewItems.Cast<object>().LastOrDefault(item => item is Document) as Document;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Document oldItem in e.OldItems)
                    {
                        DocumentTabs.Remove(GetTabFromDocument(oldItem));
                    }
                    if (e.OldItems.Contains(CurrentDocument))
                    {
                        CurrentDocument = Documents.First();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    CurrentDocument = null;
                    DocumentTabs.Clear();
                    break;
            }
            OnPropertyChanged(nameof(Documents));
        }

        public async void Open()
        {
            // Get a file from the file picker.
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.List;
            PDFHelper.PrepareFileOpenPicker(fileOpenPicker);
            StorageFile file = await fileOpenPicker.PickSingleFileAsync();

            // Create a PDFDocument and use it as the source for the PDFViewCtrl
            if (file != null)
            {
                if (Documents.FirstOrDefault(doc => doc.File.Path == file.Path) is Document document)
                {
                    CurrentDocument = document;
                }
                else
                {
                    Manager.Load(file);
                }
            }
        }
    }
}
