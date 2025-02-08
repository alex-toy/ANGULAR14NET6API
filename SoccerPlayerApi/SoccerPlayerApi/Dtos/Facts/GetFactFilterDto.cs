using System.Linq.Expressions;

namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactFilterDto
{
    public int? DataTypeId { get; set; }

    public int Dimension1Id { get; set; }
    public int Dimension1AggregationId { get; set; }
    public int Dimension1LevelId { get; set; }

    public int? Dimension2Id { get; set; }
    public int? Dimension2AggregationId { get; set; }
    public int? Dimension2LevelId { get; set; }

    public int Dimension3Id { get; set; }
    public int? Dimension3AggregationId { get; set; }
    public int? Dimension3LevelId { get; set; }

    public int? Dimension4Id { get; set; }
    public int? Dimension4AggregationId { get; set; }
    public int? Dimension4LevelId { get; set; }


    public int TimeLevelId { get; set; }
    public int TimeAggregationId { get; set; }

    public Expression<Func<FactDto, bool>> FactIsInAggregations()
    {
        return fact => fact.Aggregation1.Id == Dimension1AggregationId ||
                       fact.Aggregation2 != null && fact.Aggregation2.Id == Dimension2AggregationId ||
                       fact.Aggregation3 != null && fact.Aggregation3.Id == Dimension3AggregationId ||
                       fact.Aggregation4 != null && fact.Aggregation4.Id == Dimension4AggregationId;
    }

    public Expression<Func<FactDto, bool>> FactIsAtLevels()
    {
        return fact => Dimension1LevelId == fact.Aggregation1.LevelId && Dimension1Id == fact.Aggregation1.DimensionId ||
                        fact.Aggregation2 != null && Dimension2LevelId == fact.Aggregation2.LevelId && Dimension2Id == fact.Aggregation2.DimensionId ||
                        fact.Aggregation3 != null && Dimension3LevelId == fact.Aggregation3.LevelId && Dimension3Id == fact.Aggregation3.DimensionId ||
                        fact.Aggregation4 != null && Dimension4LevelId == fact.Aggregation4.LevelId && Dimension4Id == fact.Aggregation4.DimensionId;
    }
}
