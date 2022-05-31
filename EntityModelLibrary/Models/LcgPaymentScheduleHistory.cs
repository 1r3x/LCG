using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EntityModelLibrary.Models
{
    [Table("LCG_PaymentScheduleHistory")]
    public partial class LcgPaymentScheduleHistory
    {
        [Key]
        public int Id { get; set; }
        public int PaymentScheduleId { get; set; }
        [StringLength(150)]
        public string TransactionId { get; set; }
        [StringLength(50)]
        public string ResponseCode { get; set; }
        [StringLength(150)]
        public string ResponseMessage { get; set; }
        [StringLength(50)]
        public string AuthorizationNumber { get; set; }
        [StringLength(50)]
        public string AuthorizationText { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TimeLog { get; set; }
    }
}
