-- EXEC generate_date_series '2022-01-01', '2026-12-31';

CREATE OR ALTER PROCEDURE generate_date_series 
    @start_date DATE,
    @end_date DATE
AS
BEGIN
    -- Declare a variable to store the current date in the loop
    DECLARE @current_date DATE;

    -- Initialize the current date with the start date
    SET @current_date = @start_date;

    -- Loop through each date from start_date to end_date
    WHILE @current_date <= @end_date
    BEGIN
        -- Merge the current date and corresponding aggregations into the DateSeries table
        MERGE INTO DateSeries AS target
        USING (
            SELECT 
                @current_date AS _day,
                DATEPART(WEEK, @current_date) AS _week,
                DATEPART(MONTH, @current_date) AS _month,
                CASE
                    WHEN DATEPART(MONTH, @current_date) IN (1, 2, 3) THEN 1
                    WHEN DATEPART(MONTH, @current_date) IN (4, 5, 6) THEN 2
                    WHEN DATEPART(MONTH, @current_date) IN (7, 8, 9) THEN 3
                    WHEN DATEPART(MONTH, @current_date) IN (10, 11, 12) THEN 4
                END AS _trimester,
                CASE
                    WHEN DATEPART(MONTH, @current_date) IN (1, 2, 3, 4, 5, 6) THEN 1
                    WHEN DATEPART(MONTH, @current_date) IN (7, 8, 9, 10, 11, 12) THEN 2
                END AS _semester,
                DATEPART(YEAR, @current_date) AS _year,
                CONCAT(DATEPART(YEAR, @current_date), '-W', RIGHT('00' + CAST(DATEPART(WEEK, @current_date) AS VARCHAR(2)), 2)) AS _week_label,
                CONCAT(DATEPART(YEAR, @current_date), '-M', RIGHT('00' + CAST(DATEPART(MONTH, @current_date) AS VARCHAR(2)), 2)) AS _month_label,
                CONCAT(DATEPART(YEAR, @current_date), '-T', CAST(CASE
                    WHEN DATEPART(MONTH, @current_date) IN (1, 2, 3) THEN 1
                    WHEN DATEPART(MONTH, @current_date) IN (4, 5, 6) THEN 2
                    WHEN DATEPART(MONTH, @current_date) IN (7, 8, 9) THEN 3
                    WHEN DATEPART(MONTH, @current_date) IN (10, 11, 12) THEN 4
                END AS VARCHAR(1))) AS _trimester_label,
                CONCAT(DATEPART(YEAR, @current_date), '-S', CAST(CASE
                    WHEN DATEPART(MONTH, @current_date) IN (1, 2, 3, 4, 5, 6) THEN 1
                    WHEN DATEPART(MONTH, @current_date) IN (7, 8, 9, 10, 11, 12) THEN 2
                END AS VARCHAR(1))) AS _semester_label
        ) AS source
        ON target._day = source._day -- Match based on the date
        WHEN MATCHED THEN
            UPDATE SET
                target._week = source._week,
                target._month = source._month,
                target._trimester = source._trimester,
                target._semester = source._semester,
                target._year = source._year,
                target._week_label = source._week_label,
                target._month_label = source._month_label,
                target._trimester_label = source._trimester_label,
                target._semester_label = source._semester_label
        WHEN NOT MATCHED THEN
            INSERT (
                _day, _week, _month, _trimester, _semester, _year,
                _week_label, _month_label, _trimester_label, _semester_label
            )
            VALUES (
                source._day, source._week, source._month, source._trimester, source._semester, source._year,
                source._week_label, source._month_label, source._trimester_label, source._semester_label
            );

        -- Increment the current date by 1 day
        SET @current_date = DATEADD(DAY, 1, @current_date);
    END
END;
