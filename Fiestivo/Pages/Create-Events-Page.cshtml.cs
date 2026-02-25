using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fiestivo.Core.Models;
using Fiestivo.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Fiestivo.Pages
{
    [Authorize]
    public class Create_Events_PageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly List<int> _publicCategoryIds = new List<int> { 2, 3, 4, 5, 6, 7 }; // IDs من الصفحة الرئيسية

        public Create_Events_PageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IFormFile? Event_Picture { get; set; }

        [BindProperty]
        public Event Event { get; set; } = new Event();

        public List<SelectListItem> CategoriesSelectList { get; set; } = new List<SelectListItem>();

        public void OnGet(int? categoryId = null)
        {
            LoadCategories();

            if (categoryId.HasValue)
            {
                Event.Category_ID = categoryId.Value;
                Event.IsPublic = _publicCategoryIds.Contains(categoryId.Value);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoadCategories();

            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                ModelState.AddModelError("", "User not authenticated.");
                return Page();
            }

            // التحقق من أن التصنيفات الخاصة لا يمكن أن تكون Public
            if (!_publicCategoryIds.Contains(Event.Category_ID))
            {
                Event.IsPublic = false;
                ModelState.Remove("Event.IsPublic");
            }

            Event.UserID = userId;
            ModelState.Remove("Event.UserID");
            ModelState.Remove("Event.User");

            Event.Attends ??= new List<Attend>();
            Event.Reviews ??= new List<Review>();

            if (Event_Picture != null && Event_Picture.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Event_Picture.CopyToAsync(memoryStream);
                    Event.Event_Picture = memoryStream.ToArray();
                }
            }
            else
            {
                Event.Event_Picture = null;
            }

            var categoryExists = await _context.Categories
        .AnyAsync(c => c.Category_ID == Event.Category_ID);

            if (!categoryExists)
            {
                ModelState.AddModelError("Event.Category_ID", "Selected category is invalid");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _context.Events.Add(Event);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error: {ex.InnerException?.Message}");
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                return Page();
            }

            return RedirectToPage("/index");
        }

        private void LoadCategories()
        {
            CategoriesSelectList = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Category_ID.ToString(),
                    Text = c.Category_Name
                }).ToList();
        }
    }
}