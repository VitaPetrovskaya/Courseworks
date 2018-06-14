using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Course_work_game
{
    /// <summary>
    /// Логика взаимодействия для WindowRecipe.xaml
    /// </summary>
    public partial class WindowRecipe : Window
    {
        string FileName = "";
        public WindowRecipe(string name)
        {
            InitializeComponent();
            FileName = name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var sr = new StreamReader(FileName, Encoding.Default);
            string textline = sr.ReadToEnd();

            var document = new FlowDocument();
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(textline);
            document.Blocks.Add(paragraph);
            text.Document = document;

            lableName.Content = File.ReadLines(FileName).Skip(0).First();
            string num = FileName.Replace(".txt", "");
            if (num.IndexOf("Breakfast") != -1)
            {
                num = num.Replace("Breakfast", "");
                if (Int32.Parse(num) == 0)
                {
                    Img0.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 1)
                {
                    Img1.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 2)
                {
                    Img2.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 3)
                {
                    Img3.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 4)
                {
                    Img4.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 5)
                {
                    Img5.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 6)
                {
                    Img6.Visibility = Visibility.Visible;
                }
            }
            if (num.IndexOf("Lunch") != -1)
            {
                num = num.Replace("Lunch", "");
                if (Int32.Parse(num) == 0)
                {
                    Img7.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 1)
                {
                    Img8.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 2)
                {
                    Img9.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 3)
                {
                    Img10.Visibility = Visibility.Visible;
                }
            }
            if (num.IndexOf("Dinner") != -1)
            {
                num = num.Replace("Dinner", "");
                if (Int32.Parse(num) == 0)
                {
                    Img11.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 1)
                {
                    Img12.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 2)
                {
                    Img13.Visibility = Visibility.Visible;
                }
                if (Int32.Parse(num) == 3)
                {
                    Img14.Visibility = Visibility.Visible;
                }
            }

           
        }
    }
}
