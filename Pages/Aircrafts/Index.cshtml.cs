using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages.Aircrafts;

public class IndexModel(AppDbContext db) : PageModel
{
    public List<Aircraft> Aircrafts { get; set; } = new();
    public string? Manufacturer { get; set; }
    public List<string> Manufacturers { get; set; } = new();

    public void OnGet(string? manufacturer)
    {
        Manufacturer = manufacturer;

        if (!string.IsNullOrWhiteSpace(manufacturer) && manufacturer.Length > 100)
            Manufacturer = manufacturer = manufacturer[..100];

        var q = db.Aircrafts.Include(a => a.Airline).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(Manufacturer))
            q = q.Where(a => a.Manufacturer == Manufacturer);

        Aircrafts = q.OrderBy(a => a.Model).ToList();
        Manufacturers = db.Aircrafts
            .Select(a => a.Manufacturer)
            .Distinct()
            .OrderBy(m => m)
            .ToList();
    }
}
