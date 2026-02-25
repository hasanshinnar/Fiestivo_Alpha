using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiestivo.Core.Models
{
    public class Review
    {
        public int _User_ID { get; set; }
        public int Event_ID { get; set; }

        [Required]
        [ForeignKey("_User_ID")]
        public _User User { get; set; }

        [Required]
        [ForeignKey("Event_ID")]
        public Event Event { get; set; }

        [Required]
        public DateTime Review_Date { get; set; } = DateTime.Now;

        [Required]
        [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters.")]
        public string Comment { get; set; }

        [Required]
        [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1 and 5.")]
        public decimal Rating { get; set; }
    }
}
