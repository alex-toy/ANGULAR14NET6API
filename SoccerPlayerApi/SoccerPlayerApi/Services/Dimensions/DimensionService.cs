using SoccerPlayerApi.Entities;
using SoccerPlayerApi.Entities.Dimensions;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;

namespace SoccerPlayerApi.Services.Dimensions;

public class DimensionService : IDimensionService
{
    private readonly ApplicationDbContext _context;

    public DimensionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Test()
    {
        // Simulate data for Categories
        var allproducts = new Dimension { Id = 1, Type = "Category", Level = "AllProducts" };
        var family = new Dimension { Id = 2, Level = "family", Ancestor = allproducts };
        var product = new Dimension { Id = 4, Level = "product", Ancestor = family };

        DimensionValue<Dimension> smartPhone = new() { Dimension = product, Value = "smartPhone" };
        DimensionValue<Dimension> home = new() { Dimension = family, Value = "home" };
        DimensionValue<Dimension> professional = new() { Dimension = family, Value = "professional" };
        DimensionValue<Dimension> totalproducts = new() { Dimension = allproducts, Value = "electronics" };

        // Simulate data for Locations
        var country = new Location { Id = 1, Level = "country" };
        var region = new Location { Id = 2, Level = "region", Ancestor = country };
        var city = new Location { Id = 6, Level = "city", Ancestor = region };

        LocationValue paris = new() { Dimension = city, Value = "paris" };
        LocationValue rhone = new() { Dimension = region, Value = "rhone" };
        LocationValue france = new() { Dimension = country, Value = "france" };

        // Simulate data for Time
        var year = new Time { Id = 1, Level = "YEAR" };
        var month = new Time { Id = 1, Level = "MONTH", Ancestor = year };
        var week = new Time { Id = 1, Level = "WEEK", Ancestor = month };
        var day = new Time { Id = 1, Level = "Day", Ancestor = week };

        TimeValue d1 = new() { Dimension = day, Value = "2024-08-07" };
        TimeValue d2 = new() { Dimension = day, Value = "2024-08-09" };
        TimeValue w1 = new() { Dimension = week, Value = "2024-08-W1" };
        TimeValue m1 = new() { Dimension = month, Value = "2024-08" };
        TimeValue Y2024 = new() { Dimension = year, Value = "2024" };

        // Simulate data for Sales
        var salesData = new List<Sale>
        {
            new Sale {
                SalesAmount = 1000.00m,
                Category = totalproducts,
                Location = paris,
                Time = d1,
            },
            new Sale {
                SalesAmount = 1000.00m,
                Category = smartPhone,
                Location = rhone,
                Time = m1,
            },
            new Sale {
                SalesAmount = 1000.00m,
                Category = home,
                Location = france,
                Time = Y2024,
            },
            new Sale {
                SalesAmount = 1000.00m,
                Category = professional,
                Location = rhone,
                Time = d2,
            },
            new Sale {
                SalesAmount = 1000.00m,
                Category = smartPhone,
                Location = paris,
                Time = w1,
            },
        };

        // Output the sales data for verification
        foreach (var sale in salesData)
        {
            Console.WriteLine($"Sales: {sale.SalesAmount} in {sale.Location.Value} for {sale.Category.Value} on {sale.Time.Value}");
            Console.WriteLine($"{sale.Category.Dimension.Level} - {sale.Category.Dimension.Ancestor?.Level}");
            Console.WriteLine($"{sale.Location.Dimension.Level} - {sale.Location.Dimension.Ancestor?.Level}");
            Console.WriteLine($"{sale.Time.Dimension.Level} - {sale.Time.Dimension.Ancestor?.Level}");
            Console.WriteLine("**********************************************");
        }
    }
}
