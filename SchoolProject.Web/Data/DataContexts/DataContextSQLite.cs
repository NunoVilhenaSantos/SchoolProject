using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.OtherEntities;
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


    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de cidades.
    /// </summary>
    public required DbSet<City> Cities { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de países.
    /// </summary>
    public required DbSet<Country> Countries { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de nacionalidades.
    /// </summary>
    public required DbSet<Nationality> Nationalities { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de géneros.
    /// </summary>
    public required DbSet<Gender> Genders { get; set; }


    // ---------------------------------------------------------------------- //
    // tabelas principais
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Tabela principal para armazenar os dados de escolas "Turmas".
    /// </summary>
    public required DbSet<Course> Courses { get; set; }


    /// <summary>
    ///     Tabela principal para armazenar os dados das Disciplines.
    /// </summary>
    public required DbSet<Discipline> Disciplines { get; set; }

    /// <summary>
    ///     Tabela principal para armazenar os dados dos estudantes.
    /// </summary>
    public required DbSet<Student> Students { get; set; }

    /// <summary>
    ///     Tabela principal para armazenar os dados dos professores.
    /// </summary>
    public required DbSet<Teacher> Teachers { get; set; }


    // ---------------------------------------------------------------------- //
    // muitos para muitos
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public required DbSet<CourseStudents> CoursesStudents { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public required DbSet<CourseDisciplines> CoursesDisciplines { get; set; }


    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public required DbSet<Enrollment> Enrollments { get; set; }


    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public required DbSet<StudentCourse> StudentCourses { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public required DbSet<TeacherCourse> TeacherCourses { get; set; }


    // ---------------------------------------------------------------------- //
    // criação do modelo
    // ---------------------------------------------------------------------- //


    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // ------------------------------------------------------------------ //


        //
        // Set DeleteBehavior to Restrict for all relationships
        //
        foreach (var relationship in
                 modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;


        // ------------------------------------------------------------------ //



        // ------------------------------------------------------------------ //
        //
        // Set ValueGeneratedOnAdd for IdGuid properties in entities
        //
        // FOR MSSQL
        //          .HasDefaultValueSql("(NEWSEQUENTIALID())");
        //
        // FOR MYSQL
        //         .HasDefaultValueSql("(UUID())");
        //          OR
        //         .HasColumnType("binary(16)")
        //         .ValueGeneratedOnAdd()
        //         .HasDefaultValueSql("(UUID_TO_BIN(UUID()))");
        //
        // FOR SQLITE
        //         .HasDefaultValueSql("NEWID()");
        //
        // ------------------------------------------------------------------ //
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
        // ------------------------------------------------------------------ //
        // Configure many-to-many relationship tables section
        // ------------------------------------------------------------------ //
        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // Enrollment
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.DisciplineId });


        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);


        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Discipline)
            .WithMany(d => d.Enrollments)
            .HasForeignKey(e => e.DisciplineId);


        // Configurar coluna Id como auto-incrementada sem ser chave principal
        modelBuilder.Entity<Enrollment>()
            .Property(e => e.Id)
            // Usar a extensão específica para MySQL
            .UseIdentityColumn()
            // Nome da coluna no banco de dados
            .HasColumnName("Id")
            // Tipo de dado da coluna,
            .HasColumnType("int");


        // ------------------------------------------------------------------ //

        // ------------------------------------------------------------------ //
        // Courses and Disciplines
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<CourseDisciplines>()
            .HasKey(cd => new { cd.CourseId, cd.DisciplineId });


        modelBuilder.Entity<CourseDisciplines>()
            .HasOne(cd => cd.Course)
            .WithMany(c => c.CourseDisciplines)
            .HasForeignKey(cd => cd.CourseId);


        modelBuilder.Entity<CourseDisciplines>()
            .HasOne(cd => cd.Discipline)
            .WithMany(d => d.CourseDisciplines)
            .HasForeignKey(cd => cd.DisciplineId);


        // Configurar coluna Id como auto-incrementada sem ser chave principal
        modelBuilder.Entity<CourseDisciplines>()
            .Property(cd => cd.Id)
            // Usar a extensão específica para MySQL
            .UseIdentityColumn()
            // Nome da coluna no banco de dados
            .HasColumnName("Id")
            // Tipo de dado da coluna,
            .HasColumnType("int");


        // ------------------------------------------------------------------ //

        // ------------------------------------------------------------------ //
        // Course and Students
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<CourseStudents>()
            .HasKey(cs => new { cs.CourseId, cs.StudentId });

        modelBuilder.Entity<CourseStudents>()
            .HasOne(cs => cs.Course)
            .WithMany(c => c.CourseStudents)
            .HasForeignKey(cs => cs.CourseId);

        modelBuilder.Entity<CourseStudents>()
            .HasOne(cs => cs.Student)
            .WithMany(s => s.CourseStudents)
            .HasForeignKey(cs => cs.StudentId);

        // Configurar coluna Id como auto-incrementada sem ser chave principal
        modelBuilder.Entity<CourseStudents>()
            .Property(cs => cs.Id)
            // Usar a extensão específica para MySQL
            .UseIdentityColumn()
            // Nome da coluna no banco de dados
            .HasColumnName("Id")
            // Tipo de dado da coluna,
            .HasColumnType("int");


        // ------------------------------------------------------------------ //

        // ------------------------------------------------------------------ //
        // Student and Courses
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);

        // Configurar coluna Id como auto-incrementada sem ser chave principal
        modelBuilder.Entity<StudentCourse>()
            .Property(sc => sc.Id)
            // Usar a extensão específica para MySQL
            .UseIdentityColumn()
            // Nome da coluna no banco de dados
            .HasColumnName("Id")
            // Tipo de dado da coluna,
            .HasColumnType("int");


        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // Teacher and Courses
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<TeacherCourse>()
            .HasKey(tc => new { tc.TeacherId, tc.CourseId });

        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.Teacher)
            .WithMany(t => t.TeacherCourses)
            .HasForeignKey(tc => tc.TeacherId);

        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.Course)
            .WithMany(c => c.TeacherCourses)
            .HasForeignKey(tc => tc.CourseId);

        // Configurar coluna Id como auto-incrementada sem ser chave principal
        modelBuilder.Entity<TeacherCourse>()
            .Property(tc => tc.Id)
            // Usar a extensão específica para MySQL
            .UseIdentityColumn()
            // Nome da coluna no banco de dados
            .HasColumnName("Id")
            // Tipo de dado da coluna,
            .HasColumnType("int");


        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // ... Other configurations ...
        // ... Outras configurações ...
        // ------------------------------------------------------------------ //


        // Relação entre City e Country
        modelBuilder.Entity<City>()
            .HasOne(c => c.Country)
            .WithMany(country => country.Cities)
            .HasForeignKey(c => c.CountryId);


        // Required one-to-one with primary key to primary key relationship

        // Relação entre Country e Nationality
        modelBuilder.Entity<Country>()
            .HasOne(c => c.Nationality)
            .WithOne(n => n.Country)
            .HasForeignKey<Nationality>(n => n.CountryId)
            // Configura a exclusão em cascata
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();


        // Relação entre Country e Nationality
        modelBuilder.Entity<Nationality>()
            .HasOne(n => n.Country)
            .WithOne(c => c.Nationality)
            // .HasForeignKey<Country>(c => c.NationalityId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();


        // Relação entre Country e Nationality
        // modelBuilder.Entity<Nationality>()
        //     .HasOne(n => n.Country)
        //     .WithOne(c => c.Nationality)
        //     .HasForeignKey<Country>(c => c.NationalityId)
        //     .OnDelete(DeleteBehavior.SetNull)
        //     .IsRequired();


        // ... Other configurations ...
        // ... Outras configurações ...


        // ------------------------------------------------------------------ //
        // ------------------------------------------------------------------ //



        base.OnModelCreating(modelBuilder);


    }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
}