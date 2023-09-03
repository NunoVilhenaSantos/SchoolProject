CREATE TABLE IF NOT EXISTS "_MyMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK__MyMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "AspNetRoles" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_AspNetRoles" PRIMARY KEY,
    "Name" TEXT NULL,
    "NormalizedName" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL
);

CREATE TABLE "AspNetUsers" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_AspNetUsers" PRIMARY KEY,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "Address" TEXT NULL,
    "WasDeleted" INTEGER NOT NULL,
    "ProfilePhotoId" TEXT NOT NULL,
    "UserName" TEXT NULL,
    "NormalizedUserName" TEXT NULL,
    "Email" TEXT NULL,
    "NormalizedEmail" TEXT NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PasswordHash" TEXT NULL,
    "SecurityStamp" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnd" TEXT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY AUTOINCREMENT,
    "RoleId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" TEXT NOT NULL,
    "RoleId" TEXT NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" TEXT NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Countries" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Countries" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "ProfilePhotoId" TEXT NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UpdatedById" TEXT NULL,
    CONSTRAINT "FK_Countries_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Countries_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Genders" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Genders" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "ProfilePhotoId" TEXT NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UpdatedById" TEXT NULL,
    CONSTRAINT "FK_Genders_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Genders_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "SchoolClasses" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_SchoolClasses" PRIMARY KEY AUTOINCREMENT,
    "Code" TEXT NOT NULL,
    "Acronym" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "QnqLevel" INTEGER NOT NULL,
    "EqfLevel" INTEGER NOT NULL,
    "StartDate" TEXT NOT NULL,
    "EndDate" TEXT NOT NULL,
    "StartHour" TEXT NOT NULL,
    "EndHour" TEXT NOT NULL,
    "Location" TEXT NULL,
    "Type" TEXT NULL,
    "Area" TEXT NULL,
    "PriceForEmployed" TEXT NOT NULL,
    "PriceForUnemployed" TEXT NOT NULL,
    "ProfilePhotoId" TEXT NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UpdatedById" TEXT NULL,
    CONSTRAINT "FK_SchoolClasses_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_SchoolClasses_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Cities" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Cities" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UpdatedById" TEXT NULL,
    "ProfilePhotoId" TEXT NOT NULL,
    "CountryId" INTEGER NOT NULL,
    CONSTRAINT "FK_Cities_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Cities_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Cities_Countries_CountryId" FOREIGN KEY ("CountryId") REFERENCES "Countries" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Nationalities" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Nationalities" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UpdatedById" TEXT NULL,
    "CountryId" INTEGER NOT NULL,
    CONSTRAINT "FK_Nationalities_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Nationalities_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Nationalities_Countries_CountryId" FOREIGN KEY ("CountryId") REFERENCES "Countries" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Courses" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Courses" PRIMARY KEY AUTOINCREMENT,
    "Code" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Description" INTEGER NOT NULL,
    "Hours" INTEGER NOT NULL,
    "CreditPoints" REAL NOT NULL,
    "ProfilePhotoId" TEXT NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UpdatedById" TEXT NULL,
    "SchoolClassId" INTEGER NULL,
    CONSTRAINT "FK_Courses_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Courses_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Courses_SchoolClasses_SchoolClassId" FOREIGN KEY ("SchoolClassId") REFERENCES "SchoolClasses" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Students" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Students" PRIMARY KEY AUTOINCREMENT,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "Address" TEXT NOT NULL,
    "PostalCode" TEXT NOT NULL,
    "CityId" INTEGER NOT NULL,
    "CountryId" INTEGER NOT NULL,
    "MobilePhone" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Active" INTEGER NOT NULL,
    "GenderId" INTEGER NOT NULL,
    "DateOfBirth" TEXT NOT NULL,
    "IdentificationNumber" TEXT NOT NULL,
    "IdentificationType" TEXT NOT NULL,
    "ExpirationDateIdentificationNumber" TEXT NOT NULL,
    "TaxIdentificationNumber" TEXT NOT NULL,
    "CountryOfNationalityId" INTEGER NOT NULL,
    "BirthplaceId" INTEGER NOT NULL,
    "EnrollDate" TEXT NOT NULL,
    "UserId" TEXT NOT NULL,
    "ProfilePhotoId" TEXT NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UpdatedById" TEXT NULL,
    "SchoolClassId" INTEGER NULL,
    CONSTRAINT "FK_Students_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Students_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Students_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Students_Cities_CityId" FOREIGN KEY ("CityId") REFERENCES "Cities" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Students_Countries_BirthplaceId" FOREIGN KEY ("BirthplaceId") REFERENCES "Countries" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Students_Countries_CountryId" FOREIGN KEY ("CountryId") REFERENCES "Countries" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Students_Countries_CountryOfNationalityId" FOREIGN KEY ("CountryOfNationalityId") REFERENCES "Countries" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Students_Genders_GenderId" FOREIGN KEY ("GenderId") REFERENCES "Genders" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Students_SchoolClasses_SchoolClassId" FOREIGN KEY ("SchoolClassId") REFERENCES "SchoolClasses" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Teachers" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Teachers" PRIMARY KEY AUTOINCREMENT,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "Address" TEXT NOT NULL,
    "PostalCode" TEXT NOT NULL,
    "CityId" INTEGER NOT NULL,
    "CountryId" INTEGER NOT NULL,
    "MobilePhone" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Active" INTEGER NOT NULL,
    "GenderId" INTEGER NOT NULL,
    "DateOfBirth" TEXT NOT NULL,
    "IdentificationNumber" TEXT NOT NULL,
    "IdentificationType" TEXT NOT NULL,
    "ExpirationDateIdentificationNumber" TEXT NOT NULL,
    "TaxIdentificationNumber" TEXT NOT NULL,
    "CountryOfNationalityId" INTEGER NOT NULL,
    "BirthplaceId" INTEGER NOT NULL,
    "EnrollDate" TEXT NOT NULL,
    "UserId" TEXT NOT NULL,
    "ProfilePhotoId" TEXT NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UpdatedById" TEXT NULL,
    CONSTRAINT "FK_Teachers_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Teachers_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Teachers_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Teachers_Cities_CityId" FOREIGN KEY ("CityId") REFERENCES "Cities" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Teachers_Countries_BirthplaceId" FOREIGN KEY ("BirthplaceId") REFERENCES "Countries" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Teachers_Countries_CountryId" FOREIGN KEY ("CountryId") REFERENCES "Countries" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Teachers_Countries_CountryOfNationalityId" FOREIGN KEY ("CountryOfNationalityId") REFERENCES "Countries" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Teachers_Genders_GenderId" FOREIGN KEY ("GenderId") REFERENCES "Genders" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "SchoolClassCourses" (
    "SchoolClassId" INTEGER NOT NULL,
    "CourseId" INTEGER NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedById" TEXT NULL,
    "Id" INTEGER NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "PK_SchoolClassCourses" PRIMARY KEY ("SchoolClassId", "CourseId"),
    CONSTRAINT "FK_SchoolClassCourses_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_SchoolClassCourses_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_SchoolClassCourses_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_SchoolClassCourses_SchoolClasses_SchoolClassId" FOREIGN KEY ("SchoolClassId") REFERENCES "SchoolClasses" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Enrollments" (
    "StudentId" INTEGER NOT NULL,
    "CourseId" INTEGER NOT NULL,
    "Grade" TEXT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedById" TEXT NULL,
    "Id" int NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "SchoolClassId" INTEGER NULL,
    CONSTRAINT "PK_Enrollments" PRIMARY KEY ("StudentId", "CourseId"),
    CONSTRAINT "FK_Enrollments_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Enrollments_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Enrollments_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Enrollments_SchoolClasses_SchoolClassId" FOREIGN KEY ("SchoolClassId") REFERENCES "SchoolClasses" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Enrollments_Students_StudentId" FOREIGN KEY ("StudentId") REFERENCES "Students" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "SchoolClassStudent" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_SchoolClassStudent" PRIMARY KEY AUTOINCREMENT,
    "SchoolClassId" INTEGER NOT NULL,
    "StudentId" INTEGER NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedById" TEXT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_SchoolClassStudent_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_SchoolClassStudent_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_SchoolClassStudent_SchoolClasses_SchoolClassId" FOREIGN KEY ("SchoolClassId") REFERENCES "SchoolClasses" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_SchoolClassStudent_Students_StudentId" FOREIGN KEY ("StudentId") REFERENCES "Students" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "StudentCourses" (
    "StudentId" INTEGER NOT NULL,
    "CourseId" INTEGER NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedById" TEXT NULL,
    "Id" INTEGER NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "PK_StudentCourses" PRIMARY KEY ("StudentId", "CourseId"),
    CONSTRAINT "FK_StudentCourses_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StudentCourses_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StudentCourses_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StudentCourses_Students_StudentId" FOREIGN KEY ("StudentId") REFERENCES "Students" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "TeacherCourses" (
    "TeacherId" INTEGER NOT NULL,
    "CourseId" INTEGER NOT NULL,
    "CreatedById" TEXT NOT NULL,
    "UpdatedById" TEXT NULL,
    "Id" INTEGER NOT NULL,
    "IdGuid" TEXT NOT NULL DEFAULT (NEWID()),
    "WasDeleted" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "PK_TeacherCourses" PRIMARY KEY ("TeacherId", "CourseId"),
    CONSTRAINT "FK_TeacherCourses_AspNetUsers_CreatedById" FOREIGN KEY ("CreatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_TeacherCourses_AspNetUsers_UpdatedById" FOREIGN KEY ("UpdatedById") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_TeacherCourses_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_TeacherCourses_Teachers_TeacherId" FOREIGN KEY ("TeacherId") REFERENCES "Teachers" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

CREATE INDEX "IX_Cities_CountryId" ON "Cities" ("CountryId");

CREATE INDEX "IX_Cities_CreatedById" ON "Cities" ("CreatedById");

CREATE INDEX "IX_Cities_UpdatedById" ON "Cities" ("UpdatedById");

CREATE INDEX "IX_Countries_CreatedById" ON "Countries" ("CreatedById");

CREATE INDEX "IX_Countries_UpdatedById" ON "Countries" ("UpdatedById");

CREATE INDEX "IX_Courses_CreatedById" ON "Courses" ("CreatedById");

CREATE INDEX "IX_Courses_SchoolClassId" ON "Courses" ("SchoolClassId");

CREATE INDEX "IX_Courses_UpdatedById" ON "Courses" ("UpdatedById");

CREATE INDEX "IX_Enrollments_CourseId" ON "Enrollments" ("CourseId");

CREATE INDEX "IX_Enrollments_CreatedById" ON "Enrollments" ("CreatedById");

CREATE INDEX "IX_Enrollments_SchoolClassId" ON "Enrollments" ("SchoolClassId");

CREATE INDEX "IX_Enrollments_UpdatedById" ON "Enrollments" ("UpdatedById");

CREATE INDEX "IX_Genders_CreatedById" ON "Genders" ("CreatedById");

CREATE INDEX "IX_Genders_UpdatedById" ON "Genders" ("UpdatedById");

CREATE UNIQUE INDEX "IX_Nationalities_CountryId" ON "Nationalities" ("CountryId");

CREATE INDEX "IX_Nationalities_CreatedById" ON "Nationalities" ("CreatedById");

CREATE INDEX "IX_Nationalities_UpdatedById" ON "Nationalities" ("UpdatedById");

CREATE INDEX "IX_SchoolClassCourses_CourseId" ON "SchoolClassCourses" ("CourseId");

CREATE INDEX "IX_SchoolClassCourses_CreatedById" ON "SchoolClassCourses" ("CreatedById");

CREATE INDEX "IX_SchoolClassCourses_UpdatedById" ON "SchoolClassCourses" ("UpdatedById");

CREATE INDEX "IX_SchoolClasses_CreatedById" ON "SchoolClasses" ("CreatedById");

CREATE INDEX "IX_SchoolClasses_UpdatedById" ON "SchoolClasses" ("UpdatedById");

CREATE INDEX "IX_SchoolClassStudent_CreatedById" ON "SchoolClassStudent" ("CreatedById");

CREATE INDEX "IX_SchoolClassStudent_SchoolClassId" ON "SchoolClassStudent" ("SchoolClassId");

CREATE INDEX "IX_SchoolClassStudent_StudentId" ON "SchoolClassStudent" ("StudentId");

CREATE INDEX "IX_SchoolClassStudent_UpdatedById" ON "SchoolClassStudent" ("UpdatedById");

CREATE INDEX "IX_StudentCourses_CourseId" ON "StudentCourses" ("CourseId");

CREATE INDEX "IX_StudentCourses_CreatedById" ON "StudentCourses" ("CreatedById");

CREATE INDEX "IX_StudentCourses_UpdatedById" ON "StudentCourses" ("UpdatedById");

CREATE INDEX "IX_Students_BirthplaceId" ON "Students" ("BirthplaceId");

CREATE INDEX "IX_Students_CityId" ON "Students" ("CityId");

CREATE INDEX "IX_Students_CountryId" ON "Students" ("CountryId");

CREATE INDEX "IX_Students_CountryOfNationalityId" ON "Students" ("CountryOfNationalityId");

CREATE INDEX "IX_Students_CreatedById" ON "Students" ("CreatedById");

CREATE INDEX "IX_Students_GenderId" ON "Students" ("GenderId");

CREATE INDEX "IX_Students_SchoolClassId" ON "Students" ("SchoolClassId");

CREATE INDEX "IX_Students_UpdatedById" ON "Students" ("UpdatedById");

CREATE INDEX "IX_Students_UserId" ON "Students" ("UserId");

CREATE INDEX "IX_TeacherCourses_CourseId" ON "TeacherCourses" ("CourseId");

CREATE INDEX "IX_TeacherCourses_CreatedById" ON "TeacherCourses" ("CreatedById");

CREATE INDEX "IX_TeacherCourses_UpdatedById" ON "TeacherCourses" ("UpdatedById");

CREATE INDEX "IX_Teachers_BirthplaceId" ON "Teachers" ("BirthplaceId");

CREATE INDEX "IX_Teachers_CityId" ON "Teachers" ("CityId");

CREATE INDEX "IX_Teachers_CountryId" ON "Teachers" ("CountryId");

CREATE INDEX "IX_Teachers_CountryOfNationalityId" ON "Teachers" ("CountryOfNationalityId");

CREATE INDEX "IX_Teachers_CreatedById" ON "Teachers" ("CreatedById");

CREATE INDEX "IX_Teachers_GenderId" ON "Teachers" ("GenderId");

CREATE INDEX "IX_Teachers_UpdatedById" ON "Teachers" ("UpdatedById");

CREATE INDEX "IX_Teachers_UserId" ON "Teachers" ("UserId");

INSERT INTO "_MyMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230903203657_InitDB', '7.0.10');

COMMIT;

