using Fiestivo.Core.Models;
using Fiestivo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fiestivo.Pages
{
    public class _UsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public _UsersModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<_User> _Users { get; set; }
        public void OnGet()
        {
            _Users = _context._Users.ToList();
        }
    }
}
