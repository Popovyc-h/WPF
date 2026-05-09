using System.Windows;

namespace FileDuplicateChecker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        var sourcePath = SourceTextBox.Text;
        var receiverPath = ReceiverTextBox.Text;

        var service = new FileDuplicateService();

        try
        {
            var originals = await Task.Run(() => service.FindDuplicates(sourcePath));
            var files = await Task.Run(() => service.MoveOriginalFiles(originals, receiverPath));
            
            ResultTextBlock.Text = "Files successfully move";
            foreach (var file in files)
                ResultTextBlock.Text += $"\n{file}";

        }
        catch (Exception ex)
        {
            ResultTextBlock.Text = ex.Message;
        }
    }
}
