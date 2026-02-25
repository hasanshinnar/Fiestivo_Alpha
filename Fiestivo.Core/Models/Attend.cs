using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiestivo.Core.Models
{
    public class Attend
    {
        
        public int _User_ID { get; set; }
        
        public int Event_ID { get; set; }

        public _User User { get; set; }
        public Event Event { get; set; }
    }
}
