using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace EFCore_WPF_HomeWork_app.Models
{
    internal class MsSqlBase :DbContext
    {
        public DbSet<Custumer> Custumers { get; set; }
        private string conStr;
        public MsSqlBase()
        {

        }
        public MsSqlBase(string conStr)
        {
            this.conStr = conStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if(!string.IsNullOrEmpty(conStr)) 
            optionsBuilder.UseSqlServer(conStr);
        }
    }
}
