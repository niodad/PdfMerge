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
        private string initialDirectory = @"C:\Users\peter\Pictures\Scans";
        public MainWindow()
        {
            InitializeComponent();
            FileList = new List<string>();
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
                        FileList.Add(filename);
                    initialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                }
                RefeshListbox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefeshListbox()
        {
            FileListBox.ItemsSource = null;
            FileListBox.ItemsSource = FileList;
        }

        private void Button_Merge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var pathToSave = $@"{SaveFolder}{filename.Text}.pdf";
                PdfFunctions.Merge(FileList, pathToSave);
                ProcessStartInfo psi = new ProcessStartInfo("explorer.exe", "/n /e,/select," + pathToSave);
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                FileList.AddRange(from file in (string[])e.Data.GetData(DataFormats.FileDrop) select file);
                RefeshListbox();
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var file in FileListBox.SelectedItems)
                {
                    FileList.Remove((string)file);
                }
                RefeshListbox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Up_click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = FileListBox.SelectedIndex;

            if (selectedIndex > 0 && selectedIndex < FileListBox.Items.Count)
            {
                var itemToMoveUp = this.FileListBox.Items[selectedIndex];
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
