using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class FriendDto:BaseModel
    {

        public int User1_ID { get; set; }
        
        public int User2_ID { get; set; }

        public Status? StatusFriendship { get; set; }

        public UserDto User1 { get; set; }

        public UserDto User2 { get; set; }


    }
}
