using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EntityModelLibrary.Models
{
    public partial class DbContextForTest : DbContext
    {
        public DbContextForTest()
        {
        }

        public DbContextForTest(DbContextOptions<DbContextForTest> options)
            : base(options)
        {
        }

        public virtual DbSet<DebtorAcctInfoT> DebtorAcctInfoTs { get; set; }
        public virtual DbSet<NoteMaster> NoteMasters { get; set; }
        public virtual DbSet<PatientMaster> PatientMasters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=nv-sqltest01.aai.local;Database=collect;User Id=stephen;Password=Arizona2020!;");
            }
        }

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
