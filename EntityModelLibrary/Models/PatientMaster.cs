using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EntityModelLibrary.Models
{
    [Keyless]
    [Table("patient_master")]
    public partial class PatientMaster
    {
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("ssn")]
        [StringLength(9)]
        public string Ssn { get; set; }
        [Required]
        [Column("first_name")]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }
        [Column("middle_name")]
        [StringLength(20)]
        public string MiddleName { get; set; }
        [Column("address1")]
        [StringLength(30)]
        public string Address1 { get; set; }
        [Column("address2")]
        [StringLength(30)]
        public string Address2 { get; set; }
        [Column("city")]
        [StringLength(30)]
        public string City { get; set; }
        [Column("state_code")]
        [StringLength(2)]
        public string StateCode { get; set; }
        [Column("zip")]
        [StringLength(10)]
        public string Zip { get; set; }
        [Column("birth_date", TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column("marital_status")]
        [StringLength(10)]
        public string MaritalStatus { get; set; }
        [Column("sex")]
        [StringLength(1)]
        public string Sex { get; set; }
        [Column("residence_status")]
        [StringLength(10)]
        public string ResidenceStatus { get; set; }
        [Column("relationship")]
        [StringLength(10)]
        public string Relationship { get; set; }
    }
}
