using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EFModels
{

    [Table("tblConference")]
    public partial class Conference : BaseEntity
    {
        public Conference()
        {
            Members = new HashSet<User>();
            Messages = new HashSet<Message>();
        }

        [Required]
        public string Title { get; set; }

        public string Photo { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<User> Members { get; set; }
    }
}
