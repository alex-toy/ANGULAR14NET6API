IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ExecutionLogs')
BEGIN
    CREATE TABLE ExecutionLogs (
        LogID INT IDENTITY(1,1) PRIMARY KEY,
        ProcedureName NVARCHAR(255),
        ParameterValues NVARCHAR(MAX) NULL,
        Debug NVARCHAR(MAX) NULL,
        StartTime DATETIME,
        EndTime DATETIME,
        Status NVARCHAR(50),
        ErrorMessage NVARCHAR(MAX) NULL
    );
END
