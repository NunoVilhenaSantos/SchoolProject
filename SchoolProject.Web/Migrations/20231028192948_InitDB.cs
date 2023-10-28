using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SchoolProject.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ProfilePhotoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ProfilePhotoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Countries_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false),
                    Acronym = table.Column<string>(type: "longtext", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    QnqLevel = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    EqfLevel = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartHour = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    EndHour = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    Location = table.Column<string>(type: "longtext", nullable: true),
                    Type = table.Column<string>(type: "longtext", nullable: true),
                    Area = table.Column<string>(type: "longtext", nullable: true),
                    PriceForEmployed = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PriceForUnemployed = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ProfilePhotoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Disciplines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: false),
                    CreditPoints = table.Column<double>(type: "double", nullable: false),
                    ProfilePhotoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disciplines_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disciplines_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ProfilePhotoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genders_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Genders_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ProfilePhotoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cities_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nationalities_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nationalities_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nationalities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseDisciplines",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    DisciplineId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDisciplines", x => new { x.CourseId, x.DisciplineId });
                    table.ForeignKey(
                        name: "FK_CourseDisciplines_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseDisciplines_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseDisciplines_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseDisciplines_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    LastName = table.Column<string>(type: "longtext", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false),
                    PostalCode = table.Column<string>(type: "longtext", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    MobilePhone = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdentificationNumber = table.Column<string>(type: "longtext", nullable: false),
                    IdentificationType = table.Column<string>(type: "longtext", nullable: false),
                    ExpirationDateIdentificationNumber = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TaxIdentificationNumber = table.Column<string>(type: "longtext", nullable: false),
                    CountryOfNationalityId = table.Column<int>(type: "int", nullable: false),
                    BirthplaceId = table.Column<int>(type: "int", nullable: false),
                    EnrollDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProfilePhotoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Countries_BirthplaceId",
                        column: x => x.BirthplaceId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Countries_CountryOfNationalityId",
                        column: x => x.CountryOfNationalityId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    LastName = table.Column<string>(type: "longtext", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false),
                    PostalCode = table.Column<string>(type: "longtext", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    MobilePhone = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdentificationNumber = table.Column<string>(type: "longtext", nullable: false),
                    IdentificationType = table.Column<string>(type: "longtext", nullable: false),
                    ExpirationDateIdentificationNumber = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TaxIdentificationNumber = table.Column<string>(type: "longtext", nullable: false),
                    CountryOfNationalityId = table.Column<int>(type: "int", nullable: false),
                    BirthplaceId = table.Column<int>(type: "int", nullable: false),
                    EnrollDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProfilePhotoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_Countries_BirthplaceId",
                        column: x => x.BirthplaceId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_Countries_CountryOfNationalityId",
                        column: x => x.CountryOfNationalityId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseStudents_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseStudents_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseStudents_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    DisciplineId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Absences = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => new { x.StudentId, x.DisciplineId });
                    table.ForeignKey(
                        name: "FK_Enrollments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentDisciplines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    DisciplineId = table.Column<int>(type: "int", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDisciplines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentDisciplines_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentDisciplines_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentDisciplines_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentDisciplines_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherDisciplines",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    DisciplineId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdGuid = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    WasDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherDisciplines", x => new { x.TeacherId, x.DisciplineId });
                    table.ForeignKey(
                        name: "FK_TeacherDisciplines_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherDisciplines_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherDisciplines_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherDisciplines_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GenderId",
                table: "AspNetUsers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CreatedById",
                table: "Cities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_UpdatedById",
                table: "Cities",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CreatedById",
                table: "Countries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_UpdatedById",
                table: "Countries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDisciplines_CreatedById",
                table: "CourseDisciplines",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDisciplines_DisciplineId",
                table: "CourseDisciplines",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDisciplines_UpdatedById",
                table: "CourseDisciplines",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreatedById",
                table: "Courses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_UpdatedById",
                table: "Courses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudents_CourseId",
                table: "CourseStudents",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudents_CreatedById",
                table: "CourseStudents",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudents_StudentId",
                table: "CourseStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudents_UpdatedById",
                table: "CourseStudents",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Disciplines_CreatedById",
                table: "Disciplines",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Disciplines_UpdatedById",
                table: "Disciplines",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CreatedById",
                table: "Enrollments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_DisciplineId",
                table: "Enrollments",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_UpdatedById",
                table: "Enrollments",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Genders_CreatedById",
                table: "Genders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Genders_UpdatedById",
                table: "Genders",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_CountryId",
                table: "Nationalities",
                column: "CountryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_CreatedById",
                table: "Nationalities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_UpdatedById",
                table: "Nationalities",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDisciplines_CreatedById",
                table: "StudentDisciplines",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDisciplines_DisciplineId",
                table: "StudentDisciplines",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDisciplines_StudentId",
                table: "StudentDisciplines",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDisciplines_UpdatedById",
                table: "StudentDisciplines",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Students_BirthplaceId",
                table: "Students",
                column: "BirthplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CityId",
                table: "Students",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CountryOfNationalityId",
                table: "Students",
                column: "CountryOfNationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CreatedById",
                table: "Students",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Students_GenderId",
                table: "Students",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UpdatedById",
                table: "Students",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDisciplines_CreatedById",
                table: "TeacherDisciplines",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDisciplines_DisciplineId",
                table: "TeacherDisciplines",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDisciplines_UpdatedById",
                table: "TeacherDisciplines",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_BirthplaceId",
                table: "Teachers",
                column: "BirthplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CityId",
                table: "Teachers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CountryOfNationalityId",
                table: "Teachers",
                column: "CountryOfNationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CreatedById",
                table: "Teachers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_GenderId",
                table: "Teachers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UpdatedById",
                table: "Teachers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Genders_GenderId",
                table: "AspNetUsers",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genders_AspNetUsers_CreatedById",
                table: "Genders");

            migrationBuilder.DropForeignKey(
                name: "FK_Genders_AspNetUsers_UpdatedById",
                table: "Genders");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CourseDisciplines");

            migrationBuilder.DropTable(
                name: "CourseStudents");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "StudentDisciplines");

            migrationBuilder.DropTable(
                name: "TeacherDisciplines");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Disciplines");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Genders");
        }
    }
}
