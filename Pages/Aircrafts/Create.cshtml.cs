using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages.Aircrafts;

public class CreateModel(AppDbContext db) : PageModel
{
    [BindProperty]
    public Aircraft Aircraft { get; set; } = new();

    public List<SelectListItem> Airlines { get; set; } = new();

    public void OnGet() => LoadAirlines();

    public async Task<IActionResult> OnPostAsync()
    {
        // Нормализация
        Aircraft.Model = Aircraft.Model?.Trim() ?? string.Empty;
        Aircraft.Manufacturer = Aircraft.Manufacturer?.Trim() ?? string.Empty;
        Aircraft.TailNumber = (Aircraft.TailNumber ?? "").Trim().ToUpperInvariant();

        // FK
        if (!db.Airlines.Any(a => a.Id == Aircraft.AirlineId))
            ModelState.AddModelError("Aircraft.AirlineId", "Выбранная авиакомпания не найдена.");

        // Уникальность
        if (!string.IsNullOrEmpty(Aircraft.TailNumber) &&
            db.Aircrafts.Any(a => a.TailNumber == Aircraft.TailNumber))
            ModelState.AddModelError("Aircraft.TailNumber", "Самолёт с таким бортовым номером уже есть.");

        // Год не в будущем
        if (Aircraft.ManufactureYear > DateTime.Now.Year)
            ModelState.AddModelError("Aircraft.ManufactureYear", "Год выпуска не может быть в будущем.");

        if (!ModelState.IsValid)
        {
            LoadAirlines();
            return Page();
        }

        Aircraft.Id = 0;
        db.Aircrafts.Add(Aircraft);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }

    private void LoadAirlines()
    {
        Airlines = db.Airlines
            .Select(a => new SelectListItem(a.Name, a.Id.ToString()))
            .ToList();
    }
}
