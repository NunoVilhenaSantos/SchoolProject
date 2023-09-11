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

namespace SchoolProject.Web.Data.DataContexts.MSSQL;

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



    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de cidades.
    /// </summary>
    public DbSet<City> Cities { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de países.
    /// </summary>
    public DbSet<Country> Countries { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de nacionalidades.
    /// </summary>
    public DbSet<Nationality> Nationalities { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de gêneros.
    /// </summary>
    public DbSet<Gender> Genders { get; set; }


    // ---------------------------------------------------------------------- //
    // tabelas principais
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Tabela principal para armazenar os dados das Disciplines.
    /// </summary>
    public DbSet<Discipline> Disciplines { get; set; }

    /// <summary>
    ///     Tabela principal para armazenar os dados de escolas "Turmas".
    /// </summary>
    public DbSet<Course> Courses { get; set; }

    /// <summary>
    ///     Tabela principal para armazenar os dados dos estudantes.
    /// </summary>
    public DbSet<Student> Students { get; set; }

    /// <summary>
    ///     Tabela principal para armazenar os dados dos professores.
    /// </summary>
    public DbSet<Teacher> Teachers { get; set; }




    // ---------------------------------------------------------------------- //
    // muitos para muitos
    // ---------------------------------------------------------------------- //



    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public DbSet<Enrollment> Enrollments { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public DbSet<CourseStudents> CoursesStudents { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public DbSet<CourseDisciplines> CoursesDisciplines { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public DbSet<StudentCourse> StudentCourses { get; set; }

    /// <summary>
    ///     Tabela auxiliar para armazenar os dados de matrículas.
    /// </summary>
    public DbSet<TeacherCourse> TeacherCourses { get; set; }


    // ---------------------------------------------------------------------- //
    // criação do modelo
    // ---------------------------------------------------------------------- //


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
        // Configure many-to-many relationship between Student and Discipline
        // via Enrollment
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new {e.StudentId, e.DisciplineId});

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Discipline)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.DisciplineId);

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

        // modelBuilder.Entity<TeacherCourse>()
        //     .HasKey(tc => new {tc.TeacherGuidId, tc.CourseGuidId});
        //
        // modelBuilder.Entity<TeacherCourse>()
        //     .HasOne(tc => tc.Teacher)
        //     .WithMany(t => t.TeacherCourses)
        //     .HasForeignKey(tc => tc.TeacherGuidId);
        //
        // modelBuilder.Entity<TeacherCourse>()
        //     .HasOne(tc => tc.Discipline)
        //     .WithMany(c => c.TeacherCourses)
        //     .HasForeignKey(tc => tc.CourseGuidId);


        // ------------------------------------------------------------------ //
        // Configure many-to-many relationship between Courses and Disciplines
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<CourseDisciplines>()
            .HasKey(cd => new {cd.CourseId, cd.DisciplineId});


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
            // pode variar de acordo com suas necessidades
            .HasColumnType("int");

        // ------------------------------------------------------------------ //

        // modelBuilder.Entity<SchoolClassCourse>()
        //     .HasKey(scc => new {scc.SchoolClassGuidId, scc.CourseGuidId});
        //
        // modelBuilder.Entity<SchoolClassCourse>()
        //     .HasOne(scc => scc.Discipline)
        //     .WithMany(sc => sc.CourseDisciplines)
        //     .HasForeignKey(scc => scc.SchoolClassGuidId);
        //
        // modelBuilder.Entity<SchoolClassCourse>()
        //     .HasOne(scc => scc.Discipline)
        //     .WithMany(c => c.CourseDisciplines)
        //     .HasForeignKey(scc => scc.CourseGuidId);


        // ------------------------------------------------------------------ //
        // Configure many-to-many relationship between Student and Discipline
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new {sc.StudentId, sc.CourseId});


        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId);


        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(d => d.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);


        // Configurar coluna Id como auto-incrementada sem ser chave principal
        modelBuilder.Entity<StudentCourse>()
            .Property(sc => sc.Id)
            // Usar a extensão específica para MySQL
            .UseIdentityColumn()
            // Nome da coluna no banco de dados
            .HasColumnName("Id")
            // Tipo de dado da coluna, pode variar de acordo com suas necessidades
            .HasColumnType("int");


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
        //     .HasOne(tc => tc.Discipline)
        //     .WithMany(c => c.TeacherCourses)
        //     .HasForeignKey(tc => tc.CourseGuidId);


        // ------------------------------------------------------------------ //
        // Configure many-to-many relationship between Teacher and Discipline
        // ------------------------------------------------------------------ //

        modelBuilder.Entity<TeacherCourse>()
            .HasKey(tc => new {tc.TeacherId, tc.CourseId});


        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.Teacher)
            .WithMany(t => t.TeacherCourses)
            .HasForeignKey(tc => tc.TeacherId);


        modelBuilder.Entity<TeacherCourse>()
            .HasOne(tc => tc.Course)
            .WithMany(d => d.TeacherCourses)
            .HasForeignKey(tc => tc.CourseId);


        // Configurar coluna Id como auto-incrementada sem ser chave principal
        modelBuilder.Entity<TeacherCourse>()
            .Property(tc => tc.Id)
            // Usar a extensão específica para MySQL
            .UseIdentityColumn()
            // Nome da coluna no banco de dados
            .HasColumnName("Id")
            // Tipo de dado da coluna, pode variar de acordo com suas necessidades
            .HasColumnType("int");


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
        //     .HasOne(tc => tc.Discipline)
        //     .WithMany(c => c.TeacherCourses)
        //     .HasForeignKey(tc => tc.CourseGuidId);


        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // ------------------------------------------------------------------ //
        // ------------------------------------------------------------------ //


        // ... Other configurations ...
        // ... Outras configurações ...


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
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();


        // Relação entre Country e Nationality
        // modelBuilder.Entity<Nationality>()
        //     .HasOne(n => n.Country)
        //     .WithOne(c => c.Nationality)
        //     .HasForeignKey<Country>(c => c.NationalityId)
        //     .OnDelete(DeleteBehavior.SetNull)
        //     .IsRequired();

        // Relação entre Country e Nationality
        // modelBuilder.Entity<Nationality>()
        //     .HasOne(n => n.Country)
        //     .WithOne(c => c.Nationality)
        //     .HasForeignKey<Country>(c => c.NationalityId)
        //     .OnDelete(DeleteBehavior.Restrict)
        //     .IsRequired();


        // ... Other configurations ...
        // ... Outras configurações ...


        // ------------------------------------------------------------------ //
        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //


        //
        // Set DeleteBehavior to Restrict for all relationships
        //
        foreach (var relationship in
                 modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;


        // ------------------------------------------------------------------ //

        base.OnModelCreating(modelBuilder);
    }

    // ---------------------------------------------------------------------- //
}