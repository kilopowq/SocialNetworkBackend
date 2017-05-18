using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFModels
{
    [Table("tblMessage")]
    public partial class Message : BaseEntity
    {

        public Message()
        {
            Comments = new HashSet<Message>();
        }

        [Required]
        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }

        [Required]
        public string Text { get; set; }

        public string Location { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int ConferenceId { get; set; }

        public virtual Conference Conference { get; set; }

        public virtual ICollection<Message> Comments { get; set; }
    }
}
