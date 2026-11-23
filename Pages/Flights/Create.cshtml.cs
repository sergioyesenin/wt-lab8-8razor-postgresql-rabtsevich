using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages.Flights;

public class CreateModel(AppDbContext db) : PageModel
{
    [BindProperty]
    public Flight Flight { get; set; } = new();

    public List<SelectListItem> Airlines { get; set; } = new();
    public List<SelectListItem> Aircrafts { get; set; } = new();
    public List<string> FlightTypes { get; set; } = new() { "Пассажирский", "Грузовой", "Чартерный" };

    public void OnGet()
    {
        LoadLists();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (await db.Flights.AnyAsync(f => f.FlightNumber == Flight.FlightNumber && f.Id != 0))
            ModelState.AddModelError("Flight.FlightNumber", "Этот номер рейса уже используется.");
        if (!await db.Aircrafts.AnyAsync(a => a.Id == Flight.AircraftId && a.AirlineId == Flight.AirlineId))
            ModelState.AddModelError("Flight.AircraftId", "Выберите самолёт указанной авиакомпании.");
        if (!ModelState.IsValid)
        {
            LoadLists();
            return Page();
        }

        Flight.Id = 0;
        db.Flights.Add(Flight);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }

    private void LoadLists()
    {
        Airlines = db.Airlines
            .Select(a => new SelectListItem(a.Name, a.Id.ToString()))
            .ToList();
        Aircrafts = db.Aircrafts
            .Select(a => new SelectListItem($"{a.Model} ({a.TailNumber})", a.Id.ToString()))
            .ToList();
    }
}
