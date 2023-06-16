using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace test_kadri
{
    public partial class Page4 : Page
    {
        public static string connectString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\worktest.mdf;Integrated Security = True; Connect Timeout = 30";
        int kolotv, i, kolvse; double d, ik, z, r, k, sr, s1, s2, s3, s4; string[] tabl; string numb, datat, dolj, vopr, o1, o2, o3, pr, vib, otvetik;

        private void start_Click(object sender, RoutedEventArgs e)
        {
            file();
        }
        public Page4()
        {
            InitializeComponent();
            sotr();
            podschet();
        }
        private int rez(string tablename)
        {
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query = $"SELECT COUNT(*) FROM {tablename} WHERE Правильный=Выбранный and Выбранный!= 0;";
            SqlCommand command = new SqlCommand(query, myConnection);
            kolotv = (int)command.ExecuteScalar();
            myConnection.Close();
            return kolotv;
        }
        private void sotr()
        {
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query = $"SELECT НомерАнкеты, ДатаТестирования, Должность FROM NewSotr WHERE ФИО=N'{App.Current.Resources["fio"]}'";
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[3]);
                numb = reader[0].ToString();
                datat = reader[1].ToString();
                dolj = reader[2].ToString();
            }
            l2.Text += Convert.ToString(App.Current.Resources["fio"]);
            l3.Content += numb;
            l4.Content += $"{datat}";
            reader.Close();
            myConnection.Close();
        }
        private void podschet()
        {
            k = Convert.ToInt32(App.Current.Resources["voprosov"]);
            d = Convert.ToInt32(App.Current.Resources["dp"]);
            ik = Convert.ToInt32(App.Current.Resources["ikt"]);
            z = Convert.ToInt32(App.Current.Resources["zakon"]);
            r = Convert.ToInt32(App.Current.Resources["rus"]);
            if (d == 1)
            {
                d = rez("delpproizvodstvo"); i++; s1 = d / k;
                result.Text += $"\r\nОсновы делопроизводства и документооборота: {d}/{k} ({Math.Round(s1 * 100, 2)}%)";
            }
            if (ik == 1)
            {
                ik = rez("ikt"); i++; s2 = ik / k;
                result.Text += $"\r\nИнформационно - коммуникационные технологии: {ik}/{k} ({Math.Round(s2 * 100, 2)}%)";
            }
            if (z == 1)
            {
                z = rez("zakon") / 2; i++; s3 = z / k;
                result.Text += $"\r\nКонституция, Устав Алексеевского ГО и др.: {z}/{k} ({Math.Round(s3 * 100, 2)}%)";
            }
            if (r == 1)
            {
                r = rez("rus"); i++; s4 = r / k;
                result.Text += $"\r\nУровень грамотности письменной речи: {r}/{k} ({Math.Round(s4 * 100, 2)}%)";
            }
            sr = (((d / k) * 100) + ((ik / k) * 100) + ((z / k) * 100) + ((r / k) * 100)) / i;
            l5.Content += Convert.ToString(Math.Round(sr, 2)) + '%';
        }
        private void file()
        {
            d = Convert.ToInt32(App.Current.Resources["dp"]);
            ik = Convert.ToInt32(App.Current.Resources["ikt"]);
            z = Convert.ToInt32(App.Current.Resources["zakon"]);
            r = Convert.ToInt32(App.Current.Resources["rus"]);
            string filePath = $"C://Тестирование/{App.Current.Resources["fio"]}.html";
            string[] verh = new string[] {"<html>",
                $"\r\n<title>{App.Current.Resources["fio"]} </title>",
                "\r\n<body>",
            $"\r\n<h1><b>{App.Current.Resources["fio"]}</b></h1>",
            $"\r\nДолжность: {dolj}",
            $"\r\n<br>Дата и время тестирования: {datat}",
            $"\r\n<br>Итого: {Math.Round(sr, 2)}%" };
            string[] niz = new string[] { "\r\n</body>", "\r\n</html>" };
            try
            {
                using (FileStream fileStream = File.Open(filePath, FileMode.Create))
                {
                    using (StreamWriter output = new StreamWriter(fileStream))
                    {
                        for (i = 0; i < verh.Length; i++) output.Write(verh[i]);
                        if (d == 1)
                        {
                            d = rez("delpproizvodstvo");
                            output.Write($"\r\n<br>Основы делопроизводства и документооборота: {d}/{k} ({Math.Round(s1 * 100, 2)}%)");
                            vseotv("delpproizvodstvo");
                            for (i = 0; i < kolvse; i++)
                            { output.Write(otvet("delpproizvodstvo")); }
                        }
                        if (ik == 1)
                        {
                            ik = rez("ikt");
                            output.Write($"\r\n<br>Информационно - коммуникационные технологии: {ik}/{k} ({Math.Round(s2 * 100, 2)}%)");
                            vseotv("ikt");
                            for (i = 0; i < kolvse; i++)
                            { output.Write(otvet("ikt")); }
                        }
                        if (z == 1)
                        {
                            z = rez("zakon");
                            output.Write($"\r\n<br>Конституция, Устав Алексеевского ГО и др.: {z}/{k} ({Math.Round(s3 * 100, 2)}%)");
                            vseotv("zakon");
                            for (i = 0; i < kolvse; i++)
                            { output.Write(otvet("zakon")); }
                        }
                        if (r == 1)
                        {
                            r = rez("rus");
                            output.Write($"\r\n<br>Уровень грамотности письменной речи: {r}/{k} ({Math.Round(s4 * 100, 2)}%)");
                            vseotv("rus");
                            for (i = 0; i < kolvse; i++)
                            { output.Write(otvet("rus")); }
                        }
                        for (i = 0; i < niz.Length; i++) output.Write(niz[i]);
                    }
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            { Directory.CreateDirectory("C://Тестирование"); }
            Process.Start($"C://Тестирование/{App.Current.Resources["fio"]}.html");
        }
        private int vseotv(string tablename)
        {
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query = $"SELECT COUNT(*) FROM {tablename} WHERE Выбранный!= 0;";
            SqlCommand command = new SqlCommand(query, myConnection);
            kolvse = (int)command.ExecuteScalar();
            myConnection.Close();
            return kolvse;
        }
        private string otvet(string tablename)
        {
            SqlConnection myConnection = new SqlConnection(connectString);
            myConnection.Open();
            string query = $"SELECT Вопрос, Ответ1, Ответ2, Ответ3, Правильный, Выбранный FROM {tablename} WHERE id={i + 1}";
            SqlCommand command = new SqlCommand(query, myConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[6]);
                vopr = reader[0].ToString();
                o1 = reader[1].ToString();
                o2 = reader[2].ToString();
                o3 = reader[3].ToString();
                pr = reader[4].ToString();
                vib = reader[5].ToString();
            }
            otvetik = $"\r\n<br><b>Вопрос: </b> {vopr}" + $"\r\n<br>1. {o1}" + $"\r\n<br>2. {o2}" + $"\r\n<br>3. {o3}" + $"\r\n<br>Правильный: {pr}" + $"\r\n<br>Выбран: {vib}";
            reader.Close();
            myConnection.Close();
            return otvetik;
        }
    }
}