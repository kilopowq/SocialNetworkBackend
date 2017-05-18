using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class MessageDto : BaseModel
    {
        public DateTime Date { get; set; }
        
        public string Text { get; set; }

        public string Location { get; set; }

        public int UserId { get; set; }

        public int ConferenceId { get; set; }
        
    }
}
