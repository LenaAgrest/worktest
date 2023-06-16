using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Printing;

namespace test_kadri
{
    /// <summary>
    /// Логика взаимодействия для Page5.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        public static string connectString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\worktest.mdf;Integrated Security = True; Connect Timeout = 30";
        int result; SqlDataReader reader; DataTable dt; SqlConnection myConnection;
        public Page5()
        {
            InitializeComponent();
            myConnection = new SqlConnection(connectString);
            myConnection.Open();
            obn("NewSotr");
        }
        private void update_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["id"] = dt.DefaultView[dg.SelectedIndex]["id"].ToString();
        }
        
        private void obn(string tablename)
        {
            string query = $"SELECT * FROM {tablename}";
            SqlCommand command = new SqlCommand(query, myConnection);
            reader = command.ExecuteReader();
            dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            dg.ItemsSource = dt.DefaultView;
        }

        private void uchet_Click(object sender, RoutedEventArgs e)
        {
            obn("autorize");
            Thickness mar = dg.Margin;
            mar.Bottom = 60;
            dg.Margin = mar;
        }

        private void kandidat_Click(object sender, RoutedEventArgs e)
        {
            obn("NewSotr");
            Thickness mar = dg.Margin;
            mar.Bottom = 0;
            dg.Margin = mar;
        }

        private void exex_Click(object sender, RoutedEventArgs e)
        {
            myConnection.Close();
            NavigationService.Navigate(new Uri(@"Page1.xaml", UriKind.Relative));
        }
    }
}
