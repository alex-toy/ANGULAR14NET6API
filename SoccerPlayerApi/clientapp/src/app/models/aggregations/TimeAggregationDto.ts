import { TimeLevelDto } from "../levels/TimeLevelDto";

export class TimeAggregationDto {
    timeAggregationId: number;
    timeLevelId: number;
    timeLevel: TimeLevelDto;

    motherAggregationId?: number;
    motherAggregation?: TimeAggregationDto;

    label: string = '';

    constructor(
        timeAggregationId: number,
        timeLevelId: number,
        timeLevel: TimeLevelDto,
        motherAggregationId?: number,
        motherAggregation?: TimeAggregationDto,
        label: string = ''
    ) {
        this.timeAggregationId = timeAggregationId;
        this.timeLevelId = timeLevelId;
        this.timeLevel = timeLevel;
        this.motherAggregationId = motherAggregationId;
        this.motherAggregation = motherAggregation;
        this.label = label;
    }
}
