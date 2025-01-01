using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using System;

namespace SoccerPlayerApi.Services.Dimensions;

public class DimensionService : IDimensionService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Dimension> _dimensionRepo;
    private readonly IGenericRepo<DimensionValue> _dimensionValueRepo;
    private readonly IGenericRepo<Fact> _factRepo;

    public DimensionService(ApplicationDbContext context, IGenericRepo<Dimension> dimensionRepo, IGenericRepo<Fact> factRepo, IGenericRepo<DimensionValue> dimensionValueRepo)
    {
        _context = context;
        _dimensionRepo = dimensionRepo;
        _factRepo = factRepo;
        _dimensionValueRepo = dimensionValueRepo;
    }

    public async Task<int> CreateFactAsync(FactCreateDto fact)
    {
        var factDb = new Fact { Amount = fact.Amount, Type = fact.Type };
        int entityId = await _factRepo.CreateAsync(factDb);

        IEnumerable<DimensionFact> dimensionFacts = fact.DimensionFacts.Select(x => new DimensionFact { 
            DimensionValueId = x.DimensionValueId, 
            FactId = entityId,
        });

        factDb.DimensionFacts = dimensionFacts.ToList();

        await _context.SaveChangesAsync();
        return entityId;
    }

    public async Task<int> CreateDimensionAsync(Dimension dimension)
    {
        int entityId = await _dimensionRepo.CreateAsync(dimension);
        await _context.SaveChangesAsync();
        return entityId;
    }

    public async Task<int> CreateLevelAsync(Level level)
    {
        EntityEntry<Level> entity = await _context.Levels.AddAsync(level);
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    public async Task<int> CreateDimensionValueAsync(DimensionValue level)
    {
        EntityEntry<DimensionValue> entity = await _context.DimensionValues.AddAsync(level);
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }







    public async Task Test()
    {
        // Simulate data for Dimensions
        var product = new Dimension { Value = "Product" };
        var location = new Dimension { Value = "Location" };
        var time = new Dimension { Value = "Time" };
        await _context.Dimensions.AddRangeAsync(new List<Dimension>() { product, location, time });

        // Simulate data for product Levels
        var allproducts = new Level { Dimension = product, Value = "AllProducts" };
        var family = new Level { Dimension = product, Value = "family", Ancestor = allproducts };
        var productsku = new Level { Dimension = product, Value = "product SKU", Ancestor = family };
        await _context.Levels.AddRangeAsync(new List<Level>() { allproducts, family, productsku });

        DimensionValue smartPhone = new() { Level = productsku, Value = "smartPhone" };
        DimensionValue electronics = new() { Level = family, Value = "electronics" };
        DimensionValue home = new() { Level = family, Value = "home" };
        DimensionValue totalproducts = new() { Level = allproducts, Value = "totalproducts" };
        await _context.DimensionValues.AddRangeAsync(new List<DimensionValue>() { smartPhone, electronics, home, totalproducts });

        // Simulate data for Location levels
        var country = new Level { Dimension = location, Value = "country" };
        var region = new Level { Dimension = location, Value = "region", Ancestor = country };
        var city = new Level { Dimension = location, Value = "city", Ancestor = region };
        await _context.Levels.AddRangeAsync(new List<Level>() { country, region, city });

        DimensionValue paris = new() { Level = city, Value = "paris" };
        DimensionValue rhone = new() { Level = region, Value = "rhone" };
        DimensionValue france = new() { Level = country, Value = "france" };
        await _context.DimensionValues.AddRangeAsync(new List<DimensionValue>() { paris, rhone, france });

        // Simulate data for Time levels
        var year = new Level { Dimension = time, Value = "YEAR" };
        var month = new Level { Dimension = time, Value = "MONTH", Ancestor = year };
        var week = new Level { Dimension = time, Value = "WEEK", Ancestor = month };
        var day = new Level { Dimension = time, Value = "Day", Ancestor = week };
        await _context.Levels.AddRangeAsync(new List<Level>() { year, month, week, day });

        DimensionValue d1 = new() { Level = day, Value = "2024-08-07" };
        DimensionValue d2 = new() { Level = day, Value = "2024-08-09" };
        DimensionValue w1 = new() { Level = week, Value = "2024-08-W1" };
        DimensionValue m1 = new() { Level = month, Value = "2024-08" };
        DimensionValue Y2024 = new() { Level = year, Value = "2024" };
        await _context.DimensionValues.AddRangeAsync(new List<DimensionValue>() { d1, d2, w1, m1, Y2024 });


        Fact f1 = new Fact { Type = "sales", Amount = 8766 };
        DimensionFact df11 = new() { Fact = f1, DimensionValue = totalproducts };
        DimensionFact df12 = new() { Fact = f1, DimensionValue = paris };
        DimensionFact df13 = new() { Fact = f1, DimensionValue = d1 };
        f1.DimensionFacts = new() { df11, df12, df13 };


        Fact f2 = new Fact { Type = "sales", Amount = 7575 };
        DimensionFact df21 = new() { Fact = f2, DimensionValue = electronics };
        DimensionFact df22 = new() { Fact = f2, DimensionValue = france };
        DimensionFact df23 = new() { Fact = f2, DimensionValue = Y2024 };
        f2.DimensionFacts = new() { df21, df22, df23 };

        Fact f3 = new Fact { Type = "sales", Amount = 3449 };
        DimensionFact df31 = new() { Fact = f3, DimensionValue = home };
        DimensionFact df32 = new() { Fact = f3, DimensionValue = rhone };
        DimensionFact df33 = new() { Fact = f3, DimensionValue = d2 };
        f3.DimensionFacts = new() { df31, df32, df33 };

        Fact f4 = new Fact { Type = "sales", Amount = 4578 };
        DimensionFact df41 = new() { Fact = f4, DimensionValue = smartPhone };
        DimensionFact df42 = new() { Fact = f4, DimensionValue = paris };
        DimensionFact df43 = new() { Fact = f4, DimensionValue = w1 };
        f4.DimensionFacts = new() { df41, df42, df43 };

        // Simulate data for Sales
        var salesData = new List<Fact> { f1, f2, f3, f4, };
        await _context.Facts.AddRangeAsync(salesData);


        await _context.SaveChangesAsync();

        // Output the sales data for verification
        foreach (var sale in salesData)
        {
            Console.WriteLine($"Sales: {sale.Amount}");

            foreach(DimensionFact dimension in sale.DimensionFacts)
            {
                Console.WriteLine($"Dimension : {dimension.DimensionValue.Level.Dimension.Value}");
                Console.WriteLine($"{dimension.DimensionValue.Level.Value} : {dimension.DimensionValue.Value}");
            }
            Console.WriteLine("**********************************************");
        }
    }
}
