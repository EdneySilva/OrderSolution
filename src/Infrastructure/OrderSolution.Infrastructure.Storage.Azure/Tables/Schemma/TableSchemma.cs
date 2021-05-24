
namespace OrderSolution.Infrastructure.Storage.Azure.Tables.Schemma
{
    public class TableSchemma
    {
        public TableSchemma(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; }
    }
}
