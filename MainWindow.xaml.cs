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
            OrdersGrid.DataContext = oleDBVM.Orders;
            
            CustumersGrid.DataContext = mssqlDBVM.Custumers;
        }

        private void setMssqlState (string mess)
        {
            mssqlState.Content = mess;
        }



        #region AddCustumer
        private void AddCustumerButton_Click(object sender, RoutedEventArgs e)
        {
            //int id;
            //DataRow dr;
            //if (mssqlDBVM.IsConnectedToSql)
            //{
            //    dr = mssqlDBVM.CustumersDt.NewRow();
            //    id = (int)mssqlDBVM.CustumersDt.Rows[mssqlDBVM.CustumersDt.Rows.Count - 1]["id"];
            //    AddRecord ar = new AddRecord(dr, id);
            //    ar.ShowDialog();
            //    if (ar.DialogResult == true)
            //    {
            //        mssqlDBVM.CustumersDt.Rows.Add(dr);
            //        mssqlDBVM.Update();
            //    }
            //}
        }
        #endregion
        private void orderAddMI_Click(object sender, RoutedEventArgs e)
        {
            //if (oleDBVM.IsConnectedToAccess)
            //{
            //    int id;
            //    DataRow dr;
            //    dr = oleDBVM.OrdersDt.NewRow();
            //    id = (int)oleDBVM.OrdersDt.Rows[oleDBVM.OrdersDt.Rows.Count - 1]["id"];
            //    AddRecord ar = new AddRecord(dr, id);
            //    ar.ShowDialog();
            //    if (ar.DialogResult == true)
            //    {
            //        oleDBVM.OrdersDt.Rows.Add(dr);
            //        oleDBVM.Update();
            //    }
            //}
        }
        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            //if (OrdersGrid.SelectedItem != null)
            //{
            //    (OrdersGrid.SelectedItem as DataRowView).Row.Delete();
            //    oleDBVM.Update();
            //}
            //else MessageBox.Show("Select row for delete");
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
              // Dispatcher.Invoke(()=>mssqlDBVM.SaveChangesAsync());
            }
            else MessageBox.Show("Select row for delete");
        }
        private void menuSettingsClick(object sender, RoutedEventArgs e)
        {
            //var settings = new Settings(mssqlDBVM, oleDBVM);
            //settings.ShowDialog();
        }
    }
}
