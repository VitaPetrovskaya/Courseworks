using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
    /// Логика взаимодействия для PlayPage.xaml
    /// </summary>
    public partial class PlayPage : UserControl
    {
        public string PlayerName;
        public string winner = "";
        public string CompName;
        public int highScore;
        public Grid[] playerGrid;
        public List<int> hitList;
        public Grid[] compGrid;
        int[] shipSizes = new int[] { 1, 1, 1, 1, 2, 2, 2, 3, 3, 4 };
        string[] ships = new string[] { "single1", "single2", "single3", "single4", "double1", "double2", "double3", "triple1", "triple2", "quadruple" };
        int turnCount = 0;
        public Random random = new Random();

        TcpClient client;
        StreamReader reader;
        StreamWriter writer;
        NetworkStream stream;
        bool is_connected;

        int pSingle1Count = 1, pSingle2Count = 1, pSingle3Count = 1, pSingle4Count = 1;
        int cSingle1Count = 1, cSingle2Count = 1, cSingle3Count = 1, cSingle4Count = 1;
        int pDouble1Count = 2, pDouble2Count = 2, pDouble3Count = 2;
        int cDouble1Count = 2, cDouble2Count = 2, cDouble3Count = 2;
        int pTriple1Count = 3, pTriple2Count = 3;
        int cTriple1Count = 3, cTriple2Count = 3;
        int pQuadrupleCount = 4, cQuadrupleCount = 4;

        public PlayPage(Grid[] playerGrid, string player_name, NetworkStream rec_stream, TcpClient rec_client)
        {
            InitializeComponent();
            
            PlayerName = player_name;
            
            hitList = new List<int>();
            stream = rec_stream;
            client = rec_client;
            if (stream != null)
            {
                is_connected = true;
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream) { AutoFlush = true };
            }
            initiateSetup(playerGrid);
        }
        

        private void initiateSetup(Grid[] userGrid)
        {
            //Set computer grid
            
            compGrid = new Grid[100];
            CompGrid.Children.CopyTo(compGrid, 0);
            for (int i = 0; i < 100; i++)
            {
                compGrid[i].Tag = "water";
            }
            if (!is_connected)
            {
                setupCompGrid();
            }
            else
            {
                string input_from_server = reader.ReadLine();
                CompName = input_from_server.Replace("nameCo:", "");
                CompName = CompName.Remove(CompName.IndexOf("grid:"));
                string receiveStrGrid;
                receiveStrGrid = input_from_server.Replace("nameCo:" + CompName + "grid:", "");

                setupCompGrid_net(receiveStrGrid);
            }
            //Set player grid
            playerGrid = new Grid[100];
            PlayerGrid.Children.CopyTo(playerGrid, 0);

            //Set ships
            for (int i = 0; i < 100; i++)
            {
                playerGrid[i].Background = userGrid[i].Background;
                playerGrid[i].Tag = userGrid[i].Tag;
            }
            buttonAttack.IsEnabled = true;
            
        }

        public void CheckClose(object obj)
        {
            if (client.Available > 0)
            {
                if (reader.ReadLine().IndexOf("disconnect") != -1)
                {
                    MessageBox.Show(CompName + "disconnected.");
                    disableGrids();
                }
            }
                
        }


        /////// ИГРА С КОМПЬЮТЕРОМ ///////
        private void setupCompGrid()
        {
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
            string result_str = random_grids[RandGrid];
            setupCompGrid_net(result_str);
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (is_connected)
            {
                buttonStartOver.Content = "Disconnect";
            }
            lblCompName.Content = "Ваш соперник: "+CompName;
            MessageBox.Show("Найди все фрукты, овощи и ягоды соперника и получи меню рационального питания!");
            //TimerCallback tm = new TimerCallback(CheckClose);
            //System.Threading.Timer timer = new System.Threading.Timer(tm, 0, 0, 100);
        }

        private void setupCompGrid_net(string receiv)
        {
            for (int i = 0; i < 100; i++)
            {
               switch (receiv[i])
                {
                    case 'A':
                        compGrid[i].Tag = ships[0];
                            break;
                    case 'B':
                        compGrid[i].Tag = ships[1];
                        break;
                    case 'C':
                        compGrid[i].Tag = ships[2];
                        break;
                    case 'D':
                        compGrid[i].Tag = ships[3];
                        break;
                    case 'E':
                        compGrid[i].Tag = ships[4];
                            break;
                    case 'F':
                        compGrid[i].Tag = ships[5];
                        break;
                    case 'G':
                        compGrid[i].Tag = ships[6];
                        break;
                    case 'H':
                        compGrid[i].Tag = ships[7];
                            break;
                    case 'I':
                        compGrid[i].Tag = ships[8];
                        break;
                    case 'J':
                        compGrid[i].Tag = ships[9];
                        break;
                }
            }
            
        }



        private void buttonStartOver_Click(object sender, RoutedEventArgs e)
        {
            if (is_connected)
            {
                writer.WriteLine("disconnect");
                Application.Current.Shutdown();
            }
            Application.Current.Shutdown();
        }

        private void buttonAttack_Click(object sender, RoutedEventArgs e)
        {
            string X = validateXCoordinate(textBoxX.Text);
            string Y = validateYCoordinate(textBoxY.Text);
            int index = 0;

            if (X == "" || Y == "")
            {
                MessageBox.Show("Invalid value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            switch (X)
            {
                case "A":
                    index = 0;
                    break;
                case "B":
                    index = 10;
                    break;
                case "C":
                    index = 20;
                    break;
                case "D":
                    index = 30;
                    break;
                case "E":
                    index = 40;
                    break;
                case "F":
                    index = 50;
                    break;
                case "G":
                    index = 60;
                    break;
                case "H":
                    index = 70;
                    break;
                case "I":
                    index = 80;
                    break;
                case "J":
                    index = 90;
                    break;
            }
            index += int.Parse(Y) - 1;
            clearTextBoxes();
            gridMouseDown(compGrid[index], null);
        }
        

        private void gridMouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid square;
            int index_of_selected;
            

            square = (Grid)sender;
            
            index_of_selected = Array.IndexOf(compGrid, square);

            if (is_connected)
            {

                if (client.Available > 0)
                {
                    string input = reader.ReadLine();
                    if (input.IndexOf("Attack:") != -1)
                    {
                        compTurn(Int32.Parse(input.Replace("Attack:", "")));
                    }
                    if (input.IndexOf("lose") != -1)
                    {
                        MessageBox.Show("Вы проиграли :(");
                        disableGrids();
                        return;
                    }
                }
            }
            //Check if player turn yet
            if (turnCount % 2 != 0)
                {
                    //return;
                }
            

            switch (square.Tag.ToString())
            {
                case "water":
                    square.Tag = "miss";
                    square.Background = new SolidColorBrush(Colors.LightGray);
                    turnCount++;
                    break;
                case "miss":
                case "hit":
                    MessageBox.Show("it has already been selected.");
                    return;
                case "single1":
                    cSingle1Count--;
                    break;
                case "single2":
                    cSingle2Count--;
                    break;
                case "single3":
                    cSingle3Count--;
                    break;
                case "single4":
                    cSingle4Count--;
                    break;
                case "double1":
                    cDouble1Count--;
                    break;
                case "double2":
                    cDouble2Count--;
                    break;
                case "double3":
                    cDouble3Count--;
                    break;
                case "triple1":
                    cTriple1Count--;
                    break;
                case "triple2":
                    cTriple2Count--;
                    break;
                case "quadruple":
                    cQuadrupleCount--;
                    break;
            }
            if (square.Tag.ToString() != "miss")
            {
                square.Tag = "hit";
                square.Background = new SolidColorBrush(Colors.Red);
                turnCount++;
                checkPlayerWin();
            }
            if (!is_connected)
            {
                compTurn(0); // ход соперника
            }
            else
            {
                if (client.Available > 0)
                {
                    string input = reader.ReadLine();
                    if (input.IndexOf("Attack:") != -1)
                    {
                        compTurn(Int32.Parse(input.Replace("Attack:", "")));
                    }
                }
            }
            
        }
        

        private void compTurn(int position)
        {
            hunterMode(position);
            //turnCount++;
            checkComputerWin();
        }

        private void hunterMode(int position)
        {
            if (!is_connected)
            {
                do
                {
                    position = random.Next(100);

                } while ((playerGrid[position].Tag.Equals("miss")) || (playerGrid[position].Tag.Equals("hit")));
            }
            simpleMode(position);
            
        }

        private void simpleMode(int position)
        {
            if (!(playerGrid[position].Tag.Equals("water")))
            {
                switch (playerGrid[position].Tag.ToString())
                {
                    case "single1":
                        pSingle1Count--;
                        break;
                    case "single2":
                        pSingle2Count--;
                        break;
                    case "single3":
                        pSingle3Count--;
                        break;
                    case "single4":
                        pSingle4Count--;
                        break;
                    case "double1":
                        pDouble1Count--;
                        break;
                    case "double2":
                        pDouble2Count--;
                        break;
                    case "double3":
                        pDouble3Count--;
                        break;
                    case "triple1":
                        pTriple1Count--;
                        break;
                    case "triple2":
                        pTriple2Count--;
                        break;
                    case "quadruple":
                        pQuadrupleCount--;
                        break;
                }
                // Mark the grid as hit
                playerGrid[position].Tag = "hit";
                playerGrid[position].Background = new SolidColorBrush(Colors.Red);
            }
            else
            {
                playerGrid[position].Tag = "miss";
                playerGrid[position].Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void checkComputerWin()
        {
            if (cSingle1Count == 0)
            {
                cSingle1Count = -1;
                MessageBox.Show("Я нашел твое яблоко!");
            }
            if (cSingle2Count == 0)
            {
                cSingle2Count = -1;
                MessageBox.Show("Я нашел твое яблоко!");
            }
            if (cSingle3Count == 0)
            {
                cSingle3Count = -1;
                MessageBox.Show("Я нашел твое яблоко!");
            }
            if (cSingle4Count == 0)
            {
                cSingle4Count = -1;
                MessageBox.Show("Я нашел твое яблоко!");
            }
            if (cDouble1Count == 0)
            {
                cDouble1Count = -1;
                MessageBox.Show("Я отыскал твою корзинку моркови!");
            }
            if (cDouble2Count == 0)
            {
                cDouble2Count = -1;
                MessageBox.Show("Я отыскал твою корзинку моркови!");
            }
            if (cDouble3Count == 0)
            {
                cDouble3Count = -1;
                MessageBox.Show("Я отыскал твою корзинку моркови!");
            }
            if (cTriple1Count == 0)
            {
                cTriple1Count = -1;
                MessageBox.Show("Я нашел твое лукошко клубники!");
            }
            if (cTriple2Count == 0)
            {
                cTriple2Count = -1;
                MessageBox.Show("Я нашел твое лукошко клубники!");
            }

            if (cQuadrupleCount == 0)
            {
                cQuadrupleCount = -1;
                MessageBox.Show("Я собрал все твои сливы!");
            }

            if (cSingle1Count == -1 && cSingle2Count == -1 && cSingle3Count == -1 && cSingle4Count == -1 &&
                cDouble1Count == -1 && cDouble2Count == -1 && cDouble3Count == -1 &&
                cTriple1Count == -1 && cTriple2Count == -1 && cQuadrupleCount == -1)
            {
                if (winner == "")
                {
                    MessageBox.Show("Вы проиграли! :(");
                    winner = CompName;
                }
                disableGrids();
                //displayHighScores(saveHighScores(true));
            }
        }

        private void checkPlayerWin()
        {
            if (cSingle1Count == 0)
            {
                cSingle1Count = -1;
                MessageBox.Show("Ты нашел яблоко!");
            }
            if (cSingle2Count == 0)
            {
                cSingle2Count = -1;
                MessageBox.Show("Ты нашел яблоко!");
            }
            if (cSingle3Count == 0)
            {
                cSingle3Count = -1;
                MessageBox.Show("Ты нашел яблоко!");
            }
            if (cSingle4Count == 0)
            {
                cSingle4Count = -1;
                MessageBox.Show("Ты нашел яблоко!");
            }
            if (cDouble1Count == 0)
            {
                cDouble1Count = -1;
                MessageBox.Show("Ты отыскал корзинку моркови!");
            }
            if (cDouble2Count == 0)
            {
                cDouble2Count = -1;
                MessageBox.Show("Ты отыскал корзинку моркови!");
            }
            if (cDouble3Count == 0)
            {
                cDouble3Count = -1;
                MessageBox.Show("Ты отыскал корзинку моркови!");
            }
            if (cTriple1Count == 0)
            {
                cTriple1Count = -1;
                MessageBox.Show("Ты нашел лукошко клубники!");
            }
            if (cTriple2Count == 0)
            {
                cTriple2Count = -1;
                MessageBox.Show("Ты нашел лукошко клубники!");
            }

            if (cQuadrupleCount == 0)
            {
                cQuadrupleCount = -1;
                MessageBox.Show("Ты собрал все сливы!");
            }

            if (cSingle1Count == -1 && cSingle2Count == -1 && cSingle3Count == -1 && cSingle4Count == -1 &&
                cDouble1Count == -1 && cDouble2Count == -1 && cDouble3Count == -1 &&
                cTriple1Count == -1 && cTriple2Count == -1 && cQuadrupleCount == -1)
            {
                if (winner == "")
                {
                    MessageBox.Show("Вы выиграли!");
                    writer.WriteLine(" I'm winner. ");
                    winner = PlayerName;
                    WindowMenu w_memu = new WindowMenu();
                    w_memu.Show();
                }
                disableGrids();
                //displayHighScores(saveHighScores(true));
            }
        }

        private void disableGrids()
        {
            foreach (var element in compGrid)
            {
                if (element.Tag.Equals("water"))
                {
                    element.Background = new SolidColorBrush(Colors.LightGray);
                }
                else 
                  if (element.Tag.Equals("single1") || element.Tag.Equals("single2") || element.Tag.Equals("single3") || element.Tag.Equals("single4") || element.Tag.Equals("double1") || element.Tag.Equals("double2") 
                    || element.Tag.Equals("double3") || element.Tag.Equals("triple1") ||
                  element.Tag.Equals("triple2") || element.Tag.Equals("quadruple"))
                  {
                    element.Background = new SolidColorBrush(Colors.LightGreen);
                  }
                element.IsEnabled = false;
            }
            foreach (var element in playerGrid)
            {
                if (element.Tag.Equals("water"))
                {
                    element.Background = new SolidColorBrush(Colors.LightGray);
                }
                element.IsEnabled = false;
            }
            clearTextBoxes();
            buttonAttack.IsEnabled = false;

        }

        private void clearTextBoxes()
        {
            textBoxX.Text = "";
            textBoxY.Text = "";
        }

        private void btnLetter_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            textBoxX.Text = button.Content.ToString();
        }

        private void btnNumber_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            textBoxY.Text = button.Content.ToString();
        }

        private string validateXCoordinate(string X)
        {
            if (X.Length != 1)
            {
                return "";
            }

            if (Char.IsLetter(X[0]))
            {
                return X;
            }
            return "";
        }

        private string validateYCoordinate(string Y)
        {
            if (Y.Length > 2 || Y == "")
            {
                return "";
            }

            if (int.Parse(Y) > 0 || int.Parse(Y) <= 10)
            {
                return Y;
            }
            return "";
        }
    }
}
