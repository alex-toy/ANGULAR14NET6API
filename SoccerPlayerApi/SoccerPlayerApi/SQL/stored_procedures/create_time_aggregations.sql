
-- EXEC create_time_aggregations

CREATE OR ALTER PROCEDURE create_time_aggregations 
AS
BEGIN
    DECLARE @year_level_id INT = (SELECT Id FROM TimeLevels WHERE Label = 'YEAR');
    DECLARE @semester_level_id INT = (SELECT Id FROM TimeLevels WHERE Label = 'SEMESTER');
    DECLARE @timester_level_id INT = (SELECT Id FROM TimeLevels WHERE Label = 'TRIMESTER');
    DECLARE @month_level_id INT = (SELECT Id FROM TimeLevels WHERE Label = 'MONTH');
    DECLARE @week_level_id INT = (SELECT Id FROM TimeLevels WHERE Label = 'WEEK');

    -- year
    MERGE INTO TimeAggregations AGG
    USING (
        SELECT DISTINCT CAST(_year AS VARCHAR(4)) AS _year
        FROM [VisionDb].[dbo].[DateSeries]
    ) DS
    ON AGG.Label = DS._year AND AGG.TimeLevelId = @year_level_id
    WHEN MATCHED THEN
        UPDATE SET AGG.Label = DS._year
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (TimeLevelId, Label)
        VALUES (@year_level_id, DS._year);


    --semester
    MERGE INTO [dbo].[TimeAggregations] AGG
    USING (
        SELECT DISTINCT _semester_label, TA.Id
        FROM DateSeries DS
        JOIN TimeAggregations TA on TA.Label = DS._year
    ) DS ON AGG.Label = DS._semester_label AND AGG.TimeLevelId = @semester_level_id
    WHEN MATCHED THEN
        UPDATE SET AGG.Label = DS._semester_label, AGG.MotherAggregationId = DS.Id
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (TimeLevelId, Label, MotherAggregationId)
        VALUES (@semester_level_id, DS._semester_label, DS.Id);

    -- trimester
    MERGE INTO [dbo].[TimeAggregations] AGG
    USING (
        SELECT DISTINCT _trimester_label, TA.Id
        FROM [VisionDb].[dbo].[DateSeries] DS
        JOIN TimeAggregations TA on TA.Label = DS._semester_label
    ) DS
    ON AGG.Label = DS._trimester_label AND AGG.TimeLevelId = @timester_level_id
    WHEN MATCHED THEN
        UPDATE SET AGG.Label = DS._trimester_label, AGG.MotherAggregationId = DS.Id
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (TimeLevelId, Label, MotherAggregationId)
        VALUES (@timester_level_id, DS._trimester_label, DS.Id);

    -- month
    MERGE INTO [dbo].[TimeAggregations] AGG
    USING (
        SELECT DISTINCT _month_label, TA.Id
        FROM [VisionDb].[dbo].[DateSeries] DS
        JOIN TimeAggregations TA on TA.Label = DS._trimester_label
    ) DS
    ON AGG.Label = DS._month_label AND AGG.TimeLevelId = @month_level_id
    WHEN MATCHED THEN
        UPDATE SET AGG.Label = DS._month_label, AGG.MotherAggregationId = DS.Id
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (TimeLevelId, Label, MotherAggregationId)
        VALUES (@month_level_id, DS._month_label, DS.Id);

    -- week
    MERGE INTO [dbo].[TimeAggregations] AGG
    USING (
        SELECT DISTINCT _week_label, TA.Id
        FROM [VisionDb].[dbo].[DateSeries] DS
        JOIN TimeAggregations TA on TA.Label = DS._month_label
    ) DS
    ON AGG.Label = DS._week_label AND AGG.TimeLevelId = @week_level_id
    WHEN MATCHED THEN
        UPDATE SET AGG.Label = DS._week_label, AGG.MotherAggregationId = DS.Id
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (TimeLevelId, Label, MotherAggregationId)
        VALUES (@week_level_id, DS._week_label, DS.Id);

END;