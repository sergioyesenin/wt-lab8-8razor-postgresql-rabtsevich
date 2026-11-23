using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages.Flights;

public class EditModel(AppDbContext db) : PageModel
{
    [BindProperty]
    public Flight Flight { get; set; } = new();

    public List<SelectListItem> Airlines { get; set; } = new();
    public List<SelectListItem> Aircrafts { get; set; } = new();
    public List<string> FlightTypes { get; set; } = new() { "Пассажирский", "Грузовой", "Чартерный" };

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var existing = await db.Flights.FindAsync(id);
        if (existing is null) return NotFound();

        Flight = new Flight
        {
            Id = existing.Id,
            FlightNumber = existing.FlightNumber,
            Departure = existing.Departure,
            Destination = existing.Destination,
            DepartureTime = existing.DepartureTime,
            TicketPrice = existing.TicketPrice,
            SeatsAvailable = existing.SeatsAvailable,
            IsActive = existing.IsActive,
            AirlineId = existing.AirlineId,
            AircraftId = existing.AircraftId,
            FlightType = existing.FlightType,
            Description = existing.Description,
        };
        LoadLists();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (id != Flight.Id) return BadRequest();
        if (await db.Flights.AnyAsync(f => f.FlightNumber == Flight.FlightNumber && f.Id != id))
            ModelState.AddModelError("Flight.FlightNumber", "Этот номер рейса уже используется.");
        if (!await db.Aircrafts.AnyAsync(a => a.Id == Flight.AircraftId && a.AirlineId == Flight.AirlineId))
            ModelState.AddModelError("Flight.AircraftId", "Выберите самолёт указанной авиакомпании.");
        if (!ModelState.IsValid)
        {
            LoadLists();
            return Page();
        }

        var existing = await db.Flights.FindAsync(id);
        if (existing is null) return NotFound();

        existing.FlightNumber = Flight.FlightNumber;
        existing.Departure = Flight.Departure;
        existing.Destination = Flight.Destination;
        existing.DepartureTime = Flight.DepartureTime;
        existing.TicketPrice = Flight.TicketPrice;
        existing.SeatsAvailable = Flight.SeatsAvailable;
        existing.IsActive = Flight.IsActive;
        existing.AirlineId = Flight.AirlineId;
        existing.AircraftId = Flight.AircraftId;
        existing.FlightType = Flight.FlightType;
        existing.Description = Flight.Description;

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
