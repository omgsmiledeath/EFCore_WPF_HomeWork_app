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
    internal class OleDbViewModel:IDisposable,IBaseState
    {
        
        OleDbBase OrdersBase { get; set; }
        public ObservableCollection<Order> Orders = new ObservableCollection<Order>();

        public event Action<string>? State;

        public OleDbViewModel(string conStr)
        {
            try
            {
                OrdersBase = new OleDbBase("");
                Orders = new ObservableCollection<Order>(OrdersBase.Orders);
                Orders.CollectionChanged += OrdersCollectionsChanged;
                State?.Invoke("Connected");
            }
            catch (Exception ex) 
            {
                State?.Invoke(ex.Message);
            }
        }

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

            }
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
