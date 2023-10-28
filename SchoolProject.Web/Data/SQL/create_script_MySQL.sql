CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) NOT NULL,
    `Name` varchar(256) NULL,
    `NormalizedName` varchar(256) NULL,
    `ConcurrencyStamp` longtext NULL,
    PRIMARY KEY (`Id`)
);


CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
);


CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`)
);


CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) NOT NULL,
    `ProviderKey` varchar(255) NOT NULL,
    `ProviderDisplayName` longtext NULL,
    `UserId` varchar(255) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`)
);


CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) NOT NULL,
    `RoleId` varchar(255) NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
);


CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) NOT NULL,
    `FirstName` varchar(50) NOT NULL,
    `LastName` varchar(50) NOT NULL,
    `Address` varchar(100) NULL,
    `WasDeleted` tinyint(1) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `GenderId` int NULL,
    `UserName` varchar(256) NULL,
    `NormalizedUserName` varchar(256) NULL,
    `Email` varchar(256) NULL,
    `NormalizedEmail` varchar(256) NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext NULL,
    `SecurityStamp` longtext NULL,
    `ConcurrencyStamp` longtext NULL,
    `PhoneNumber` longtext NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    PRIMARY KEY (`Id`)
);


CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) NOT NULL,
    `LoginProvider` varchar(255) NOT NULL,
    `Name` varchar(255) NOT NULL,
    `Value` longtext NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);


CREATE TABLE `Countries` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Countries_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Countries_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `Courses` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Code` varchar(7) NOT NULL,
    `Acronym` longtext NOT NULL,
    `Name` longtext NOT NULL,
    `QnqLevel` tinyint unsigned NOT NULL,
    `EqfLevel` tinyint unsigned NOT NULL,
    `StartDate` datetime(6) NOT NULL,
    `EndDate` datetime(6) NOT NULL,
    `StartHour` time(6) NOT NULL,
    `EndHour` time(6) NOT NULL,
    `Location` longtext NULL,
    `Type` longtext NULL,
    `Area` longtext NULL,
    `PriceForEmployed` decimal(10,2) NOT NULL,
    `PriceForUnemployed` decimal(10,2) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Courses_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Courses_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `Disciplines` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Code` varchar(7) NOT NULL,
    `Name` longtext NOT NULL,
    `Description` longtext NOT NULL,
    `Hours` int NOT NULL,
    `CreditPoints` double NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Disciplines_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Disciplines_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `Genders` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(20) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Genders_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Genders_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `Cities` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `CountryId` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Cities_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Cities_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Cities_Countries_CountryId` FOREIGN KEY (`CountryId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `Nationalities` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `CountryId` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Nationalities_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Nationalities_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Nationalities_Countries_CountryId` FOREIGN KEY (`CountryId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `CourseDisciplines` (
    `CourseId` int NOT NULL,
    `DisciplineId` int NOT NULL,
    `Id` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`CourseId`, `DisciplineId`),
    CONSTRAINT `FK_CourseDisciplines_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_CourseDisciplines_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_CourseDisciplines_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_CourseDisciplines_Disciplines_DisciplineId` FOREIGN KEY (`DisciplineId`) REFERENCES `Disciplines` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `Students` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FirstName` longtext NOT NULL,
    `LastName` longtext NOT NULL,
    `Address` longtext NOT NULL,
    `PostalCode` longtext NOT NULL,
    `CityId` int NOT NULL,
    `MobilePhone` longtext NOT NULL,
    `Email` longtext NOT NULL,
    `Active` tinyint(1) NOT NULL,
    `GenderId` int NOT NULL,
    `DateOfBirth` datetime(6) NOT NULL,
    `IdentificationNumber` longtext NOT NULL,
    `IdentificationType` longtext NOT NULL,
    `ExpirationDateIdentificationNumber` datetime(6) NOT NULL,
    `TaxIdentificationNumber` longtext NOT NULL,
    `CountryOfNationalityId` int NOT NULL,
    `BirthplaceId` int NOT NULL,
    `EnrollDate` datetime(6) NOT NULL,
    `UserId` varchar(255) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Students_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Cities_CityId` FOREIGN KEY (`CityId`) REFERENCES `Cities` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Countries_BirthplaceId` FOREIGN KEY (`BirthplaceId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Countries_CountryOfNationalityId` FOREIGN KEY (`CountryOfNationalityId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Genders_GenderId` FOREIGN KEY (`GenderId`) REFERENCES `Genders` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `Teachers` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FirstName` longtext NOT NULL,
    `LastName` longtext NOT NULL,
    `Address` longtext NOT NULL,
    `PostalCode` longtext NOT NULL,
    `CityId` int NOT NULL,
    `MobilePhone` longtext NOT NULL,
    `Email` longtext NOT NULL,
    `Active` tinyint(1) NOT NULL,
    `GenderId` int NOT NULL,
    `DateOfBirth` datetime(6) NOT NULL,
    `IdentificationNumber` longtext NOT NULL,
    `IdentificationType` longtext NOT NULL,
    `ExpirationDateIdentificationNumber` datetime(6) NOT NULL,
    `TaxIdentificationNumber` longtext NOT NULL,
    `CountryOfNationalityId` int NOT NULL,
    `BirthplaceId` int NOT NULL,
    `EnrollDate` datetime(6) NOT NULL,
    `UserId` varchar(255) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Teachers_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Cities_CityId` FOREIGN KEY (`CityId`) REFERENCES `Cities` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Countries_BirthplaceId` FOREIGN KEY (`BirthplaceId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Countries_CountryOfNationalityId` FOREIGN KEY (`CountryOfNationalityId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Genders_GenderId` FOREIGN KEY (`GenderId`) REFERENCES `Genders` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `CourseStudents` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CourseId` int NOT NULL,
    `StudentId` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CourseStudents_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_CourseStudents_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_CourseStudents_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_CourseStudents_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `Enrollments` (
    `StudentId` int NOT NULL,
    `DisciplineId` int NOT NULL,
    `Grade` decimal(18,2) NULL,
    `Absences` int NOT NULL,
    `Id` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    `CourseId` int NULL,
    PRIMARY KEY (`StudentId`, `DisciplineId`),
    CONSTRAINT `FK_Enrollments_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Enrollments_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Enrollments_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Enrollments_Disciplines_DisciplineId` FOREIGN KEY (`DisciplineId`) REFERENCES `Disciplines` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Enrollments_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `StudentDisciplines` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `StudentId` int NOT NULL,
    `DisciplineId` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_StudentDisciplines_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_StudentDisciplines_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_StudentDisciplines_Disciplines_DisciplineId` FOREIGN KEY (`DisciplineId`) REFERENCES `Disciplines` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_StudentDisciplines_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`Id`) ON DELETE RESTRICT
);


CREATE TABLE `TeacherDisciplines` (
    `TeacherId` int NOT NULL,
    `DisciplineId` int NOT NULL,
    `Id` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`TeacherId`, `DisciplineId`),
    CONSTRAINT `FK_TeacherDisciplines_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TeacherDisciplines_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TeacherDisciplines_Disciplines_DisciplineId` FOREIGN KEY (`DisciplineId`) REFERENCES `Disciplines` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TeacherDisciplines_Teachers_TeacherId` FOREIGN KEY (`TeacherId`) REFERENCES `Teachers` (`Id`) ON DELETE RESTRICT
);


CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);


CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);


CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);


CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);


CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);


CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);


CREATE INDEX `IX_AspNetUsers_GenderId` ON `AspNetUsers` (`GenderId`);


CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);


CREATE INDEX `IX_Cities_CountryId` ON `Cities` (`CountryId`);


CREATE INDEX `IX_Cities_CreatedById` ON `Cities` (`CreatedById`);


CREATE INDEX `IX_Cities_UpdatedById` ON `Cities` (`UpdatedById`);


CREATE INDEX `IX_Countries_CreatedById` ON `Countries` (`CreatedById`);


CREATE INDEX `IX_Countries_UpdatedById` ON `Countries` (`UpdatedById`);


CREATE INDEX `IX_CourseDisciplines_CreatedById` ON `CourseDisciplines` (`CreatedById`);


CREATE INDEX `IX_CourseDisciplines_DisciplineId` ON `CourseDisciplines` (`DisciplineId`);


CREATE INDEX `IX_CourseDisciplines_UpdatedById` ON `CourseDisciplines` (`UpdatedById`);


CREATE INDEX `IX_Courses_CreatedById` ON `Courses` (`CreatedById`);


CREATE INDEX `IX_Courses_UpdatedById` ON `Courses` (`UpdatedById`);


CREATE INDEX `IX_CourseStudents_CourseId` ON `CourseStudents` (`CourseId`);


CREATE INDEX `IX_CourseStudents_CreatedById` ON `CourseStudents` (`CreatedById`);


CREATE INDEX `IX_CourseStudents_StudentId` ON `CourseStudents` (`StudentId`);


CREATE INDEX `IX_CourseStudents_UpdatedById` ON `CourseStudents` (`UpdatedById`);


CREATE INDEX `IX_Disciplines_CreatedById` ON `Disciplines` (`CreatedById`);


CREATE INDEX `IX_Disciplines_UpdatedById` ON `Disciplines` (`UpdatedById`);


CREATE INDEX `IX_Enrollments_CourseId` ON `Enrollments` (`CourseId`);


CREATE INDEX `IX_Enrollments_CreatedById` ON `Enrollments` (`CreatedById`);


CREATE INDEX `IX_Enrollments_DisciplineId` ON `Enrollments` (`DisciplineId`);


CREATE INDEX `IX_Enrollments_UpdatedById` ON `Enrollments` (`UpdatedById`);


CREATE INDEX `IX_Genders_CreatedById` ON `Genders` (`CreatedById`);


CREATE INDEX `IX_Genders_UpdatedById` ON `Genders` (`UpdatedById`);


CREATE UNIQUE INDEX `IX_Nationalities_CountryId` ON `Nationalities` (`CountryId`);


CREATE INDEX `IX_Nationalities_CreatedById` ON `Nationalities` (`CreatedById`);


CREATE INDEX `IX_Nationalities_UpdatedById` ON `Nationalities` (`UpdatedById`);


CREATE INDEX `IX_StudentDisciplines_CreatedById` ON `StudentDisciplines` (`CreatedById`);


CREATE INDEX `IX_StudentDisciplines_DisciplineId` ON `StudentDisciplines` (`DisciplineId`);


CREATE INDEX `IX_StudentDisciplines_StudentId` ON `StudentDisciplines` (`StudentId`);


CREATE INDEX `IX_StudentDisciplines_UpdatedById` ON `StudentDisciplines` (`UpdatedById`);


CREATE INDEX `IX_Students_BirthplaceId` ON `Students` (`BirthplaceId`);


CREATE INDEX `IX_Students_CityId` ON `Students` (`CityId`);


CREATE INDEX `IX_Students_CountryOfNationalityId` ON `Students` (`CountryOfNationalityId`);


CREATE INDEX `IX_Students_CreatedById` ON `Students` (`CreatedById`);


CREATE INDEX `IX_Students_GenderId` ON `Students` (`GenderId`);


CREATE INDEX `IX_Students_UpdatedById` ON `Students` (`UpdatedById`);


CREATE INDEX `IX_Students_UserId` ON `Students` (`UserId`);


CREATE INDEX `IX_TeacherDisciplines_CreatedById` ON `TeacherDisciplines` (`CreatedById`);


CREATE INDEX `IX_TeacherDisciplines_DisciplineId` ON `TeacherDisciplines` (`DisciplineId`);


CREATE INDEX `IX_TeacherDisciplines_UpdatedById` ON `TeacherDisciplines` (`UpdatedById`);


CREATE INDEX `IX_Teachers_BirthplaceId` ON `Teachers` (`BirthplaceId`);


CREATE INDEX `IX_Teachers_CityId` ON `Teachers` (`CityId`);


CREATE INDEX `IX_Teachers_CountryOfNationalityId` ON `Teachers` (`CountryOfNationalityId`);


CREATE INDEX `IX_Teachers_CreatedById` ON `Teachers` (`CreatedById`);


CREATE INDEX `IX_Teachers_GenderId` ON `Teachers` (`GenderId`);


CREATE INDEX `IX_Teachers_UpdatedById` ON `Teachers` (`UpdatedById`);


CREATE INDEX `IX_Teachers_UserId` ON `Teachers` (`UserId`);


ALTER TABLE `AspNetUserClaims` ADD CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;


ALTER TABLE `AspNetUserLogins` ADD CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;


ALTER TABLE `AspNetUserRoles` ADD CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;


ALTER TABLE `AspNetUsers` ADD CONSTRAINT `FK_AspNetUsers_Genders_GenderId` FOREIGN KEY (`GenderId`) REFERENCES `Genders` (`Id`) ON DELETE RESTRICT;


