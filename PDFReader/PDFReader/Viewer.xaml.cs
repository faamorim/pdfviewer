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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PDFReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Viewer : Page
    {
        pdftron.PDF.PDFViewCtrl PDFViewCtrl;

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof(Document), typeof(Document), typeof(Viewer), new PropertyMetadata(null, OnDocumentChanged));

        public Document Document
        {
            get => (Document)GetValue(DocumentProperty);
            set => SetValue(DocumentProperty, value);
        }
        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Viewer viewer = (Viewer)d;
            viewer.PDFViewCtrl.SetDoc(((Document)e.NewValue).PDF);
        }

        public Viewer()
        {
            PDFHelper.Init();
            this.InitializeComponent();
            PDFViewCtrl = new pdftron.PDF.PDFViewCtrl();
            PDFViewBorder.Child = PDFViewCtrl;
        }
    }
}
