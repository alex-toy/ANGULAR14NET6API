﻿using Microsoft.EntityFrameworkCore;

namespace SoccerPlayerApi.Entities.Forecasts;

public class SimulationFact : Entity
{
    public int SimulationId { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public int TimeAggregationId { get; set; }
}
