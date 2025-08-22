using Microsoft.AspNetCore.Mvc.RazorPages;
using LinkShortener.Data;
using LinkShortener.Models;

namespace LinkShortener.Pages.Links
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Link> Links { get; set; } = new List<Link>();

        public void OnGet()
        {
            Links = _context.Links.ToList();
        }
    }
}