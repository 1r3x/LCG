using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EntityModelLibrary.Models
{
    public partial class DbContextForProd : DbContext
    {
        public DbContextForProd()
        {
        }

        public DbContextForProd(DbContextOptions<DbContextForProd> options)
            : base(options)
        {
        }

        public virtual DbSet<DebtorAcctInfoT> DebtorAcctInfoTs { get; set; }
        public virtual DbSet<NoteMaster> NoteMasters { get; set; }
        public virtual DbSet<PatientMaster> PatientMasters { get; set; }
        public virtual DbSet<LcgCardInfo> LcgCardInfos { get; set; }
        public virtual DbSet<LcgPaymentScheduleHistory> LcgPaymentScheduleHistories { get; set; }
        public virtual DbSet<LcgPaymentSchedule> LcgPaymentSchedules { get; set; }
        public virtual DbSet<CcPayment> CcPayments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DebtorAcctInfoT>(entity =>
            {
                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.AccountAlert)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AcctDesc).IsUnicode(false);

                entity.Property(e => e.AcctStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AcctType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ActivityCode).IsUnicode(false);

                entity.Property(e => e.AgencyNum).IsUnicode(false);

                entity.Property(e => e.BankAcctClosed)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BillAs)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ClientRating).IsUnicode(false);

                entity.Property(e => e.ContactByMail)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ContactByPhone)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Corporate)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CosignerFirstName).IsUnicode(false);

                entity.Property(e => e.CosignerLastName).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.DoNotChargeInterest)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.EmailApproved)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.EmailOptIn)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.EmailOptOut)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FinClass)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.InsuranceType).IsUnicode(false);

                entity.Property(e => e.Legal)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.MailReturn)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.MediaOnFile)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NsfCheckOnFile)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrigLenderName).IsUnicode(false);

                entity.Property(e => e.OutOfStatute)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RestrictPromo)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ServiceAddrSame)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifAllowed)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StatusCode).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct2).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct3).IsUnicode(false);

                entity.Property(e => e.SuppliedAcct4).IsUnicode(false);

                entity.Property(e => e.WroteNsfCheck)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<NoteMaster>(entity =>
            {
                entity.HasIndex(e => new { e.DebtorAcct, e.NoteDate }, "x_acct_date")
                    .HasFillFactor((byte)90);

                entity.Property(e => e.ActionCode).IsUnicode(false);

                entity.Property(e => e.ActivityCode).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.Important)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NoteText).IsUnicode(false);
            });

            modelBuilder.Entity<PatientMaster>(entity =>
            {
                entity.HasIndex(e => e.DebtorAcct, "x_debtor_acct")
                    .IsUnique()
                    .IsClustered()
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.FirstName, "x_first_name")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.LastName, "x_last_name")
                    .HasFillFactor((byte)90);

                entity.HasIndex(e => e.Ssn, "x_ssn")
                    .HasFillFactor((byte)90);

                entity.Property(e => e.Address1).IsUnicode(false);

                entity.Property(e => e.Address2).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.MaritalStatus).IsUnicode(false);

                entity.Property(e => e.MiddleName).IsUnicode(false);

                entity.Property(e => e.Relationship).IsUnicode(false);

                entity.Property(e => e.ResidenceStatus)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('UNVERIFIED')");

                entity.Property(e => e.Sex)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Ssn)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StateCode).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);
            });
            modelBuilder.Entity<LcgCardInfo>(entity =>
            {
                entity.Property(e => e.AssociateDebtorAcct).IsUnicode(false);

                entity.Property(e => e.BinNumber).IsUnicode(false);

                entity.Property(e => e.CardHolderName).IsUnicode(false);

                entity.Property(e => e.EntryMode).IsUnicode(false);

                entity.Property(e => e.LastFour).IsUnicode(false);

                entity.Property(e => e.PaymentMethodId).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);
            });

            modelBuilder.Entity<LcgPaymentSchedule>(entity =>
            {
                entity.Property(e => e.PatientAccount).IsUnicode(false);
            });

            modelBuilder.Entity<LcgPaymentScheduleHistory>(entity =>
            {
                entity.Property(e => e.AuthorizationNumber).IsUnicode(false);

                entity.Property(e => e.AuthorizationText).IsUnicode(false);

                entity.Property(e => e.ResponseCode).IsUnicode(false);

                entity.Property(e => e.ResponseMessage).IsUnicode(false);

                entity.Property(e => e.TransactionId).IsUnicode(false);
            });

            modelBuilder.Entity<CcPayment>(entity =>
            {
                entity.Property(e => e.ApprovalCode).IsUnicode(false);

                entity.Property(e => e.ApprovalStatus).IsUnicode(false);

                entity.Property(e => e.BillingAddress1).IsUnicode(false);

                entity.Property(e => e.BillingAddress2).IsUnicode(false);

                entity.Property(e => e.BillingAreaCode).IsUnicode(false);

                entity.Property(e => e.BillingCity).IsUnicode(false);

                entity.Property(e => e.BillingName).IsUnicode(false);

                entity.Property(e => e.BillingPhone).IsUnicode(false);

                entity.Property(e => e.BillingState)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BillingZip).IsUnicode(false);

                entity.Property(e => e.CardCvv).IsUnicode(false);

                entity.Property(e => e.CardExpMonth)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CardExpYear)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CardNum).IsUnicode(false);

                entity.Property(e => e.CardType).IsUnicode(false);

                entity.Property(e => e.CbrFee).HasDefaultValueSql("((0))");

                entity.Property(e => e.Company).IsUnicode(false);

                entity.Property(e => e.DebtorAcct).IsUnicode(false);

                entity.Property(e => e.DebtorAddress1).IsUnicode(false);

                entity.Property(e => e.DebtorAddress2).IsUnicode(false);

                entity.Property(e => e.DebtorAreaCode).IsUnicode(false);

                entity.Property(e => e.DebtorCity).IsUnicode(false);

                entity.Property(e => e.DebtorName).IsUnicode(false);

                entity.Property(e => e.DebtorPhone).IsUnicode(false);

                entity.Property(e => e.DebtorState)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DebtorZip).IsUnicode(false);

                entity.Property(e => e.ErrorCode).IsUnicode(false);

                entity.Property(e => e.Hsa)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrderNumber).IsUnicode(false);

                entity.Property(e => e.RefNumber).IsUnicode(false);

                entity.Property(e => e.Sif)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.SmsDecline)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).IsUnicode(false);

                entity.Property(e => e.UserName).IsUnicode(false);

                entity.Property(e => e.VoidSale)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
