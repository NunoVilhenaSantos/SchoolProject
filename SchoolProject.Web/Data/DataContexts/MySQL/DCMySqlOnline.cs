namespace SchoolProject.Web.Data.DataContexts;

public class DCMySqlOnline : DataContextMySql
{
    /// <inheritdoc />
    public DCMySqlOnline(DbContextOptions<DCMySqlOnline> options) :
        base(options)
    {
    }
}