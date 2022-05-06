using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EntityModelLibrary.Models
{
    [Table("cc_payment")]
    [Index(nameof(DebtorAcct), Name = "x_acct")]
    [Index(nameof(PaymentDate), nameof(VoidSale), Name = "x_payment_date")]
    public partial class CcPayment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("debtor_acct")]
        [StringLength(50)]
        public string DebtorAcct { get; set; }
        [Column("company")]
        [StringLength(50)]
        public string Company { get; set; }
        [Column("user_id")]
        [StringLength(50)]
        public string UserId { get; set; }
        [Column("user_name")]
        [StringLength(50)]
        public string UserName { get; set; }
        [Column("debtor_name")]
        [StringLength(100)]
        public string DebtorName { get; set; }
        [Column("debtor_address1")]
        [StringLength(100)]
        public string DebtorAddress1 { get; set; }
        [Column("debtor_address2")]
        [StringLength(100)]
        public string DebtorAddress2 { get; set; }
        [Column("debtor_city")]
        [StringLength(50)]
        public string DebtorCity { get; set; }
        [Column("debtor_state")]
        [StringLength(2)]
        public string DebtorState { get; set; }
        [Column("debtor_zip")]
        [StringLength(10)]
        public string DebtorZip { get; set; }
        [Column("debtor_area_code")]
        [StringLength(3)]
        public string DebtorAreaCode { get; set; }
        [Column("debtor_phone")]
        [StringLength(50)]
        public string DebtorPhone { get; set; }
        [Column("billing_name")]
        [StringLength(100)]
        public string BillingName { get; set; }
        [Column("billing_address1")]
        [StringLength(100)]
        public string BillingAddress1 { get; set; }
        [Column("billing_address2")]
        [StringLength(100)]
        public string BillingAddress2 { get; set; }
        [Column("billing_city")]
        [StringLength(50)]
        public string BillingCity { get; set; }
        [Column("billing_state")]
        [StringLength(2)]
        public string BillingState { get; set; }
        [Column("billing_zip")]
        [StringLength(10)]
        public string BillingZip { get; set; }
        [Column("billing_area_code")]
        [StringLength(3)]
        public string BillingAreaCode { get; set; }
        [Column("billing_phone")]
        [StringLength(50)]
        public string BillingPhone { get; set; }
        [Column("card_type")]
        [StringLength(50)]
        public string CardType { get; set; }
        [Column("card_num")]
        [StringLength(50)]
        public string CardNum { get; set; }
        [Column("card_exp_month")]
        [StringLength(2)]
        public string CardExpMonth { get; set; }
        [Column("card_exp_year")]
        [StringLength(2)]
        public string CardExpYear { get; set; }
        [Column("card_cvv")]
        [StringLength(4)]
        public string CardCvv { get; set; }
        [Column("subtotal", TypeName = "money")]
        public decimal? Subtotal { get; set; }
        [Column("processing_fee", TypeName = "money")]
        public decimal? ProcessingFee { get; set; }
        [Column("pif_letter", TypeName = "money")]
        public decimal? PifLetter { get; set; }
        [Column("del_letter", TypeName = "money")]
        public decimal? DelLetter { get; set; }
        [Column("charge_total", TypeName = "money")]
        public decimal? ChargeTotal { get; set; }
        [Column("payment_date", TypeName = "datetime")]
        public DateTime? PaymentDate { get; set; }
        [Column("approval_status")]
        [StringLength(50)]
        public string ApprovalStatus { get; set; }
        [Column("approval_code")]
        [StringLength(50)]
        public string ApprovalCode { get; set; }
        [Column("error_code")]
        [StringLength(200)]
        public string ErrorCode { get; set; }
        [Column("order_number")]
        [StringLength(50)]
        public string OrderNumber { get; set; }
        [Column("ref_number")]
        [StringLength(50)]
        public string RefNumber { get; set; }
        [Column("void_sale")]
        [StringLength(1)]
        public string VoidSale { get; set; }
        [Column("sif")]
        [StringLength(1)]
        public string Sif { get; set; }
        [Column("talk_off_for_collector")]
        public int? TalkOffForCollector { get; set; }
        [Column("ecard_num")]
        [MaxLength(100)]
        public byte[] EcardNum { get; set; }
        [Column("sms_decline")]
        [StringLength(1)]
        public string SmsDecline { get; set; }
        [Column("cbr_fee", TypeName = "money")]
        public decimal? CbrFee { get; set; }
        [Column("ecfee")]
        [MaxLength(100)]
        public byte[] Ecfee { get; set; }
        [Column("hsa")]
        [StringLength(1)]
        public string Hsa { get; set; }
    }
}
