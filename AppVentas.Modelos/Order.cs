using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVentas.Modelos
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaYHora { get; set; } = DateTime.Now;
        public double Subtotal { get; set; }
        public double Total { get; set; }
        public User? User { get; set; }
        public Product? Product { get; set; }

    }
}
