using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EntityModelLibrary.Models
{
    [Table("LCG_PaymentSchedule")]
    public partial class LcgPaymentSchedule
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string PatientAccount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EffectiveDate { get; set; }
        public int CardInfoId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public int NumberOfPayments { get; set; }
        public bool IsActive { get; set; }
    }
}
