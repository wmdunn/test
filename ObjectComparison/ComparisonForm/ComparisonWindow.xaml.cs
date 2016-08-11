using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Brush = System.Drawing.Brush;

namespace ComparisonForm
{
    /// <summary>
    /// Interaction logic for ComparisonWindow.xaml
    /// </summary>
    public partial class ComparisonWindow : Window
    {
        internal static readonly BackgroundWorker WorkerParser = new BackgroundWorker();
        public ComparisonWindow()
        {
            InitializeComponent();
            WorkerParser.DoWork += WorkerParser_DoWork;
            WorkerParser.ProgressChanged += WorkerParser_ProgressChanged;
            WorkerParser.RunWorkerCompleted += WorkerParser_RunWorkerCompleted;
            WorkerParser.WorkerReportsProgress = true;
        }

        #region Background Worker Methods

        private void WorkerParser_DoWork(object sender, DoWorkEventArgs e)
        {
            Compare c = new Compare();
            Tuple<string, string, string, bool?> inputTuple = (Tuple<string, string, string, bool?>) e.Argument;
            c.DoStuff(inputTuple.Item1, inputTuple.Item2, inputTuple.Item3, inputTuple.Item4);
        }

        private void WorkerParser_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress.Value = e.ProgressPercentage;
            ProgressLabel.Content = (string) e.UserState;
            Random rand = new Random();
            byte r = (byte) rand.Next(0, 255);
            byte b = (byte) rand.Next(0, 255);
            byte g = (byte) rand.Next(0, 255);
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, r, b, g));
            Progress.Foreground = brush;
        }

        private void WorkerParser_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("All Done!");
        }

        #endregion //Background Worker Methods
        
        private void BaseFileButton_Click(object sender, RoutedEventArgs e)
        {
            BaseFilePath.Text = OpenFile.GetFile(null, ".csv");
        }

        private void ComparisonFileButton_Click(object sender, RoutedEventArgs e)
        {
            ComparisonFilePath.Text = OpenFile.GetFile(null, ".csv");
        }

        private void ConfigFileButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigFilePath.Text = OpenFile.GetFile(null, ".xml");
        }

        private void RunComparisonButton_Click(object sender, RoutedEventArgs e)
        {
            Compare c = new Compare();
            if (!string.IsNullOrWhiteSpace(ConfigFilePath.Text) &&
                !string.IsNullOrWhiteSpace(BaseFilePath.Text) &&
                !string.IsNullOrWhiteSpace(ComparisonFilePath.Text))
            {
                Tuple<string, string, string, bool?> inputTuple = new Tuple<string, string, string, bool?>(BaseFilePath.Text, ComparisonFilePath.Text, ConfigFilePath.Text, SampleCheck.IsChecked);
                WorkerParser.RunWorkerAsync(inputTuple);
            }
        }
    }
}
