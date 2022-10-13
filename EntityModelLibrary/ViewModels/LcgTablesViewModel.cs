namespace EntityModelLibrary.ViewModels
{
    public class LcgTablesViewModel
    {
        //cardInfo
        public int IdForCardInfo { get; set; }
        public string PaymentMethodId { get; set; }
        public string EntryMode { get; set; }
        public string Type { get; set; }
        public string BinNumber { get; set; }
        public string LastFour { get; set; }
        public int? ExpirationMonth { get; set; }
        public int? ExpirationYear { get; set; }
        public string CardHolderName { get; set; }
        public string AssociateDebtorAcct { get; set; }
        public bool IsActiveForCardInfo { get; set; }
        //paymentSchedule
        public int IdForPaymentSchedule { get; set; }
        public string PatientAccount { get; set; }
        public int CardInfoId { get; set; }
        public decimal Amount { get; set; }
        public int NumberOfPayments { get; set; }
        public bool IsActiveForPaymentSchedule { get; set; }
        //paymentScheduleHistory
        public int IdForPaymentScheduleHistory { get; set; }
        public int PaymentScheduleId { get; set; }
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string AuthorizationNumber { get; set; }
        public string AuthorizationText { get; set; }


    }
}
