
-- Create time aggregations

DECLARE @year_level_id INT = 1;
DECLARE @semester_level_id INT = 2;
DECLARE @timester_level_id INT = 3;
DECLARE @month_level_id INT = 4;
DECLARE @week_level_id INT = 5;

-- year
MERGE INTO [dbo].[Aggregations] AGG
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
MERGE INTO [dbo].[Aggregations] AGG
USING (SELECT DISTINCT _semester_label FROM [VisionDb].[dbo].[DateSeries]) DS
ON AGG.Value = DS._semester_label AND AGG.LevelId = @semester_level_id
WHEN MATCHED THEN
    UPDATE SET AGG.Value = DS._semester_label
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LevelId, Value)
    VALUES (@semester_level_id, DS._semester_label);

-- trimester
MERGE INTO [dbo].[Aggregations] AGG
USING (SELECT DISTINCT _trimester_label FROM [VisionDb].[dbo].[DateSeries]) DS
ON AGG.Value = DS._trimester_label AND AGG.LevelId = @timester_level_id
WHEN MATCHED THEN
    UPDATE SET AGG.Value = DS._trimester_label
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LevelId, Value)
    VALUES (@timester_level_id, DS._trimester_label);

-- month
MERGE INTO [dbo].[Aggregations] AGG
USING (SELECT DISTINCT _month_label FROM [VisionDb].[dbo].[DateSeries]) DS
ON AGG.Value = DS._month_label AND AGG.LevelId = @month_level_id
WHEN MATCHED THEN
    UPDATE SET AGG.Value = DS._month_label
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LevelId, Value)
    VALUES (@month_level_id, DS._month_label);

-- week
MERGE INTO [dbo].[Aggregations] AGG
USING (SELECT DISTINCT _week_label FROM [VisionDb].[dbo].[DateSeries]) DS
ON AGG.Value = DS._week_label AND AGG.LevelId = @week_level_id
WHEN MATCHED THEN
    UPDATE SET AGG.Value = DS._week_label
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LevelId, Value)
    VALUES (@week_level_id, DS._week_label);
