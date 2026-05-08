using System.IO;
using System.Text;
using System.Windows;

namespace TextAnalyzerApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CancellationTokenSource _cancellationTokenSource;

    public MainWindow()
    {
        InitializeComponent();
    }
    private async void ShowResultButton_Click(object sender, RoutedEventArgs e)
    {
        string text = InputTextBox.Text;
        var analyzer = new TextAnalyzer();
        await Task.Run(() => analyzer.Analyze(text));
        ResultTextBlock.Text = BuildReport(analyzer);
    }

    private async void DownloadInFileButton_Click(object sender, RoutedEventArgs e)
    {
        string text = InputTextBox.Text;
        var analyzer = new TextAnalyzer();
        await Task.Run(() => analyzer.Analyze(text));
        await File.WriteAllTextAsync("report.txt", BuildReport(analyzer));
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        string text = InputTextBox.Text;
        var analyzer = new TextAnalyzer();
        await Task.Run(() => analyzer.Analyze(text), token);
        ResultTextBlock.Text = BuildReport(analyzer);
    }

    private string BuildReport(TextAnalyzer analyzer)
    {
        var result = new StringBuilder();

        if (SentenceCountBox.IsChecked == true)
            result.AppendLine($"Кількість речень: {analyzer.sentenceCount}");

        if (CharCountBox.IsChecked == true)
            result.AppendLine($"Кількість символів: {analyzer.charCount}");

        if (WordCountBox.IsChecked == true)
            result.AppendLine($"Кількість слів: {analyzer.wordCount}");

        if (QuestionCountBox.IsChecked == true)
            result.AppendLine($"Кількість питальних речень: {analyzer.questionCount}");

        if (ExclamationCountBox.IsChecked == true)
            result.AppendLine($"Кількість окличних речень: {analyzer.exclamationCount}");

        return result.ToString();
    }
}
