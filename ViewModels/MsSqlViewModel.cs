using EFCore_WPF_HomeWork_app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_WPF_HomeWork_app.ViewModels
{
    public class MsSqlViewModel : IDisposable, IBaseState
    {
        MsSqlBase CustumerBase { get; set; }
        public ObservableCollection<Custumer> Custumers = new ObservableCollection<Custumer>();
        private bool isConnectedToSql = false;
        public bool IsConnectedToSql { get 
            {
                if(isConnectedToSql) State?.Invoke("Connected"); 
                else State?.Invoke("Not Connected");
                return isConnectedToSql;
            } }
        public event Action<string>? State;

       
        public MsSqlViewModel(string conStr)
        {
            try
            {
                CustumerBase = new MsSqlBase(conStr);
                Custumers = new ObservableCollection<Custumer>(CustumerBase.Custumers);
                Custumers.CollectionChanged += OrdersCollectionsChanged;
                isConnectedToSql = true;
            }
            catch(Exception ex)
            {
                State?.Invoke(ex.Message);
            }
            
        }

        public MsSqlViewModel()
        {
            CustumerBase = new MsSqlBase();
            Custumers = new ObservableCollection<Custumer>();
            Custumers.CollectionChanged += OrdersCollectionsChanged;

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
        public void CustumerUpdate()
        {
            State?.Invoke("Custumer Updated");
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
