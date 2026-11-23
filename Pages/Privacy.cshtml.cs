using Microsoft.EntityFrameworkCore;
﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages;

public class PrivacyModel(AppDbContext db) : PageModel
{
    public int AirlinesCount { get; set; }
    public int AircraftsCount { get; set; }
    public int FlightsCount { get; set; }
    public int ActiveFlightsCount { get; set; }
    public decimal TotalActiveRevenue { get; set; }
    public List<Flight> UpcomingFlights { get; set; } = new();
    public List<Airline> TopAirlines { get; set; } = new();

    public void OnGet()
    {
        AirlinesCount = db.Airlines.Count();
        AircraftsCount = db.Aircrafts.Count();
        FlightsCount = db.Flights.Count();
        ActiveFlightsCount = db.Flights.Count(f => f.IsActive);
        TotalActiveRevenue = db.Flights
            .Where(f => f.IsActive)
            .Sum(f => f.TicketPrice * f.SeatsAvailable);

        UpcomingFlights = db.Flights
            .Where(f => f.IsActive)
            .OrderBy(f => f.DepartureTime)
            .Take(4)
            .ToList();

        TopAirlines = db.Airlines
            .OrderByDescending(a => a.Rating)
            .Take(3)
            .ToList();
    }
}
