using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EntityModelLibrary.Models
{
    [Table("LCG_CardInfo")]
    public partial class LcgCardInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column("PaymentMethodID")]
        [StringLength(150)]
        public string PaymentMethodId { get; set; }
        [StringLength(50)]
        public string EntryMode { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(50)]
        public string BinNumber { get; set; }
        [StringLength(4)]
        public string LastFour { get; set; }
        public int? ExpirationMonth { get; set; }
        public int? ExpirationYear { get; set; }
        [Required]
        [StringLength(50)]
        public string CardHolderName { get; set; }
        [Required]
        [StringLength(12)]
        public string AssociateDebtorAcct { get; set; }
        public bool IsActive { get; set; }
    }
}
