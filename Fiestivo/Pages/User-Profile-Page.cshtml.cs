using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Fiestivo.Data;
using System.Security.Claims;
using Fiestivo.Core.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fiestivo.Pages
{
    [Authorize]
    public class User_Profile_PageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<User_Profile_PageModel> _logger;
        public _User CurrentUser { get; set; }

        [BindProperty]
        public EditProfileModel EditProfile { get; set; }
        public List<Event> BookedEvents { get; set; } = new List<Event>();

        public User_Profile_PageModel(ApplicationDbContext context, ILogger<User_Profile_PageModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGetProfilePicture()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
                return Redirect("/img/LoginSignup-Page-imgs/default_image.png");

            var user = _context._Users.Find(int.Parse(userId));

            if (user?.ProfilePicture == null)
                return Redirect("/img/LoginSignup-Page-imgs/default_image.png");

            return File(user.ProfilePicture, "image/jpeg");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadUserData();
            return Page();
        }

        public JsonResult OnGetGuests(int eventId)
        {
            var guests = _context.Attends
                .Where(a => a.Event_ID == eventId)
                .Include(a => a.User)
                .Select(a => new {
                    fullName = a.User.Full_Name
                })
                .ToList();

            return new JsonResult(guests);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadUserData();
                return Page();
            }

            var userId = User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
            {
                var user = await _context._Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                // Always update profile info
                user.Full_Name = EditProfile.FullName;
                user.Bio = EditProfile.Bio;

                if (EditProfile.ProfileImage != null && EditProfile.ProfileImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await EditProfile.ProfileImage.CopyToAsync(memoryStream);
                        user.ProfilePicture = memoryStream.ToArray();
                    }
                }

                // Check if password fields are filled
                bool anyPasswordFieldFilled = !string.IsNullOrWhiteSpace(EditProfile.CurrentPassword)
                            || !string.IsNullOrWhiteSpace(EditProfile.NewPassword)
                            || !string.IsNullOrWhiteSpace(EditProfile.ConfirmPassword);

                if (anyPasswordFieldFilled)
                {
                    // Validate password fields
                    if (string.IsNullOrWhiteSpace(EditProfile.CurrentPassword) ||
                        string.IsNullOrWhiteSpace(EditProfile.NewPassword) ||
                        string.IsNullOrWhiteSpace(EditProfile.ConfirmPassword))
                    {
                        ModelState.AddModelError("", "If you want to change your password, all password fields must be filled.");
                        await LoadUserData();
                        return Page();
                    }

                    if (EditProfile.NewPassword != EditProfile.ConfirmPassword)
                    {
                        ModelState.AddModelError("EditProfile.ConfirmPassword", "New password and confirmation do not match.");
                        await LoadUserData();
                        return Page();
                    }

                    if (user._User_Password != EditProfile.CurrentPassword)
                    {
                        ModelState.AddModelError("EditProfile.CurrentPassword", "Current password is incorrect.");
                        await LoadUserData();
                        return Page();
                    }

                    // Update password
                    user._User_Password = EditProfile.NewPassword;
                }

                try
                {
                    // Save all changes
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/User-Profile-Page");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating profile");
                    ModelState.AddModelError("", "An error occurred while saving your profile.");
                    await LoadUserData();
                    return Page();
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostDeleteEvent(int id)
        {
            var userId = User.FindFirstValue("UserId");
            if (int.TryParse(userId, out int userIdInt))
            {
                var eventToDelete = await _context.Events
                    .FirstOrDefaultAsync(e => e.Event_ID == id && e.UserID == userIdInt);

                if (eventToDelete != null)
                {
                    _context.Events.Remove(eventToDelete);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToPage();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostEditEventAsync([FromForm] EditEventModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadUserData();
                return Page();
            }

            var userId = User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
            {
                var existingEvent = await _context.Events
                    .FirstOrDefaultAsync(e => e.Event_ID == model.Event_ID && e.UserID == id);

                if (existingEvent == null)
                {
                    return NotFound();
                }

                // Update properties
                existingEvent.Event_Title = model.Event_Title;
                existingEvent.Category_ID = model.Category_ID;
                existingEvent.Event_Date = model.Event_Date;
                existingEvent.Event_time = model.Event_time;
                existingEvent.Event_Duration = model.Event_Duration;
                existingEvent.Attendees_Number = model.Attendees_Number;
                existingEvent.Event_Location = model.Event_Location;
                existingEvent.Event_Location_Details = model.Event_Location_Details;
                existingEvent.Event_Discription = model.Event_Discription;
                existingEvent.IsPublic = model.IsPublic;

                // Handle image upload
                if (model.EventPicture != null && model.EventPicture.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.EventPicture.CopyToAsync(memoryStream);
                        existingEvent.Event_Picture = memoryStream.ToArray();
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/User-Profile-Page");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating event");
                    ModelState.AddModelError("", "An error occurred while updating the event.");
                    await LoadUserData();
                    return Page();
                }
            }
            return NotFound();
        }

        private async Task LoadUserData()
        {
            var userId = User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
            {
                CurrentUser = await _context._Users
                    .Include(u => u.Events)
                    .ThenInclude(e => e.Attends)
                    .ThenInclude(a => a.User)
                    .FirstOrDefaultAsync(u => u._User_ID == id);

                BookedEvents = await _context.Attends
                    .Where(a => a._User_ID == id)
                    .Include(a => a.Event)
                    .ThenInclude(e => e.User)
                    .Select(a => a.Event)
                    .ToListAsync();

                if (CurrentUser != null)
                {
                    EditProfile = new EditProfileModel
                    {
                        FullName = CurrentUser.Full_Name,
                        Bio = CurrentUser.Bio
                    };
                }
            }
        }
    }

    public class EditProfileModel
    {
        public string FullName { get; set; }
        public string? Bio { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public class EditEventModel
    {
        [BindRequired]
        public int Event_ID { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(100, ErrorMessage = "Event name must be less than 100 characters")]
        public string Event_Title { get; set; }

        [Required(ErrorMessage = "Event type is required")]
        public int Category_ID { get; set; }

        [Required(ErrorMessage = "Event date is required")]
        public DateTime Event_Date { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public TimeSpan Event_time { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 24, ErrorMessage = "Duration must be between 1 and 24 hours")]
        public int Event_Duration { get; set; }

        [Required(ErrorMessage = "Expected attendance is required")]
        [Range(1, 1000, ErrorMessage = "Attendance must be between 1 and 1000")]
        public int Attendees_Number { get; set; }

        [Required(ErrorMessage = "Event location is required")]
        [StringLength(200, ErrorMessage = "Location must be less than 200 characters")]
        public string Event_Location { get; set; }

        [StringLength(500, ErrorMessage = "Location details must be less than 500 characters")]
        public string Event_Location_Details { get; set; }

        [Required(ErrorMessage = "Event description is required")]
        [StringLength(2000, ErrorMessage = "Description must be less than 2000 characters")]
        public string Event_Discription { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? EventPicture { get; set; }
    }
}