export class AggregationDto {
    id: number;
    label: string = '';

    levelId: number;
    levelLabel: string = '';

    dimensionId: number;
    dimensionLabel: string = '';

    motherAggregationId?: number;
    motherAggregationValue?: string;

    motherLevelId?: number;
    motherLevelLabel?: string;

    constructor(
        id: number,
        label: string = '',
        levelId: number,
        levelLabel: string = '',
        dimensionId: number,
        dimensionLabel: string = '',
        motherAggregationId?: number,
        motherAggregationValue?: string,
        motherLevelId?: number,
        motherLevelLabel?: string
    ) {
        this.id = id;
        this.label = label;
        this.levelId = levelId;
        this.levelLabel = levelLabel;
        this.dimensionId = dimensionId;
        this.dimensionLabel = dimensionLabel;
        this.motherAggregationId = motherAggregationId;
        this.motherAggregationValue = motherAggregationValue;
        this.motherLevelId = motherLevelId;
        this.motherLevelLabel = motherLevelLabel;
    }
}
