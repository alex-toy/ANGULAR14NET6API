export class ScopeFilterDto {
    Dimension1Id: number;
    Level1Id: number;

    Dimension2Id?: number;
    Level2Id?: number;

    Dimension3Id?: number;
    Level3Id?: number;

    Dimension4Id?: number;
    Level4Id?: number;

    constructor(
        Dimension1Id: number,
        Level1Id: number,
        Dimension2Id?: number,
        Level2Id?: number,
        Dimension3Id?: number,
        Level3Id?: number,
        Dimension4Id?: number,
        Level4Id?: number
    ) {
        this.Dimension1Id = Dimension1Id;
        this.Level1Id = Level1Id;
        this.Dimension2Id = Dimension2Id;
        this.Level2Id = Level2Id;
        this.Dimension3Id = Dimension3Id;
        this.Level3Id = Level3Id;
        this.Dimension4Id = Dimension4Id;
        this.Level4Id = Level4Id;
    }
}
