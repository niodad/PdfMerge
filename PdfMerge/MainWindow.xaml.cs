using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };
                if (openFileDialog.ShowDialog() == true)
                {

                    foreach (string filename in openFileDialog.FileNames)
                        FileList.Add(filename);
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

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileList.Remove((string)FileListBox.SelectedItem);
                RefeshListbox();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
          
    }
}
