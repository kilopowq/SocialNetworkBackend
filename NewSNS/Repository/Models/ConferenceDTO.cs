using EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ConferenceDto : BaseModel
    {

        public string Title { get; set; }

        public string Photo { get; set; }

        public virtual ICollection<MessageDto> Messages { get; set; }

        public virtual ICollection<UserDto> Members { get; set; }


    }
}
