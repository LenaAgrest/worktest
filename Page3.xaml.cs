using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace test_kadri
{
    public partial class Page3 : Page
    {
        public static string connectString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\worktest.mdf;Integrated Security = True; Connect Timeout = 30";
        string vop, one, two, three, etot, nametable, c; int koltab, index, i, p, k, d, ik, z, r; int[] idi;
        SolidColorBrush greenback = new SolidColorBrush(Color.FromArgb(255, 0, 150, 64));
        SolidColorBrush redback = new SolidColorBrush(Color.FromArgb(255, 150, 0, 0));
        public Page3()
        {
            InitializeComponent();
            table();
        }
        public void table()
        {
            i = 0;
            d = Convert.ToInt32(App.Current.Resources["dp"]);
            if (d == 2)
            {
                name.Text = "Основы делопроизводства и документооборота";
                nametable = "delpproizvodstvo"; c = "dp";
            }
            else
            {
                ik = Convert.ToInt32(App.Current.Resources["ikt"]);
                if (ik == 2)
                {
                    name.Text = "Информационно - коммуникационные технологии";
                    nametable = "ikt"; c = "ikt";
                }
                else
                {
                    z = Convert.ToInt32(App.Current.Resources["zakon"]);
                    if (z == 2)
                    {
                        name.Text = "Конституция, Устав Алексеевского ГО и др.";
                        nametable = "zakon"; c = "zakon";
                    }
                    else
                    {
                        r = Convert.ToInt32(App.Current.Resources["rus"]);
                        if (r == 2)
                        {
                            name.Text = "Уровень грамотности письменной речи";
                            nametable = "rus"; c = "rus";
                        }
                        else if ((d != 2) && (ik != 2) && (z != 2) && (r != 2)) NavigationService.Navigate(new Uri(@"Page4.xaml", UriKind.Relative));
                    }
                }
            }
            odin.IsChecked = false;
            dva.IsChecked = false;
            tri.IsChecked = false;
            kolvo_v_tabl(nametable);
            mass(koltab);
            ran();
        }
        public int kolvo_v_tabl(string nametable)
        {
            try
            { k = Convert.ToInt32(App.Current.Resources["voprosov"]); }
            catch (System.FormatException) { k = 0; }
            finally
            {
                if (k != 0) koltab = k + 1;
                else
                {
                    SqlConnection myConnection = new SqlConnection(connectString);
                    myConnection.Open();
                    string query = $"SELECT COUNT(*) FROM {nametable}";
                    SqlCommand command = new SqlCommand(query, myConnection);
                    koltab = (int)command.ExecuteScalar();
                    myConnection.Close();
                }
            }
            return koltab;
        }
        public int chto()
        {
            if (odin.IsChecked == true) { p = 1; }
            else if (dva.IsChecked == true) p = 2;
            else if (tri.IsChecked == true) p = 3;
            else p = 0;
            return p;
        }
        private void start_Click(object sender, RoutedEventArgs e)
        {
            chto();
            if (p == 0)
            { start.Background = redback; etot = null; }
            else
            {
                start.Background = greenback;
                etot = Convert.ToString(p);
                SqlConnection myConnection = new SqlConnection(connectString);
                myConnection.Open();
                string query = $"update {nametable} set Выбранный={etot} where id={index};";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.ExecuteNonQuery();
                myConnection.Close();
                if (i < koltab - 1)
                {
                    ran();
                    odin.IsChecked = false;
                    dva.IsChecked = false;
                    tri.IsChecked = false;
                }
                else { App.Current.Resources[$"{c}"] = 1; table(); }
            }
        }
        private void new_vop_text(int index)
        {
            if (index != 0)
            {
                SqlConnection myConnection = new SqlConnection(connectString);
                myConnection.Open();
                string query = $"SELECT Вопрос, Ответ1, Ответ2, Ответ3, Правильный FROM {nametable} WHERE id={index}";
                SqlCommand command = new SqlCommand(query, myConnection);
                SqlDataReader reader = command.ExecuteReader();
                List<string[]> data = new List<string[]>();
                while (reader.Read())
                {
                    data.Add(new string[5]);
                    vop = reader[0].ToString();
                    one = reader[1].ToString();
                    two = reader[2].ToString();
                    three = reader[3].ToString();
                    etot = reader[4].ToString();
                }
                vopros.Text = vop + "\n";
                otv1.Text = one;
                otv2.Text = two;
                otv3.Text = three;
                reader.Close();
                myConnection.Close();
            }
            else ran();
        }
        private void mass(int kol)
        {
            idi = new int[kol];
            for (int q = 0; q < kol; q++)
            { idi[q] = q; }
        }
        private void ran()
        {
            Random rand = new Random();
            do { index = rand.Next(idi.Length); } while (idi[index] == 0);
            new_vop_text(index);
            idi[index] = 0;
            i++;
        }
    }
}