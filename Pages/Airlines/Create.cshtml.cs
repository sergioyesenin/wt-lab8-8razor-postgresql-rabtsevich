using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages.Airlines;

public class CreateModel(AppDbContext db) : PageModel
{
    [BindProperty]
    public Airline Airline { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        // Нормализация
        Airline.Name = Airline.Name?.Trim() ?? string.Empty;
        Airline.IataCode = (Airline.IataCode ?? "").Trim().ToUpperInvariant();
        Airline.Country = Airline.Country?.Trim() ?? string.Empty;
        Airline.Description = string.IsNullOrWhiteSpace(Airline.Description) ? null : Airline.Description.Trim();

        // Бизнес-правила
        if (!string.IsNullOrEmpty(Airline.IataCode) && db.Airlines.Any(a => a.IataCode == Airline.IataCode))
            ModelState.AddModelError("Airline.IataCode", "Авиакомпания с таким кодом IATA уже существует.");

        if (Airline.FoundedYear > DateTime.Now.Year)
            ModelState.AddModelError("Airline.FoundedYear", "Год основания не может быть в будущем.");

        if (!ModelState.IsValid)
            return Page();

        Airline.Id = 0;
        Airline.IsActive = true;
        db.Airlines.Add(Airline);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
