using Fiestivo.Core.Models;
using Fiestivo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fiestivo.Pages
{
    public class ReviewsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReviewsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Review> Reviews { get; set; }
        public void OnGet()
        {
            Reviews = _context.Reviews.ToList();
        }
    }
}
