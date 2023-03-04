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
            OrdersBase = new OleDbBase("");
            Orders= new ObservableCollection<Order>(OrdersBase.Orders);
            Orders.CollectionChanged += OrdersCollectionsChanged;
            State?.Invoke("Connected");
        }

        private void OrdersCollectionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OrdersBase.Orders.AddRange(e.NewItems.Cast<Order>().ToArray());
            State?.Invoke("Orders base changed");
        }

        public void Dispose()
        {
            State?.Invoke("Disposed");
            OrdersBase.Dispose();           
        }
    }
}
