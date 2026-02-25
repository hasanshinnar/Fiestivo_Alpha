using Fiestivo.Core.Models;
using Fiestivo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fiestivo.Pages
{
    public class AttendsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public AttendsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Attend> Attends { get; set; }
        public void OnGet()
        {
            Attends = _context.Attends.ToList();
        }
    }
}
