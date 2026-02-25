using Fiestivo.Core.Models;
using Fiestivo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fiestivo.Pages
{
    public class CategoriesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CategoriesModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Category> Categories { get; set; }
        public void OnGet()
        {
            Categories = _context.Categories.ToList();
        }
    }
}
