using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DummyWebAPI.Models
{
    public class MessageForCorrect : BaseModel
    {
        public DateTime Date { get; set; }

        public string Text { get; set; }
    }
}