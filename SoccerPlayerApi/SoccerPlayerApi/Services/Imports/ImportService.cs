using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Imports;
using System.Data;
using SoccerPlayerApi.Repo;
using System.Data.Common;

namespace SoccerPlayerApi.Services.Imports;

public class ImportService : IImportService
{
    private readonly ApplicationDbContext _context;

    public ImportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ImportErrorDto>> CreateImportFactAsync(IEnumerable<ImportFactDto> facts)
    {
        List<ImportErrorDto> errors = new();

        DataTable factsTable = GetDataTable(facts);

        using DbConnection connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using DbCommand command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "CreateImportFacts";

        SqlParameter factsParameter = new()
        {
            ParameterName = "@facts",
            SqlDbType = SqlDbType.Structured,
            TypeName = "dbo.ImportFactType",
            Value = factsTable
        };

        command.Parameters.Add(factsParameter);

        await command.ExecuteNonQueryAsync();
        using DbDataReader reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            while (await reader.ReadAsync())
            {
                ImportErrorDto error = new()
                {
                    RowNumber = reader.GetInt32(reader.GetOrdinal("RowNumber")),
                    Description = reader.GetString(reader.GetOrdinal("Description"))
                };

                errors.Add(error);
            }
        }

        return errors;
    }

    private static DataTable GetDataTable(IEnumerable<ImportFactDto> facts)
    {
        var factsTable = new DataTable();
        factsTable.Columns.Add("RowNumber", typeof(int));
        factsTable.Columns.Add("Amount", typeof(decimal));
        factsTable.Columns.Add("DataType", typeof(string));
        factsTable.Columns.Add("Dimension1Aggregation", typeof(string));
        factsTable.Columns.Add("Dimension2Aggregation", typeof(string));
        factsTable.Columns.Add("Dimension3Aggregation", typeof(string));
        factsTable.Columns.Add("Dimension4Aggregation", typeof(string));
        factsTable.Columns.Add("TimeAggregation", typeof(string));

        foreach (var fact in facts)
        {
            factsTable.Rows.Add(fact.RowNumber, fact.Amount, fact.DataType, fact.Dimension1Aggregation,
                                fact.Dimension2Aggregation, fact.Dimension3Aggregation,
                                fact.Dimension4Aggregation, fact.TimeAggregation);
        }

        return factsTable;
    }
}
