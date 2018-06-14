using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Course_work_game
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string name = "";
        public string ip = "";

        StreamReader reader;
        StreamWriter writer;
        NetworkStream stream;
        TcpClient client;

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            name = textboxName.Text;
            ip = textboxIP.Text;
            if (name == "" && ip == "")
            {
                MessageBox.Show("You must enter a name and IP", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(ip, 1234);
                    stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream) { AutoFlush = true };

                }
                catch
                {
                    MessageBox.Show("Подключение не установлено");
                }
                //ClientPage player = new ClientPage();
                //player.Show();
                Window1 w = new Window1();
                w.Content = new ClientPage(name, stream, client);
                w.Show();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
        }
    }
}
