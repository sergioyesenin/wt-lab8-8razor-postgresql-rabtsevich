using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

    public async Task OnGetAsync(
        string? search,
        int? airlineId,
        string? flightType,
        bool onlyActive = false)
    {
        Search = search;
        AirlineId = airlineId;
        FlightType = flightType;
        OnlyActive = onlyActive;

        var query = db.Flights.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();

            query = query.Where(f =>
                f.FlightNumber.ToLower().Contains(term) ||
                f.Departure.ToLower().Contains(term) ||
                f.Destination.ToLower().Contains(term));
        }

        if (airlineId.HasValue)
        {
            query = query.Where(f => f.AirlineId == airlineId.Value);
        }

        if (!string.IsNullOrWhiteSpace(flightType))
        {
            query = query.Where(f => f.FlightType == flightType);
        }

        if (onlyActive)
        {
            query = query.Where(f => f.IsActive);
        }

        // DefaultIfEmpty сохраняет рейс в результате, даже если
        // в справочнике отсутствует его авиакомпания или самолёт.
        var rows = await (
            from flight in query
            join airline in db.Airlines.AsNoTracking()
                on flight.AirlineId equals airline.Id into airlineMatches
            from airline in airlineMatches.DefaultIfEmpty()
            join aircraft in db.Aircrafts.AsNoTracking()
                on flight.AircraftId equals aircraft.Id into aircraftMatches
            from aircraft in aircraftMatches.DefaultIfEmpty()
            orderby flight.DepartureTime, flight.Id
            select new
            {
                Flight = flight,
                Airline = airline,
                Aircraft = aircraft
            }
        ).ToListAsync();

        foreach (var row in rows)
        {
            row.Flight.Airline = row.Airline;
            row.Flight.Aircraft = row.Aircraft;
            Flights.Add(row.Flight);
        }

        AirlineOptions = await db.Airlines
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .Select(a => new SelectListItem(
                a.Name,
                a.Id.ToString(),
                a.Id == airlineId))
            .ToListAsync();

        FlightTypes = await db.Flights
            .Select(f => f.FlightType)
            .Distinct()
            .OrderBy(type => type)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var flight = await db.Flights.FindAsync(id);

        if (flight is not null)
        {
            db.Flights.Remove(flight);
            await db.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}