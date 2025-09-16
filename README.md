# PDF Merger

A modern, user-friendly Windows application for merging multiple PDF files into a single document. Built with WPF and .NET 6.0.

## 🚀 Features

- **Multiple File Selection**: Add multiple PDF files at once
- **Drag & Drop Support**: Simply drag PDF files into the application
- **File Reordering**: Move files up/down to control merge order
- **Duplicate Prevention**: Automatically prevents adding the same file twice
- **Modern UI**: Clean, intuitive interface with color-coded buttons
- **Error Handling**: Comprehensive error messages and validation
- **Auto-save**: Saves merged PDFs to `C:\temp\` directory
- **Auto-open**: Automatically opens the folder containing the merged file

## 📋 Requirements

- **.NET 6.0 Runtime** or later
- **Windows 10/11** (WPF application)
- **iText7 Library** (included in project)

## 🛠️ Installation

### Option 1: Run from Source
1. Clone or download this repository
2. Navigate to the `PdfMerge` folder
3. Run the following commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

### Option 2: Build Executable
1. Navigate to the `PdfMerge` folder
2. Build the application:
   ```bash
   dotnet build --configuration Release
   ```
3. Find the executable in `bin\Release\net6.0-windows\PdfMerge.exe`

## 🎯 How to Use

### Adding Files
- **Method 1**: Click the green "Add Files" button and select multiple PDFs
- **Method 2**: Drag and drop PDF files directly into the file list area

### Managing Files
- **Reorder**: Select a file and use "Move Up" or "Move Down" buttons
- **Delete**: Select files and click "Delete Selected" (red button)
- **Clear All**: Click "Clear All" (purple button) to remove all files

### Merging PDFs
1. Ensure you have files in the list
2. Enter a filename in the text box (default: "merged_document")
3. Click the orange "Merge PDFs" button
4. The merged PDF will be saved to `C:\temp\` and the folder will open automatically

## 🎨 User Interface

The application features a modern, card-based design with:

- **Header**: "PDF Merger" title
- **Control Buttons**: 
  - 🟢 **Add Files** - Select PDF files
  - 🔴 **Delete Selected** - Remove selected files
  - 🟣 **Clear All** - Clear entire file list
  - 🔵 **Move Up/Down** - Reorder files
- **File List**: Shows selected PDFs with 📄 icons
- **Output Section**: Filename input and merge button

## 🔧 Technical Details

### Architecture
- **Framework**: .NET 6.0 Windows Desktop
- **UI Technology**: WPF (Windows Presentation Foundation)
- **PDF Library**: iText7 (version 7.1.9)
- **Language**: C#

### Project Structure
```
PdfMerge/
├── App.xaml                 # Application definition
├── App.xaml.cs             # Application code-behind
├── MainWindow.xaml         # Main UI layout
├── MainWindow.xaml.cs      # Main window logic
├── PdfFunctions.cs         # PDF merging functionality
├── PdfMerge.csproj         # Project configuration
└── bin/Debug/              # Compiled output
```

### Key Components

#### PdfFunctions.cs
Contains the core PDF merging logic using iText7:
- Validates input files
- Creates merged PDF document
- Handles error cases
- Ensures proper resource cleanup

#### MainWindow.xaml.cs
Handles user interface interactions:
- File selection and management
- Drag & drop functionality
- Error handling and user feedback
- File list operations

## 🐛 Troubleshooting

### Common Issues

**Application won't start**
- Ensure .NET 6.0 Runtime is installed
- Check Windows version compatibility
- Try running as administrator

**Files not merging**
- Verify all files are valid PDFs
- Check file permissions
- Ensure `C:\temp\` directory exists or is writable

**UI not visible**
- Check if window is minimized in taskbar
- Try Alt+Tab to cycle through windows
- Check multiple monitor setup

### Error Messages

- **"No files provided for merging"**: Add PDF files before merging
- **"File not found"**: Selected file may have been moved or deleted
- **"PDF merge failed"**: Check file integrity and permissions

## 📝 Development

### Building from Source
```bash
# Restore dependencies
dotnet restore

# Build in Debug mode
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Run the application
dotnet run
```

### Dependencies
- **iText7**: PDF manipulation library
- **Microsoft.NET.Sdk**: .NET SDK
- **WPF**: Windows Presentation Foundation

## 📄 License

This project is open source. Please check the license file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## 📞 Support

If you encounter any issues or have questions:
1. Check the troubleshooting section above
2. Review error messages carefully
3. Ensure all requirements are met
4. Create an issue with detailed information

## 🔄 Version History

- **v1.0**: Initial release with basic PDF merging
- **v1.1**: Added modern UI and improved error handling
- **v1.2**: Fixed duplicate file issues and enhanced user experience

---

**Enjoy merging your PDFs! 📄✨**
