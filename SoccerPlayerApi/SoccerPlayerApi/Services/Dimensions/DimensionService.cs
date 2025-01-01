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
        var location = new Dimension { Id = 2, Value = "Location" };
        var time = new Dimension { Id = 3, Value = "Time" };
        await _context.Dimensions.AddRangeAsync(new List<Dimension>() { product, location, time });

        // Simulate data for product Levels
        var allproducts = new Level { Id = 1, Dimension = product, Value = "AllProducts" };
        var family = new Level { Id = 2, Dimension = product, Value = "family", Ancestor = allproducts };
        var productsku = new Level { Id = 3, Dimension = product, Value = "product SKU", Ancestor = family };
        await _context.Levels.AddRangeAsync(new List<Level>() { allproducts, family, productsku });

        DimensionValue smartPhone = new() { Level = productsku, Value = "smartPhone" };
        DimensionValue electronics = new() { Level = family, Value = "electronics" };
        DimensionValue home = new() { Level = family, Value = "home" };
        DimensionValue totalproducts = new() { Level = allproducts, Value = "totalproducts" };
        await _context.DimensionValues.AddRangeAsync(new List<DimensionValue>() { smartPhone, electronics, home, totalproducts });

        // Simulate data for Location levels
        var country = new Level { Id = 4, Dimension = location, Value = "country" };
        var region = new Level { Id = 5, Dimension = location, Value = "region", Ancestor = country };
        var city = new Level { Id = 6, Dimension = location, Value = "city", Ancestor = region };
        await _context.Levels.AddRangeAsync(new List<Level>() { country, region, city });

        DimensionValue paris = new() { Level = city, Value = "paris" };
        DimensionValue rhone = new() { Level = region, Value = "rhone" };
        DimensionValue france = new() { Level = country, Value = "france" };
        await _context.DimensionValues.AddRangeAsync(new List<DimensionValue>() { paris, rhone, france });

        // Simulate data for Time levels
        var year = new Level { Id = 7, Dimension = time, Value = "YEAR" };
        var month = new Level { Id = 8, Dimension = time, Value = "MONTH", Ancestor = year };
        var week = new Level { Id = 9, Dimension = time, Value = "WEEK", Ancestor = month };
        var day = new Level { Id = 10, Dimension = time, Value = "Day", Ancestor = week };
        await _context.Levels.AddRangeAsync(new List<Level>() { year, month, week, day });

        DimensionValue d1 = new() { Level = day, Value = "2024-08-07" };
        DimensionValue d2 = new() { Level = day, Value = "2024-08-09" };
        DimensionValue w1 = new() { Level = week, Value = "2024-08-W1" };
        DimensionValue m1 = new() { Level = month, Value = "2024-08" };
        DimensionValue Y2024 = new() { Level = year, Value = "2024" };
        await _context.DimensionValues.AddRangeAsync(new List<DimensionValue>() { d1, d2, w1, m1, Y2024 });

        //await _context.SaveChangesAsync();

        Fact f1 = new Fact
        {
            Type = "sales",
            Amount = 1000.00m,
            DimensionValues = new List<DimensionFact> {
                new() { DimensionValue = totalproducts },
                new() { DimensionValue = paris },
                new() { DimensionValue = d1 }
            }
        };


        Fact f2 = new Fact { Type = "sales", Amount = 1000.00m };
        DimensionFact df21 = new() { Fact = f2, DimensionValue = electronics };
        DimensionFact df22 = new() { Fact = f2, DimensionValue = france };
        DimensionFact df23 = new() { Fact = f2, DimensionValue = Y2024 };
        f2.DimensionValues = new() { df21, df22, df23 };

        Fact f3 = new Fact { Type = "sales", Amount = 1000.00m };
        DimensionFact df31 = new() { Fact = f3, DimensionValue = home };
        DimensionFact df32 = new() { Fact = f3, DimensionValue = rhone };
        DimensionFact df33 = new() { Fact = f3, DimensionValue = d2 };
        f3.DimensionValues = new() { df31, df32, df33 };

        Fact f4 = new Fact { Type = "sales", Amount = 1000.00m };
        DimensionFact df41 = new() { Fact = f4, DimensionValue = smartPhone };
        DimensionFact df42 = new() { Fact = f4, DimensionValue = paris };
        DimensionFact df43 = new() { Fact = f4, DimensionValue = w1 };
        f4.DimensionValues = new() { df41, df42, df43 };

        // Simulate data for Sales
        var salesData = new List<Fact> { f1, f2, f3, f4, };
        await _context.Sales.AddRangeAsync(salesData);

        // Output the sales data for verification
        foreach (var sale in salesData)
        {
            Console.WriteLine($"Sales: {sale.Amount}");

            foreach(DimensionFact dimension in sale.DimensionValues)
            {
                Console.WriteLine($"Dimension : {dimension.DimensionValue.Level.Dimension.Value}");
                Console.WriteLine($"{dimension.DimensionValue.Level.Value} : {dimension.DimensionValue.Value}");
            }
            Console.WriteLine("**********************************************");
        }
    }
}
