using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Data.DataContexts;

public class DataContextMySql : IdentityDbContext<User>
{
    public DataContextMySql(DbContextOptions<DataContextMySql> options) :
        base(options)
    {
    }


    // ---------------------------------------------------------------------- //
    //
    // um para muitos
    //
    // ---------------------------------------------------------------------- //
    public DbSet<Student> Students { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<SchoolClass> SchoolClasses { get; set; }


    // ---------------------------------------------------------------------- //
    //
    // muitos para muitos
    //
    // ---------------------------------------------------------------------- //
    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<SchoolClassCourse> SchoolClassCourses { get; set; }

    public DbSet<StudentCourse> StudentCourses { get; set; }

    public DbSet<TeacherCourse> TeacherCourses { get; set; }

    public DbSet<Genre> Genre { get; set; } = default!;


    // ---------------------------------------------------------------------- //
    //
    // OnDelete de muitos para muitos para Restrict
    //
    // ---------------------------------------------------------------------- //
    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        foreach (
            var relationship in
            modelbuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelbuilder);
    }
}