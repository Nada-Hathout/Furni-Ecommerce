using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public string PaymentStatus { get; set; }

        public string? TransactionId { get; set; }
    }
}
