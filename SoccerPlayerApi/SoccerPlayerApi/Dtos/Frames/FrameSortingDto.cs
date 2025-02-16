using SoccerPlayerApi.Entities.Frames;

namespace SoccerPlayerApi.Dtos.Frames;

public class FrameSortingDto
{
    public int FrameId { get; set; }
    public int OrderIndex { get; set; }
    public int Aggregator { get; set; } // 0 pour SUM, 1 pour AVERAGE, 3 pour Alphabetique
    public int StartTimeSpan { get; set; }
    public int EndTimeSpan { get; set; }
    public int IsAscending { get; set; }
    public int TimeSpanBase { get; set; } //0 pour history, 1 pour simulation
    public int DataTypeId { get; set; }
    public int TimeLevelId { get; set; }

    public FrameSorting ToDb(int frameId)
    {
        return new FrameSorting
        {
            FrameId = frameId,
            OrderIndex = OrderIndex,
            Aggregator = Aggregator,
            StartTimeSpan = StartTimeSpan,
            EndTimeSpan = EndTimeSpan,
            IsAscending = IsAscending,
            TimeSpanBase = TimeSpanBase,
            DataTypeId = DataTypeId,
            TimeLevelId = TimeLevelId,
        };
    }
}
