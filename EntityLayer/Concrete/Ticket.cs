using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Ticket
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public decimal Price { get; set; }  // Bilet fiyatı
        public string QRCode { get; set; } // QR Kod verisi
        public DateTime PurchaseDate { get; set; }
        public bool IsVerified { get; set; } // QR kod doğrulandı mı?
    }
}
