namespace SchoolProject.Web.Data.DataContexts.MySQL;

/// <inheritdoc />
public class DcMySqlLocal : DataContextMySql
{
    /// <inheritdoc />
    public DcMySqlLocal(DbContextOptions<DcMySqlLocal> options) :
        base(options)
    {
    }
}