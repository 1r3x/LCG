using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EntityModelLibrary.Models
{
    public partial class DbContextForTestEnvironment : DbContext
    {
        public DbContextForTestEnvironment()
        {
        }

        public DbContextForTestEnvironment(DbContextOptions<DbContextForTestEnvironment> options)
            : base(options)
        {
        }

        public virtual DbSet<DebtorAcctInfoT> DebtorAcctInfoTs { get; set; }
        public virtual DbSet<NoteMaster> NoteMasters { get; set; }
        public virtual DbSet<PatientMaster> PatientMasters { get; set; }


    }
}
