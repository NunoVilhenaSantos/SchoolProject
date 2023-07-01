#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolProject.Web.Migrations;

/// <inheritdoc />
public partial class DBInit : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "AspNetRoles",
            table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                Name = table.Column<string>("nvarchar(256)", maxLength: 256,
                    nullable: true),
                NormalizedName = table.Column<string>("nvarchar(256)",
                    maxLength: 256, nullable: true),
                ConcurrencyStamp =
                    table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "AspNetUsers",
            table => new
            {
                Id = table.Column<string>("nvarchar(450)", nullable: false),
                FirstName = table.Column<string>("nvarchar(50)", maxLength: 50,
                    nullable: false),
                LastName = table.Column<string>("nvarchar(50)", maxLength: 50,
                    nullable: false),
                Address = table.Column<string>("nvarchar(100)", maxLength: 100,
                    nullable: true),
                ProfilePhotoId =
                    table.Column<Guid>("uniqueidentifier", nullable: false),
                WasDeleted = table.Column<bool>("bit", nullable: false),
                UserName = table.Column<string>("nvarchar(256)", maxLength: 256,
                    nullable: true),
                NormalizedUserName = table.Column<string>("nvarchar(256)",
                    maxLength: 256, nullable: true),
                Email = table.Column<string>("nvarchar(256)", maxLength: 256,
                    nullable: true),
                NormalizedEmail = table.Column<string>("nvarchar(256)",
                    maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>("bit", nullable: false),
                PasswordHash =
                    table.Column<string>("nvarchar(max)", nullable: true),
                SecurityStamp =
                    table.Column<string>("nvarchar(max)", nullable: true),
                ConcurrencyStamp =
                    table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumber =
                    table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumberConfirmed =
                    table.Column<bool>("bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>("bit", nullable: false),
                LockoutEnd =
                    table.Column<DateTimeOffset>("datetimeoffset",
                        nullable: true),
                LockoutEnabled = table.Column<bool>("bit", nullable: false),
                AccessFailedCount = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Courses",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>("nvarchar(max)", nullable: false),
                WorkLoad = table.Column<int>("int", nullable: false),
                Credits = table.Column<int>("int", nullable: false),
                StudentsCount = table.Column<int>("int", nullable: true),
                ProfilePhotoId =
                    table.Column<Guid>("uniqueidentifier", nullable: false),
                WasDeleted = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Courses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Genre",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                WasDeleted = table.Column<bool>("bit", nullable: false),
                Name = table.Column<string>("nvarchar(20)", maxLength: 20,
                    nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Genre", x => x.Id); });

        migrationBuilder.CreateTable(
            "SchoolClassCourses",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                SchoolClassId = table.Column<int>("int", nullable: false),
                CourseId = table.Column<int>("int", nullable: false),
                WasDeleted = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SchoolClassCourses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "SchoolClasses",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ClassAcronym =
                    table.Column<string>("nvarchar(max)", nullable: false),
                ClassName =
                    table.Column<string>("nvarchar(max)", nullable: false),
                StartDate =
                    table.Column<DateTime>("datetime2", nullable: false),
                EndDate = table.Column<DateTime>("datetime2", nullable: false),
                StartHour = table.Column<TimeSpan>("time", nullable: false),
                EndHour = table.Column<TimeSpan>("time", nullable: false),
                Location =
                    table.Column<string>("nvarchar(max)", nullable: false),
                Type = table.Column<string>("nvarchar(max)", nullable: false),
                Area = table.Column<string>("nvarchar(max)", nullable: false),
                CoursesCount = table.Column<int>("int", nullable: true),
                WorkHourLoad = table.Column<int>("int", nullable: true),
                StudentsCount = table.Column<int>("int", nullable: true),
                ClassAverage =
                    table.Column<decimal>("decimal(18,2)", nullable: true),
                HighestGrade =
                    table.Column<decimal>("decimal(18,2)", nullable: true),
                LowestGrade =
                    table.Column<decimal>("decimal(18,2)", nullable: true),
                ProfilePhotoId =
                    table.Column<Guid>("uniqueidentifier", nullable: false),
                WasDeleted = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SchoolClasses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "StudentCourses",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StudentId = table.Column<int>("int", nullable: false),
                CourseId = table.Column<int>("int", nullable: false),
                WasDeleted = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StudentCourses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "TeacherCourses",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                TeacherId = table.Column<int>("int", nullable: false),
                CourseId = table.Column<int>("int", nullable: false),
                WasDeleted = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TeacherCourses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "AspNetRoleClaims",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<string>("nvarchar(450)", nullable: false),
                ClaimType =
                    table.Column<string>("nvarchar(max)", nullable: true),
                ClaimValue =
                    table.Column<string>("nvarchar(max)", nullable: true)
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
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                ClaimType =
                    table.Column<string>("nvarchar(max)", nullable: true),
                ClaimValue =
                    table.Column<string>("nvarchar(max)", nullable: true)
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
                LoginProvider =
                    table.Column<string>("nvarchar(450)", nullable: false),
                ProviderKey =
                    table.Column<string>("nvarchar(450)", nullable: false),
                ProviderDisplayName =
                    table.Column<string>("nvarchar(max)", nullable: true),
                UserId = table.Column<string>("nvarchar(450)", nullable: false)
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
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                RoleId = table.Column<string>("nvarchar(450)", nullable: false)
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
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                LoginProvider =
                    table.Column<string>("nvarchar(450)", nullable: false),
                Name = table.Column<string>("nvarchar(450)", nullable: false),
                Value = table.Column<string>("nvarchar(max)", nullable: true)
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
            "Students",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName =
                    table.Column<string>("nvarchar(max)", nullable: false),
                LastName =
                    table.Column<string>("nvarchar(max)", nullable: false),
                Address =
                    table.Column<string>("nvarchar(max)", nullable: false),
                PostalCode =
                    table.Column<string>("nvarchar(max)", nullable: false),
                City = table.Column<string>("nvarchar(max)", nullable: false),
                MobilePhone =
                    table.Column<string>("nvarchar(max)", nullable: false),
                Email = table.Column<string>("nvarchar(max)", nullable: false),
                Active = table.Column<bool>("bit", nullable: false),
                Genre = table.Column<string>("nvarchar(max)", nullable: false),
                DateOfBirth =
                    table.Column<DateTime>("datetime2", nullable: false),
                IdentificationNumber =
                    table.Column<string>("nvarchar(max)", nullable: false),
                ExpirationDateIdentificationNumber =
                    table.Column<DateTime>("datetime2", nullable: false),
                TaxIdentificationNumber =
                    table.Column<string>("nvarchar(max)", nullable: false),
                Nationality =
                    table.Column<string>("nvarchar(max)", nullable: false),
                Birthplace =
                    table.Column<string>("nvarchar(max)", nullable: false),
                CoursesCount = table.Column<int>("int", nullable: false),
                TotalWorkHours = table.Column<int>("int", nullable: false),
                EnrollDate =
                    table.Column<DateTime>("datetime2", nullable: false),
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                ProfilePhotoId =
                    table.Column<Guid>("uniqueidentifier", nullable: false),
                WasDeleted = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Students", x => x.Id);
                table.ForeignKey(
                    "FK_Students_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Teachers",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName =
                    table.Column<string>("nvarchar(max)", nullable: false),
                LastName =
                    table.Column<string>("nvarchar(max)", nullable: false),
                Address =
                    table.Column<string>("nvarchar(max)", nullable: false),
                PostalCode =
                    table.Column<string>("nvarchar(max)", nullable: false),
                City = table.Column<string>("nvarchar(max)", nullable: false),
                MobilePhone =
                    table.Column<string>("nvarchar(max)", nullable: false),
                Email = table.Column<string>("nvarchar(max)", nullable: false),
                Active = table.Column<bool>("bit", nullable: false),
                Genre = table.Column<string>("nvarchar(max)", nullable: false),
                DateOfBirth =
                    table.Column<DateTime>("datetime2", nullable: false),
                IdentificationNumber =
                    table.Column<string>("nvarchar(max)", nullable: false),
                ExpirationDateIdentificationNumber =
                    table.Column<DateTime>("datetime2", nullable: false),
                TaxIdentificationNumber =
                    table.Column<string>("nvarchar(max)", nullable: false),
                Nationality =
                    table.Column<string>("nvarchar(max)", nullable: false),
                Birthplace =
                    table.Column<string>("nvarchar(max)", nullable: false),
                CoursesCount = table.Column<int>("int", nullable: false),
                TotalWorkHours = table.Column<int>("int", nullable: false),
                EnrollDate =
                    table.Column<DateTime>("datetime2", nullable: false),
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                ProfilePhotoId =
                    table.Column<Guid>("uniqueidentifier", nullable: false),
                WasDeleted = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Teachers", x => x.Id);
                table.ForeignKey(
                    "FK_Teachers_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Enrollments",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StudentId = table.Column<int>("int", nullable: false),
                CourseId = table.Column<int>("int", nullable: false),
                Grade = table.Column<decimal>("decimal(18,2)", nullable: true),
                WasDeleted = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Enrollments", x => x.Id);
                table.ForeignKey(
                    "FK_Enrollments_Courses_CourseId",
                    x => x.CourseId,
                    "Courses",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_Enrollments_Students_StudentId",
                    x => x.StudentId,
                    "Students",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_AspNetRoleClaims_RoleId",
            "AspNetRoleClaims",
            "RoleId");

        migrationBuilder.CreateIndex(
            "RoleNameIndex",
            "AspNetRoles",
            "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

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
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_Enrollments_CourseId",
            "Enrollments",
            "CourseId");

        migrationBuilder.CreateIndex(
            "IX_Enrollments_StudentId",
            "Enrollments",
            "StudentId");

        migrationBuilder.CreateIndex(
            "IX_Students_UserId",
            "Students",
            "UserId");

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
            "Genre");

        migrationBuilder.DropTable(
            "SchoolClassCourses");

        migrationBuilder.DropTable(
            "SchoolClasses");

        migrationBuilder.DropTable(
            "StudentCourses");

        migrationBuilder.DropTable(
            "TeacherCourses");

        migrationBuilder.DropTable(
            "Teachers");

        migrationBuilder.DropTable(
            "AspNetRoles");

        migrationBuilder.DropTable(
            "Courses");

        migrationBuilder.DropTable(
            "Students");

        migrationBuilder.DropTable(
            "AspNetUsers");
    }
}