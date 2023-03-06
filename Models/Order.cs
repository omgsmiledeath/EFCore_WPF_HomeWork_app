using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_WPF_HomeWork_app.Models
{
    public class Order
    {
        public int id { get; set; }
        public string email { get; set; }
        public int productId { get; set; }
        public string productDescription { get; set; }
    }
}
