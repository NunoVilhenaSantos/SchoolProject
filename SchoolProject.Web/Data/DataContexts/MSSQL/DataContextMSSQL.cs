﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.DataContexts;

// public class DataContextMsSql : IdentityDbContext<User>
/// <inheritdoc />
public class DataContextMsSql : IdentityDbContext<User, IdentityRole, string>

{
    /// <inheritdoc />
    public DataContextMsSql(DbContextOptions<DataContextMsSql> options) :
        base(options)
    {
        // Enable lazy-loading EF 6.x
        // Configuration.LazyLoadingEnabled = true;

        // Enable lazy-loading EF 7.x
        // ChangeTracker.LazyLoadingEnabled = true;
    }


    /// <inheritdoc />
    protected DataContextMsSql(DbContextOptions<DcMsSqlLocal> options)
    {
    }


    /// <inheritdoc />
    protected DataContextMsSql(DbContextOptions<DcMsSqlOnline> options)
    {
    }


    // ---------------------------------------------------------------------- //
    // tabelas auxiliares
    // ---------------------------------------------------------------------- //

    public DbSet<City> Cities { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Nationality> Nationalities { get; set; }

    public DbSet<Gender> Genders { get; set; }


    // ---------------------------------------------------------------------- //
    // tabelas principais com relações de um para muitos e de muitos para um
    // ---------------------------------------------------------------------- //

    public DbSet<Course> Courses { get; set; }

    public DbSet<SchoolClass> SchoolClasses { get; set; }

    public DbSet<Student> Students { get; set; }

    public DbSet<Teacher> Teachers { get; set; }


    // ---------------------------------------------------------------------- //
    // muitos para muitos
    // ---------------------------------------------------------------------- //

    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<SchoolClassCourse> SchoolClassCourses { get; set; }

    public DbSet<StudentCourse> StudentCourses { get; set; }

    public DbSet<TeacherCourse> TeacherCourses { get; set; }


    // ---------------------------------------------------------------------- //
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
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Verifica se a entidade possui a propriedade "IdGuid" do tipo Guid
            var idGuidProperty =
                entityType.ClrType.GetProperty("IdGuid", typeof(Guid));

            if (idGuidProperty != null)
                // Configura a propriedade "IdGuid" para ser gerada automaticamente
                // modelBuilder.Entity(entityType.ClrType)
                //     .Property("IdGuid")
                //     .ValueGeneratedOnAdd()
                //     .HasValueGeneratorFactory(
                //         typeof(SequentialGuidValueGenerator));
                // modelBuilder.Entity(entityType.ClrType)
                //     .Property("IdGuid")
                //     .ValueGeneratedOnAdd()
                //     .HasValueGenerator<SequentialGuidValueGenerator>();
                modelBuilder.Entity(entityType.ClrType)
                    .Property("IdGuid")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("(NEWSEQUENTIALID())");
        }
        // ------------------------------------------------------------------ //

        // Modify table names for all entities (excluding "AspNet" tables)
        // foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        // {
        //     var table = entityType.GetTableName();
        //     if (table.StartsWith("AspNet")) continue;
        //     entityType.SetTableName(table[0..^1]);
        // }

        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // Configure many-to-many relationship between SchoolClass and Course
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<SchoolClassCourse>()
            .HasKey(scc => new {scc.SchoolClassId, scc.CourseId});

        modelBuilder.Entity<SchoolClassCourse>()
            .HasOne(scc => scc.SchoolClass)
            .WithMany(sc => sc.SchoolClassCourses)
            .HasForeignKey(scc => scc.SchoolClassId);

        modelBuilder.Entity<SchoolClassCourse>()
            .HasOne(scc => scc.Course)
            .WithMany(c => c.SchoolClassCourses)
            .HasForeignKey(scc => scc.CourseId);

        // ------------------------------------------------------------------ //

        // modelBuilder.Entity<SchoolClassCourse>()
        //     .HasKey(scc => new {scc.SchoolClassGuidId, scc.CourseGuidId});
        //
        // modelBuilder.Entity<SchoolClassCourse>()
        //     .HasOne(scc => scc.SchoolClass)
        //     .WithMany(sc => sc.SchoolClassCourses)
        //     .HasForeignKey(scc => scc.SchoolClassGuidId);
        //
        // modelBuilder.Entity<SchoolClassCourse>()
        //     .HasOne(scc => scc.Course)
        //     .WithMany(c => c.SchoolClassCourses)
        //     .HasForeignKey(scc => scc.CourseGuidId);


        // ------------------------------------------------------------------ //
        // Configure many-to-many relationship between Teacher and Course
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<TeacherCourse>()
            .HasKey(tc => new {tc.TeacherId, tc.CourseId});

        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.Teacher)
            .WithMany(t => t.TeacherCourses)
            .HasForeignKey(tc => tc.TeacherId);

        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.Course)
            .WithMany(c => c.TeacherCourses)
            .HasForeignKey(tc => tc.CourseId);

        // ------------------------------------------------------------------ //

        // modelBuilder.Entity<TeacherCourse>()
        //     .HasKey(tc => new {tc.TeacherGuidId, tc.CourseGuidId});
        //
        // modelBuilder.Entity<TeacherCourse>()
        //     .HasOne(tc => tc.Teacher)
        //     .WithMany(t => t.TeacherCourses)
        //     .HasForeignKey(tc => tc.TeacherGuidId);
        //
        // modelBuilder.Entity<TeacherCourse>()
        //     .HasOne(tc => tc.Course)
        //     .WithMany(c => c.TeacherCourses)
        //     .HasForeignKey(tc => tc.CourseGuidId);


        // ------------------------------------------------------------------ //
        // Configure many-to-many relationship between Teacher and Course
        // ------------------------------------------------------------------ //


        // Other configurations...


        base.OnModelCreating(modelBuilder);
    }

    // ---------------------------------------------------------------------- //
}