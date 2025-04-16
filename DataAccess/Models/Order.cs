using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        [Required]
        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address? Address { get; set; }

        public int? PaymentId { get; set; }

        [ForeignKey("PaymentId")]
        public virtual Payment? Payment { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; }

        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
