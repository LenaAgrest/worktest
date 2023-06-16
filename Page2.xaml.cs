using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace test_kadri
{
    public partial class Page2 : Page
    {
        public static string connectString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\worktest.mdf;Integrated Security = True; Connect Timeout = 30";
        private SqlConnection con; string DateStart;
        SolidColorBrush greenback = new SolidColorBrush(Color.FromArgb(255, 0, 150, 64));
        SolidColorBrush greenkontur = new SolidColorBrush(Color.FromArgb(255, 0, 133, 57));
        SolidColorBrush greentext = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        SolidColorBrush grayback = new SolidColorBrush(Color.FromArgb(255, 229, 229, 229));
        SolidColorBrush graykontur = new SolidColorBrush(Color.FromArgb(255, 209, 209, 209));
        SolidColorBrush graytext = new SolidColorBrush(Color.FromArgb(178, 95, 94, 94));
        public Page2()
        {
            InitializeComponent();
            dp.Background = grayback;
            ikt.Background = grayback;
            rus.Background = grayback;
            zakon.Background = grayback;
            otv_null("delpproizvodstvo");
            otv_null("ikt");
            otv_null("zakon");
            otv_null("rus");
        }
        private void dp_Click(object sender, RoutedEventArgs e)
        {
            if (dp.Background == greenback)
            {
                dp.Background = grayback;
                dp.BorderBrush = graykontur;
                dp.Foreground = graytext;
                App.Current.Resources["dp"] = 0;
            }
            else if (dp.Background == grayback)
            {
                dp.Background = greenback;
                dp.BorderBrush = greenkontur;
                dp.Foreground = greentext;
                App.Current.Resources["dp"] = 2;
            }
        }
        private void ikt_Click(object sender, RoutedEventArgs e)
        {
            if (ikt.Background == greenback)
            {
                ikt.Background = grayback;
                ikt.BorderBrush = graykontur;
                ikt.Foreground = graytext;
                App.Current.Resources["ikt"] = 0;
            }
            else if (ikt.Background == grayback)
            {
                ikt.Background = greenback;
                ikt.BorderBrush = greenkontur;
                ikt.Foreground = greentext;
                App.Current.Resources["ikt"] = 2;
            }
        }
        private void zakon_Click(object sender, RoutedEventArgs e)
        {
            if (zakon.Background == greenback)
            {
                zakon.Background = grayback;
                zakon.BorderBrush = graykontur;
                zakon.Foreground = graytext;
                App.Current.Resources["zakon"] = 0;
            }
            else if (zakon.Background == grayback)
            {
                zakon.Background = greenback;
                zakon.BorderBrush = greenkontur;
                zakon.Foreground = greentext;
                App.Current.Resources["zakon"] = 2;
            }
        }
        private void rus_Click(object sender, RoutedEventArgs e)
        {
            if (rus.Background == greenback)
            {
                rus.Background = grayback;
                rus.BorderBrush = graykontur;
                rus.Foreground = graytext;
                App.Current.Resources["rus"] = 0;
            }
            else if (rus.Background == grayback)
            {
                rus.Background = greenback;
                rus.BorderBrush = greenkontur;
                rus.Foreground = greentext;
                App.Current.Resources["rus"] = 2;
                con = new SqlConnection(connectString);
                string query = $"SELECT ФИО FROM NewSotr WHERE NewSotr.Должность = '2';";
                using (SqlConnection connection = new SqlConnection(connectString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    kolvop.Text = Convert.ToString(cmd.ExecuteScalar());
                }
                con.Close();
            }
        }
        private void otv_null(string tablename)
        {
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query2 = $"update {tablename} set Выбранный=null;";
            SqlCommand command = new SqlCommand(query2, myConnection);
            command.ExecuteNonQuery();
            myConnection.Close();
        }
        private void start_Click(object sender, RoutedEventArgs e)
        {
            DateStart = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            App.Current.Resources["voprosov"] = kolvop.Text;
            if ((rus.Background == grayback) && (zakon.Background == grayback) && (ikt.Background == grayback) && (dp.Background == grayback))
            {
                MessageBox.Show("Ни один тест не выбран! \nP.S. нажмите на кнопку с названием теста(ов)");
            }
            else if (fiobox.Text == "") MessageBox.Show("Внесите сведения о кандидате!");
            else if ((kolvop.Text == "") || ((Convert.ToInt32(kolvop.Text) > 25))) { MessageBox.Show("Введите количество вопросов (не более 25)"); }
            else
            {
                try
                {
                    using (SqlConnection myConnection = new SqlConnection(connectString))
                    {
                        string query = $"INSERT INTO NewSotr (ФИО, Должность, НомерАнкеты, Телефон, ДатаТестирования) " +
                        $" VALUES (@fio,@dol,@ank,@phone,@DateStart)";
                        using (SqlCommand command = new SqlCommand(query, myConnection))
                        {
                            command.Parameters.AddWithValue("@fio", fiobox.Text);
                            command.Parameters.AddWithValue("@dol", dolbox.Text);
                            command.Parameters.AddWithValue("@ank", ankbox.Text);
                            command.Parameters.AddWithValue("@phone", phonebox.Text);
                            command.Parameters.AddWithValue("@DateStart", DateStart);
                            myConnection.Open();
                            command.ExecuteNonQuery();
                            App.Current.Resources["fio"] = fiobox.Text;
                            NavigationService.Navigate(new Uri(@"Page3.xaml", UriKind.Relative));
                        }
                    }
                }
                catch (System.FormatException) { MessageBox.Show("Какое-либо поле пустое или формат данных неверен. Введите/проверьте данные, пожалуйста"); }
            }
        }
    }
}