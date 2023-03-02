using EFCore_WPF_HomeWork_app.Models;
//using ADO_WPF_HomeWork_app.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace EFCore_WPF_HomeWork_app
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        MSSQLDBViewModel mssqlDBVM;
        OleDBViewModel oleDBVM;
        SecurityBot sb;
        SettingsSave ss = new SettingsSave();
        public Settings()
        {
            InitializeComponent();
        }
        public Settings(MSSQLDBViewModel mssqlDBVM, OleDBViewModel oleDBVM) : this()
        {
            this.mssqlDBVM = mssqlDBVM;
            this.oleDBVM = oleDBVM;
            sb = new SecurityBot();
            MSSQLPanel.Visibility = Visibility.Collapsed;
            OleDBPanel.Visibility = Visibility.Collapsed;
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(loginTxt.Text) && !string.IsNullOrWhiteSpace(passTxt.Text))
            {
                if (sb.TryToLogin(loginTxt.Text, passTxt.Text))
                {
                    MessageBox.Show("Successfully");

                    AuthenticationPanel.Visibility = Visibility.Collapsed;
                    MSSQLPanel.Visibility = Visibility.Visible;
                    OleDBPanel.Visibility = Visibility.Visible;
                    if (File.Exists("Settings.json"))
                        using (var sr = new StreamReader("Settings.json"))
                        {
                            var json = sr.ReadToEnd();
                            if (!string.IsNullOrEmpty(json))
                                ss = JsonConvert.DeserializeObject<SettingsSave>(json);
                            dataSourceTxt.Text = ss.MssqlDataSource;
                            initialCatTxt.Text = ss.MssqlInitialCatalog;
                            accessPathBox.Text = ss.OledbDataSource;
                        }
                }
                else MessageBox.Show("Wrong Lorin or Password");
            }
            else MessageBox.Show("Incorrect values in boxes");
        }

        private async void msqlConButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(dataSourceTxt.Text) && !string.IsNullOrWhiteSpace(initialCatTxt.Text))
            {
                var conStr = new SqlConnectionStringBuilder()
                {

                    DataSource = dataSourceTxt.Text,
                    InitialCatalog = initialCatTxt.Text,

                    IntegratedSecurity = true
                };
                mssqlConStr.Text = conStr.ConnectionString;
                var connectionStateString = await Dispatcher.InvokeAsync(() => mssqlDBVM.ConnectToSQL(conStr.ConnectionString)).Result;
                if (mssqlDBVM.IsConnectedToSql)
                {
                    MssqlEllipse.Fill = new SolidColorBrush() { Color = Colors.Green };
                    MssqlConStateBlock.Text = connectionStateString;
                    ss.MssqlDataSource = dataSourceTxt.Text;
                    ss.MssqlInitialCatalog = initialCatTxt.Text;
                }
                else
                {
                    MessageBox.Show($"{connectionStateString}");
                }
            }
            else MessageBox.Show("Enter Data Source and Initial Catalog");
        }

        private async void oleDBButton_Click(object sender, RoutedEventArgs e)
        {
            if (!oleDBVM.IsConnectedToAccess)
            {
                var shecduler = TaskScheduler.FromCurrentSynchronizationContext();
                if (!string.IsNullOrWhiteSpace(accessPathBox.Text))
                {
                    var conStr = @$"Provider=Microsoft.ACE.OLEDB.12.0; Data Source ={accessPathBox.Text}";

                    oleDBConStr.Text = conStr;
                    var connectionStateString = Dispatcher.InvokeAsync(() => oleDBVM.ConnectToAccess(conStr)).Result;

                    if (oleDBVM.IsConnectedToAccess)
                    {
                        OleDbEllipse.Fill = new SolidColorBrush() { Color = Colors.Green };
                        OleDBConStateBlock.Text = $"{connectionStateString.Result}";
                        ss.OledbDataSource = accessPathBox.Text;
                    }
                    else
                    {
                        MessageBox.Show($"{connectionStateString.Result}");
                    }
                }
                else MessageBox.Show("Enter data path");
            }
            else OleDBConStateBlock.Text = "Alredy connected!";
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ss.MssqlDataSource) && !string.IsNullOrEmpty(ss.MssqlInitialCatalog) && !string.IsNullOrEmpty(ss.OledbDataSource))
            {
                var ser = new JsonSerializer();
                using (var sw = new StreamWriter("settings.json"))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    ser.Serialize(jw, ss);
                }
                MessageBox.Show("Save settings done");
            }
        }
    }
}
