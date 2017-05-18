using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFModels
{

    [Table("tblFriend")]
    public partial class Friend : BaseEntity
    {
        [Required]
        public int User1_ID { get; set; }

        [Required]
        public int User2_ID { get; set; }

        [Required]
        public Status? StatusFriendship { get; set; }

        public virtual User User1 { get; set; }

        public virtual User User2 { get; set; }
    }
}
