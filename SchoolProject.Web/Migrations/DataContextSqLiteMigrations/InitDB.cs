﻿#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolProject.Web.Migrations.DataContextSqLiteMigrations;

/// <inheritdoc />
public partial class InitDB : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "AspNetRoles",
            table => new
            {
                Id = table.Column<string>("TEXT", nullable: false),
                Name = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                NormalizedName = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                ConcurrencyStamp = table.Column<string>("TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "AspNetUsers",
            table => new
            {
                Id = table.Column<string>("TEXT", nullable: false),
                FirstName = table.Column<string>("TEXT", maxLength: 50,
                    nullable: false),
                LastName = table.Column<string>("TEXT", maxLength: 50,
                    nullable: false),
                Address = table.Column<string>("TEXT", maxLength: 100,
                    nullable: true),
                ProfilePhotoId = table.Column<Guid>("TEXT", nullable: false),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                UserName = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                NormalizedUserName = table.Column<string>("TEXT",
                    maxLength: 256, nullable: true),
                Email = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                NormalizedEmail = table.Column<string>("TEXT", maxLength: 256,
                    nullable: true),
                EmailConfirmed = table.Column<bool>("INTEGER", nullable: false),
                PasswordHash = table.Column<string>("TEXT", nullable: true),
                SecurityStamp = table.Column<string>("TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>("TEXT", nullable: true),
                PhoneNumber = table.Column<string>("TEXT", nullable: true),
                PhoneNumberConfirmed =
                    table.Column<bool>("INTEGER", nullable: false),
                TwoFactorEnabled =
                    table.Column<bool>("INTEGER", nullable: false),
                LockoutEnd =
                    table.Column<DateTimeOffset>("TEXT", nullable: true),
                LockoutEnabled = table.Column<bool>("INTEGER", nullable: false),
                AccessFailedCount =
                    table.Column<int>("INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "AspNetRoleClaims",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                RoleId = table.Column<string>("TEXT", nullable: false),
                ClaimType = table.Column<string>("TEXT", nullable: true),
                ClaimValue = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "AspNetUserClaims",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<string>("TEXT", nullable: false),
                ClaimType = table.Column<string>("TEXT", nullable: true),
                ClaimValue = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "AspNetUserLogins",
            table => new
            {
                LoginProvider = table.Column<string>("TEXT", nullable: false),
                ProviderKey = table.Column<string>("TEXT", nullable: false),
                ProviderDisplayName =
                    table.Column<string>("TEXT", nullable: true),
                UserId = table.Column<string>("TEXT", nullable: false)
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
            });

        migrationBuilder.CreateTable(
            "AspNetUserRoles",
            table => new
            {
                UserId = table.Column<string>("TEXT", nullable: false),
                RoleId = table.Column<string>("TEXT", nullable: false)
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
            });

        migrationBuilder.CreateTable(
            "AspNetUserTokens",
            table => new
            {
                UserId = table.Column<string>("TEXT", nullable: false),
                LoginProvider = table.Column<string>("TEXT", nullable: false),
                Name = table.Column<string>("TEXT", nullable: false),
                Value = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "Genres",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>("TEXT", maxLength: 20,
                    nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "Nationalities",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>("TEXT", maxLength: 50,
                    nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "SchoolClasses",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Code = table.Column<string>("TEXT", maxLength: 7,
                    nullable: false),
                Acronym = table.Column<string>("TEXT", nullable: false),
                Name = table.Column<string>("TEXT", nullable: false),
                QnqLevel = table.Column<byte>("INTEGER", nullable: false),
                EqfLevel = table.Column<byte>("INTEGER", nullable: false),
                StartDate = table.Column<DateTime>("TEXT", nullable: false),
                EndDate = table.Column<DateTime>("TEXT", nullable: false),
                StartHour = table.Column<TimeSpan>("TEXT", nullable: false),
                EndHour = table.Column<TimeSpan>("TEXT", nullable: false),
                Location = table.Column<string>("TEXT", nullable: true),
                Type = table.Column<string>("TEXT", nullable: true),
                Area = table.Column<string>("TEXT", nullable: true),
                PriceForEmployed = table.Column<decimal>("TEXT", precision: 10,
                    scale: 2, nullable: false),
                PriceForUnemployed = table.Column<decimal>("TEXT",
                    precision: 10, scale: 2, nullable: false),
                ProfilePhotoId = table.Column<Guid>("TEXT", nullable: true),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "Countries",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>("TEXT", maxLength: 50,
                    nullable: false),
                NationalityId = table.Column<int>("INTEGER", nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "Courses",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Code = table.Column<string>("TEXT", maxLength: 7,
                    nullable: false),
                Name = table.Column<string>("TEXT", nullable: false),
                Hours = table.Column<int>("INTEGER", nullable: false),
                CreditPoints = table.Column<double>("REAL", nullable: false),
                ProfilePhotoId = table.Column<Guid>("TEXT", nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true),
                SchoolClassId = table.Column<int>("INTEGER", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "Cities",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>("TEXT", maxLength: 50,
                    nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true),
                CountryId = table.Column<int>("INTEGER", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "SchoolClassCourses",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                SchoolClassId = table.Column<int>("INTEGER", nullable: false),
                CourseId = table.Column<int>("INTEGER", nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "Students",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                FirstName = table.Column<string>("TEXT", nullable: false),
                LastName = table.Column<string>("TEXT", nullable: false),
                Address = table.Column<string>("TEXT", nullable: false),
                PostalCode = table.Column<string>("TEXT", nullable: false),
                CityId = table.Column<int>("INTEGER", nullable: false),
                CountryId = table.Column<int>("INTEGER", nullable: false),
                MobilePhone = table.Column<string>("TEXT", nullable: false),
                Email = table.Column<string>("TEXT", nullable: false),
                Active = table.Column<bool>("INTEGER", nullable: false),
                GenreId = table.Column<int>("INTEGER", nullable: false),
                DateOfBirth = table.Column<DateTime>("TEXT", nullable: false),
                IdentificationNumber =
                    table.Column<string>("TEXT", nullable: false),
                IdentificationType =
                    table.Column<string>("TEXT", nullable: false),
                ExpirationDateIdentificationNumber =
                    table.Column<DateTime>("TEXT", nullable: false),
                TaxIdentificationNumber =
                    table.Column<string>("TEXT", nullable: false),
                CountryOfNationalityId =
                    table.Column<int>("INTEGER", nullable: false),
                BirthplaceId = table.Column<int>("INTEGER", nullable: false),
                EnrollDate = table.Column<DateTime>("TEXT", nullable: false),
                UserId = table.Column<string>("TEXT", nullable: false),
                ProfilePhotoId = table.Column<Guid>("TEXT", nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "Teachers",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                FirstName = table.Column<string>("TEXT", nullable: false),
                LastName = table.Column<string>("TEXT", nullable: false),
                Address = table.Column<string>("TEXT", nullable: false),
                PostalCode = table.Column<string>("TEXT", nullable: false),
                CityId = table.Column<int>("INTEGER", nullable: false),
                CountryId = table.Column<int>("INTEGER", nullable: false),
                MobilePhone = table.Column<string>("TEXT", nullable: false),
                Email = table.Column<string>("TEXT", nullable: false),
                Active = table.Column<bool>("INTEGER", nullable: false),
                GenreId = table.Column<int>("INTEGER", nullable: false),
                DateOfBirth = table.Column<DateTime>("TEXT", nullable: false),
                IdentificationNumber =
                    table.Column<string>("TEXT", nullable: false),
                IdentificationType =
                    table.Column<string>("TEXT", nullable: false),
                ExpirationDateIdentificationNumber =
                    table.Column<DateTime>("TEXT", nullable: false),
                TaxIdentificationNumber =
                    table.Column<string>("TEXT", nullable: false),
                CountryOfNationalityId =
                    table.Column<int>("INTEGER", nullable: false),
                BirthplaceId = table.Column<int>("INTEGER", nullable: false),
                EnrollDate = table.Column<DateTime>("TEXT", nullable: false),
                UserId = table.Column<string>("TEXT", nullable: false),
                ProfilePhotoId = table.Column<Guid>("TEXT", nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "Enrollments",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                StudentId = table.Column<int>("INTEGER", nullable: false),
                CourseId = table.Column<int>("INTEGER", nullable: false),
                Grade = table.Column<decimal>("TEXT", precision: 18, scale: 2,
                    nullable: true),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true),
                SchoolClassId = table.Column<int>("INTEGER", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "SchoolClassStudent",
            table => new
            {
                SchoolClassesId = table.Column<int>("INTEGER", nullable: false),
                StudentsId = table.Column<int>("INTEGER", nullable: false)
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
            });

        migrationBuilder.CreateTable(
            "StudentCourses",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                StudentId = table.Column<int>("INTEGER", nullable: false),
                CourseId = table.Column<int>("INTEGER", nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true)
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
            });

        migrationBuilder.CreateTable(
            "TeacherCourses",
            table => new
            {
                Id = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                TeacherId = table.Column<int>("INTEGER", nullable: false),
                CourseId = table.Column<int>("INTEGER", nullable: false),
                IdGuid = table.Column<Guid>("TEXT", nullable: false,
                    defaultValueSql: "NEWID()"),
                WasDeleted = table.Column<bool>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedById = table.Column<string>("TEXT", nullable: true),
                UpdatedAt = table.Column<DateTime>("TEXT", nullable: true),
                UpdatedById = table.Column<string>("TEXT", nullable: true)
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
            });

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