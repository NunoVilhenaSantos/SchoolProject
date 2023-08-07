namespace SchoolProject.Web.Data.DataContexts;

public class DCMySqlLocal : DataContextMySql
{
    /// <inheritdoc />
    public DCMySqlLocal(DbContextOptions<DCMySqlLocal> options) :
        base(options)
    {
    }
}