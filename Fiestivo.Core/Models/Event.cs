using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Fiestivo.Core.Models
{
    public class Event
    {
        [Key]
        public int Event_ID { get; set; }
        [Required]
        public string Event_Title { get; set; }
        
        public string Event_Discription { get; set; }
        [Required]
        public string Event_Location { get; set; }
        [Required]
        public int Event_Duration { get; set; }
        public string Event_Location_Details { get; set; }
        [Required]
        public DateTime Event_Date { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Event_time { get; set; }
        public bool IsPublic { get; set; }
        [Range(0, int.MaxValue)]
        public int Attendees_Number { get; set; }
        public byte[]? Event_Picture { get; set; }


        [BindNever]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        [ValidateNever]
        public _User User { get; set; }



        public int Category_ID { get; set; } // العلاقة مع الفئة
        [ForeignKey("Category_ID")]
        [ValidateNever]
        public Category Category { get; set; } // العلاقة مع الفئة

        public decimal CalculateAverageRating()
        {
            if (Reviews == null || !Reviews.Any())
                return 0;

            return Math.Round(Reviews.Average(r => r.Rating), 2);
        }

        public ICollection<Attend> Attends { get; set; } = new List<Attend>(); // العلاقة مع الحضور
        public ICollection<Review> Reviews { get; set; } = new List<Review>(); // العلاقة مع المراجعات

    }
}
