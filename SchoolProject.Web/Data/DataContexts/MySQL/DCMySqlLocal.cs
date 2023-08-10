namespace SchoolProject.Web.Data.DataContexts.MySQL;

public class DCMySqlLocal : DataContextMySql
{
    /// <inheritdoc />
    public DCMySqlLocal(DbContextOptions<DCMySqlLocal> options) :
        base(options: options)
    {
    }
}