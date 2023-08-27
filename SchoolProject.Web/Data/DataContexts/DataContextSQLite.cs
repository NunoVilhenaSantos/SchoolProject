﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Data.DataContexts;

/// <inheritdoc />
public class DataContextSqLite : IdentityDbContext<User, IdentityRole, string>
{
    /// <inheritdoc />
    public DataContextSqLite(DbContextOptions<DataContextSqLite> options) :
        base(options)
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
    // um para muitos
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


    /// <inheritdoc />
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
                modelBuilder.Entity(entityType.ClrType)
                    .Property("IdGuid")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NEWID()");
        }

        // ------------------------------------------------------------------ //

        // foreach (var property in modelBuilder.Model.GetEntityTypes()
        //              .SelectMany(t => t.GetProperties())
        //              .Where(p => p.ClrType == typeof(Guid)))
        // {
        //     property.SetDefaultValueSql("NEWID()");
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


        // ------------------------------------------------------------------ //
        // Configure many-to-many relationship between Student and Course
        // via Enrollment
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new {e.StudentId, e.CourseId});

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId);

        // Configurar coluna Id como auto-incrementada sem ser chave principal
        modelBuilder.Entity<Enrollment>()
            .Property(e => e.Id)
            // Usar a extensão específica para MySQL
            .UseIdentityColumn()
            // Nome da coluna no banco de dados
            .HasColumnName("Id")
            // Tipo de dado da coluna,
            // pode variar de acordo com suas necessidades
            .HasColumnType("int");


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
        // Configure many-to-many relationship between Student and Course
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new {sc.StudentId, sc.CourseId});

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);

        // // Configurar coluna Id como auto-incrementada sem ser chave principal
        // modelBuilder.Entity<StudentCourse>()
        //     .Property<int>(sc => sc.Id)
        //     // Usar a extensão específica para MySQL
        //     .UseIdentityColumn()
        //     // Nome da coluna no banco de dados
        //     .HasColumnName("Id")
        //     // Tipo de dado da coluna, pode variar de acordo com suas necessidades
        //     .HasColumnType("int");


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
        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // Countries and Nationalities (one-to-one)
        // ------------------------------------------------------------------ //

        // Other configurations...
        // modelBuilder.Entity<Country>()
        //     // Configure Country.Nationality as principal
        //     .HasOne(c => c.Nationality)
        //     // Configure Nationality.Country as dependent
        //     .WithOne(n => n.Country)
        //     // Configure the foreign key property
        //     .HasForeignKey<Nationality>(n => n.CountryId);
        //
        // // Configure Country.Nationality
        // modelBuilder.Entity<Country>()
        //     .HasOne(c => c.Nationality)
        //     .WithOne(n => n.Country)
        //     .HasForeignKey<Nationality>(n => n.CountryId);
        //
        // // Configure Nationality.Country
        // modelBuilder.Entity<Nationality>()
        //     .HasOne(n => n.Country)
        //     .WithOne(country => country.Nationality)
        //     .HasForeignKey<Country>(c => c.Nationality);



        // ------------------------------------------------------------------ //
        // Countries and Cities (one-to-many)
        // ------------------------------------------------------------------ //

        // Other configurations...
        // modelBuilder.Entity<City>()
        //     .HasOne(c => c.Country)
        //     .WithMany(country => country.Cities)
        //     .HasForeignKey(c => c.CountryId);


        // ------------------------------------------------------------------ //
        // ------------------------------------------------------------------ //


        // ... outras configurações ...


        // ------------------------------------------------------------------ //


        base.OnModelCreating(modelBuilder);
    }

    // ---------------------------------------------------------------------- //
}