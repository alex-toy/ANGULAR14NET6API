using SoccerPlayerApi.Entities;
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

    public async Task Test()
    {
        // Simulate data for Dimensions
        var product = new Dimension { Id = 1, Value = "Product" };
        var location = new Dimension { Id = 1, Value = "Location" };
        var time = new Dimension { Id = 1, Value = "Time" };
        //await _context.Dimensions.AddRangeAsync(new List<Dimension>() { product, location, time });

        // Simulate data for product Levels
        var allproducts = new Level { Id = 1, Dimension = product, Value = "AllProducts" };
        var family = new Level { Id = 2, Dimension = product, Value = "family", Ancestor = allproducts };
        var productsku = new Level { Id = 4, Dimension = product, Value = "product", Ancestor = family };

        DimensionValue smartPhone = new() { Level = productsku, Value = "smartPhone" };
        DimensionValue home = new() { Level = family, Value = "home" };
        DimensionValue professional = new() { Level = family, Value = "professional" };
        DimensionValue totalproducts = new() { Level = allproducts, Value = "electronics" };

        // Simulate data for Location levels
        var country = new Level { Id = 1, Dimension = location, Value = "country" };
        var region = new Level { Id = 2, Dimension = location, Value = "region", Ancestor = country };
        var city = new Level { Id = 6, Dimension = location, Value = "city", Ancestor = region };

        DimensionValue paris = new() { Level = city, Value = "paris" };
        DimensionValue rhone = new() { Level = region, Value = "rhone" };
        DimensionValue france = new() { Level = country, Value = "france" };

        // Simulate data for Time levels
        var year = new Level { Id = 1, Dimension = time, Value = "YEAR" };
        var month = new Level { Id = 1, Dimension = time, Value = "MONTH", Ancestor = year };
        var week = new Level { Id = 1, Dimension = time, Value = "WEEK", Ancestor = month };
        var day = new Level { Id = 1, Dimension = time, Value = "Day", Ancestor = week };

        DimensionValue d1 = new() { Level = day, Value = "2024-08-07" };
        DimensionValue d2 = new() { Level = day, Value = "2024-08-09" };
        DimensionValue w1 = new() { Level = week, Value = "2024-08-W1" };
        DimensionValue m1 = new() { Level = month, Value = "2024-08" };
        DimensionValue Y2024 = new() { Level = year, Value = "2024" };

        //await _context.SaveChangesAsync();

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
            Console.WriteLine($"{sale.Category.Level.Dimension.Value} - {sale.Category.Level.Value}");
            Console.WriteLine($"{sale.Location.Level.Dimension.Value} - {sale.Location.Level.Value}");
            Console.WriteLine($"{sale.Time.Level.Dimension.Value} - {sale.Time.Level.Value}");
            Console.WriteLine("**********************************************");
        }
    }
}
