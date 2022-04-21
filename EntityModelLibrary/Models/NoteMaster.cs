using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EntityModelLibrary.Models
{
    //[Keyless]
    [Table("note_master")]
    public partial class NoteMaster
    {
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("note_date", TypeName = "datetime")]
        [Key]//experimental
        public DateTime NoteDate { get; set; }
        [Column("employee")]
        public int Employee { get; set; }
        [Required]
        [Column("activity_code")]
        [StringLength(3)]
        public string ActivityCode { get; set; }
        [Required]
        [Column("note_text")]
        [StringLength(255)]
        public string NoteText { get; set; }
        [Column("important")]
        [StringLength(1)]
        public string Important { get; set; }
        [Column("action_code")]
        [StringLength(3)]
        public string ActionCode { get; set; }
    }
}
