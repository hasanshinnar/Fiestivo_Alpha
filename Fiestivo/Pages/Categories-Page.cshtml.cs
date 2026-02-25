using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fiestivo.Data;
using Fiestivo.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiestivo.Pages
{
    public class Categories_PageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public string CategoryName { get; set; }
        public List<Event> Events { get; set; }

        [BindProperty(SupportsGet = true)] // Allows the property to be bound from query string
        public string SearchTerm { get; set; }

        public Categories_PageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(int? categoryId) // Make categoryId nullable
        {
            IQueryable<Event> eventsQuery = _context.Events
        .Include(e => e.Reviews)
        .Where(e => e.IsPublic);

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                // If a search term is provided, filter by event title
                eventsQuery = eventsQuery.Where(e => e.Event_Title.Contains(SearchTerm));
                CategoryName = $"Search Results for \"{SearchTerm}\""; // Update title for search results
            }
            else if (categoryId.HasValue)
            {
                // If no search term but a categoryId, filter by category
                var category = await _context.Categories.FindAsync(categoryId.Value);
                if (category != null)
                {
                    CategoryName = category.Category_Name;
                    eventsQuery = eventsQuery.Where(e => e.Category_ID == categoryId.Value);
                }
                else
                {
                    // Handle case where categoryId is provided but not found
                    CategoryName = "All Events";
                }
            }
            else
            {
                // If neither search term nor categoryId, display all public events
                CategoryName = "All Events";
            }

            Events = await eventsQuery.ToListAsync();
        }
    }
}
