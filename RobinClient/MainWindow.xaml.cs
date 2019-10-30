using RobinClient.utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace RobinClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Sender sender;
        private DispatcherTimer timer;

        public ObservableCollection<Query> ResponseList { get; set; }

        private int period = 1;

        public int PeriodInSeconds
        {
            get
            {
                return period;
            }

            set
            {
                period = value;
                OnPropertyChanged(nameof(PeriodInSeconds));
                Console.WriteLine($"Changing period to {period} secs");
            }
        }

        private object _lock;

        public MainWindow()
        {
            InitializeComponent();
            _lock = new object();
            ResponseList = new ObservableCollection<Query>();
            BindingOperations.EnableCollectionSynchronization(ResponseList, _lock);

            timer = new DispatcherTimer();
            timer.Tick += OnTimerTick;
            timer.Interval = new TimeSpan(0, 0, period);

            DataContext = this;

            sender = new Sender("ws://localhost:8765");
            sender.Connect();
            sender.OnResult += ShowResult;
        }

        private void OnTimerTick(object sender, EventArgs eventArgs)
        {
            SendValue(TextBoxValue.Text);
        }

        private void ShowResult(Query value)
        {
            lock (_lock)
            {
                ResponseList.Add(value);
            }
        }

        private void SendValue(string value)
        {
            if (value?.Length == 0)
            {
                value = new Random().Next(1, 10).ToString();
            }
            Dispatcher.Invoke(() => ListViewRequests.Items.Add($"Input [{ListViewRequests.Items.Count}]: {value}"), DispatcherPriority.Send);

            sender.Send(value);

        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {

            SendValue(TextBoxValue.Text);
            TextBoxValue.Clear();

        }

        private void CheckBoxPeriodically_Checked(object sender, RoutedEventArgs e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
            }
        }

        private void CheckBoxPeriodically_Unchecked(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
        }
    }
}
