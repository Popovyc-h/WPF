using System.IO;
using System.Windows;

namespace TextAnalyzerApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private async void ShowResultButton_Click(object sender, RoutedEventArgs e)
    {
        string text = InputTextBox.Text;
        var analyzer = new TextAnalyzer();
        await Task.Run(() => analyzer.Analyze(text));
        ResultTextBlock.Text = analyzer.ToString();
    }

    private async void DownloadInFileButton_Click(object sender, RoutedEventArgs e)
    {
        string text = InputTextBox.Text;
        var analyzer = new TextAnalyzer();
        await Task.Run(() => analyzer.Analyze(text));
        await File.WriteAllTextAsync("report.txt", analyzer.ToString());
    }
}
