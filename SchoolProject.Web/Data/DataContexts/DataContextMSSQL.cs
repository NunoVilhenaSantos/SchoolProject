using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities;


namespace SchoolProject.Web.Data.DataContexts;


public class DataContextMSSQL : IdentityDbContext<User>
{
    public DataContextMSSQL(DbContextOptions<DataContextMSSQL> options) : base(options)
    {
       
    }

    // public DbSet<Owner> Owners { get; set; }
    //
    // public DbSet<Lessee> Lessees { get; set; }


    public DbSet<Course> Courses { get; set; }

    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<SchoolClass> SchoolClasses { get; set; }

    public DbSet<Student> Students { get; set; }

    public DbSet<Teacher> Teachers { get; set; }
}