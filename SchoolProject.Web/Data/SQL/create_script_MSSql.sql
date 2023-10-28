CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
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
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey])
);
GO


CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Address] nvarchar(100) NULL,
    [WasDeleted] bit NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [GenderId] int NULL,
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
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Countries_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Countries_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Courses] (
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
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Courses_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Courses_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Disciplines] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(7) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Hours] int NOT NULL,
    [CreditPoints] float NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Disciplines] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Disciplines_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Disciplines_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Genders] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    [ProfilePhotoId] uniqueidentifier NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Genders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Genders_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Genders_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
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
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
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
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Nationalities] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Nationalities_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Nationalities_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Nationalities_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [CourseDisciplines] (
    [CourseId] int NOT NULL,
    [DisciplineId] int NOT NULL,
    [Id] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_CourseDisciplines] PRIMARY KEY ([CourseId], [DisciplineId]),
    CONSTRAINT [FK_CourseDisciplines_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CourseDisciplines_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CourseDisciplines_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CourseDisciplines_Disciplines_DisciplineId] FOREIGN KEY ([DisciplineId]) REFERENCES [Disciplines] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Students] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [PostalCode] nvarchar(max) NOT NULL,
    [CityId] int NOT NULL,
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
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Students_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Countries_BirthplaceId] FOREIGN KEY ([BirthplaceId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Countries_CountryOfNationalityId] FOREIGN KEY ([CountryOfNationalityId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Genders_GenderId] FOREIGN KEY ([GenderId]) REFERENCES [Genders] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Teachers] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [PostalCode] nvarchar(max) NOT NULL,
    [CityId] int NOT NULL,
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
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_Teachers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Teachers_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_Countries_BirthplaceId] FOREIGN KEY ([BirthplaceId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_Countries_CountryOfNationalityId] FOREIGN KEY ([CountryOfNationalityId]) REFERENCES [Countries] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Teachers_Genders_GenderId] FOREIGN KEY ([GenderId]) REFERENCES [Genders] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [CourseStudents] (
    [Id] int NOT NULL IDENTITY,
    [CourseId] int NOT NULL,
    [StudentId] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_CourseStudents] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CourseStudents_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CourseStudents_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CourseStudents_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CourseStudents_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Enrollments] (
    [StudentId] int NOT NULL,
    [DisciplineId] int NOT NULL,
    [Grade] decimal(18,2) NULL,
    [Absences] int NOT NULL,
    [Id] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [CourseId] int NULL,
    CONSTRAINT [PK_Enrollments] PRIMARY KEY ([StudentId], [DisciplineId]),
    CONSTRAINT [FK_Enrollments_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_Disciplines_DisciplineId] FOREIGN KEY ([DisciplineId]) REFERENCES [Disciplines] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [StudentDisciplines] (
    [Id] int NOT NULL IDENTITY,
    [StudentId] int NOT NULL,
    [DisciplineId] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_StudentDisciplines] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StudentDisciplines_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_StudentDisciplines_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_StudentDisciplines_Disciplines_DisciplineId] FOREIGN KEY ([DisciplineId]) REFERENCES [Disciplines] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_StudentDisciplines_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [TeacherDisciplines] (
    [TeacherId] int NOT NULL,
    [DisciplineId] int NOT NULL,
    [Id] int NOT NULL,
    [IdGuid] uniqueidentifier NOT NULL DEFAULT ((NEWSEQUENTIALID())),
    [WasDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    CONSTRAINT [PK_TeacherDisciplines] PRIMARY KEY ([TeacherId], [DisciplineId]),
    CONSTRAINT [FK_TeacherDisciplines_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TeacherDisciplines_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TeacherDisciplines_Disciplines_DisciplineId] FOREIGN KEY ([DisciplineId]) REFERENCES [Disciplines] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TeacherDisciplines_Teachers_TeacherId] FOREIGN KEY ([TeacherId]) REFERENCES [Teachers] ([Id]) ON DELETE NO ACTION
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


CREATE INDEX [IX_AspNetUsers_GenderId] ON [AspNetUsers] ([GenderId]);
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


CREATE INDEX [IX_CourseDisciplines_CreatedById] ON [CourseDisciplines] ([CreatedById]);
GO


CREATE INDEX [IX_CourseDisciplines_DisciplineId] ON [CourseDisciplines] ([DisciplineId]);
GO


CREATE INDEX [IX_CourseDisciplines_UpdatedById] ON [CourseDisciplines] ([UpdatedById]);
GO


CREATE INDEX [IX_Courses_CreatedById] ON [Courses] ([CreatedById]);
GO


CREATE INDEX [IX_Courses_UpdatedById] ON [Courses] ([UpdatedById]);
GO


CREATE INDEX [IX_CourseStudents_CourseId] ON [CourseStudents] ([CourseId]);
GO


CREATE INDEX [IX_CourseStudents_CreatedById] ON [CourseStudents] ([CreatedById]);
GO


CREATE INDEX [IX_CourseStudents_StudentId] ON [CourseStudents] ([StudentId]);
GO


CREATE INDEX [IX_CourseStudents_UpdatedById] ON [CourseStudents] ([UpdatedById]);
GO


CREATE INDEX [IX_Disciplines_CreatedById] ON [Disciplines] ([CreatedById]);
GO


CREATE INDEX [IX_Disciplines_UpdatedById] ON [Disciplines] ([UpdatedById]);
GO


CREATE INDEX [IX_Enrollments_CourseId] ON [Enrollments] ([CourseId]);
GO


CREATE INDEX [IX_Enrollments_CreatedById] ON [Enrollments] ([CreatedById]);
GO


CREATE INDEX [IX_Enrollments_DisciplineId] ON [Enrollments] ([DisciplineId]);
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


CREATE INDEX [IX_StudentDisciplines_CreatedById] ON [StudentDisciplines] ([CreatedById]);
GO


CREATE INDEX [IX_StudentDisciplines_DisciplineId] ON [StudentDisciplines] ([DisciplineId]);
GO


CREATE INDEX [IX_StudentDisciplines_StudentId] ON [StudentDisciplines] ([StudentId]);
GO


CREATE INDEX [IX_StudentDisciplines_UpdatedById] ON [StudentDisciplines] ([UpdatedById]);
GO


CREATE INDEX [IX_Students_BirthplaceId] ON [Students] ([BirthplaceId]);
GO


CREATE INDEX [IX_Students_CityId] ON [Students] ([CityId]);
GO


CREATE INDEX [IX_Students_CountryOfNationalityId] ON [Students] ([CountryOfNationalityId]);
GO


CREATE INDEX [IX_Students_CreatedById] ON [Students] ([CreatedById]);
GO


CREATE INDEX [IX_Students_GenderId] ON [Students] ([GenderId]);
GO


CREATE INDEX [IX_Students_UpdatedById] ON [Students] ([UpdatedById]);
GO


CREATE INDEX [IX_Students_UserId] ON [Students] ([UserId]);
GO


CREATE INDEX [IX_TeacherDisciplines_CreatedById] ON [TeacherDisciplines] ([CreatedById]);
GO


CREATE INDEX [IX_TeacherDisciplines_DisciplineId] ON [TeacherDisciplines] ([DisciplineId]);
GO


CREATE INDEX [IX_TeacherDisciplines_UpdatedById] ON [TeacherDisciplines] ([UpdatedById]);
GO


CREATE INDEX [IX_Teachers_BirthplaceId] ON [Teachers] ([BirthplaceId]);
GO


CREATE INDEX [IX_Teachers_CityId] ON [Teachers] ([CityId]);
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


ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
GO


ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
GO


ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
GO


ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_Genders_GenderId] FOREIGN KEY ([GenderId]) REFERENCES [Genders] ([Id]) ON DELETE NO ACTION;
GO


