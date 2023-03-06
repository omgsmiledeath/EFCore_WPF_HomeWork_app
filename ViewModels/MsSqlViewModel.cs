using EFCore_WPF_HomeWork_app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_WPF_HomeWork_app.ViewModels
{
    internal class MsSqlViewModel:IDisposable, IBaseState
    {
        MsSqlBase CustumerBase { get; set; }
        public ObservableCollection<Custumer> Custumers = new ObservableCollection<Custumer>();

        public event Action<string>? State;

        public MsSqlViewModel(string conStr)
        {
            CustumerBase = new MsSqlBase();
            Custumers = new ObservableCollection<Custumer>(CustumerBase.Custumers);
            Custumers.CollectionChanged += OrdersCollectionsChanged;
            State?.Invoke("Connected");
        }

        private void OrdersCollectionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action) 
            {
                case NotifyCollectionChangedAction.Add:
                    CustumerBase.Custumers.AddRange(e.NewItems.Cast<Custumer>().ToArray());
                    State?.Invoke($"Added new Custumer");
                    break;
                case NotifyCollectionChangedAction.Remove:
                    CustumerBase.Custumers.RemoveRange(e.OldItems.Cast<Custumer>().ToArray());
                    State?.Invoke($"Custumer Deleted");
                    break;
                    
            } 
        }

        public void Dispose()
        {
            State?.Invoke("Disposed");
            CustumerBase.Dispose();
        }
        public async Task SaveChangesAsync()
        {
            State?.Invoke("Change saved");
            await Task.Run (()=>CustumerBase.SaveChangesAsync());
        }

    }
}
