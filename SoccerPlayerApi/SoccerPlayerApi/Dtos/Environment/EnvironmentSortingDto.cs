using SoccerPlayerApi.Entities.Environments;

namespace SoccerPlayerApi.Dtos.Environment;

public class EnvironmentSortingDto
{
    public int EnvironmentId { get; set; }
    public int Aggregator { get; set; } // 0 pour SUM, 1 pour AVERAGE, 3 pour Alphabetique
    public int StartTimeSpan { get; set; }
    public int EndTimeSpan { get; set; }
    public int IsAscending { get; set; }
    public int TimeSpanBase { get; set; } //0 pour history, 1 pour simulation
    public int DataTypeId { get; set; }

    public EnvironmentSorting ToDb(int environmentId)
    {
        return new EnvironmentSorting
        {
            EnvironmentId = environmentId, 
            Aggregator = Aggregator, 
            StartTimeSpan = StartTimeSpan,      
            EndTimeSpan = EndTimeSpan, 
            IsAscending = IsAscending, 
            TimeSpanBase = TimeSpanBase,
            DataTypeId = DataTypeId
        };
    }
}
