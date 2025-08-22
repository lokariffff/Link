using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LinkShortener.Data;
using LinkShortener.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LinkShortener.Pages.Links
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Link Link { get; set; } = new Link();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                // ModelState.AddModelError("ShortCode", "Короткий код не сгенерирован.");
                return Page();
            }

            Link.ShortCode = GenerateShortCode(6);

            while (_context.Links.Any(l => l.ShortCode == Link.ShortCode))
            {
                Link.ShortCode = GenerateShortCode(6);
            }

            _context.Links.Add(Link);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


        private string GenerateShortCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
            }
            var sb = new StringBuilder(length);
            foreach (byte b in random)
            {
                sb.Append(chars[b % chars.Length]);
            }

            return sb.ToString();
        }

    }
}