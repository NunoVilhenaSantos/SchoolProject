using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.DataContexts.MySQL;

/// <inheritdoc />
public class DataContextMySql : IdentityDbContext<User, IdentityRole, string>
{
    /// <inheritdoc />
    public DataContextMySql(DbContextOptions<DataContextMySql> options) :
        base(options)
    {
    }


    /// <inheritdoc />
    protected DataContextMySql(DbContextOptions<DCMySqlOnline> options) :
        base(options)
    {
    }


    /// <inheritdoc />
    protected DataContextMySql(DbContextOptions<DCMySqlLocal> options) :
        base(options)
    {
    }


    // ---------------------------------------------------------------------- //
    // tabelas auxiliares
    // ---------------------------------------------------------------------- //

    public DbSet<City?> Cities { get; set; }

    public DbSet<Country?> Countries { get; set; }


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


    // ---------------------------------------------------------------------- //
    // criação do modelo
    // ---------------------------------------------------------------------- //

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //
        // Set DeleteBehavior to Restrict for all relationships
        //
        foreach (var relationship in
                 modelBuilder.Model.GetEntityTypes()
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

            // Configura a propriedade "IdGuid" para ser gerada automaticamente
            if (idGuidProperty != null)
                modelBuilder.Entity(entityType.ClrType)
                    .Property("IdGuid")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("(UUID())");

            // Configura a propriedade "IdGuid" para ser do tipo binary(16)
            // e com o valor padrão UUID_TO_BIN(UUID()) e NOT NULL
            // if (idGuidProperty != null)
            //     modelBuilder.Entity(entityType.ClrType)
            //         // Use byte[] to represent 16-byte binary
            //         .Property<byte[]>("IdGuid")
            //         // Set the column type to binary(16)
            //         .HasColumnType("binary(16)")
            //         .ValueGeneratedOnAdd()
            //         .HasDefaultValueSql("(UUID_TO_BIN(UUID()))");
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

        // Configurar coluna Id como autoincrementada sem ser chave principal
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
        //     .HasOne(tc => tc.Course)
        //     .WithMany(c => c.TeacherCourses)
        //     .HasForeignKey(tc => tc.CourseGuidId);


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

        // Configurar coluna Id como autoincrementada sem ser chave principal
        modelBuilder.Entity<SchoolClassCourse>()
            .Property(scc => scc.Id)
            // Usar a extensão específica para MySQL
            .UseIdentityColumn()
            // Nome da coluna no banco de dados
            .HasColumnName("Id")
            // Tipo de dado da coluna, pode variar de acordo com suas necessidades
            .HasColumnType("int");

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

        // Configurar coluna Id como autoincrementada sem ser chave principal
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
        //     .HasOne(tc => tc.Course)
        //     .WithMany(c => c.TeacherCourses)
        //     .HasForeignKey(tc => tc.CourseGuidId);


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

        // Configurar coluna Id como autoincrementada sem ser chave principal
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
        //     .HasOne(tc => tc.Course)
        //     .WithMany(c => c.TeacherCourses)
        //     .HasForeignKey(tc => tc.CourseGuidId);

        // ------------------------------------------------------------------ //


        // Other configurations...


        base.OnModelCreating(modelBuilder);
    }

    // ---------------------------------------------------------------------- //
}