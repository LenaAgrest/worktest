using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace test_kadri
{
    public partial class Page1 : Page
    {
        public static string connectString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\worktest.mdf;Integrated Security = True; Connect Timeout = 30";
        private SqlConnection con; string role;
        public Page1()
        { InitializeComponent();  }

        private void Button_Click(object sender, RoutedEventArgs e)
        {  try
            {   con = new SqlConnection(connectString);
                string query = $"SELECT Роль FROM autorize WHERE autorize.login = '{log.Text}' AND autorize.password='{pass.Text}';";
                using (SqlConnection connection = new SqlConnection(connectString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    role = Convert.ToString(cmd.ExecuteScalar());
                    if (role == "adm") NavigationService.Navigate(new Uri(@"Page5.xaml", UriKind.Relative));
                    else if (role == "kadr") NavigationService.Navigate(new Uri(@"Page2.xaml", UriKind.Relative));
                    else { MessageBox.Show("Неправильный логин или пароль, попробуйте еще раз!"); }
                }
                con.Close();
            }
            catch(Exception ex) { MessageBox.Show($"{ex.Message}"); }
        }  
    }
}