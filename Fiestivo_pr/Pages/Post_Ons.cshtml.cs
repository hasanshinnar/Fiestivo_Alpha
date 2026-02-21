using Fiestivo.Core.Models;
using Fiestivo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fiestivo.Pages
{
    public class Post_OnsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Post_OnsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Post_On> Post_Ons { get; set; }
        public void OnGet()
        {
            Post_Ons = _context.Post_Ons.ToList();
        }
    }
}
