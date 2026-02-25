using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiestivo.Core.Models
{
    public class Category
    {
        [Key]
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
