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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conBuilder = new SqlConnectionStringBuilder { InitialCatalog = "EFCore_HomeWork", DataSource = @"(localdb)\MSSQLLocalDB", IntegratedSecurity = true };

            optionsBuilder.UseSqlServer(conBuilder.ConnectionString);
        }
    }
}
