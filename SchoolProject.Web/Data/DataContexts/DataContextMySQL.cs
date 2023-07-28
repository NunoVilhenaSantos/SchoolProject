using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Data.DataContexts;

public class DataContextMySql : IdentityDbContext<User, IdentityRole, string>
{
    public DataContextMySql(DbContextOptions<DataContextMySql> options) :
        base(options)
    {
    }


    // --------------------------------------------------------------------- //
    // tabelas auxiliares
    // --------------------------------------------------------------------- //

    public DbSet<City> Cities { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Nationality> Nationalities { get; set; }

    public DbSet<Genre> Genres { get; set; }


    // --------------------------------------------------------------------- //
    // um para muitos
    // --------------------------------------------------------------------- //

    public DbSet<Course> Courses { get; set; }

    public DbSet<SchoolClass> SchoolClasses { get; set; }

    public DbSet<Student> Students { get; set; }

    public DbSet<Teacher> Teachers { get; set; }


    // --------------------------------------------------------------------- //
    // muitos para muitos
    // --------------------------------------------------------------------- //

    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<SchoolClassCourse> SchoolClassCourses { get; set; }

    public DbSet<StudentCourse> StudentCourses { get; set; }

    public DbSet<TeacherCourse> TeacherCourses { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //
        // Set DeleteBehavior to Restrict for all relationships
        //
        foreach (var relationship
                 in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        // ------------------------------------------------------------------ //


        //
        // Set ValueGeneratedOnAdd for IdGuid properties in entities
        //
        // foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        // {
        //     // Verifica se a entidade possui a propriedade "IdGuid" do tipo Guid
        //     var idGuidProperty =
        //         entityType.ClrType.GetProperty("IdGuid", typeof(Guid));
        //     if (idGuidProperty != null)
        //     {
        //         // Configura a propriedade "IdGuid" para ser gerada automaticamente
        //         modelBuilder.Entity(entityType.ClrType)
        //             .Property("IdGuid")
        //             .ValueGeneratedOnAdd();
        //     }
        // }
        // ------------------------------------------------------------------ //


        // Modify table names for all entities (excluding "AspNet" tables)
        // foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        // {
        //     var table = entityType.GetTableName();
        //     if (table.StartsWith("AspNet")) continue;
        //     entityType.SetTableName(table[0..^1]);
        // }
        // ------------------------------------------------------------------ //


        base.OnModelCreating(modelBuilder);
    }
}