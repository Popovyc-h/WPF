using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ProgressBar[] horses;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            progressBarsPanel.Children.Clear();
            int count = int.Parse(txtCount.Text);

            for (int i = 0; i < count; i++)
            {
                ProgressBar pb = new ProgressBar();
                pb.Height = 20;
                pb.Margin = new Thickness(5);
                progressBarsPanel.Children.Add(pb);

                Thread t = new Thread(() =>
                {
                    Random random = new Random();
                    int num = random.Next(1, 101);

                    byte r = (byte)random.Next(0, 255);
                    byte g = (byte)random.Next(0, 255);
                    byte b = (byte)random.Next(0, 255);

                    Dispatcher.Invoke(() =>
                    {
                        pb.Foreground = new SolidColorBrush(Color.FromRgb(r, g, b));
                    });

                    double value = 0;

                    while (value < 100)
                    {
                        value = Math.Min(value + num, 100);
                        Dispatcher.Invoke(() =>
                        {
                            pb.Value = value;
                        });
                        Thread.Sleep(150);
                    }
                });
                t.Start();
            }
        }

        private void txtCount_TextChanged(object sender, TextChangedEventArgs e) { }

        // task 2
        private void btrStartRace_Click(object sender, RoutedEventArgs e)
        {
            horses = new ProgressBar[] { horse1, horse2, horse3, horse4, horse5 };

            foreach (var h in horses)
                h.Value = 0;

            for (int i = 0; i < horses.Length; i++)
            {
                int index = i;

                Thread t = new Thread(() =>
                {
                    double value = 0;

                    while (value < 100)
                    {
                        Random random = new Random();
                        int num = random.Next(1, 101);

                        value = Math.Min(value + num, 100);

                        Dispatcher.Invoke(() =>
                        {
                            horses[index].Value = value;
                        });
                        Thread.Sleep(150);
                    }
                });

                t.Start();
            }
        }
    }
}
