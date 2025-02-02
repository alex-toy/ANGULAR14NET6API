
-- Create time aggregations

DECLARE @year_level_id INT = (SELECT Id FROM Levels WHERE Value = 'YEAR');
DECLARE @semester_level_id INT = (SELECT Id FROM Levels WHERE Value = 'SEMESTER');
DECLARE @timester_level_id INT = (SELECT Id FROM Levels WHERE Value = 'TRIMESTER');
DECLARE @month_level_id INT = (SELECT Id FROM Levels WHERE Value = 'MONTH');
DECLARE @week_level_id INT = (SELECT Id FROM Levels WHERE Value = 'WEEK');

DECLARE @semester_ancestor_levelId INT = (SELECT AncestorId FROM Levels WHERE Value = 'SEMESTER');
DECLARE @trimester_ancestor_levelId INT = (SELECT AncestorId FROM Levels WHERE Value = 'TRIMESTER');
DECLARE @month_ancestor_levelId INT = (SELECT AncestorId FROM Levels WHERE Value = 'MONTH');
DECLARE @week_ancestor_levelId INT = (SELECT AncestorId FROM Levels WHERE Value = 'WEEK');

-- year
MERGE INTO TimeAggregations AGG
USING (
    SELECT DISTINCT CAST(_year AS VARCHAR(4)) AS _year
    FROM [VisionDb].[dbo].[DateSeries]
) DS
ON AGG.Value = DS._year AND AGG.LevelId = @year_level_id
WHEN MATCHED THEN
    UPDATE SET AGG.Value = DS._year
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LevelId, Value)
    VALUES (@year_level_id, DS._year);


-- semester
--MERGE INTO [dbo].[TimeAggregations] AGG
--USING (
--	SELECT DISTINCT _semester_label, TA.Id
--	FROM DateSeries DS
--	JOIN TimeAggregations TA on TA.Value = DS._year
--) DS ON AGG.Value = DS._semester_label AND AGG.LevelId = @semester_level_id
--WHEN MATCHED THEN
--    UPDATE SET AGG.Value = DS._semester_label, AGG.MotherAggregationId = DS.Id
--WHEN NOT MATCHED BY TARGET THEN
--    INSERT (LevelId, Value, MotherAggregationId)
--    VALUES (@semester_level_id, DS._semester_label, DS.Id);

-- trimester
MERGE INTO [dbo].[TimeAggregations] AGG
USING (
	SELECT DISTINCT _trimester_label, TA.Id
	FROM [VisionDb].[dbo].[DateSeries] DS
	JOIN TimeAggregations TA on TA.Value = DS._semester_label
) DS
ON AGG.Value = DS._trimester_label AND AGG.LevelId = @timester_level_id
WHEN MATCHED THEN
    UPDATE SET AGG.Value = DS._trimester_label, AGG.MotherAggregationId = DS.Id
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LevelId, Value, MotherAggregationId)
    VALUES (@timester_level_id, DS._trimester_label, DS.Id);

-- month
MERGE INTO [dbo].[TimeAggregations] AGG
USING (
	SELECT DISTINCT _month_label, TA.Id
	FROM [VisionDb].[dbo].[DateSeries] DS
	JOIN TimeAggregations TA on TA.Value = DS._trimester_label
) DS
ON AGG.Value = DS._month_label AND AGG.LevelId = @month_level_id
WHEN MATCHED THEN
    UPDATE SET AGG.Value = DS._month_label, AGG.MotherAggregationId = DS.Id
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LevelId, Value, MotherAggregationId)
    VALUES (@month_level_id, DS._month_label, DS.Id);

-- week
MERGE INTO [dbo].[TimeAggregations] AGG
USING (
	SELECT DISTINCT _week_label, TA.Id
	FROM [VisionDb].[dbo].[DateSeries] DS
	JOIN TimeAggregations TA on TA.Value = DS._month_label
) DS
ON AGG.Value = DS._week_label AND AGG.LevelId = @week_level_id
WHEN MATCHED THEN
    UPDATE SET AGG.Value = DS._week_label, AGG.MotherAggregationId = DS.Id
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LevelId, Value, MotherAggregationId)
    VALUES (@week_level_id, DS._week_label, DS.Id);
