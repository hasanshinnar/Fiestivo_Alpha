using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiestivo.Core.Models
{
    public class _User
    {
        [Key]
        public int _User_ID { get; set; }
        public string _User_Name { get; set; }
        public string Full_Name { get; set; }
        public string User_Email { get; set; }
        public string _User_Password { get; set; }
        public string? Bio { get; set; }
        public byte[]? ProfilePicture { get; set; }


        public ICollection<Review> Reviews { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}

