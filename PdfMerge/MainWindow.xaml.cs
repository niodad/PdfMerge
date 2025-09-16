using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        public List<string> FileList { get; set; }
        public string SaveFolder = @"C:\temp\";
        private string initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        
        public MainWindow()
        {
            InitializeComponent();
            FileList = new List<string>();
            DataContext = this;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*",
                    InitialDirectory = initialDirectory
                };
                
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        // Only add PDF files and avoid duplicates
                        if (Path.GetExtension(filename).ToLower() == ".pdf" && !FileList.Contains(filename))
                        {
                            FileList.Add(filename);
                        }
                    }
                    initialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                }
                RefeshListbox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefeshListbox()
        {
            FileListBox.ItemsSource = null;
            FileListBox.ItemsSource = FileList;
        }
        
        private void ClearFileList()
        {
            FileList.Clear();
            RefeshListbox();
        }
        
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            if (FileList.Count > 0)
            {
                var result = MessageBox.Show($"Are you sure you want to clear all {FileList.Count} files from the list?", 
                                            "Clear All Files", 
                                            MessageBoxButton.YesNo, 
                                            MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    ClearFileList();
                }
            }
            else
            {
                MessageBox.Show("The file list is already empty.", "No Files", MessageBoxButton.OK, MessageBoxImage.Information);
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
                
                var pathToSave = $@"{SaveFolder}{filename.Text}.pdf";
                
                // Ensure temp directory exists
                Directory.CreateDirectory(SaveFolder);
                
                // Show progress message
                MessageBox.Show($"Merging {FileList.Count} PDF files...\n\nFiles to merge:\n{string.Join("\n", FileList.Select(f => Path.GetFileName(f)))}", 
                              "Merging PDFs", MessageBoxButton.OK, MessageBoxImage.Information);
                
                PdfFunctions.Merge(FileList, pathToSave);
                
                MessageBox.Show($"PDF merged successfully!\n\nFiles merged: {FileList.Count}\nSaved to: {pathToSave}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Open the folder containing the merged file
                ProcessStartInfo psi = new ProcessStartInfo("explorer.exe", "/n /e,/select," + pathToSave);
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error merging PDFs: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                {
                    if (Path.GetExtension(file).ToLower() == ".pdf" && !FileList.Contains(file))
                    {
                        FileList.Add(file);
                    }
                }
                RefeshListbox();
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
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
                RefeshListbox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Up_click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = FileListBox.SelectedIndex;

            if (selectedIndex > 0 && selectedIndex < FileListBox.Items.Count)
            {
                var itemToMoveUp = FileListBox.Items[selectedIndex];
                FileList.Remove((string)itemToMoveUp);
                FileList.Insert(selectedIndex - 1, (string)itemToMoveUp);
                RefeshListbox();
                FileListBox.SelectedIndex = selectedIndex - 1;
            }
        }
        
        private void Down_click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = FileListBox.SelectedIndex;

            if (selectedIndex + 1 < FileListBox.Items.Count && selectedIndex >= 0)
            {
                var itemToMoveDown = FileListBox.Items[selectedIndex];
                FileList.Remove((string)itemToMoveDown);
                FileList.Insert(selectedIndex + 1, (string)itemToMoveDown);
                RefeshListbox();
                FileListBox.SelectedIndex = selectedIndex + 1;
            }
        }
    }
}