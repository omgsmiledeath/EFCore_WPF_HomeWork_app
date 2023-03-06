using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
using EFCore_WPF_HomeWork_app.Models;
using EFCore_WPF_HomeWork_app.ViewModels;

namespace EFCore_WPF_HomeWork_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Переменные
        
        OleDbViewModel oleDBVM;
        MsSqlViewModel mssqlDBVM;
        
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            oleDBVM = new OleDbViewModel();
            mssqlDBVM = new MsSqlViewModel();
            custumerAddMI.IsEnabled = false;
            custumerSaveChanges.IsEnabled = false;
            orderAddMI.IsEnabled = false;
            OrdersSaveChanges.IsEnabled=false;
        }

        
        /// <summary>
        /// Для привязки к событию MssqlViewModel, и вывода сообщений состояния базы
        /// </summary>
        /// <param name="mess">Сообщение</param>
        private void setMssqlState (string mess)
        {
            mssqlState.Content = mess;
        }
        /// <summary>
        /// Для привязки к событию OleDBViewModel, и вывода сообщений состояния базы
        /// </summary>
        /// <param name="mess">Сообщение</param>
        private void setOleDbState (string mess)
        {
            oledblState.Content = mess;
        }


        /// <summary>
        /// Добавление нового Custumer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCustumerButton_Click(object sender, RoutedEventArgs e)
        {
            Custumer newCustumer = new Custumer();
            
            AddRecord ar = new AddRecord(newCustumer, mssqlDBVM.Custumers.Last().id+1);
            ar.ShowDialog();
            if (ar.DialogResult == true)
            {
                mssqlDBVM.Custumers.Add(newCustumer);
                
            }
        }
    
        /// <summary>
        /// Добавление нового Order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderAddMI_Click(object sender, RoutedEventArgs e)
        {
            Order newOrder = new Order();

            AddRecord ar = new AddRecord(newOrder, oleDBVM.Orders.Last().id + 1);
            ar.ShowDialog();
            if (ar.DialogResult == true)
            {
                oleDBVM.Orders.Add(newOrder);
                
            }
        }
        /// <summary>
        /// Удаление выделенного Order в OrdersGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                var order = OrdersGrid.SelectedItem as Order;
                oleDBVM.Orders.Remove(order);
            }
            else MessageBox.Show("Select row for delete");
        }
        /// <summary>
        /// Для оповещения об изменении строки с Order в OrdersGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrdersGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                
                oleDBVM.OrderUpdate();
            }
        }

        /// <summary>
        /// Для оповещения об изменении строки с Custumer в CustumersGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void CustumersGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (CustumersGrid.SelectedItem != null)
            {
                mssqlDBVM.CustumerUpdate();
                
            }
        }
        /// <summary>
        /// Удаление выделеного Custumer из CustumersGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustumersDeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            if (CustumersGrid.SelectedItem != null)
            {
                var custumer= CustumersGrid.SelectedItem as Custumer;
                mssqlDBVM.Custumers.Remove(custumer);
              
            }
            else MessageBox.Show("Select row for delete");
        }
        /// <summary>
        /// Создание экземпляра Settings, для ввода строк подключения к базам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSettingsClick(object sender, RoutedEventArgs e)
        {
           
            var settings = new Settings();
            settings.Owner= this;
            settings.ShowDialog();
            
        }
        /// <summary>
        /// Создание экземпляра класса MsSqlViewModel ,для подключения к MSSQL базе
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="initcatalog"></param>
        /// <returns></returns>
        public async Task MssqlInit(string datasource,string initcatalog)
        {
            var conStr = new SqlConnectionStringBuilder()
            {
                DataSource = datasource,
                InitialCatalog = initcatalog,
                IntegratedSecurity = true
            };
            await Task.Run(()=>
            mssqlDBVM = new MsSqlViewModel(conStr.ConnectionString)
            );
            mssqlDBVM.State += setMssqlState;
            setMssqlState("Open base");
            CustumersGrid.DataContext = mssqlDBVM.Custumers;
            custumerAddMI.IsEnabled = true;
            custumerSaveChanges.IsEnabled = true;
        }
        /// <summary>
        /// Создание экземпляра класса OleDbInit ,для подключения к Access базе
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="initcatalog"></param>
        /// <returns></returns>
        public async Task OleDbInit(string datasource)
        {
            
            await Task.Run(() =>
            oleDBVM = new OleDbViewModel(datasource)
            );
            oleDBVM.State += setOleDbState;
            setOleDbState("Open base");
            OrdersGrid.DataContext = oleDBVM.Orders;
            orderAddMI.IsEnabled = true;
            OrdersSaveChanges.IsEnabled = true;
            
        }
        /// <summary>
        /// Сохранение изменений в базе MSSQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void custumerSaveChanges_Click(object sender, RoutedEventArgs e)
        {
             Dispatcher.Invoke(()=>mssqlDBVM.SaveChangesAsync());
        }
        /// <summary>
        /// Сохранение измененеий в базе Access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrdersSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => oleDBVM.SaveChangesAsync());
        }
    }
}
