using System;
using Microsoft.Win32;

namespace ComparisonForm
{
    static class OpenFile
    {
        [STAThread]
        public static string GetFile(string initialDirectory, params string[] extensions)
        {
            string initialDirect = !string.IsNullOrWhiteSpace(initialDirectory) ? initialDirectory : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            OpenFileDialog oFile = new OpenFileDialog
            {
                InitialDirectory = initialDirect,
                Filter = $@"{string.Join(", ", extensions)} files (*{string.Join(", *", extensions)})|*{string.Join(", *", extensions)}",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            oFile.ShowDialog();
            string textfile = oFile.FileName;

            return textfile;
        }
    }
}
