using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EntityModelLibrary.Models
{
    [Keyless]
    [Table("debtor_acct_info_t")]
    [Index(nameof(ActivityCode), Name = "x_activity_code")]
    [Index(nameof(Employee), nameof(ScheduleDate), Name = "x_emp_sched_date")]
    [Index(nameof(SuppliedAcct), Name = "x_supplied_acct")]
    public partial class DebtorAcctInfoT
    {
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Required]
        [Column("acct_type")]
        [StringLength(1)]
        public string AcctType { get; set; }
        [Column("supplied_acct")]
        [StringLength(25)]
        public string SuppliedAcct { get; set; }
        [Required]
        [Column("acct_status")]
        [StringLength(1)]
        public string AcctStatus { get; set; }
        [Column("corporate")]
        [StringLength(1)]
        public string Corporate { get; set; }
        [Column("legal")]
        [StringLength(1)]
        public string Legal { get; set; }
        [Required]
        [Column("status_code")]
        [StringLength(3)]
        public string StatusCode { get; set; }
        [Column("activity_code")]
        [StringLength(3)]
        public string ActivityCode { get; set; }
        [Column("disposition")]
        public int? Disposition { get; set; }
        [Column("schedule_date", TypeName = "datetime")]
        public DateTime? ScheduleDate { get; set; }
        [Column("ivr_schedule_date", TypeName = "datetime")]
        public DateTime? IvrScheduleDate { get; set; }
        [Column("amount_placed", TypeName = "money")]
        public decimal AmountPlaced { get; set; }
        [Column("adjustments_life", TypeName = "money")]
        public decimal AdjustmentsLife { get; set; }
        [Column("interest_amt_life", TypeName = "money")]
        public decimal InterestAmtLife { get; set; }
        [Column("payment_amt_life", TypeName = "money")]
        public decimal PaymentAmtLife { get; set; }
        [Column("admin_fees_due", TypeName = "money")]
        public decimal AdminFeesDue { get; set; }
        [Column("admin_fees_paid", TypeName = "money")]
        public decimal AdminFeesPaid { get; set; }
        [Column("admin_fees_balance", TypeName = "money")]
        public decimal AdminFeesBalance { get; set; }
        [Column("costs_due", TypeName = "money")]
        public decimal CostsDue { get; set; }
        [Column("costs_paid", TypeName = "money")]
        public decimal CostsPaid { get; set; }
        [Column("costs_balance", TypeName = "money")]
        public decimal CostsBalance { get; set; }
        [Column("attorney_fees_due", TypeName = "money")]
        public decimal AttorneyFeesDue { get; set; }
        [Column("attorney_fees_paid", TypeName = "money")]
        public decimal AttorneyFeesPaid { get; set; }
        [Column("attorney_fees_balance", TypeName = "money")]
        public decimal AttorneyFeesBalance { get; set; }
        [Column("damages_due", TypeName = "money")]
        public decimal DamagesDue { get; set; }
        [Column("damages_paid", TypeName = "money")]
        public decimal DamagesPaid { get; set; }
        [Column("damages_balance", TypeName = "money")]
        public decimal DamagesBalance { get; set; }
        [Column("return_check_fees_due", TypeName = "money")]
        public decimal ReturnCheckFeesDue { get; set; }
        [Column("return_check_fees_paid", TypeName = "money")]
        public decimal ReturnCheckFeesPaid { get; set; }
        [Column("return_check_fees_balance", TypeName = "money")]
        public decimal ReturnCheckFeesBalance { get; set; }
        [Column("total_fees_balance", TypeName = "money")]
        public decimal TotalFeesBalance { get; set; }
        [Column("balance", TypeName = "money")]
        public decimal Balance { get; set; }
        [Column("broken_promises")]
        public int BrokenPromises { get; set; }
        [Column("placement")]
        public int Placement { get; set; }
        [Required]
        [Column("mail_return")]
        [StringLength(1)]
        public string MailReturn { get; set; }
        [Required]
        [Column("media_on_file")]
        [StringLength(1)]
        public string MediaOnFile { get; set; }
        [Column("acct_desc")]
        [StringLength(40)]
        public string AcctDesc { get; set; }
        [Column("employee")]
        public int? Employee { get; set; }
        [Column("entered_by")]
        public int? EnteredBy { get; set; }
        [Column("date_of_service", TypeName = "datetime")]
        public DateTime? DateOfService { get; set; }
        [Column("date_placed_precollect", TypeName = "datetime")]
        public DateTime? DatePlacedPrecollect { get; set; }
        [Column("date_placed", TypeName = "datetime")]
        public DateTime DatePlaced { get; set; }
        [Column("date_inactivated", TypeName = "datetime")]
        public DateTime? DateInactivated { get; set; }
        [Column("date_last_accessed", TypeName = "datetime")]
        public DateTime? DateLastAccessed { get; set; }
        [Column("begin_age_date", TypeName = "datetime")]
        public DateTime BeginAgeDate { get; set; }
        [Column("last_payment_amt", TypeName = "money")]
        public decimal? LastPaymentAmt { get; set; }
        [Required]
        [Column("bill_as")]
        [StringLength(1)]
        public string BillAs { get; set; }
        [Required]
        [Column("account_alert")]
        [StringLength(1)]
        public string AccountAlert { get; set; }
        [Required]
        [Column("nsf_check_on_file")]
        [StringLength(1)]
        public string NsfCheckOnFile { get; set; }
        [Required]
        [Column("wrote_nsf_check")]
        [StringLength(1)]
        public string WroteNsfCheck { get; set; }
        [Required]
        [Column("bank_acct_closed")]
        [StringLength(1)]
        public string BankAcctClosed { get; set; }
        [Column("fee_entry_date", TypeName = "datetime")]
        public DateTime? FeeEntryDate { get; set; }
        [Column("supplied_acct2")]
        [StringLength(20)]
        public string SuppliedAcct2 { get; set; }
        [Column("orig_lender_name")]
        [StringLength(40)]
        public string OrigLenderName { get; set; }
        [Column("cosigner_first_name")]
        [StringLength(20)]
        public string CosignerFirstName { get; set; }
        [Column("cosigner_last_name")]
        [StringLength(30)]
        public string CosignerLastName { get; set; }
        [Column("agency_num")]
        [StringLength(4)]
        public string AgencyNum { get; set; }
        [Column("supplied_acct3")]
        [StringLength(20)]
        public string SuppliedAcct3 { get; set; }
        [Column("supplied_acct4")]
        [StringLength(40)]
        public string SuppliedAcct4 { get; set; }
        [Column("client_rating")]
        [StringLength(20)]
        public string ClientRating { get; set; }
        [Column("agency_rating")]
        public int? AgencyRating { get; set; }
        [Column("agency_attorney_code")]
        public int? AgencyAttorneyCode { get; set; }
        [Column("insurance_type")]
        [StringLength(10)]
        public string InsuranceType { get; set; }
        [Column("out_of_statute")]
        [StringLength(1)]
        public string OutOfStatute { get; set; }
        [Column("costs_entry_date", TypeName = "datetime")]
        public DateTime? CostsEntryDate { get; set; }
        [Column("email_address")]
        [StringLength(40)]
        public string EmailAddress { get; set; }
        [Column("restrict_promo")]
        [StringLength(1)]
        public string RestrictPromo { get; set; }
        [Column("liquid_edge", TypeName = "money")]
        public decimal? LiquidEdge { get; set; }
        [Column("service_addr_same")]
        [StringLength(1)]
        public string ServiceAddrSame { get; set; }
        [Column("promo_start", TypeName = "datetime")]
        public DateTime? PromoStart { get; set; }
        [Column("collection_fees_due", TypeName = "money")]
        public decimal? CollectionFeesDue { get; set; }
        [Column("collection_fees_paid", TypeName = "money")]
        public decimal? CollectionFeesPaid { get; set; }
        [Column("collection_fees_balance", TypeName = "money")]
        public decimal? CollectionFeesBalance { get; set; }
        [Column("cfpb_complaint", TypeName = "datetime")]
        public DateTime? CfpbComplaint { get; set; }
        [Column("email_approved_date", TypeName = "datetime")]
        public DateTime? EmailApprovedDate { get; set; }
        [Column("email_approved_by")]
        public int? EmailApprovedBy { get; set; }
        [Column("email_approved")]
        [StringLength(1)]
        public string EmailApproved { get; set; }
        [Column("do_not_charge_interest")]
        [StringLength(1)]
        public string DoNotChargeInterest { get; set; }
        [Column("mail_return_set_date", TypeName = "datetime")]
        public DateTime? MailReturnSetDate { get; set; }
        [Column("orig_amount_placed", TypeName = "money")]
        public decimal? OrigAmountPlaced { get; set; }
        [Column("discharge_date", TypeName = "datetime")]
        public DateTime? DischargeDate { get; set; }
        [Column("total_charges", TypeName = "money")]
        public decimal? TotalCharges { get; set; }
        [Column("fin_class")]
        [StringLength(1)]
        public string FinClass { get; set; }
        [Column("email_opt_in")]
        [StringLength(1)]
        public string EmailOptIn { get; set; }
        [Column("email_opt_in_date", TypeName = "datetime")]
        public DateTime? EmailOptInDate { get; set; }
        [Column("email_opt_out")]
        [StringLength(1)]
        public string EmailOptOut { get; set; }
        [Column("email_opt_out_date", TypeName = "datetime")]
        public DateTime? EmailOptOutDate { get; set; }
        [Column("sif_allowed")]
        [StringLength(1)]
        public string SifAllowed { get; set; }
        [Column("charge_off_date", TypeName = "datetime")]
        public DateTime? ChargeOffDate { get; set; }
        [Column("last_payment_client", TypeName = "datetime")]
        public DateTime? LastPaymentClient { get; set; }
        [Column("last_statement_client", TypeName = "datetime")]
        public DateTime? LastStatementClient { get; set; }
        [Column("last_email_sent", TypeName = "datetime")]
        public DateTime? LastEmailSent { get; set; }
        [Column("last_text_sent", TypeName = "datetime")]
        public DateTime? LastTextSent { get; set; }
        [Column("contact_by_mail")]
        [StringLength(1)]
        public string ContactByMail { get; set; }
        [Column("contact_by_phone")]
        [StringLength(1)]
        public string ContactByPhone { get; set; }
        [Column("charity_app_sent", TypeName = "datetime")]
        public DateTime? CharityAppSent { get; set; }
        [Column("charity_decision", TypeName = "datetime")]
        public DateTime? CharityDecision { get; set; }
    }
}
