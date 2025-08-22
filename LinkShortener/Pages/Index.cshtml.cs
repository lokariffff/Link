using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LinkShortener.Data;
using LinkShortener.Models;
using System.Security.Cryptography;
using System.Text;

namespace LinkShortener.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Link NewLink { get; set; } = new Link();

        public string? ShortUrl { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            NewLink.ShortCode = GenerateShortCode(6);
            while (_context.Links.Any(l => l.ShortCode == NewLink.ShortCode))
            {
                NewLink.ShortCode = GenerateShortCode(6);
            }
            ModelState.ClearValidationState(nameof(NewLink.ShortCode));
            ModelState.MarkFieldValid(nameof(NewLink.ShortCode));
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Links.Add(NewLink);
            await _context.SaveChangesAsync();

            ShortUrl = $"{Request.Scheme}://{Request.Host}/{NewLink.ShortCode}";
            return Page();
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