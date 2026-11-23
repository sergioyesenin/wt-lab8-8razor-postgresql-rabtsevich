using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab8_8razor_postgresql_rabtsevich.Data;
using wt_lab8_8razor_postgresql_rabtsevich.Models;

namespace wt_lab8_8razor_postgresql_rabtsevich.Pages.Airlines;

public class IndexModel(AppDbContext db) : PageModel
{
    public List<Airline> Airlines { get; set; } = new();
    public string? Search { get; set; }
    public string? Country { get; set; }
    public List<string> Countries { get; set; } = new();

    public Dictionary<int, int> FlightCounts { get; private set; } = new();

    public void OnGet(string? search, string? country)
    {
        Search = search;
        Country = country;

        if (!string.IsNullOrWhiteSpace(search) && search.Length > 100)
            Search = search = search[..100];

        var query = db.Airlines.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(Search))
            query = query.Where(a =>
                a.Name.ToLower().Contains(Search.ToLower()) ||
                a.IataCode.ToLower().Contains(Search.ToLower()));

        if (!string.IsNullOrWhiteSpace(Country))
            query = query.Where(a => a.Country == Country);

        Airlines = query.OrderByDescending(a => a.Rating).ToList();
        FlightCounts = db.Flights.GroupBy(f => f.AirlineId).Select(g => new { Id = g.Key, Count = g.Count() }).ToDictionary(x => x.Id, x => x.Count);
        Countries = db.Airlines.Select(a => a.Country).Distinct().OrderBy(c => c).ToList();
    }
}
