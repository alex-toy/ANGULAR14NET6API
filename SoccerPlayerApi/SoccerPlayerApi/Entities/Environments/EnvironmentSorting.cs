using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Entities.Environments;

public class EnvironmentSorting : Entity
{
    public int EnvironmentId { get; set; }
    public Environment Environment { get; set; }

    public int OrderIndex { get; set; }
    public int Aggregator { get; set; } // 0 pour SUM, 1 pour AVERAGE, 3 pour Alphabetique
    public int StartTimeSpan { get; set; }
    public int EndTimeSpan { get; set; }
    public int IsAscending { get; set; }
    public int TimeSpanBase { get; set; } //0 pour history, 1 pour simulation
    public int TimeLevelId { get; set; }
    public int DataTypeId { get; set; }
    public DataType DataType { get; set; }
}
