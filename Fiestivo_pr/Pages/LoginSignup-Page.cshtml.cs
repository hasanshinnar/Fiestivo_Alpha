using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fiestivo.Data;
using Fiestivo.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Fiestivo.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Fiestivo.Data;





namespace Fiestivo.Pages;


public class LoginSignupModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public LoginSignupModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string FormType { get; set; } // login or signup

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Fullname { get; set; }
    [BindProperty]
    public string VerificationCode { get; set; }

    public string ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync(string Username)
    {
        

        if (FormType == "login")
        {
            var user = await _context._Users.FirstOrDefaultAsync(u => u._User_Name == Username);

            if (user == null || user._User_Password != Password)
            {
                ErrorMessage = "اسم المستخدم أو كلمة السر غلط.";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", user._User_ID);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user._User_Name),
                new Claim("UserId", user._User_ID.ToString())
            };


            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToPage("/Index");
        }
        else if (FormType == "signup")
        {
            // Check if we already sent the code and waiting for verification
            var sessionCode = HttpContext.Session.GetString(SESSION_VERIFICATION_CODE);
            var sessionSignupDataJson = HttpContext.Session.GetString(SESSION_SIGNUP_DATA);

            if (sessionCode == null || sessionSignupDataJson == null)
            {
                // Phase 1: Initial signup submission - validate inputs and send email

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "كل الحقول مطلوبة.";
                    return Page();
                }
                if (!IsValidEmail(Email))
                {
                    ErrorMessage = "البريد الإلكتروني غير صحيح";
                    return Page();
                }

                var existingUser = await _context._Users.AnyAsync(u => u._User_Name == Username || u.User_Email == Email);
                if (existingUser)
                {
                    ErrorMessage = "يوجد حساب بهذا الإيميل أو الاسم.";
                    return Page();
                }

                // Generate verification code
                var code = GenerateVerificationCode();

                // Save signup data & code in session
                var signupData = new SignupTempData
                {
                    Username = Username,
                    Email = Email,
                    Password = Password,
                    Fullname = Fullname
                };
                HttpContext.Session.SetString(SESSION_SIGNUP_DATA, System.Text.Json.JsonSerializer.Serialize(signupData));
                HttpContext.Session.SetString(SESSION_VERIFICATION_CODE, code);

                // Send email with code
                await SendVerificationEmailAsync(Email, code);

                // Show a page or flag to show verification code input
                ViewData["ShowVerification"] = true;
                ErrorMessage = "تم إرسال رمز التحقق إلى بريدك الإلكتروني. يرجى إدخاله هنا.";
                return Page();
            }
            else
            {
                // Phase 2: Verify the code input by user
                if (string.IsNullOrWhiteSpace(VerificationCode))
                {
                    ErrorMessage = "يرجى إدخال رمز التحقق.";
                    ViewData["ShowVerification"] = true;
                    return Page();
                }

                if (VerificationCode != sessionCode)
                {
                    ErrorMessage = "رمز التحقق غير صحيح.";
                    ViewData["ShowVerification"] = true;
                    return Page();
                }

                // Code valid, create user
                var signupData = System.Text.Json.JsonSerializer.Deserialize<SignupTempData>(sessionSignupDataJson);

                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/LoginSignup-Page-imgs/default_image.png");
                byte[] defaultImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);

                var newUser = new _User
                {
                    _User_Name = signupData.Username,
                    Full_Name = signupData.Fullname,
                    User_Email = signupData.Email,
                    _User_Password = signupData.Password,
                    ProfilePicture = defaultImageBytes
                };

                _context._Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Clear session
                HttpContext.Session.Remove(SESSION_VERIFICATION_CODE);
                HttpContext.Session.Remove(SESSION_SIGNUP_DATA);

                // Redirect to login or home
                return RedirectToPage("/LoginSignup-Page");
            }
        }

        return Page();
    }
    private string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // 6-digit code
    }
    private async Task SendVerificationEmailAsync(string email, string code)
    {
        // Simple SMTP example, replace with your real email sending setup
        using var client = new System.Net.Mail.SmtpClient("smtp.your-email-provider.com")
        {
            Port = 587,
            Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-email-password"),
            EnableSsl = true,
        };

        var mail = new System.Net.Mail.MailMessage();
        mail.From = new System.Net.Mail.MailAddress("no-reply@fiestivo.com");
        mail.To.Add(email);
        mail.Subject = "رمز التحقق لتأكيد البريد الإلكتروني";
        mail.Body = $"رمز التحقق الخاص بك هو: {code}";

        await client.SendMailAsync(mail);
    }
    private const string SESSION_VERIFICATION_CODE = "_VerificationCode";
    private const string SESSION_SIGNUP_DATA = "_SignupData";
    public class SignupTempData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
    }
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
