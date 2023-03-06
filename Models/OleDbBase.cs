using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_WPF_HomeWork_app.Models
{
    internal class OleDbBase:DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;
        private string _conStr = string.Empty; 

        public OleDbBase(string conStr)
        {
            _conStr = conStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseJetOleDb(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = D:\OrdersBase.accdb");


    }
}
