using Fiestivo.Core.Models;
using Fiestivo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fiestivo.Pages
{
    public class EventsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EventsModel (ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Event> Events { get; set; }
        public void OnGet()
        {
            Events = _context.Events.ToList();
        }
    }
}
