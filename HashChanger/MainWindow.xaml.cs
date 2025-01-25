using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace HashChanger
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _filePath;
        bool skiphash = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    _filePath = files[0];
                    FilePathTextBox.Text = _filePath;
                    StatusLabel.Content = "File selected via Drag & Drop.";
                }
            }
        }

        private async void ModifyHash_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                MessageBox.Show("Please select a file first.");
                return;
            }

            ProgressBar.Value = 0;
            StatusLabel.Content = "Processing...";
            OriginalHashTextBox.Text = string.Empty;
            ModifiedHashTextBox.Text = string.Empty;

            try
            {
                var progress = new Progress<int>(value => ProgressBar.Value = value);
                var result = await Task.Run(() => ModifyFileHash(_filePath, progress));

                // Update UI with the hashes
                OriginalHashTextBox.Text = result.originalHash;
                ModifiedHashTextBox.Text = result.modifiedHash;
                StatusLabel.Content = "Hash modified successfully!";
            }
            catch (Exception ex)
            {
                StatusLabel.Content = "Error occurred.";
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private (string originalHash, string modifiedHash) ModifyFileHash(string filePath, IProgress<int> progress)
        {
            string originalHash;
            string modifiedHash;
            const int bufferSize = 32 * 1024; // 32 KB buffer size
            if (!skiphash)
            {
                // Calculate original MD5 hash
                Dispatcher.Invoke(() => StatusLabel.Content = "Calculating original hash...");
                originalHash = ComputeFileHash(filePath, progress);
            }
            else
            {
                originalHash = "Skipped!";
            }


            // Append meaningless data to the file to modify the hash
            Dispatcher.Invoke(() => StatusLabel.Content = "Appending random data to file...");
            var randomData = Encoding.UTF8.GetBytes($"RandomData{Guid.NewGuid()}");
            using (var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var bufferedStream = new BufferedStream(fileStream, bufferSize))
            {
                long totalBytes = randomData.Length;
                long processedBytes = 0;

                while (processedBytes < totalBytes)
                {
                    int chunkSize = Math.Min(bufferSize, (int)(totalBytes - processedBytes));
                    bufferedStream.Write(randomData, (int)processedBytes, chunkSize);
                    processedBytes += chunkSize;

                    // Update progress
                    progress?.Report((int)((double)processedBytes / totalBytes * 100));
                }
            }

            if (!skiphash)
            {
                // Calculate new MD5 hash
                Dispatcher.Invoke(() => StatusLabel.Content = "Calculating modified hash...");
                modifiedHash = ComputeFileHash(filePath, progress);
            }
            else
            {
                modifiedHash = "Skipped!";
            }



            Dispatcher.Invoke(() => StatusLabel.Content = "Completed!");
            return (originalHash, modifiedHash);
        }

        private string ComputeFileHash(string filePath, IProgress<int> progress)
        {
            const int bufferSize = 32 * 1024; // 32 KB buffer size

            using (var md5 = MD5.Create())
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var buffer = new byte[bufferSize];
                long totalBytes = fileStream.Length;
                long processedBytes = 0;

                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    md5.TransformBlock(buffer, 0, bytesRead, null, 0);
                    processedBytes += bytesRead;

                    // Update progress
                    progress?.Report((int)((double)processedBytes / totalBytes * 100));
                }

                // Finalize the hash
                md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

                // Return the computed hash as a string
                return BitConverter.ToString(md5.Hash).Replace("-", "").ToUpperInvariant();
            }
        }

        private void ToggleHashChecks(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            OriginalHashTextBox.IsEnabled = !(bool)checkBox.IsChecked;
            ModifiedHashTextBox.IsEnabled = !(bool)checkBox.IsChecked;
            skiphash = (bool)checkBox.IsChecked;
        }
    }
}
