using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace PdfMerge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string PDF_FILTER = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
        private const string PDF_EXTENSION = ".pdf";

        public ObservableCollection<string> FileList { get; set; }
        private string saveFolder;
        public string SaveFolder
        {
            get => saveFolder;
            set => saveFolder = value;
        }
        private string initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        
        public MainWindow()
        {
            InitializeComponent();
            FileList = new ObservableCollection<string>();
            SaveFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PdfMerge");
            DataContext = this;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = PDF_FILTER,
                    InitialDirectory = initialDirectory
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    int addedCount = 0;
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        // Only add PDF files and avoid duplicates
                        if (Path.GetExtension(filename).Equals(PDF_EXTENSION, StringComparison.OrdinalIgnoreCase) && !FileList.Contains(filename))
                        {
                            FileList.Add(filename);
                            addedCount++;
                        }
                    }

                    if (addedCount > 0)
                    {
                        initialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ClearFileList()
        {
            FileList.Clear();
        }
        
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            if (FileList.Count == 0)
            {
                MessageBox.Show("The file list is already empty.", "No Files", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to clear all {FileList.Count} files from the list?", 
                                        "Clear All Files", 
                                        MessageBoxButton.YesNo, 
                                        MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ClearFileList();
            }
        }

        private void Button_Merge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FileList.Count == 0)
                {
                    MessageBox.Show("Please add some PDF files first.", "No Files", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validate output filename
                if (filename?.Text == null || string.IsNullOrWhiteSpace(filename.Text))
                {
                    MessageBox.Show("Please enter a filename for the merged PDF.", "No Filename", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var pathToSave = Path.Combine(SaveFolder, $"{filename.Text.Trim()}.pdf");

                // Ensure directory exists
                Directory.CreateDirectory(SaveFolder);

                // Show progress message
                MessageBox.Show($"Merging {FileList.Count} PDF files...\n\nFiles to merge:\n{string.Join("\n", FileList.Select(f => Path.GetFileName(f)))}",
                              "Merging PDFs", MessageBoxButton.OK, MessageBoxImage.Information);

                PdfFunctions.Merge(FileList, pathToSave);

                MessageBox.Show($"PDF merged successfully!\n\nFiles merged: {FileList.Count}\nSaved to: {pathToSave}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Open the folder containing the merged file
                    var startInfo = new ProcessStartInfo("explorer.exe", $"/n /e,/select,\"{pathToSave}\"");
                Process.Start(startInfo);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Error merging PDFs: {ex.Message}", "Merge Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    int addedCount = 0;
                    foreach (var file in files)
                    {
                        if (Path.GetExtension(file).Equals(PDF_EXTENSION, StringComparison.OrdinalIgnoreCase) && !FileList.Contains(file))
                        {
                            FileList.Add(file);
                            addedCount++;
                        }
                    }

                    if (addedCount == 0 && files.Length > 0)
                    {
                        MessageBox.Show("No PDF files were added. Only PDF files are supported.", "Invalid Files", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (FileListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select files to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedItems = FileListBox.SelectedItems.Cast<string>().ToList();
            foreach (var file in selectedItems)
            {
                FileList.Remove(file);
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = FileListBox.SelectedIndex;

            if (selectedIndex > 0)
            {
                var itemToMoveUp = (string)FileListBox.Items[selectedIndex];
                FileList.Remove(itemToMoveUp);
                FileList.Insert(selectedIndex - 1, itemToMoveUp);
                FileListBox.SelectedIndex = selectedIndex - 1;
            }
        }
        
        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = FileListBox.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex + 1 < FileListBox.Items.Count)
            {
                var itemToMoveDown = (string)FileListBox.Items[selectedIndex];
                FileList.Remove(itemToMoveDown);
                FileList.Insert(selectedIndex + 1, itemToMoveDown);
                FileListBox.SelectedIndex = selectedIndex + 1;
            }
        }
    }
}