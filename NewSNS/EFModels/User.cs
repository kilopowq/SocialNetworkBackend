using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFModels
{
    [Table("tblUser")]
    public partial class User : BaseEntity
    {
        public User()
        {
            Messages = new HashSet<Message>();
            Friends1 = new HashSet<Friend>();
            Friends2 = new HashSet<Friend>();
            Conferences = new HashSet<Conference>();
        }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Info { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Avatar { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<Friend> Friends1 { get; set; }

        public virtual ICollection<Friend> Friends2 { get; set; }

        [Required]
        public State? UserState { get; set; }

        public virtual ICollection<Conference> Conferences { get; set; }
    }
}
