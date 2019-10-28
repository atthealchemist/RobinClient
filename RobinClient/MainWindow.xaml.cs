using RobinClient.utils;
using System.Collections.ObjectModel;
using System.Windows;

namespace RobinClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Sender sender;

        public ObservableCollection<string> ResponseList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ResponseList = new ObservableCollection<string>();

            DataContext = this;

            sender = new Sender("ws://localhost:8765");
            sender.OnResult += ShowResult;
        }

        private void ShowResult(string value)
        {
            listViewResponses.Dispatcher.Invoke(() =>
            {
                if (!ResponseList.Contains(value))
                {
                    ResponseList.Add(value);
                }
            });

        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            string value = textBoxValue.Text;

            int requestsCount = listViewRequests.Items.Count;
            int responsesCount = listViewResponses.Items.Count;

            listViewRequests.Items.Add($"Input [{requestsCount}]: {value}");

            this.sender.Send(value);

            textBoxValue.Clear();

        }
    }
}
