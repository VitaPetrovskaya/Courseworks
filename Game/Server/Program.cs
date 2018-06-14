using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Data.SqlClient;
using System.Threading;

namespace Server
{
    class Program
    {
        static SqlConnection sqlConnection;
        static string[] array_of_players = new string[100];

        static async void AddName(string name)
        {
            bool check = true;
            for (int k = 0; k < array_of_players.Length; k++)
            {
                if (name == array_of_players[k])
                {
                    check = false;
                    Console.WriteLine("Player exist");
                    break;
                }
            }
            if (check)
            {
                SqlCommand command = new SqlCommand("INSERT INTO [TableOfPlayers] (Name_of_player)VALUES(@Name_of_player)", sqlConnection);
                command.Parameters.AddWithValue("Name_of_player", name);
                await command.ExecuteNonQueryAsync();
                Console.WriteLine("Player was added.");
            }
            
        }

        static async void AddCompName(string name_pl, string name_co)
        {
            SqlCommand command = new SqlCommand("UPDATE [TableOfPlayers] SET [Name_of_rival]=@Name_of_rival WHERE [Name_of_player]=@Name_of_player", sqlConnection);
            command.Parameters.AddWithValue("Name_of_rival", name_co);
            command.Parameters.AddWithValue("Name_of_player", name_pl);
            await command.ExecuteNonQueryAsync();
            Console.WriteLine("Rival was added.");
        }

        static async void AddResult(string name_win, string name)
        {
            try
            {
                SqlCommand command = new SqlCommand("UPDATE [TableOfPlayers] SET [Result]=@Result, [Date_of_last_game]=@Date_of_last_game WHERE [Name_of_player]=@Name_of_player", sqlConnection);
                if (name_win == name)
                {
                    command.Parameters.AddWithValue("Result", "win");
                }
                else
                {
                    command.Parameters.AddWithValue("Result", "lose");
                }
                DateTime thisDay = DateTime.Today;
                command.Parameters.AddWithValue("Date_of_last_game", thisDay.ToString());
                command.Parameters.AddWithValue("Name_of_player", name);
                await command.ExecuteNonQueryAsync();
                Console.WriteLine("Game ended.");
            }
            catch { }
            
        }

        static async void Start()
        {
            Console.WriteLine("Starting server...");
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Asus\Documents\Visual Studio 2015\Projects\Course_work_battleship\Server\DB_players.mdf;Integrated Security=True;MultipleActiveResultSets=True"; //строка подключения базы данных о продуктах
            sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [TableOfPlayers]", sqlConnection);
            int i = 0;
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    array_of_players[i++]=Convert.ToString(sqlReader["Name_of_player"]);  // вывод всех продуктов на экран
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        
        

        
        
        

        static void Main(string[] args)
        {
            TcpListener listener;
            TcpClient client1;
            NetworkStream stream1;
            TcpClient client2;
            NetworkStream stream2;

            StreamWriter writer1;
            StreamReader reader1;
            StreamWriter writer2;
            StreamReader reader2;

            string name1 = "";
            string name2 = "";
            string grid1 = "";
            string grid2 = "";

            string inputLine1 = "";
            string inputLine2 = "";

            Start();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 1234);
            listener.Start();
            client1 = listener.AcceptTcpClient();
            stream1 = client1.GetStream();
            writer1 = new StreamWriter(stream1, Encoding.ASCII) { AutoFlush = true };
            reader1 = new StreamReader(stream1, Encoding.ASCII);

            client2 = listener.AcceptTcpClient();
            stream2 = client2.GetStream();
            writer2 = new StreamWriter(stream2, Encoding.ASCII) { AutoFlush = true };
            reader2 = new StreamReader(stream2, Encoding.ASCII);

            while (true)
            {
                if (client1.Available > 0)
                {
                    inputLine1 = reader1.ReadLine();
                    if (inputLine1.IndexOf("name:") != -1)
                    {
                        name1 = inputLine1.Replace("name:", "");
                        name1 = name1.Remove(name1.IndexOf("grid:"));
                       // AddName(name1);

                        grid1 = inputLine1.Replace("name:" + name1 + "grid:", "");
                        
                        if (grid2 != "")
                        {
                            writer2.WriteLine("nameCo:" + name1 + "grid:" + grid1);
                            
                            writer1.WriteLine("nameCo:" + name2 + "grid:" + grid2);

                            if (name2 != "")
                            {
                               // AddCompName(name2, name1);
                               // AddCompName(name1, name2);
                            }

                        }
                        
                    }
                    if (inputLine1.IndexOf("winner") != -1)
                    {
                        writer2.WriteLine("you lose");
                        AddName(name1);
                        AddName(name2);
                        AddCompName(name2, name1);
                        AddCompName(name1, name2);
                        AddResult(name1, name1);
                    }

                    if (inputLine1.IndexOf("Attack:") != -1)
                    {
                        writer2.WriteLine(inputLine1);
                    }
                    if (inputLine1.IndexOf("disconnect") != -1)
                    {
                        writer2.WriteLine(inputLine1);
                    }

                    Console.WriteLine("Received string: " + inputLine1);
                }
                if (client2.Available > 0)
                {
                    inputLine2 = reader2.ReadLine();
                    if (inputLine2.IndexOf("name:") != -1)
                    {
                        name2 = inputLine2.Replace("name:", "");
                        name2 = name2.Remove(name2.IndexOf("grid:"));
                        AddName(name2);
                        grid2 = inputLine2.Replace("name:" + name2 + "grid:", "");
                        if (name1 != "")
                        {
                            AddCompName(name2, name1);
                            AddCompName(name1, name2);
                        }
                        if (grid1 != "")
                        {
                            writer2.WriteLine("nameCo:" + name1+"grid:"+grid1);
                            
                            writer1.WriteLine("nameCo:" + name2 + "grid:"+grid2);

                        }

                    }
                    if (inputLine2.IndexOf("winner") != -1)
                    {
                        writer1.WriteLine("you lose");
                        AddName(name1);
                        AddName(name2);
                        AddCompName(name2, name1);
                        AddCompName(name1, name2);
                        AddResult(name2, name2);
                    }

                    if (inputLine2.IndexOf("Attack:") != -1)
                    {
                        writer1.WriteLine(inputLine2);
                    }
                    if (inputLine2.IndexOf("disconnect") != -1)
                    {
                        writer1.WriteLine(inputLine2);
                    }

                    Console.WriteLine("Received string: " + inputLine2);
                }
            }
            

        }
    }
    }
