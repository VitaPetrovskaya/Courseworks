using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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

namespace Course_work_game
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : UserControl
    {
        enum Orientation { VERTICAL, HORIZONTAL }; // выбор пользователем направления расположения
        Orientation orientation = Orientation.HORIZONTAL;
        SolidColorBrush unselected = new SolidColorBrush(Colors.Black);
        SolidColorBrush selected = new SolidColorBrush(Colors.Green);
        String elem = "";
        int size;
        int numElemsPlaced;
        System.Windows.Shapes.Path lastElem;
        System.Windows.Shapes.Path[] elems;
        Polygon lastArrow;
        public Grid[] playerGrid;
        string PlayerName;
        TcpClient client;
        StreamReader reader;
        StreamWriter writer;
        NetworkStream stream;
        bool is_connected;
        bool check_random = false;
        string result_str;

        System.Timers.Timer t_CheckData = new System.Timers.Timer();

        SolidColorBrush[] elemColors = new SolidColorBrush[] {(SolidColorBrush)(new BrushConverter().ConvertFrom("#88cc00")), (SolidColorBrush)(new BrushConverter().ConvertFrom("#33cc33")),
                                                                  (SolidColorBrush)(new BrushConverter().ConvertFrom("#00e64d")),(SolidColorBrush)(new BrushConverter().ConvertFrom("#00cc00"))}; // цвет поставленного элемента

        public ClientPage(string player_name, NetworkStream rec_stream, TcpClient rec_client)
        {
            InitializeComponent();
            PlayerName = player_name;
            playerGrid = new Grid[] { gridA1, gridA2, gridA3, gridA4, gridA5, gridA6, gridA7,gridA8,gridA9,gridA10,
                                gridB1, gridB2, gridB3, gridB4, gridB5, gridB6, gridB7,gridB8,gridB9,gridB10,
                                gridC1, gridC2, gridC3, gridC4, gridC5, gridC6, gridC7,gridC8,gridC9,gridC10,
                                gridD1, gridD2, gridD3, gridD4, gridD5, gridD6, gridD7,gridD8,gridD9,gridD10,
                                gridE1, gridE2, gridE3, gridE4, gridE5, gridE6, gridE7,gridE8,gridE9,gridE10,
                                gridF1, gridF2, gridF3, gridF4, gridF5, gridF6, gridF7,gridF8,gridF9,gridF10,
                                gridG1, gridG2, gridG3, gridG4, gridG5, gridG6, gridG7,gridG8,gridG9,gridG10,
                                gridH1, gridH2, gridH3, gridH4, gridH5, gridH6, gridH7,gridH8,gridH9,gridH10,
                                gridI1, gridI2, gridI3, gridI4, gridI5, gridI6, gridI7,gridI8,gridI9,gridI10,
                                gridJ1, gridJ2, gridJ3, gridJ4, gridJ5, gridJ6, gridJ7,gridJ8,gridJ9,gridJ10 };
            elems = new System.Windows.Shapes.Path[] { single1, single2, single3, single4, double1, double2, double3, triple1, triple2, quadruple };
            stream = rec_stream;
            client = rec_client;
            if (stream != null)
            {
                is_connected = true;
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream) { AutoFlush = true };
            }
            reset();
        }
        

        private void reset() // очищение таблицы
        {
            if (lastArrow != null)
            {
                lastArrow.Stroke = unselected;  // стрелка поворота черного цвета
            }
            lastArrow = rightPoly;  // текущее направление
            rightPoly.Stroke = selected;  // стрелка зеленая

            foreach (var element in playerGrid)
            {
                element.Tag = "water";  // везде вода
                element.Background = new SolidColorBrush(Colors.White);
            }

            foreach (var element in elems)
            {
                element.IsEnabled = true;
                element.Opacity = 100; // прозрачность 
                if (element.Stroke != unselected)
                {
                    element.Stroke = unselected;
                }
            }
            numElemsPlaced = 0;
            lastElem = null;
            
        }

        private SolidColorBrush selectColor()
        {
            switch (elem)
            {
                case "single1":
                case "single2":
                case "single3":
                case "single4":
                    return elemColors[0];
                case "double1":
                case "double2":
                case "double3":
                    return elemColors[1];
                case "triple1":
                case "triple2":
                    return elemColors[2];
                case "quadruple":
                    return elemColors[3];
            }
            return elemColors[0];
        }

        private void gridMouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid square = (Grid)sender;
            int index = -1;
            int temp;
            int counter = 1;

            //Check if elem has been selected
            if (lastElem == null)
            {
                MessageBox.Show("You must choose an elem", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Check if square has a elem already in place
            if (!square.Tag.Equals("water"))
            {
                return;
            }

            //Find chosen square. Index should never be -1.
            index = Array.IndexOf(playerGrid, square);

            //Check if there is enough space for the elem

            if (orientation.Equals(Orientation.HORIZONTAL))
            {
                try
                {
                    counter = 1;
                    for (int i = 0; i < size; i++)
                    {
                        //This sees if the index is within the grid going ---->
                        if (index + i <= 99)
                        {
                            if (!playerGrid[index + i].Tag.Equals("water"))
                            {
                                throw new IndexOutOfRangeException("Invalid elem placement, not enough space!");
                            }
                        }
                        //Goes <---- to see if there is space
                        else
                        {
                            if (!playerGrid[index - counter].Tag.Equals("water"))
                            {
                                throw new IndexOutOfRangeException("Invalid elem placement");
                            }
                            counter++;
                        }

                    }
                }
                catch (IndexOutOfRangeException iore)
                {
                    MessageBox.Show(iore.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // проверка на ближайщие элементы
                int count_el_hor = index;
                try
                {
                    if (index % 10 != 0)
                    {
                        if (!playerGrid[index - 1].Tag.Equals("water"))
                        {
                            MessageBox.Show("Invalid placement! Other items nearby.");
                            return;
                        }
                    }

                    for (int t = 0; t < size; t++)
                    {
                        if ((index + t) % 10 != 0)
                        {
                            if (count_el_hor < 10)
                            {
                                if ((!playerGrid[count_el_hor + 10].Tag.Equals("water")) ||
                                        (!playerGrid[count_el_hor + 1].Tag.Equals("water")))
                                {
                                    MessageBox.Show("Invalid placement! Other items nearby.");
                                    return;
                                }
                            }
                            else
                            {
                                if (count_el_hor > 89)
                                {
                                    if (count_el_hor != 99)
                                    {
                                        if ((!playerGrid[count_el_hor - 10].Tag.Equals("water")) ||
                                        (!playerGrid[count_el_hor + 1].Tag.Equals("water")))
                                        {
                                            MessageBox.Show("Invalid placement! Other items nearby.");
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((!playerGrid[count_el_hor + 10].Tag.Equals("water")) ||
                                        (!playerGrid[count_el_hor - 10].Tag.Equals("water")) ||
                                        (!playerGrid[count_el_hor + 1].Tag.Equals("water")))
                                    {
                                        MessageBox.Show("Invalid placement! Other items nearby.");
                                        return;
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (index < 10)
                            {
                                if (index % 10 != 0)
                                {
                                    if (index % 10 == 9)
                                    {
                                        if ((!playerGrid[index + 10].Tag.Equals("water")))
                                        {
                                            MessageBox.Show("Invalid placement! Other items nearby.");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if ((!playerGrid[index - 1 + 10].Tag.Equals("water")) ||
                                           (!playerGrid[index - 2].Tag.Equals("water")))
                                        {
                                            MessageBox.Show("Invalid placement! Other items nearby.");
                                            return;
                                        }
                                    }

                                }
                                else
                                {
                                    if ((!playerGrid[index + 10].Tag.Equals("water")))
                                    {
                                        MessageBox.Show("Invalid placement! Other items nearby.");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (index > 89)
                                {
                                    if ((!playerGrid[index - 1 - 10].Tag.Equals("water")) ||
                                    (!playerGrid[index - 2].Tag.Equals("water")))
                                    {
                                        MessageBox.Show("Invalid placement! Other items nearby.");
                                        return;
                                    }
                                }
                                else
                                {
                                    if (index % 10 != 0)
                                    {
                                        if ((!playerGrid[index - 1 + 10].Tag.Equals("water")) ||
                                         (!playerGrid[index - 1 - 10].Tag.Equals("water")) ||
                                         (!playerGrid[index - 2].Tag.Equals("water")))
                                        {
                                            MessageBox.Show("Invalid placement! Other items nearby.");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if ((!playerGrid[index + 10].Tag.Equals("water")) ||
                                         (!playerGrid[index - 10].Tag.Equals("water")))
                                        {
                                            MessageBox.Show("Invalid placement! Other items nearby.");
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        count_el_hor++;
                    }
                }
                catch { }
            }
            else //for orientation down
            {
                try
                { 
                    counter = 10;
                    for (int i = 0; i < size * 10; i += 10)
                    {
                        if (index + i <= 99)
                        {
                            if (!playerGrid[index + i].Tag.Equals("water"))
                            {
                                throw new IndexOutOfRangeException("Invalid elem placement!");
                            }
                        }
                        else
                        {
                            if (!playerGrid[index - counter].Tag.Equals("water"))
                            {
                                throw new IndexOutOfRangeException("Invalid elem placement! Wrong counter.");
                            }
                            counter += 10;
                        }
                    }
                    if ((index / 10) + (size * 10) > 100)
                    {
                        throw new IndexOutOfRangeException("Invalid elem placement, not enough space!");
                    }
                }
                catch (IndexOutOfRangeException iore)
                {
                    MessageBox.Show(iore.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // проверка на ближайщие элементы
                int count_el_ver = index;
                try
                {
                    if (index > 10)
                    {
                        if (!playerGrid[index - 10].Tag.Equals("water"))
                        {
                            MessageBox.Show("Invalid placement! Other items nearby.");
                            return;
                        }
                    }
                    for (int j = 0; j < size * 10; j += 10)
                    {
                        if ((index + j) < 99)
                        {
                            if (count_el_ver % 10 == 0)
                            {
                                if (count_el_ver > 89)
                                {
                                    if (!playerGrid[count_el_ver + 1].Tag.Equals("water"))
                                    {
                                        MessageBox.Show("Invalid placement! Other items nearby.");
                                        return;
                                    }
                                }
                                else
                                {
                                    if ((!playerGrid[count_el_ver + 1].Tag.Equals("water")) ||
                                    (!playerGrid[count_el_ver + 10].Tag.Equals("water")))
                                    {
                                        MessageBox.Show("Invalid placement! Other items nearby.");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (count_el_ver % 10 == 9)
                                {
                                    if (count_el_ver > 89)
                                    {
                                        if (!playerGrid[count_el_ver - 1].Tag.Equals("water"))
                                        {
                                            MessageBox.Show("Invalid placement! Other items nearby.");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if ((!playerGrid[count_el_ver - 1].Tag.Equals("water")) ||
                                           (!playerGrid[count_el_ver + 10].Tag.Equals("water")))
                                        {
                                            MessageBox.Show("Invalid placement! Other items nearby.");
                                            return;
                                        }
                                    }

                                }
                                else
                                {
                                    if ((!playerGrid[count_el_ver + 1].Tag.Equals("water")) ||
                                       (!playerGrid[count_el_ver - 1].Tag.Equals("water")) ||
                                       (!playerGrid[count_el_ver + 10].Tag.Equals("water")))
                                    {
                                        MessageBox.Show("Invalid placement! Other items nearby.");
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if ((!playerGrid[index - 1 - 10].Tag.Equals("water")) ||
                                (!playerGrid[index + 1 - 10].Tag.Equals("water")) ||
                                (!playerGrid[index - 20].Tag.Equals("water")))
                                {
                                    MessageBox.Show("Invalid placement! Other items nearby.");
                                    return;
                                }
                            }
                            catch { }

                        }
                        count_el_ver += 10;
                    }
                }
                catch { }

            }

            //Set the elem to grid
            if (orientation.Equals(Orientation.HORIZONTAL))
            {
                //If two rows
                if ((index + size - 1) % 10 < size - 1)
                {
                    counter = 0;
                    temp = 1;

                    while ((index + counter) % 10 > 1)
                    {
                        playerGrid[index + counter].Background = selectColor();
                        playerGrid[index + counter].Tag = elem;
                        counter++;
                    }
                    for (int i = counter; i < size; i++)
                    {
                        playerGrid[index - temp].Background = selectColor();
                        playerGrid[index - temp].Tag = elem;
                        temp++;
                    }
                }
                //If one row
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        playerGrid[index + i].Background = selectColor();
                        playerGrid[index + i].Tag = elem;
                    }
                }
            }
            else
            {
                //If two columns
                if (index + (size * 10) > 100)
                {
                    counter = 0;
                    temp = 10;
                    while ((index / 10 + counter) % 100 < 10)
                    {
                        playerGrid[index + counter * 10].Background = selectColor();
                        playerGrid[index + counter * 10].Tag = elem;
                        counter++;
                    }
                    for (int i = counter; i < size; i++)
                    {
                        playerGrid[index - temp].Background = selectColor();
                        playerGrid[index - temp].Tag = elem;
                        temp += 10;
                    }
                }
                //If one column
                else
                {
                    counter = 0;
                    for (int i = 0; i < size * 10; i += 10)
                    {
                        playerGrid[index + i].Background = selectColor();
                        playerGrid[index + i].Tag = elem;
                    }
                }
            }
            lastElem.IsEnabled = false;
            lastElem.Opacity = 0.5;
            lastElem.Stroke = unselected;
            lastElem = null;
            numElemsPlaced++;
        }

        private void orientationMouseDown(object sender, MouseButtonEventArgs e)
        {
            Polygon arrow = (Polygon)sender;

            lastArrow.Stroke = unselected;
            lastArrow = arrow;
            arrow.Stroke = selected;

            if (arrow.Name.Equals("rightPoly"))
            {
                orientation = Orientation.HORIZONTAL;
            }
            else
            {
                orientation = Orientation.VERTICAL;
            }
        }

        private void ship_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Shapes.Path elemPath = (System.Windows.Shapes.Path)sender;
            if (!elemPath.IsEnabled)
            {
                return;
            }
            if (lastElem != null)
            {
                lastElem.Stroke = unselected;
            }

            lastElem = elemPath;
            elem = elemPath.Name;
            elemPath.Stroke = selected;

            switch (elem)
            {
                case "quadruple":
                    size = 4;
                    break;
                case "triple1":
                case "triple2":
                    size = 3;
                    break;
                case "double1":
                case "double2":
                case "double3":
                    size = 2;
                    break;
                case "single1":
                case "single2":
                case "single3":
                case "single4":
                    size = 1;
                    break;
            }
        }
        

        private void buttonRandomize_Click(object sender, RoutedEventArgs e)
        {
            check_random = true;
            string[] random_grids = new string[17];
            random_grids[0] = "0000D000000000000000FF0GG0EE0A0000000000B00JJJJ00C00000000000HHH00III0000000000000000000000000000000";
            random_grids[1] = "000000000E0000HHH00E0F000000000F000000000000JJJJ00GG000000000000C000I000D000B0I000000000I00A00000000";
            random_grids[2] = "000D00000000000J00H00EE00J00H000000J00H000A00J000000000000I00B00F0C0I00000F000I00000000000000000GG00";
            random_grids[3] = "000000000E0000III00E0A000000000000000HHH000GG000000000000FF000000C00000000000000JJJJ00D0B00000000000";
            random_grids[4] = "00EE0FF0000000000000III0J0HHH00000J000000000J0000000C0J00000000000A0000D00000000000GG0B0000000000000";
            random_grids[5] = "00000000000EE0FF0GG000000000000D0B0A0C0000000000000HHH00III00000000000000JJJJ00000000000000000000000";
            random_grids[6] = "00000B0000000A000C00000000000000FF0D0EE00000000000GG0HHH0III000000000000JJJJ000000000000000000000000";
            random_grids[7] = "0000I00F0E0000I00F0E00A0I00000000000G00H0000C0G00H000000000H00JJJJ00000000000B0000D00000000000000000";
            random_grids[8] = "0A0E0000FF000E00000000000III000B0000000C000JJJJ00000000000000D0G00HHH0000G00000000000000000000000000";
            random_grids[9] = "HHH000000B0000JJJJ0000D000000000000C0EE000FF000000000000III0000000000000GG00A00000000000000000000000";
            random_grids[10] = "00000000FF0000B000000000000HHH00000000000000III0000C0000000A0000JJJJ0000000000000D00EE00GG0000000000";
            random_grids[11] = "000000000000000EE00J00I000000J00I000000J00I000B00J000000000000A00000G00000H0C0G0FF00H000000000H00D00";
            random_grids[12] = "B00A00EE0000000000000C00000000000FF0HHH00000000000000000000000III0JJJJ0000000000GG00D000000000000000";
            random_grids[13] = "000000000000000B00EE0000000000000FF00HHH00000000000000000000III000JJJJ00000000000A0GG00C00000000000D";
            random_grids[14] = "0000D000EE0000000000JJJJ000A000000000000000B00C00000000000FF0000III00000000000GGHHH00000000000000000";
            random_grids[15] = "00000000000III000EE000000A00000FF0000000000000HHH00GG00000000000B000000C0000JJJJ0000000000000D000000";
            random_grids[16] = "0D00FF000G000000000GJJJJ000A0000000B000000000000000III00EE0C000000000000000HHH0000000000000000000000";
            Random rand_gr = new Random();
            int RandGrid = rand_gr.Next(16);
            result_str = random_grids[RandGrid];
                for (int i = 0; i < 100; i++)
            {
                switch (result_str[i])
                {
                    case '0':
                        playerGrid[i].Tag = "water";
                        break;
                    case 'A':
                        playerGrid[i].Tag = elems[0];
                        playerGrid[i].Background = elemColors[0];
                        break;
                    case 'B':
                        playerGrid[i].Tag = elems[1];
                        playerGrid[i].Background = elemColors[0];
                        break;
                    case 'C':
                        playerGrid[i].Tag = elems[2];
                        playerGrid[i].Background = elemColors[0];
                        break;
                    case 'D':
                        playerGrid[i].Tag = elems[3];
                        playerGrid[i].Background = elemColors[0];
                        break;
                    case 'E':
                        playerGrid[i].Tag = elems[4];
                        playerGrid[i].Background = elemColors[1];
                        break;
                    case 'F':
                        playerGrid[i].Tag = elems[5];
                        playerGrid[i].Background = elemColors[1];
                        break;
                    case 'G':
                        playerGrid[i].Tag = elems[6];
                        playerGrid[i].Background = elemColors[1];
                        break;
                    case 'H':
                        playerGrid[i].Tag = elems[7];
                        playerGrid[i].Background = elemColors[2];
                        break;
                    case 'I':
                        playerGrid[i].Tag = elems[8];
                        playerGrid[i].Background = elemColors[2];
                        break;
                    case 'J':
                        playerGrid[i].Tag = elems[9];
                        playerGrid[i].Background = elemColors[3];
                        break;
                }
            }
            numElemsPlaced = 10;

        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            string str_to_server = "";
            if (numElemsPlaced != 10)
            {
                MessageBox.Show("Arrange all the elements.");
                return;
            }
            foreach (var el in playerGrid)
            {
                if (el.Tag.Equals("water"))
                {
                    str_to_server += "0";
                }
                else
                {
                    if (el.Tag.Equals("single1"))
                    {
                        str_to_server += "A";
                    }
                    if (el.Tag.Equals("single2"))
                    {
                        str_to_server += "B";
                    }
                    if (el.Tag.Equals("single3"))
                    {
                        str_to_server += "C";
                    }
                    if (el.Tag.Equals("single4"))
                    {
                        str_to_server += "D";
                    }
                    if (el.Tag.Equals("double1"))
                    {
                        str_to_server += "E";
                    }
                    if (el.Tag.Equals("double2"))
                    {
                        str_to_server += "F";
                    }
                    if (el.Tag.Equals("double3"))
                    {
                        str_to_server += "G";
                    }
                    if (el.Tag.Equals("triple1"))
                    {
                        str_to_server += "H";
                    }
                    if (el.Tag.Equals("triple2"))
                    {
                        str_to_server += "I";
                    }
                    if (el.Tag.Equals("quadruple"))
                    {
                        str_to_server += "J";
                    }
                }
            }
            if (check_random)
            {
                str_to_server = result_str;
            }
            try
            {
                if (is_connected)
                {
                    writer.WriteLine("name:" + PlayerName + "grid:" + str_to_server);
                }
            }
            catch { }
            
            Window2 w = new Window2();
            w.Content = new PlayPage(playerGrid, PlayerName, stream, client);
            w.Show();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Wait signal.");
            //t_CheckData.Start();
        }
        
    }
}
