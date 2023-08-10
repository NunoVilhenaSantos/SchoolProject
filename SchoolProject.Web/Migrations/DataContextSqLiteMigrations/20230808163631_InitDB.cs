#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolProject.Web.Migrations.DataContextSqLiteMigrations;

/// <inheritdoc />
public partial class InitDB : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            columns: table => new
            {
                Id = table.Column<string>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 256,
                    nullable: true),
                NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256,
                    nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_AspNetRoles", columns: x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUsers",
            columns: table => new
            {
                Id = table.Column<string>(type: "TEXT", nullable: false),
                FirstName = table.Column<string>(type: "TEXT", maxLength: 50,
                    nullable: false),
                LastName = table.Column<string>(type: "TEXT", maxLength: 50,
                    nullable: false),
                Address = table.Column<string>(type: "TEXT", maxLength: 100,
                    nullable: true),
                ProfilePhotoId = table.Column<Guid>(type: "TEXT", nullable: false),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                UserName = table.Column<string>(type: "TEXT", maxLength: 256,
                    nullable: true),
                NormalizedUserName = table.Column<string>(type: "TEXT",
                    maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "TEXT", maxLength: 256,
                    nullable: true),
                NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256,
                    nullable: true),
                EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumberConfirmed =
                    table.Column<bool>(type: "INTEGER", nullable: false),
                TwoFactorEnabled =
                    table.Column<bool>(type: "INTEGER", nullable: false),
                LockoutEnd =
                    table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                AccessFailedCount =
                    table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_AspNetUsers", columns: x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                RoleId = table.Column<string>(type: "TEXT", nullable: false),
                ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_AspNetRoleClaims", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                UserId = table.Column<string>(type: "TEXT", nullable: false),
                ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_AspNetUserClaims", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                ProviderDisplayName =
                    table.Column<string>(type: "TEXT", nullable: true),
                UserId = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_AspNetUserLogins",
                    columns: x => new {x.LoginProvider, x.ProviderKey});
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            columns: table => new
            {
                UserId = table.Column<string>(type: "TEXT", nullable: false),
                RoleId = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_AspNetUserRoles",
                    columns: x => new {x.UserId, x.RoleId});
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            columns: table => new
            {
                UserId = table.Column<string>(type: "TEXT", nullable: false),
                LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Value = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_AspNetUserTokens",
                    columns: x => new {x.UserId, x.LoginProvider, x.Name});
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Genders",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                Name = table.Column<string>(type: "TEXT", maxLength: 20,
                    nullable: false),
                ProfilePhotoId = table.Column<Guid>(type: "TEXT", nullable: true),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_Genders", columns: x => x.Id);
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
            });

        migrationBuilder.CreateTable(
            name: "Nationalities",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                Name = table.Column<string>(type: "TEXT", maxLength: 50,
                    nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_Nationalities", columns: x => x.Id);
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
            });

        migrationBuilder.CreateTable(
            name: "SchoolClasses",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                Code = table.Column<string>(type: "TEXT", maxLength: 7,
                    nullable: false),
                Acronym = table.Column<string>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                QnqLevel = table.Column<byte>(type: "INTEGER", nullable: false),
                EqfLevel = table.Column<byte>(type: "INTEGER", nullable: false),
                StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                StartHour = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                EndHour = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                Location = table.Column<string>(type: "TEXT", nullable: true),
                Type = table.Column<string>(type: "TEXT", nullable: true),
                Area = table.Column<string>(type: "TEXT", nullable: true),
                PriceForEmployed = table.Column<decimal>(type: "TEXT", precision: 10,
                    scale: 2, nullable: false),
                PriceForUnemployed = table.Column<decimal>(type: "TEXT",
                    precision: 10, scale: 2, nullable: false),
                ProfilePhotoId = table.Column<Guid>(type: "TEXT", nullable: true),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_SchoolClasses", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_SchoolClasses_AspNetUsers_CreatedById",
                    column: x => x.CreatedById,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_SchoolClasses_AspNetUsers_UpdatedById",
                    column: x => x.UpdatedById,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Countries",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                Name = table.Column<string>(type: "TEXT", maxLength: 50,
                    nullable: false),
                ProfilePhotoId = table.Column<Guid>(type: "TEXT", nullable: true),
                NationalityId = table.Column<int>(type: "INTEGER", nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_Countries", columns: x => x.Id);
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
                table.ForeignKey(
                    name: "FK_Countries_Nationalities_NationalityId",
                    column: x => x.NationalityId,
                    principalTable: "Nationalities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Courses",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                Code = table.Column<string>(type: "TEXT", maxLength: 7,
                    nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Description = table.Column<int>(type: "INTEGER", nullable: false),
                Hours = table.Column<int>(type: "INTEGER", nullable: false),
                CreditPoints = table.Column<double>(type: "REAL", nullable: false),
                ProfilePhotoId = table.Column<Guid>(type: "TEXT", nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true),
                SchoolClassId = table.Column<int>(type: "INTEGER", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_Courses", columns: x => x.Id);
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
                table.ForeignKey(
                    name: "FK_Courses_SchoolClasses_SchoolClassId",
                    column: x => x.SchoolClassId,
                    principalTable: "SchoolClasses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Cities",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                Name = table.Column<string>(type: "TEXT", maxLength: 50,
                    nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true),
                CountryId = table.Column<int>(type: "INTEGER", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_Cities", columns: x => x.Id);
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
            });

        migrationBuilder.CreateTable(
            name: "SchoolClassCourses",
            columns: table => new
            {
                SchoolClassId = table.Column<int>(type: "INTEGER", nullable: false),
                CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                Id = table.Column<int>(type: "INTEGER", nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_SchoolClassCourses",
                    columns: x => new {x.SchoolClassId, x.CourseId});
                table.ForeignKey(
                    name: "FK_SchoolClassCourses_AspNetUsers_CreatedById",
                    column: x => x.CreatedById,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_SchoolClassCourses_AspNetUsers_UpdatedById",
                    column: x => x.UpdatedById,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_SchoolClassCourses_Courses_CourseId",
                    column: x => x.CourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_SchoolClassCourses_SchoolClasses_SchoolClassId",
                    column: x => x.SchoolClassId,
                    principalTable: "SchoolClasses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Students",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                FirstName = table.Column<string>(type: "TEXT", nullable: false),
                LastName = table.Column<string>(type: "TEXT", nullable: false),
                Address = table.Column<string>(type: "TEXT", nullable: false),
                PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                CityId = table.Column<int>(type: "INTEGER", nullable: false),
                CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                MobilePhone = table.Column<string>(type: "TEXT", nullable: false),
                Email = table.Column<string>(type: "TEXT", nullable: false),
                Active = table.Column<bool>(type: "INTEGER", nullable: false),
                GenderId = table.Column<int>(type: "INTEGER", nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                IdentificationNumber =
                    table.Column<string>(type: "TEXT", nullable: false),
                IdentificationType =
                    table.Column<string>(type: "TEXT", nullable: false),
                ExpirationDateIdentificationNumber =
                    table.Column<DateTime>(type: "TEXT", nullable: false),
                TaxIdentificationNumber =
                    table.Column<string>(type: "TEXT", nullable: false),
                CountryOfNationalityId =
                    table.Column<int>(type: "INTEGER", nullable: false),
                BirthplaceId = table.Column<int>(type: "INTEGER", nullable: false),
                EnrollDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                UserId = table.Column<string>(type: "TEXT", nullable: false),
                ProfilePhotoId = table.Column<Guid>(type: "TEXT", nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_Students", columns: x => x.Id);
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
                    name: "FK_Students_Countries_CountryId",
                    column: x => x.CountryId,
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
            });

        migrationBuilder.CreateTable(
            name: "Teachers",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation(name: "Sqlite:Autoincrement", value: true),
                FirstName = table.Column<string>(type: "TEXT", nullable: false),
                LastName = table.Column<string>(type: "TEXT", nullable: false),
                Address = table.Column<string>(type: "TEXT", nullable: false),
                PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                CityId = table.Column<int>(type: "INTEGER", nullable: false),
                CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                MobilePhone = table.Column<string>(type: "TEXT", nullable: false),
                Email = table.Column<string>(type: "TEXT", nullable: false),
                Active = table.Column<bool>(type: "INTEGER", nullable: false),
                GenderId = table.Column<int>(type: "INTEGER", nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                IdentificationNumber =
                    table.Column<string>(type: "TEXT", nullable: false),
                IdentificationType =
                    table.Column<string>(type: "TEXT", nullable: false),
                ExpirationDateIdentificationNumber =
                    table.Column<DateTime>(type: "TEXT", nullable: false),
                TaxIdentificationNumber =
                    table.Column<string>(type: "TEXT", nullable: false),
                CountryOfNationalityId =
                    table.Column<int>(type: "INTEGER", nullable: false),
                BirthplaceId = table.Column<int>(type: "INTEGER", nullable: false),
                EnrollDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                UserId = table.Column<string>(type: "TEXT", nullable: false),
                ProfilePhotoId = table.Column<Guid>(type: "TEXT", nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_Teachers", columns: x => x.Id);
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
                    name: "FK_Teachers_Countries_CountryId",
                    column: x => x.CountryId,
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
            });

        migrationBuilder.CreateTable(
            name: "Enrollments",
            columns: table => new
            {
                StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                Grade = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2,
                    nullable: true),
                Id = table.Column<int>(type: "int", nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true),
                SchoolClassId = table.Column<int>(type: "INTEGER", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_Enrollments",
                    columns: x => new {x.StudentId, x.CourseId});
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
                    name: "FK_Enrollments_SchoolClasses_SchoolClassId",
                    column: x => x.SchoolClassId,
                    principalTable: "SchoolClasses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Enrollments_Students_StudentId",
                    column: x => x.StudentId,
                    principalTable: "Students",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "SchoolClassStudent",
            columns: table => new
            {
                SchoolClassesId = table.Column<int>(type: "INTEGER", nullable: false),
                StudentsId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_SchoolClassStudent",
                    columns: x => new {x.SchoolClassesId, x.StudentsId});
                table.ForeignKey(
                    name: "FK_SchoolClassStudent_SchoolClasses_SchoolClassesId",
                    column: x => x.SchoolClassesId,
                    principalTable: "SchoolClasses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_SchoolClassStudent_Students_StudentsId",
                    column: x => x.StudentsId,
                    principalTable: "Students",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "StudentCourses",
            columns: table => new
            {
                StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                Id = table.Column<int>(type: "INTEGER", nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_StudentCourses",
                    columns: x => new {x.StudentId, x.CourseId});
                table.ForeignKey(
                    name: "FK_StudentCourses_AspNetUsers_CreatedById",
                    column: x => x.CreatedById,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_StudentCourses_AspNetUsers_UpdatedById",
                    column: x => x.UpdatedById,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_StudentCourses_Courses_CourseId",
                    column: x => x.CourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_StudentCourses_Students_StudentId",
                    column: x => x.StudentId,
                    principalTable: "Students",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "TeacherCourses",
            columns: table => new
            {
                TeacherId = table.Column<int>(type: "INTEGER", nullable: false),
                CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                Id = table.Column<int>(type: "INTEGER", nullable: false),
                IdGuid = table.Column<Guid>(type: "TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedById = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedById = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_TeacherCourses",
                    columns: x => new {x.TeacherId, x.CourseId});
                table.ForeignKey(
                    name: "FK_TeacherCourses_AspNetUsers_CreatedById",
                    column: x => x.CreatedById,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_TeacherCourses_AspNetUsers_UpdatedById",
                    column: x => x.UpdatedById,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_TeacherCourses_Courses_CourseId",
                    column: x => x.CourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_TeacherCourses_Teachers_TeacherId",
                    column: x => x.TeacherId,
                    principalTable: "Teachers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

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
            name: "IX_Countries_NationalityId",
            table: "Countries",
            column: "NationalityId");

        migrationBuilder.CreateIndex(
            name: "IX_Countries_UpdatedById",
            table: "Countries",
            column: "UpdatedById");

        migrationBuilder.CreateIndex(
            name: "IX_Courses_CreatedById",
            table: "Courses",
            column: "CreatedById");

        migrationBuilder.CreateIndex(
            name: "IX_Courses_SchoolClassId",
            table: "Courses",
            column: "SchoolClassId");

        migrationBuilder.CreateIndex(
            name: "IX_Courses_UpdatedById",
            table: "Courses",
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
            name: "IX_Enrollments_SchoolClassId",
            table: "Enrollments",
            column: "SchoolClassId");

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
            name: "IX_Nationalities_CreatedById",
            table: "Nationalities",
            column: "CreatedById");

        migrationBuilder.CreateIndex(
            name: "IX_Nationalities_UpdatedById",
            table: "Nationalities",
            column: "UpdatedById");

        migrationBuilder.CreateIndex(
            name: "IX_SchoolClassCourses_CourseId",
            table: "SchoolClassCourses",
            column: "CourseId");

        migrationBuilder.CreateIndex(
            name: "IX_SchoolClassCourses_CreatedById",
            table: "SchoolClassCourses",
            column: "CreatedById");

        migrationBuilder.CreateIndex(
            name: "IX_SchoolClassCourses_UpdatedById",
            table: "SchoolClassCourses",
            column: "UpdatedById");

        migrationBuilder.CreateIndex(
            name: "IX_SchoolClasses_CreatedById",
            table: "SchoolClasses",
            column: "CreatedById");

        migrationBuilder.CreateIndex(
            name: "IX_SchoolClasses_UpdatedById",
            table: "SchoolClasses",
            column: "UpdatedById");

        migrationBuilder.CreateIndex(
            name: "IX_SchoolClassStudent_StudentsId",
            table: "SchoolClassStudent",
            column: "StudentsId");

        migrationBuilder.CreateIndex(
            name: "IX_StudentCourses_CourseId",
            table: "StudentCourses",
            column: "CourseId");

        migrationBuilder.CreateIndex(
            name: "IX_StudentCourses_CreatedById",
            table: "StudentCourses",
            column: "CreatedById");

        migrationBuilder.CreateIndex(
            name: "IX_StudentCourses_UpdatedById",
            table: "StudentCourses",
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
            name: "IX_Students_CountryId",
            table: "Students",
            column: "CountryId");

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
            name: "IX_TeacherCourses_CourseId",
            table: "TeacherCourses",
            column: "CourseId");

        migrationBuilder.CreateIndex(
            name: "IX_TeacherCourses_CreatedById",
            table: "TeacherCourses",
            column: "CreatedById");

        migrationBuilder.CreateIndex(
            name: "IX_TeacherCourses_UpdatedById",
            table: "TeacherCourses",
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
            name: "IX_Teachers_CountryId",
            table: "Teachers",
            column: "CountryId");

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
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
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
            name: "Enrollments");

        migrationBuilder.DropTable(
            name: "SchoolClassCourses");

        migrationBuilder.DropTable(
            name: "SchoolClassStudent");

        migrationBuilder.DropTable(
            name: "StudentCourses");

        migrationBuilder.DropTable(
            name: "TeacherCourses");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "Students");

        migrationBuilder.DropTable(
            name: "Courses");

        migrationBuilder.DropTable(
            name: "Teachers");

        migrationBuilder.DropTable(
            name: "SchoolClasses");

        migrationBuilder.DropTable(
            name: "Cities");

        migrationBuilder.DropTable(
            name: "Genders");

        migrationBuilder.DropTable(
            name: "Countries");

        migrationBuilder.DropTable(
            name: "Nationalities");

        migrationBuilder.DropTable(
            name: "AspNetUsers");
    }
}