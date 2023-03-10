using EFCore_WPF_HomeWork_app.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    /// Логика взаимодействия для AddRecord.xaml
    /// </summary>
    public partial class AddRecord : Window
    {
        Custumer newCustumer = null;
        Order newOrder = null;
        bool isMSSQL, isOleDB, isComplete = false;
        int id;
        public AddRecord()
        {
            InitializeComponent();
            CustumerPanel.Visibility = Visibility.Collapsed;
            OrdersPanel.Visibility = Visibility.Collapsed;
        }
        
        public AddRecord(object record, int id) : this()
        {

            if (record is Order)
            {
                this.newOrder = record as Order;
                OrdersPanel.Visibility = Visibility.Visible;
                this.id = id;
                orderId.Text = id.ToString();
                isOleDB = true;
            }
            if (record is Custumer)
            {
                this.newCustumer = record as Custumer;
                this.id = id;
                custumerId.Text = id.ToString();
                CustumerPanel.Visibility = Visibility.Visible;
                isMSSQL = true;
            }       
            }



        /// <summary>
        /// Проверка на введенные значения и ввод данных в экземпляры классов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            if (CheckOleDBboxes() && isComplete)
            {
                newOrder.id = id;
                newOrder.email = emailTxt.Text;
                int productId;
                if (Int32.TryParse(productIdTxt.Text, out productId))
                {
                    newOrder.productId = productId;
                }
                else return;
                newOrder.productDescription = productDescTxt.Text;
                this.DialogResult = true;
            }
                if (CheckMSSQLDBboxes() && isComplete)
                {
                    newCustumer.id = id;
                    newCustumer.lastName = lastNameTxt.Text;
                    newCustumer.firstName = firstNameTxt.Text;
                    newCustumer.middleName = midleNameTxt.Text;
                    newCustumer.phone = phoneTxt.Text;
                    newCustumer.email = emailTxt2.Text;
                    this.DialogResult = true;
                }
        }
        
        /// <summary>
        /// Закрытие окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        /// <summary>
        /// Проверка полей для экземпляра Order
        /// </summary>
        /// <returns></returns>
        private bool CheckOleDBboxes()
        {
            if (!isOleDB) return false;
            if (!String.IsNullOrWhiteSpace(emailTxt.Text) && !String.IsNullOrWhiteSpace(productIdTxt.Text) && !String.IsNullOrWhiteSpace(productDescTxt.Text))
            {
                foreach (char c in productIdTxt.Text)
                {
                    if (!Char.IsDigit(c))
                    {
                        MessageBox.Show("Product ID only numbers");
                        isComplete = false;
                        return false;
                    }
                }
                isComplete = true;
                return true;
            }
            else
            {
                MessageBox.Show("Enter all fields");
                isComplete = false;
                return false;
            }
        }
        /// <summary>
        /// Проверка полей для экземпляра Custumer
        /// </summary>
        /// <returns></returns>
        private bool CheckMSSQLDBboxes()
        {
            if (isOleDB) return false;
            if (!String.IsNullOrWhiteSpace(firstNameTxt.Text) &&
                !String.IsNullOrWhiteSpace(lastNameTxt.Text) &&
                !String.IsNullOrWhiteSpace(midleNameTxt.Text) &&
                !String.IsNullOrWhiteSpace(emailTxt2.Text))
            {
                isComplete = true;
                return true;
            }
            else
            {
                MessageBox.Show("Enter all fields '\n'*Phone can be empty!");
                isComplete = false;
                return false;
            }
        }
    }
}



