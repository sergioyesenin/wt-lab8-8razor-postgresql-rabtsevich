using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages.Airlines;

public class DetailsModel(AppDbContext db) : PageModel
{
    public Airline? Airline { get; set; }
    public List<Aircraft> Aircrafts { get; set; } = new();
    public List<Flight> Flights { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null || id <= 0)
            return BadRequest();

        Airline = await db.Airlines.FindAsync(id.Value);
        if (Airline is null)
            return NotFound();

        Aircrafts = db.Aircrafts.Where(x => x.AirlineId == id.Value).ToList();
        Flights = db.Flights.Where(x => x.AirlineId == id.Value).OrderBy(f => f.DepartureTime).ToList();
        return Page();
    }
}
