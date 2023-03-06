using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        DataRowView dr;
        OleDbViewModel oleDBVM;
        MsSqlViewModel mssqlDBVM;
        
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            oleDBVM = new OleDbViewModel("");
            mssqlDBVM = new MsSqlViewModel("");
            mssqlDBVM.State += setMssqlState;
            oleDBVM.State += setOleDbState;
            OrdersGrid.DataContext = oleDBVM.Orders;
            
            CustumersGrid.DataContext = mssqlDBVM.Custumers;
            
        }

        

        private void setMssqlState (string mess)
        {
            mssqlState.Content = mess;
        }

        private void setOleDbState (string mess)
        {
            oledblState.Content = mess;
        }


        #region AddCustumer
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
    
        #endregion
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
        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                var order = OrdersGrid.SelectedItem as Order;
                oleDBVM.Orders.Remove(order);
                // Dispatcher.Invoke(()=>mssqlDBVM.SaveChangesAsync());
            }
            else MessageBox.Show("Select row for delete");
        }

        private void OrdersGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            //if (dr != null)
            //{
            //    dr.EndEdit();
            //    oleDBVM.Update();
            //}
        }

        private void OrdersGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {


            //if (OrdersGrid.SelectedItem != null)
            //{
            //    dr = (DataRowView)OrdersGrid.SelectedItem;
            //    dr.BeginEdit();
            //}
        }

        private void CustumersGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            //if (dr != null)
            //{
            //    dr.EndEdit();
            //    mssqlDBVM.Update();
            //}
        }

        private void CustumersGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //if (CustumersGrid.SelectedItem != null)
            //{
            //    dr = (DataRowView)CustumersGrid.SelectedItem;
            //    dr.BeginEdit();
            //}
        }

        private void CustumersDeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            if (CustumersGrid.SelectedItem != null)
            {
                var custumer= CustumersGrid.SelectedItem as Custumer;
                mssqlDBVM.Custumers.Remove(custumer);
              
            }
            else MessageBox.Show("Select row for delete");
        }
        private void menuSettingsClick(object sender, RoutedEventArgs e)
        {
            //var settings = new Settings(mssqlDBVM, oleDBVM);
            //settings.ShowDialog();
        }

        private void custumerSaveChanges_Click(object sender, RoutedEventArgs e)
        {
             Dispatcher.Invoke(()=>mssqlDBVM.SaveChangesAsync());
        }

        private void OrdersSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => oleDBVM.SaveChangesAsync());
        }
    }
}
