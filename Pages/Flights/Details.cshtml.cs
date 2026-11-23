using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages.Flights;

public class DetailsModel(AppDbContext db) : PageModel
{
    public Flight? Flight { get; set; }
    public Airline? Airline { get; set; }
    public Aircraft? Aircraft { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Flight = await db.Flights.FindAsync(id);
        if (Flight is null) return NotFound();
        Airline = await db.Airlines.FindAsync(Flight.AirlineId);
        Aircraft = await db.Aircrafts.FindAsync(Flight.AircraftId);
        return Page();
    }
}
