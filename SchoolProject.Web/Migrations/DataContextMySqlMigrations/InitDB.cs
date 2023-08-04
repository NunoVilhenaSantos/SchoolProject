#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace SchoolProject.Web.Migrations.DataContextMySqlMigrations;

/// <inheritdoc />
public partial class InitDB : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new
                {
                    Id = table.Column<string>("varchar(255)", nullable: false),
                    Name = table.Column<string>("varchar(256)", maxLength: 256,
                        nullable: true),
                    NormalizedName = table.Column<string>("varchar(256)",
                        maxLength: 256, nullable: true),
                    ConcurrencyStamp =
                        table.Column<string>("longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new
                {
                    Id = table.Column<string>("varchar(255)", nullable: false),
                    FirstName = table.Column<string>("varchar(50)",
                        maxLength: 50, nullable: false),
                    LastName = table.Column<string>("varchar(50)",
                        maxLength: 50, nullable: false),
                    Address = table.Column<string>("varchar(100)",
                        maxLength: 100, nullable: true),
                    ProfilePhotoId =
                        table.Column<Guid>("char(36)", nullable: false),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    UserName = table.Column<string>("varchar(256)",
                        maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>("varchar(256)",
                        maxLength: 256, nullable: true),
                    Email = table.Column<string>("varchar(256)", maxLength: 256,
                        nullable: true),
                    NormalizedEmail = table.Column<string>("varchar(256)",
                        maxLength: 256, nullable: true),
                    EmailConfirmed =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    PasswordHash =
                        table.Column<string>("longtext", nullable: true),
                    SecurityStamp =
                        table.Column<string>("longtext", nullable: true),
                    ConcurrencyStamp =
                        table.Column<string>("longtext", nullable: true),
                    PhoneNumber =
                        table.Column<string>("longtext", nullable: true),
                    PhoneNumberConfirmed =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    TwoFactorEnabled =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    LockoutEnd =
                        table.Column<DateTimeOffset>("datetime(6)",
                            nullable: true),
                    LockoutEnabled =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    AccessFailedCount =
                        table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId =
                        table.Column<string>("varchar(255)", nullable: false),
                    ClaimType =
                        table.Column<string>("longtext", nullable: true),
                    ClaimValue =
                        table.Column<string>("longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUserClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    ClaimType =
                        table.Column<string>("longtext", nullable: true),
                    ClaimValue =
                        table.Column<string>("longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetUserClaims_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUserLogins",
                table => new
                {
                    LoginProvider =
                        table.Column<string>("varchar(255)", nullable: false),
                    ProviderKey =
                        table.Column<string>("varchar(255)", nullable: false),
                    ProviderDisplayName =
                        table.Column<string>("longtext", nullable: true),
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins",
                        x => new {x.LoginProvider, x.ProviderKey});
                    table.ForeignKey(
                        "FK_AspNetUserLogins_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUserRoles",
                table => new
                {
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    RoleId =
                        table.Column<string>("varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles",
                        x => new {x.UserId, x.RoleId});
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "AspNetUserTokens",
                table => new
                {
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    LoginProvider =
                        table.Column<string>("varchar(255)", nullable: false),
                    Name =
                        table.Column<string>("varchar(255)", nullable: false),
                    Value = table.Column<string>("longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens",
                        x => new {x.UserId, x.LoginProvider, x.Name});
                    table.ForeignKey(
                        "FK_AspNetUserTokens_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Genres",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(20)", maxLength: 20,
                        nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                    table.ForeignKey(
                        "FK_Genres_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Genres_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Nationalities",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(50)", maxLength: 50,
                        nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                    table.ForeignKey(
                        "FK_Nationalities_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Nationalities_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "SchoolClasses",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>("varchar(7)", maxLength: 7,
                        nullable: false),
                    Acronym = table.Column<string>("longtext", nullable: false),
                    Name = table.Column<string>("longtext", nullable: false),
                    QnqLevel = table.Column<byte>("tinyint unsigned",
                        nullable: false),
                    EqfLevel = table.Column<byte>("tinyint unsigned",
                        nullable: false),
                    StartDate =
                        table.Column<DateTime>("datetime(6)", nullable: false),
                    EndDate =
                        table.Column<DateTime>("datetime(6)", nullable: false),
                    StartHour =
                        table.Column<TimeSpan>("time(6)", nullable: false),
                    EndHour =
                        table.Column<TimeSpan>("time(6)", nullable: false),
                    Location = table.Column<string>("longtext", nullable: true),
                    Type = table.Column<string>("longtext", nullable: true),
                    Area = table.Column<string>("longtext", nullable: true),
                    PriceForEmployed = table.Column<decimal>("decimal(10,2)",
                        precision: 10, scale: 2, nullable: false),
                    PriceForUnemployed = table.Column<decimal>("decimal(10,2)",
                        precision: 10, scale: 2, nullable: false),
                    ProfilePhotoId =
                        table.Column<Guid>("char(36)", nullable: true),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClasses", x => x.Id);
                    table.ForeignKey(
                        "FK_SchoolClasses_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_SchoolClasses_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Countries",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(50)", maxLength: 50,
                        nullable: false),
                    NationalityId = table.Column<int>("int", nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        "FK_Countries_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Countries_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Countries_Nationalities_NationalityId",
                        x => x.NationalityId,
                        "Nationalities",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Courses",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>("varchar(7)", maxLength: 7,
                        nullable: false),
                    Name = table.Column<string>("longtext", nullable: false),
                    Hours = table.Column<int>("int", nullable: false),
                    CreditPoints =
                        table.Column<double>("double", nullable: false),
                    ProfilePhotoId =
                        table.Column<Guid>("char(36)", nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    SchoolClassId = table.Column<int>("int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        "FK_Courses_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Courses_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Courses_SchoolClasses_SchoolClassId",
                        x => x.SchoolClassId,
                        "SchoolClasses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Cities",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>("varchar(50)", maxLength: 50,
                        nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    CountryId = table.Column<int>("int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        "FK_Cities_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Cities_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Cities_Countries_CountryId",
                        x => x.CountryId,
                        "Countries",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "SchoolClassCourses",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    SchoolClassId = table.Column<int>("int", nullable: false),
                    CourseId = table.Column<int>("int", nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClassCourses", x => x.Id);
                    table.ForeignKey(
                        "FK_SchoolClassCourses_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_SchoolClassCourses_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_SchoolClassCourses_Courses_CourseId",
                        x => x.CourseId,
                        "Courses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_SchoolClassCourses_SchoolClasses_SchoolClassId",
                        x => x.SchoolClassId,
                        "SchoolClasses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Students",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName =
                        table.Column<string>("longtext", nullable: false),
                    LastName =
                        table.Column<string>("longtext", nullable: false),
                    Address = table.Column<string>("longtext", nullable: false),
                    PostalCode =
                        table.Column<string>("longtext", nullable: false),
                    CityId = table.Column<int>("int", nullable: false),
                    CountryId = table.Column<int>("int", nullable: false),
                    MobilePhone =
                        table.Column<string>("longtext", nullable: false),
                    Email = table.Column<string>("longtext", nullable: false),
                    Active = table.Column<bool>("tinyint(1)", nullable: false),
                    GenreId = table.Column<int>("int", nullable: false),
                    DateOfBirth =
                        table.Column<DateTime>("datetime(6)", nullable: false),
                    IdentificationNumber =
                        table.Column<string>("longtext", nullable: false),
                    IdentificationType =
                        table.Column<string>("longtext", nullable: false),
                    ExpirationDateIdentificationNumber =
                        table.Column<DateTime>("datetime(6)", nullable: false),
                    TaxIdentificationNumber =
                        table.Column<string>("longtext", nullable: false),
                    CountryOfNationalityId =
                        table.Column<int>("int", nullable: false),
                    BirthplaceId = table.Column<int>("int", nullable: false),
                    EnrollDate =
                        table.Column<DateTime>("datetime(6)", nullable: false),
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    ProfilePhotoId =
                        table.Column<Guid>("char(36)", nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        "FK_Students_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Students_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Students_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Students_Cities_CityId",
                        x => x.CityId,
                        "Cities",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Students_Countries_BirthplaceId",
                        x => x.BirthplaceId,
                        "Countries",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Students_Countries_CountryId",
                        x => x.CountryId,
                        "Countries",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Students_Countries_CountryOfNationalityId",
                        x => x.CountryOfNationalityId,
                        "Countries",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Students_Genres_GenreId",
                        x => x.GenreId,
                        "Genres",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Teachers",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName =
                        table.Column<string>("longtext", nullable: false),
                    LastName =
                        table.Column<string>("longtext", nullable: false),
                    Address = table.Column<string>("longtext", nullable: false),
                    PostalCode =
                        table.Column<string>("longtext", nullable: false),
                    CityId = table.Column<int>("int", nullable: false),
                    CountryId = table.Column<int>("int", nullable: false),
                    MobilePhone =
                        table.Column<string>("longtext", nullable: false),
                    Email = table.Column<string>("longtext", nullable: false),
                    Active = table.Column<bool>("tinyint(1)", nullable: false),
                    GenreId = table.Column<int>("int", nullable: false),
                    DateOfBirth =
                        table.Column<DateTime>("datetime(6)", nullable: false),
                    IdentificationNumber =
                        table.Column<string>("longtext", nullable: false),
                    IdentificationType =
                        table.Column<string>("longtext", nullable: false),
                    ExpirationDateIdentificationNumber =
                        table.Column<DateTime>("datetime(6)", nullable: false),
                    TaxIdentificationNumber =
                        table.Column<string>("longtext", nullable: false),
                    CountryOfNationalityId =
                        table.Column<int>("int", nullable: false),
                    BirthplaceId = table.Column<int>("int", nullable: false),
                    EnrollDate =
                        table.Column<DateTime>("datetime(6)", nullable: false),
                    UserId =
                        table.Column<string>("varchar(255)", nullable: false),
                    ProfilePhotoId =
                        table.Column<Guid>("char(36)", nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        "FK_Teachers_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Teachers_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Teachers_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Teachers_Cities_CityId",
                        x => x.CityId,
                        "Cities",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Teachers_Countries_BirthplaceId",
                        x => x.BirthplaceId,
                        "Countries",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Teachers_Countries_CountryId",
                        x => x.CountryId,
                        "Countries",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Teachers_Countries_CountryOfNationalityId",
                        x => x.CountryOfNationalityId,
                        "Countries",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Teachers_Genres_GenreId",
                        x => x.GenreId,
                        "Genres",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "Enrollments",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>("int", nullable: false),
                    CourseId = table.Column<int>("int", nullable: false),
                    Grade = table.Column<decimal>("decimal(18,2)",
                        precision: 18, scale: 2, nullable: true),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    SchoolClassId = table.Column<int>("int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        "FK_Enrollments_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Enrollments_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Enrollments_Courses_CourseId",
                        x => x.CourseId,
                        "Courses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Enrollments_SchoolClasses_SchoolClassId",
                        x => x.SchoolClassId,
                        "SchoolClasses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Enrollments_Students_StudentId",
                        x => x.StudentId,
                        "Students",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "SchoolClassStudent",
                table => new
                {
                    SchoolClassesId = table.Column<int>("int", nullable: false),
                    StudentsId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClassStudent",
                        x => new {x.SchoolClassesId, x.StudentsId});
                    table.ForeignKey(
                        "FK_SchoolClassStudent_SchoolClasses_SchoolClassesId",
                        x => x.SchoolClassesId,
                        "SchoolClasses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_SchoolClassStudent_Students_StudentsId",
                        x => x.StudentsId,
                        "Students",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "StudentCourses",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>("int", nullable: false),
                    CourseId = table.Column<int>("int", nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => x.Id);
                    table.ForeignKey(
                        "FK_StudentCourses_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_StudentCourses_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_StudentCourses_Courses_CourseId",
                        x => x.CourseId,
                        "Courses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_StudentCourses_Students_StudentId",
                        x => x.StudentId,
                        "Students",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
                "TeacherCourses",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    TeacherId = table.Column<int>("int", nullable: false),
                    CourseId = table.Column<int>("int", nullable: false),
                    IdGuid = table.Column<Guid>("char(36)", nullable: false,
                        defaultValueSql: "(UUID())"),
                    WasDeleted =
                        table.Column<bool>("tinyint(1)", nullable: false),
                    CreatedAt = table
                        .Column<DateTime>("datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy",
                            MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById =
                        table.Column<string>("varchar(255)", nullable: true),
                    UpdatedAt =
                        table.Column<DateTime>("datetime(6)", nullable: true),
                    UpdatedById =
                        table.Column<string>("varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherCourses", x => x.Id);
                    table.ForeignKey(
                        "FK_TeacherCourses_AspNetUsers_CreatedById",
                        x => x.CreatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_TeacherCourses_AspNetUsers_UpdatedById",
                        x => x.UpdatedById,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_TeacherCourses_Courses_CourseId",
                        x => x.CourseId,
                        "Courses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_TeacherCourses_Teachers_TeacherId",
                        x => x.TeacherId,
                        "Teachers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateIndex(
            "IX_AspNetRoleClaims_RoleId",
            "AspNetRoleClaims",
            "RoleId");

        migrationBuilder.CreateIndex(
            "RoleNameIndex",
            "AspNetRoles",
            "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_AspNetUserClaims_UserId",
            "AspNetUserClaims",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserLogins_UserId",
            "AspNetUserLogins",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserRoles_RoleId",
            "AspNetUserRoles",
            "RoleId");

        migrationBuilder.CreateIndex(
            "EmailIndex",
            "AspNetUsers",
            "NormalizedEmail");

        migrationBuilder.CreateIndex(
            "UserNameIndex",
            "AspNetUsers",
            "NormalizedUserName",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Cities_CountryId",
            "Cities",
            "CountryId");

        migrationBuilder.CreateIndex(
            "IX_Cities_CreatedById",
            "Cities",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_Cities_UpdatedById",
            "Cities",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_Countries_CreatedById",
            "Countries",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_Countries_NationalityId",
            "Countries",
            "NationalityId");

        migrationBuilder.CreateIndex(
            "IX_Countries_UpdatedById",
            "Countries",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_Courses_CreatedById",
            "Courses",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_Courses_SchoolClassId",
            "Courses",
            "SchoolClassId");

        migrationBuilder.CreateIndex(
            "IX_Courses_UpdatedById",
            "Courses",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_Enrollments_CourseId",
            "Enrollments",
            "CourseId");

        migrationBuilder.CreateIndex(
            "IX_Enrollments_CreatedById",
            "Enrollments",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_Enrollments_SchoolClassId",
            "Enrollments",
            "SchoolClassId");

        migrationBuilder.CreateIndex(
            "IX_Enrollments_StudentId",
            "Enrollments",
            "StudentId");

        migrationBuilder.CreateIndex(
            "IX_Enrollments_UpdatedById",
            "Enrollments",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_Genres_CreatedById",
            "Genres",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_Genres_UpdatedById",
            "Genres",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_Nationalities_CreatedById",
            "Nationalities",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_Nationalities_UpdatedById",
            "Nationalities",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_SchoolClassCourses_CourseId",
            "SchoolClassCourses",
            "CourseId");

        migrationBuilder.CreateIndex(
            "IX_SchoolClassCourses_CreatedById",
            "SchoolClassCourses",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_SchoolClassCourses_SchoolClassId",
            "SchoolClassCourses",
            "SchoolClassId");

        migrationBuilder.CreateIndex(
            "IX_SchoolClassCourses_UpdatedById",
            "SchoolClassCourses",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_SchoolClasses_CreatedById",
            "SchoolClasses",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_SchoolClasses_UpdatedById",
            "SchoolClasses",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_SchoolClassStudent_StudentsId",
            "SchoolClassStudent",
            "StudentsId");

        migrationBuilder.CreateIndex(
            "IX_StudentCourses_CourseId",
            "StudentCourses",
            "CourseId");

        migrationBuilder.CreateIndex(
            "IX_StudentCourses_CreatedById",
            "StudentCourses",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_StudentCourses_StudentId",
            "StudentCourses",
            "StudentId");

        migrationBuilder.CreateIndex(
            "IX_StudentCourses_UpdatedById",
            "StudentCourses",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_Students_BirthplaceId",
            "Students",
            "BirthplaceId");

        migrationBuilder.CreateIndex(
            "IX_Students_CityId",
            "Students",
            "CityId");

        migrationBuilder.CreateIndex(
            "IX_Students_CountryId",
            "Students",
            "CountryId");

        migrationBuilder.CreateIndex(
            "IX_Students_CountryOfNationalityId",
            "Students",
            "CountryOfNationalityId");

        migrationBuilder.CreateIndex(
            "IX_Students_CreatedById",
            "Students",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_Students_GenreId",
            "Students",
            "GenreId");

        migrationBuilder.CreateIndex(
            "IX_Students_UpdatedById",
            "Students",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_Students_UserId",
            "Students",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_TeacherCourses_CourseId",
            "TeacherCourses",
            "CourseId");

        migrationBuilder.CreateIndex(
            "IX_TeacherCourses_CreatedById",
            "TeacherCourses",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_TeacherCourses_TeacherId",
            "TeacherCourses",
            "TeacherId");

        migrationBuilder.CreateIndex(
            "IX_TeacherCourses_UpdatedById",
            "TeacherCourses",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_Teachers_BirthplaceId",
            "Teachers",
            "BirthplaceId");

        migrationBuilder.CreateIndex(
            "IX_Teachers_CityId",
            "Teachers",
            "CityId");

        migrationBuilder.CreateIndex(
            "IX_Teachers_CountryId",
            "Teachers",
            "CountryId");

        migrationBuilder.CreateIndex(
            "IX_Teachers_CountryOfNationalityId",
            "Teachers",
            "CountryOfNationalityId");

        migrationBuilder.CreateIndex(
            "IX_Teachers_CreatedById",
            "Teachers",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_Teachers_GenreId",
            "Teachers",
            "GenreId");

        migrationBuilder.CreateIndex(
            "IX_Teachers_UpdatedById",
            "Teachers",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_Teachers_UserId",
            "Teachers",
            "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "AspNetRoleClaims");

        migrationBuilder.DropTable(
            "AspNetUserClaims");

        migrationBuilder.DropTable(
            "AspNetUserLogins");

        migrationBuilder.DropTable(
            "AspNetUserRoles");

        migrationBuilder.DropTable(
            "AspNetUserTokens");

        migrationBuilder.DropTable(
            "Enrollments");

        migrationBuilder.DropTable(
            "SchoolClassCourses");

        migrationBuilder.DropTable(
            "SchoolClassStudent");

        migrationBuilder.DropTable(
            "StudentCourses");

        migrationBuilder.DropTable(
            "TeacherCourses");

        migrationBuilder.DropTable(
            "AspNetRoles");

        migrationBuilder.DropTable(
            "Students");

        migrationBuilder.DropTable(
            "Courses");

        migrationBuilder.DropTable(
            "Teachers");

        migrationBuilder.DropTable(
            "SchoolClasses");

        migrationBuilder.DropTable(
            "Cities");

        migrationBuilder.DropTable(
            "Genres");

        migrationBuilder.DropTable(
            "Countries");

        migrationBuilder.DropTable(
            "Nationalities");

        migrationBuilder.DropTable(
            "AspNetUsers");
    }
}