using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
namespace EFCore_WPF_HomeWork_app.Models
{
    public class Custumer
    {
        
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

    }
}
