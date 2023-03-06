using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EFCore_WPF_HomeWork_app.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore_WPF_HomeWork_app.ViewModels
{
    public class OleDbViewModel:IDisposable
    {
        
        OleDbBase OrdersBase { get; set; }
        public ObservableCollection<Order> Orders = new ObservableCollection<Order>();

        public event Action<string>? State;

        public OleDbViewModel()
        {
            Orders = new ObservableCollection<Order>();
        }
        public OleDbViewModel(string conStr)
        {
            try
            {
                OrdersBase = new OleDbBase(conStr);
                Orders = new ObservableCollection<Order>(OrdersBase.Orders);
                Orders.CollectionChanged += OrdersCollectionsChanged;
                State?.Invoke("Connected");
            }
            catch (Exception ex) 
            {
                State?.Invoke(ex.Message);
            }
        }
        /// <summary>
        /// При добавлении в коллекцию Orders , изменяет состояние базы для дальнейшего сохранения элементов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrdersCollectionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OrdersBase.Orders.AddRange(e.NewItems.Cast<Order>().ToArray());
                    State?.Invoke($"Added new order");
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OrdersBase.Orders.RemoveRange(e.OldItems.Cast<Order>().ToArray());
                    State?.Invoke($"Order Deleted");
                    break;
                case NotifyCollectionChangedAction.Replace:
                    OrdersBase.Orders.UpdateRange(e.NewItems.Cast<Order>().ToArray());
                    State?.Invoke("Update Order");
                    break;

            }
        }

        public void OrderUpdate()
        {
            State?.Invoke("Order Updated");
        }

        public void Dispose()
        {
            State?.Invoke("Disposed");
            OrdersBase.Dispose();           
        }

        public async Task SaveChangesAsync()
        {
            State?.Invoke("Change saved");
            await Task.Run(() => OrdersBase.SaveChangesAsync());
        }
    }
}
