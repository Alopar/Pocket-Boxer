namespace Services.Database
{
    public interface IDatabaseService
    {
        AbstractTable<T> GetTable<T>(string name) where T : AbstractTableData , new();
    }
}