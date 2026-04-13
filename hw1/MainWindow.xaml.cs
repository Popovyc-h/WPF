using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int lowerBound;
        private int upperBound;

        private Thread primeNumbersThread;
        private Thread fibonacciNumbersThread;

        private bool stopPrimes = false;
        private bool stopFibonacci = false;

        private ManualResetEventSlim eventPrimes = new ManualResetEventSlim(true);
        private ManualResetEventSlim eventFibonacci = new ManualResetEventSlim(true);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GeneratePrimes(int lower, int upper)
        {
            for (int i = lower; i <= upper; i++)
            {
                bool isPrime = true;
                
                if (stopPrimes)
                    break;

                eventPrimes.Wait();

                for (int j = 2; j < i; j++)
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break; 
                    }
                }
                if(isPrime)
                {
                    Dispatcher.Invoke(() => 
                    { 
                        lstPrimes.Items.Add(i); 
                    });
                }
            }
        }

        private void GenerateFibonacci()
        {
            int a = 0;
            int b = 1;

            while (true)
            {
                if (stopFibonacci)
                    break;

                eventFibonacci.Wait();

                Dispatcher.Invoke(() =>
                {
                    Fibonacci.Items.Add(a);
                });
                int c = a + b;
                a = b;
                b = c;
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (txtFrom.Text == null || string.IsNullOrWhiteSpace(txtFrom.Text))
                lowerBound = 2;
            else
                lowerBound = int.Parse(txtFrom.Text);

            if (txtTo.Text == null || string.IsNullOrWhiteSpace(txtTo.Text))
                upperBound = 99999;
            else
                upperBound = int.Parse(txtTo.Text);

            primeNumbersThread = new Thread(() =>
            {
                GeneratePrimes(lowerBound, upperBound);
            });

            stopPrimes = false;
            primeNumbersThread.Start();
        }

        private void btnFibonacci_Click(object sender, RoutedEventArgs e)
        {
            fibonacciNumbersThread = new Thread(() =>
            {
                GenerateFibonacci();
            });

            stopFibonacci = false;
            fibonacciNumbersThread.Start();
        }

        private void btnFibonacci_Stop(object sender, RoutedEventArgs e)
        {
            stopFibonacci = true;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            stopPrimes = true;
        }

        private void btnPrimePause_Click(object sender, RoutedEventArgs e)
        {
            eventPrimes.Reset();
        }

        private void btnPrimeResume_Click(object sender, RoutedEventArgs e)
        {
            eventPrimes.Set();
        }

        private void btnFibonacciPause_Click(object sender, RoutedEventArgs e)
        {
            eventFibonacci.Reset();
        }

        private void btnFibonacciResume_Click(object sender, RoutedEventArgs e)
        {
            eventFibonacci.Set();
        }

        private void btnPrimeRestart_Click(object sender, RoutedEventArgs e)
        {
            eventPrimes.Set();
            stopPrimes = true;
            primeNumbersThread.Join();

            Dispatcher.Invoke(() => lstPrimes.Items.Clear());

            if (txtFrom.Text == null || string.IsNullOrWhiteSpace(txtFrom.Text))
                lowerBound = 2;
            else
                lowerBound = int.Parse(txtFrom.Text);

            if (txtTo.Text == null || string.IsNullOrWhiteSpace(txtTo.Text))
                upperBound = 99999;
            else
                upperBound = int.Parse(txtTo.Text);

            primeNumbersThread = new Thread(() =>
            {
                GeneratePrimes(lowerBound, upperBound);
            });

            stopPrimes = false;
            eventPrimes.Set();
            primeNumbersThread.Start();
        }

        private void btnFibonacciRestart_Click(object sender, RoutedEventArgs e)
        {
            eventFibonacci.Set();
            stopFibonacci = true;
            fibonacciNumbersThread.Join();

            Dispatcher.Invoke(() => Fibonacci.Items.Clear());

            stopFibonacci = false;
            eventFibonacci.Set();

            fibonacciNumbersThread = new Thread(() =>
            {
                GenerateFibonacci();
            });

            fibonacciNumbersThread.Start();
        }
    }
}
