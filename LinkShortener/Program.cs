using Microsoft.EntityFrameworkCore;
using LinkShortener.Data;
using LinkShortener.Models;

namespace LinkShortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();
            app.MapGet("/{code:regex(^[a-zA-Z0-9]{{6,10}}$)}", async (string code, AppDbContext db) =>
            {
                var link = await db.Links.FirstOrDefaultAsync(l => l.ShortCode == code);
                if (link == null)
                {
                    return Results.NotFound("Short link not found.");
                }
                return Results.Redirect(link.OriginalUrl, permanent: false);
            })
            .WithName("ShortRedirect");
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
