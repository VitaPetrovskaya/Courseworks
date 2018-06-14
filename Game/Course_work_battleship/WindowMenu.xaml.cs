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
    /// Логика взаимодействия для WindowMenu.xaml
    /// </summary>
    public partial class WindowMenu : Window
    {
        public WindowMenu()
        {
            InitializeComponent();
        }

        string name_Br = "";
        string name_Lu = "";
        string name_Di = "";


        private void lableBr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowRecipe w_Br = new WindowRecipe(name_Br);
            w_Br.Show();
        }

        private void lableLu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowRecipe w_Lu = new WindowRecipe(name_Lu);
            w_Lu.Show();
        }

        private void lableDi_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowRecipe w_Di = new WindowRecipe(name_Di);
            w_Di.Show();
        }

        public struct TDish         //блюдо
        {
            public string name;  //название
            public float proteins;    //белки
            public float fats;        //жиры
            public float carbohydrates;   //углеводы
            public float caloricity;    //калорийность
        };

        public TDish[] BreakfastDishes = new TDish[7];  
        public TDish[] LunchDishes = new TDish[4];  
        public TDish[] DinnerDishes = new TDish[4]; 
        public static int RandomRecipeBr = 0, RandomRecipeLu = 0, RandomRecipeDi = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int j = 0;
            for (int i = 0; i < BreakfastDishes.Length; i++)
            {
                BreakfastDishes[i].name = File.ReadLines("BreakfastDishes.txt").Skip(j++).First();
                BreakfastDishes[i].proteins = Convert.ToSingle(File.ReadLines("BreakfastDishes.txt").Skip(j++).First());
                BreakfastDishes[i].fats = Convert.ToSingle(File.ReadLines("BreakfastDishes.txt").Skip(j++).First());
                BreakfastDishes[i].carbohydrates = Convert.ToSingle(File.ReadLines("BreakfastDishes.txt").Skip(j++).First());
                BreakfastDishes[i].caloricity = Convert.ToSingle(File.ReadLines("BreakfastDishes.txt").Skip(j++).First());
            }
            j = 0;
            for (int i = 0; i < LunchDishes.Length; i++)
            {
                LunchDishes[i].name = File.ReadLines("LunchDishes.txt").Skip(j++).First();
                LunchDishes[i].proteins = Convert.ToSingle(File.ReadLines("LunchDishes.txt").Skip(j++).First());
                LunchDishes[i].fats = Convert.ToSingle(File.ReadLines("LunchDishes.txt").Skip(j++).First());
                LunchDishes[i].carbohydrates = Convert.ToSingle(File.ReadLines("LunchDishes.txt").Skip(j++).First());
                LunchDishes[i].caloricity = Convert.ToSingle(File.ReadLines("LunchDishes.txt").Skip(j++).First());
            }
            j = 0;
            for (int i = 0; i < DinnerDishes.Length; i++)
            {
                DinnerDishes[i].name = File.ReadLines("DinnerDishes.txt").Skip(j++).First();
                DinnerDishes[i].proteins = Convert.ToSingle(File.ReadLines("DinnerDishes.txt").Skip(j++).First());
                DinnerDishes[i].fats = Convert.ToSingle(File.ReadLines("DinnerDishes.txt").Skip(j++).First());
                DinnerDishes[i].carbohydrates = Convert.ToSingle(File.ReadLines("DinnerDishes.txt").Skip(j++).First());
                DinnerDishes[i].caloricity = Convert.ToSingle(File.ReadLines("DinnerDishes.txt").Skip(j++).First());
            }

            Random rand = new Random();
            RandomRecipeBr = rand.Next(6);
            RandomRecipeLu = rand.Next(3);
            RandomRecipeDi = rand.Next(3);

            name_Br = "Breakfast" + RandomRecipeBr.ToString() + ".txt";
            name_Lu = "Lunch" + RandomRecipeLu.ToString() + ".txt";
            name_Di = "Dinner" + RandomRecipeDi.ToString() + ".txt";

            lableBreakfast.Text = BreakfastDishes[RandomRecipeBr].name;
            lableLunch.Text = LunchDishes[RandomRecipeLu].name;
            lableDinner.Text = DinnerDishes[RandomRecipeDi].name;

            lableBr.Text = "Белки: " + BreakfastDishes[RandomRecipeBr].proteins + " г" + Environment.NewLine;
            lableBr.Text += "Жиры: " + BreakfastDishes[RandomRecipeBr].fats + " г" + Environment.NewLine;
            lableBr.Text += "Углеводы: " + BreakfastDishes[RandomRecipeBr].carbohydrates + " г" + Environment.NewLine;
            lableBr.Text += "Калорийность: " + BreakfastDishes[RandomRecipeBr].caloricity + " ККал" + Environment.NewLine;

            lableLu.Text = "Белки: " + LunchDishes[RandomRecipeLu].proteins + " г" + Environment.NewLine;
            lableLu.Text += "Жиры: " + LunchDishes[RandomRecipeLu].fats + " г" + Environment.NewLine;
            lableLu.Text += "Углеводы: " + LunchDishes[RandomRecipeLu].carbohydrates + " г" + Environment.NewLine;
            lableLu.Text += "Калорийность: " + LunchDishes[RandomRecipeLu].caloricity + " ККал" + Environment.NewLine;

            lableDi.Text = "Белки: " + DinnerDishes[RandomRecipeDi].proteins + " г" + Environment.NewLine;
            lableDi.Text += "Жиры: " + DinnerDishes[RandomRecipeDi].fats + " г" + Environment.NewLine;
            lableDi.Text += "Углеводы: " + DinnerDishes[RandomRecipeDi].carbohydrates + " г" + Environment.NewLine;
            lableDi.Text += "Калорийность: " + DinnerDishes[RandomRecipeDi].caloricity + " ККал" + Environment.NewLine;
        }
    }
}
