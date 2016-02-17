using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Forms;

namespace CSV_Partials_Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public string filename;
        public string filepath;
        private const string Delimiter = ",";
        static ParseData _pd = new ParseData();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseFileButton_Click(object sender, RoutedEventArgs e)
        {
            //configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "document";
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV documents (.csv) | *.csv";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dlg.FileName;
                filepath = System.IO.Path.GetFileName(filename);
                BrowseFileLabel.Content = System.IO.Path.GetFileName(filename);
                
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileHandler myReader = new FileHandler();
            myReader.Initialize(filename);

        }
    }
}
