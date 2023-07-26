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

public class DataContextSqLite : IdentityDbContext<User>
{
    public DataContextSqLite(
        DbContextOptions<DataContextSqLite> options) : base(options)
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


    // ---------------------------------------------------------------------- //
    // OnDelete de muitos para muitos para Restrict
    // ---------------------------------------------------------------------- //
    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (
            var relationship in
            builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(builder);
    }
}