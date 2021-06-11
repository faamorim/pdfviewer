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
using System.ComponentModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PDFReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            this.InitializeComponent();
            OpenButton.Click += OpenButton_Click;
        }

        Document _currentDocument;
        public Document CurrentDocument
        {
            get => _currentDocument;
            set
            {
                _currentDocument = value;

                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(nameof(CurrentDocument)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Get a file from the file picker.
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.List;
            PDFHelper.PrepareFileOpenPicker(fileOpenPicker);
            StorageFile file = await fileOpenPicker.PickSingleFileAsync();

            // Create a PDFDocument and use it as the source for the PDFViewCtrl
            if (file != null)
            {
                CurrentDocument = await Document.Load(file);
            }
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(nameof(CurrentDocument)));
            }
        }
    }
}
