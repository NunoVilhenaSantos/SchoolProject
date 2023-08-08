namespace SchoolProject.Web.Data.DataContexts.MySQL;

public class DCMySqlOnline : DataContextMySql
{
    /// <inheritdoc />
    public DCMySqlOnline(DbContextOptions<DCMySqlOnline> options) :
        base(options)
    {
    }
}