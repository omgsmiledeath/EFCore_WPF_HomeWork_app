using EFCore_WPF_HomeWork_app.Models;
using EFCore_WPF_HomeWork_app.ViewModels;
//using ADO_WPF_HomeWork_app.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
        
        SecurityBot sb;

        SettingsSave connectSettings = new SettingsSave(); 
        public Settings()
        {
            InitializeComponent();
            sb = new SecurityBot();
            MSSQLPanel.Visibility = Visibility.Collapsed;
            OleDBPanel.Visibility = Visibility.Collapsed;
            SaveSettings.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Проверяются входные данные для аутентификации , при наличае файла с настройками выводит последние сохраненные 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                                connectSettings = JsonConvert.DeserializeObject<SettingsSave>(json);
                            dataSourceTxt.Text = connectSettings.MssqlDataSource;
                            initialCatTxt.Text = connectSettings.MssqlInitialCatalog;
                            accessPathBox.Text = connectSettings.OledbDataSource;
                        }
                }
                else MessageBox.Show("Wrong Lorin or Password");
            }
            else MessageBox.Show("Incorrect values in boxes");
        }
        /// <summary>
        /// Проверка подключения к базе Mssql , если успешно то производится подключение в основном окне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
               
                var sqlbase = new MsSqlBase(conStr.ConnectionString);

                bool canSqlConnect = await Dispatcher.Invoke(async Task<bool> () =>
                {
                    try
                    {
                        return await sqlbase.Database.CanConnectAsync();
                    }
                    catch(Exception ex)
                    {
                        MssqlConStateBlock.Text = ex.Message;
                        return false;
                    }
                });
                if (canSqlConnect)
                {
                    MssqlEllipse.Fill = new SolidColorBrush() { Color = Colors.Green };
                    MssqlConStateBlock.Text = "Success";
                    connectSettings.MssqlDataSource = dataSourceTxt.Text;
                    connectSettings.MssqlInitialCatalog = initialCatTxt.Text;
                    await (this.Owner as MainWindow).MssqlInit(connectSettings.MssqlDataSource, connectSettings.MssqlInitialCatalog);
                }
            }
            else MessageBox.Show("Enter Data Source and Initial Catalog");
        }
        
        /// <summary>
        /// Проверка подключения к базе Access , если успешно то производится подключение в основном окне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void oleDBButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(accessPathBox.Text))
            {
                var conStr = @$"Provider=Microsoft.ACE.OLEDB.12.0; Data Source ={accessPathBox.Text}";
                var oledbbase = new OleDbBase(conStr);
                bool canOledbConnect = await Dispatcher.Invoke(async Task<bool> () =>
                {
                    
                    try
                    {
                        return await oledbbase.Database.CanConnectAsync();
                    }
                    catch (Exception ex)
                    {
                        OleDBConStateBlock.Text = ex.Message;
                        return false;
                    }
                });

                if(canOledbConnect) 
                {
                    OleDbEllipse.Fill = new SolidColorBrush() { Color = Colors.Green };
                    OleDBConStateBlock.Text = "Success";
                    connectSettings.OledbDataSource = accessPathBox.Text;
                    await (this.Owner as MainWindow).OleDbInit(connectSettings.OledbDataSource);
                }
            }
            else MessageBox.Show("Enter data path");


        }
        /// <summary>
        /// Сохранение данных подключения к базам в экземпляре SettingsSave в файл, сериализованные в JSON 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(connectSettings.MssqlDataSource) && !string.IsNullOrEmpty(connectSettings.MssqlInitialCatalog) && !string.IsNullOrEmpty(connectSettings.OledbDataSource))
            {
                var ser = new JsonSerializer();
                using (var sw = new StreamWriter("settings.json"))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    ser.Serialize(jw, connectSettings);
                }
                MessageBox.Show("Save settings done");
            }
        }

       
    }
}
