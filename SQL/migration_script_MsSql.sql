IF OBJECT_ID(N'[_MyMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [_MyMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK__MyMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Address] nvarchar(100) NULL,
    [WasDeleted] bit NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Countries] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Countries_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Countries_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Genders] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Genders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Genders_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Genders_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [SchoolClasses] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(7) NOT NULL,
    [Acronym] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [QnqLevel] tinyint NOT NULL,
    [EqfLevel] tinyint NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [StartHour] time NOT NULL,
    [EndHour] time NOT NULL,
    [Location] nvarchar(max) NULL,
    [Type] nvarchar(max) NULL,
    [Area] nvarchar(max) NULL,
    [PriceForEmployed] decimal(10,2) NOT NULL,
    [PriceForUnemployed] decimal(10,2) NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_SchoolClasses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SchoolClasses_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SchoolClasses_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Cities] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [CountryId] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Cities] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cities_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Cities_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Cities_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Nationalities] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [CountryId] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Nationalities] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Nationalities_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Nationalities_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Nationalities_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Courses] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(7) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Hours] int NOT NULL,
    [CreditPoints] float NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedById] nvarchar(450) NULL,
    [SchoolClassId] int NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Courses_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Courses_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Courses_SchoolClasses_SchoolClassId] FOREIGN KEY ([SchoolClassId]) REFERENCES [SchoolClasses] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Students] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [PostalCode] nvarchar(max) NOT NULL,
    [CityId] int NOT NULL,
    [CountryId] int NOT NULL,
    [MobilePhone] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Active] bit NOT NULL,
    [GenderId] int NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [IdentificationNumber] nvarchar(max) NOT NULL,
    [IdentificationType] nvarchar(max) NOT NULL,
    [ExpirationDateIdentificationNumber] datetime2 NOT NULL,
    [TaxIdentificationNumber] nvarchar(max) NOT NULL,
    [CountryOfNationalityId] int NOT NULL,
    [BirthplaceId] int NOT NULL,
    [EnrollDate] datetime2 NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedById] nvarchar(450) NULL,
    [SchoolClassId] int NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Students_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Countries_BirthplaceId] FOREIGN KEY ([BirthplaceId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Countries_CountryOfNationalityId] FOREIGN KEY ([CountryOfNationalityId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Genders_GenderId] FOREIGN KEY ([GenderId]) REFERENCES [Genders] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_SchoolClasses_SchoolClassId] FOREIGN KEY ([SchoolClassId]) REFERENCES [SchoolClasses] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Teachers] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [PostalCode] nvarchar(max) NOT NULL,
    [CityId] int NOT NULL,
    [CountryId] int NOT NULL,
    [MobilePhone] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Active] bit NOT NULL,
    [GenderId] int NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [IdentificationNumber] nvarchar(max) NOT NULL,
    [IdentificationType] nvarchar(max) NOT NULL,
    [ExpirationDateIdentificationNumber] datetime2 NOT NULL,
    [TaxIdentificationNumber] nvarchar(max) NOT NULL,
    [CountryOfNationalityId] int NOT NULL,
    [BirthplaceId] int NOT NULL,
    [EnrollDate] datetime2 NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Teachers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Teachers_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_Countries_BirthplaceId] FOREIGN KEY ([BirthplaceId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_Countries_CountryOfNationalityId] FOREIGN KEY ([CountryOfNationalityId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_Genders_GenderId] FOREIGN KEY ([GenderId]) REFERENCES [Genders] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [SchoolClassCourses] (
    [SchoolClassId] int NOT NULL,
    [CourseId] int NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [Id] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_SchoolClassCourses] PRIMARY KEY ([SchoolClassId], [CourseId]),
    CONSTRAINT [FK_SchoolClassCourses_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SchoolClassCourses_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SchoolClassCourses_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SchoolClassCourses_SchoolClasses_SchoolClassId] FOREIGN KEY ([SchoolClassId]) REFERENCES [SchoolClasses] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Enrollments] (
    [StudentId] int NOT NULL,
    [CourseId] int NOT NULL,
    [Grade] decimal(18,2) NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [Id] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [SchoolClassId] int NULL,
    CONSTRAINT [PK_Enrollments] PRIMARY KEY ([StudentId], [CourseId]),
    CONSTRAINT [FK_Enrollments_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_SchoolClasses_SchoolClassId] FOREIGN KEY ([SchoolClassId]) REFERENCES [SchoolClasses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [SchoolClassStudent] (
    [Id] int NOT NULL IDENTITY,
    [SchoolClassId] int NOT NULL,
    [StudentId] int NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_SchoolClassStudent] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SchoolClassStudent_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SchoolClassStudent_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SchoolClassStudent_SchoolClasses_SchoolClassId] FOREIGN KEY ([SchoolClassId]) REFERENCES [SchoolClasses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SchoolClassStudent_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [StudentCourses] (
    [StudentId] int NOT NULL,
    [CourseId] int NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [Id] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_StudentCourses] PRIMARY KEY ([StudentId], [CourseId]),
    CONSTRAINT [FK_StudentCourses_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_StudentCourses_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_StudentCourses_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_StudentCourses_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [TeacherCourses] (
    [TeacherId] int NOT NULL,
    [CourseId] int NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [Id] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_TeacherCourses] PRIMARY KEY ([TeacherId], [CourseId]),
    CONSTRAINT [FK_TeacherCourses_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TeacherCourses_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TeacherCourses_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TeacherCourses_Teachers_TeacherId] FOREIGN KEY ([TeacherId]) REFERENCES [Teachers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_Cities_CountryId] ON [Cities] ([CountryId]);
GO

CREATE INDEX [IX_Cities_CreatedById] ON [Cities] ([CreatedById]);
GO

CREATE INDEX [IX_Cities_UpdatedById] ON [Cities] ([UpdatedById]);
GO

CREATE INDEX [IX_Countries_CreatedById] ON [Countries] ([CreatedById]);
GO

CREATE INDEX [IX_Countries_UpdatedById] ON [Countries] ([UpdatedById]);
GO

CREATE INDEX [IX_Courses_CreatedById] ON [Courses] ([CreatedById]);
GO

CREATE INDEX [IX_Courses_SchoolClassId] ON [Courses] ([SchoolClassId]);
GO

CREATE INDEX [IX_Courses_UpdatedById] ON [Courses] ([UpdatedById]);
GO

CREATE INDEX [IX_Enrollments_CourseId] ON [Enrollments] ([CourseId]);
GO

CREATE INDEX [IX_Enrollments_CreatedById] ON [Enrollments] ([CreatedById]);
GO

CREATE INDEX [IX_Enrollments_SchoolClassId] ON [Enrollments] ([SchoolClassId]);
GO

CREATE INDEX [IX_Enrollments_UpdatedById] ON [Enrollments] ([UpdatedById]);
GO

CREATE INDEX [IX_Genders_CreatedById] ON [Genders] ([CreatedById]);
GO

CREATE INDEX [IX_Genders_UpdatedById] ON [Genders] ([UpdatedById]);
GO

CREATE UNIQUE INDEX [IX_Nationalities_CountryId] ON [Nationalities] ([CountryId]);
GO

CREATE INDEX [IX_Nationalities_CreatedById] ON [Nationalities] ([CreatedById]);
GO

CREATE INDEX [IX_Nationalities_UpdatedById] ON [Nationalities] ([UpdatedById]);
GO

CREATE INDEX [IX_SchoolClassCourses_CourseId] ON [SchoolClassCourses] ([CourseId]);
GO

CREATE INDEX [IX_SchoolClassCourses_CreatedById] ON [SchoolClassCourses] ([CreatedById]);
GO

CREATE INDEX [IX_SchoolClassCourses_UpdatedById] ON [SchoolClassCourses] ([UpdatedById]);
GO

CREATE INDEX [IX_SchoolClasses_CreatedById] ON [SchoolClasses] ([CreatedById]);
GO

CREATE INDEX [IX_SchoolClasses_UpdatedById] ON [SchoolClasses] ([UpdatedById]);
GO

CREATE INDEX [IX_SchoolClassStudent_CreatedById] ON [SchoolClassStudent] ([CreatedById]);
GO

CREATE INDEX [IX_SchoolClassStudent_SchoolClassId] ON [SchoolClassStudent] ([SchoolClassId]);
GO

CREATE INDEX [IX_SchoolClassStudent_StudentId] ON [SchoolClassStudent] ([StudentId]);
GO

CREATE INDEX [IX_SchoolClassStudent_UpdatedById] ON [SchoolClassStudent] ([UpdatedById]);
GO

CREATE INDEX [IX_StudentCourses_CourseId] ON [StudentCourses] ([CourseId]);
GO

CREATE INDEX [IX_StudentCourses_CreatedById] ON [StudentCourses] ([CreatedById]);
GO

CREATE INDEX [IX_StudentCourses_UpdatedById] ON [StudentCourses] ([UpdatedById]);
GO

CREATE INDEX [IX_Students_BirthplaceId] ON [Students] ([BirthplaceId]);
GO

CREATE INDEX [IX_Students_CityId] ON [Students] ([CityId]);
GO

CREATE INDEX [IX_Students_CountryId] ON [Students] ([CountryId]);
GO

CREATE INDEX [IX_Students_CountryOfNationalityId] ON [Students] ([CountryOfNationalityId]);
GO

CREATE INDEX [IX_Students_CreatedById] ON [Students] ([CreatedById]);
GO

CREATE INDEX [IX_Students_GenderId] ON [Students] ([GenderId]);
GO

CREATE INDEX [IX_Students_SchoolClassId] ON [Students] ([SchoolClassId]);
GO

CREATE INDEX [IX_Students_UpdatedById] ON [Students] ([UpdatedById]);
GO

CREATE INDEX [IX_Students_UserId] ON [Students] ([UserId]);
GO

CREATE INDEX [IX_TeacherCourses_CourseId] ON [TeacherCourses] ([CourseId]);
GO

CREATE INDEX [IX_TeacherCourses_CreatedById] ON [TeacherCourses] ([CreatedById]);
GO

CREATE INDEX [IX_TeacherCourses_UpdatedById] ON [TeacherCourses] ([UpdatedById]);
GO

CREATE INDEX [IX_Teachers_BirthplaceId] ON [Teachers] ([BirthplaceId]);
GO

CREATE INDEX [IX_Teachers_CityId] ON [Teachers] ([CityId]);
GO

CREATE INDEX [IX_Teachers_CountryId] ON [Teachers] ([CountryId]);
GO

CREATE INDEX [IX_Teachers_CountryOfNationalityId] ON [Teachers] ([CountryOfNationalityId]);
GO

CREATE INDEX [IX_Teachers_CreatedById] ON [Teachers] ([CreatedById]);
GO

CREATE INDEX [IX_Teachers_GenderId] ON [Teachers] ([GenderId]);
GO

CREATE INDEX [IX_Teachers_UpdatedById] ON [Teachers] ([UpdatedById]);
GO

CREATE INDEX [IX_Teachers_UserId] ON [Teachers] ([UserId]);
GO

INSERT INTO [_MyMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230906163530_InitDB', N'7.0.10');
GO

COMMIT;
GO

