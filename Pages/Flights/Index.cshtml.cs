using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages.Flights;

public class IndexModel(AppDbContext db) : PageModel
{
    public List<Flight> Flights { get; set; } = new();
    public string? Search { get; set; }
    public int? AirlineId { get; set; }
    public string? FlightType { get; set; }
    public bool OnlyActive { get; set; }

    public List<SelectListItem> AirlineOptions { get; set; } = new();
    public List<string> FlightTypes { get; set; } = new();

    public async Task OnGetAsync(string? search, int? airlineId, string? flightType, bool onlyActive = false)
    {
        Search = search;
        AirlineId = airlineId;
        FlightType = flightType;
        OnlyActive = onlyActive;

        var q = db.Flights.Include(f => f.Airline).Include(f => f.Aircraft).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(f =>
                f.Destination.ToLower().Contains(search.ToLower()) ||
                f.Departure.ToLower().Contains(search.ToLower()) ||
                f.FlightNumber.ToLower().Contains(search.ToLower()));

        if (airlineId.HasValue)
            q = q.Where(f => f.AirlineId == airlineId.Value);

        if (!string.IsNullOrWhiteSpace(flightType))
            q = q.Where(f => f.FlightType == flightType);

        if (onlyActive)
            q = q.Where(f => f.IsActive);

        Flights = await q.OrderBy(f => f.DepartureTime).ThenBy(f => f.Id).ToListAsync();

        AirlineOptions = db.Airlines
            .Select(a => new SelectListItem(a.Name, a.Id.ToString()))
            .ToList();

        FlightTypes = db.Flights
            .Select(f => f.FlightType).Distinct().OrderBy(x => x).ToList();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var f = await db.Flights.FindAsync(id);
        if (f is not null) db.Flights.Remove(f);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
