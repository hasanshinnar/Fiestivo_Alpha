using Fiestivo.Data;
using Fiestivo.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Fiestivo.Pages
{
    public class Event_View_PageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Event Event { get; set; }
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public bool IsBooked { get; set; }
        public bool IsCreator { get; set; }

        [BindProperty]
        public Review NewReview { get; set; } = new Review { Rating = 0 };

        public Event_View_PageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            return await LoadEventData(id);
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // 1. التحقق من تسجيل الدخول بشكل صحيح
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            // 2. الحصول على UserId بشكل صحيح
            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToPage("/Login");
            }

            // 3. التحقق من صحة الـ UserId
            if (!int.TryParse(userIdString, out int userId))
            {
                ModelState.AddModelError("", "Invalid user ID");
                return await LoadEventData(id);
            }

            // 4. التحقق من صحة التقييم
            if (NewReview.Rating < 1 || NewReview.Rating > 5)
            {
                ModelState.AddModelError("NewReview.Rating", "Please select a rating between 1 and 5 stars");
                return await LoadEventData(id);
            }

            // 5. التحقق من وجود تقييم سابق
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r._User_ID == userId && r.Event_ID == id);

            if (existingReview != null)
            {
                ModelState.AddModelError("", "You have already reviewed this event.");
                return await LoadEventData(id);
            }

            // 6. إنشاء التقييم الجديد
            NewReview._User_ID = userId;
            NewReview.Event_ID = id;
            NewReview.Review_Date = DateTime.Now;

            _context.Reviews.Add(NewReview);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostBookEventAsync(int id)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToPage("/Login");

            var existing = await _context.Attends
                .FirstOrDefaultAsync(a => a._User_ID == int.Parse(userId) && a.Event_ID == id);

            if (existing == null)
            {
                _context.Attends.Add(new Attend
                {
                    _User_ID = int.Parse(userId),
                    Event_ID = id
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostCancelBookingAsync(int id)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToPage("/Login");

            var attend = await _context.Attends
                .FirstOrDefaultAsync(a => a._User_ID == int.Parse(userId) && a.Event_ID == id);

            if (attend != null)
            {
                _context.Attends.Remove(attend);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id });
        }


        public async Task<IActionResult> OnPostDeleteEventAsync(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete == null) return NotFound();

            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId) || eventToDelete.UserID != int.Parse(userId))
                return Forbid();

            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage("/User-Profile-Page");
        }

        private async Task<IActionResult> LoadEventData(int id)
        {
            // 7. تضمين التقييمات والبيانات المرتبطة
            Event = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.User)
                .Include(e => e.Reviews)
                    .ThenInclude(r => r.User)
                .Include(e => e.Attends)
                .FirstOrDefaultAsync(e => e.Event_ID == id);

            if (Event == null)
            {
                return NotFound();
            }

            // 8. تحديث حالة الحجز ومالك الحدث
            var userIdString = User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int currentUserId))
            {
                IsBooked = Event.Attends.Any(a => a._User_ID == currentUserId);
                IsCreator = Event.UserID == currentUserId;
            }

            // 9. حساب التقييمات
            AverageRating = Event.CalculateAverageRating();
            ReviewCount = Event.Reviews?.Count ?? 0;

            return Page();
        }
    }
}