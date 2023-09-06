CREATE TABLE IF NOT EXISTS `_MyMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) NOT NULL,
    `Name` varchar(256) NULL,
    `NormalizedName` varchar(256) NULL,
    `ConcurrencyStamp` longtext NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) NOT NULL,
    `FirstName` varchar(50) NOT NULL,
    `LastName` varchar(50) NOT NULL,
    `Address` varchar(100) NULL,
    `WasDeleted` tinyint(1) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
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
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) NOT NULL,
    `ProviderKey` varchar(255) NOT NULL,
    `ProviderDisplayName` longtext NULL,
    `UserId` varchar(255) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) NOT NULL,
    `RoleId` varchar(255) NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
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
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Countries_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Countries_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `Genders` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(20) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Genders_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Genders_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `SchoolClasses` (
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
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_SchoolClasses_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_SchoolClasses_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `Cities` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `CountryId` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
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
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Nationalities_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Nationalities_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Nationalities_Countries_CountryId` FOREIGN KEY (`CountryId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `Courses` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Code` varchar(7) NOT NULL,
    `Name` longtext NOT NULL,
    `Description` longtext NULL,
    `Hours` int NOT NULL,
    `CreditPoints` double NOT NULL,
    `ProfilePhotoId` char(36) NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `UpdatedById` varchar(255) NULL,
    `SchoolClassId` int NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Courses_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Courses_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Courses_SchoolClasses_SchoolClassId` FOREIGN KEY (`SchoolClassId`) REFERENCES `SchoolClasses` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `Students` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FirstName` longtext NOT NULL,
    `LastName` longtext NOT NULL,
    `Address` longtext NOT NULL,
    `PostalCode` longtext NOT NULL,
    `CityId` int NOT NULL,
    `CountryId` int NOT NULL,
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
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `UpdatedById` varchar(255) NULL,
    `SchoolClassId` int NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Students_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Cities_CityId` FOREIGN KEY (`CityId`) REFERENCES `Cities` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Countries_BirthplaceId` FOREIGN KEY (`BirthplaceId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Countries_CountryId` FOREIGN KEY (`CountryId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Countries_CountryOfNationalityId` FOREIGN KEY (`CountryOfNationalityId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_Genders_GenderId` FOREIGN KEY (`GenderId`) REFERENCES `Genders` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Students_SchoolClasses_SchoolClassId` FOREIGN KEY (`SchoolClassId`) REFERENCES `SchoolClasses` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `Teachers` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FirstName` longtext NOT NULL,
    `LastName` longtext NOT NULL,
    `Address` longtext NOT NULL,
    `PostalCode` longtext NOT NULL,
    `CityId` int NOT NULL,
    `CountryId` int NOT NULL,
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
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedAt` datetime(6) NULL,
    `UpdatedById` varchar(255) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Teachers_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Cities_CityId` FOREIGN KEY (`CityId`) REFERENCES `Cities` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Countries_BirthplaceId` FOREIGN KEY (`BirthplaceId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Countries_CountryId` FOREIGN KEY (`CountryId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Countries_CountryOfNationalityId` FOREIGN KEY (`CountryOfNationalityId`) REFERENCES `Countries` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Teachers_Genders_GenderId` FOREIGN KEY (`GenderId`) REFERENCES `Genders` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `SchoolClassCourses` (
    `SchoolClassId` int NOT NULL,
    `CourseId` int NOT NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    `Id` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    PRIMARY KEY (`SchoolClassId`, `CourseId`),
    CONSTRAINT `FK_SchoolClassCourses_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_SchoolClassCourses_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_SchoolClassCourses_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_SchoolClassCourses_SchoolClasses_SchoolClassId` FOREIGN KEY (`SchoolClassId`) REFERENCES `SchoolClasses` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `Enrollments` (
    `StudentId` int NOT NULL,
    `CourseId` int NOT NULL,
    `Grade` decimal(18,2) NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    `Id` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    `SchoolClassId` int NULL,
    PRIMARY KEY (`StudentId`, `CourseId`),
    CONSTRAINT `FK_Enrollments_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Enrollments_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Enrollments_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Enrollments_SchoolClasses_SchoolClassId` FOREIGN KEY (`SchoolClassId`) REFERENCES `SchoolClasses` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Enrollments_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `SchoolClassStudents` (
    `SchoolClassId` int NOT NULL,
    `StudentId` int NOT NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    `Id` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    PRIMARY KEY (`SchoolClassId`, `StudentId`),
    CONSTRAINT `FK_SchoolClassStudents_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_SchoolClassStudents_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_SchoolClassStudents_SchoolClasses_SchoolClassId` FOREIGN KEY (`SchoolClassId`) REFERENCES `SchoolClasses` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_SchoolClassStudents_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `StudentCourses` (
    `StudentId` int NOT NULL,
    `CourseId` int NOT NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    `Id` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    PRIMARY KEY (`StudentId`, `CourseId`),
    CONSTRAINT `FK_StudentCourses_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_StudentCourses_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_StudentCourses_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_StudentCourses_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `TeacherCourses` (
    `TeacherId` int NOT NULL,
    `CourseId` int NOT NULL,
    `CreatedById` varchar(255) NOT NULL,
    `UpdatedById` varchar(255) NULL,
    `Id` int NOT NULL,
    `IdGuid` char(36) NOT NULL DEFAULT (UUID()),
    `WasDeleted` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NULL,
    PRIMARY KEY (`TeacherId`, `CourseId`),
    CONSTRAINT `FK_TeacherCourses_AspNetUsers_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TeacherCourses_AspNetUsers_UpdatedById` FOREIGN KEY (`UpdatedById`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TeacherCourses_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TeacherCourses_Teachers_TeacherId` FOREIGN KEY (`TeacherId`) REFERENCES `Teachers` (`Id`) ON DELETE RESTRICT
);

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

CREATE INDEX `IX_Cities_CountryId` ON `Cities` (`CountryId`);

CREATE INDEX `IX_Cities_CreatedById` ON `Cities` (`CreatedById`);

CREATE INDEX `IX_Cities_UpdatedById` ON `Cities` (`UpdatedById`);

CREATE INDEX `IX_Countries_CreatedById` ON `Countries` (`CreatedById`);

CREATE INDEX `IX_Countries_UpdatedById` ON `Countries` (`UpdatedById`);

CREATE INDEX `IX_Courses_CreatedById` ON `Courses` (`CreatedById`);

CREATE INDEX `IX_Courses_SchoolClassId` ON `Courses` (`SchoolClassId`);

CREATE INDEX `IX_Courses_UpdatedById` ON `Courses` (`UpdatedById`);

CREATE INDEX `IX_Enrollments_CourseId` ON `Enrollments` (`CourseId`);

CREATE INDEX `IX_Enrollments_CreatedById` ON `Enrollments` (`CreatedById`);

CREATE INDEX `IX_Enrollments_SchoolClassId` ON `Enrollments` (`SchoolClassId`);

CREATE INDEX `IX_Enrollments_UpdatedById` ON `Enrollments` (`UpdatedById`);

CREATE INDEX `IX_Genders_CreatedById` ON `Genders` (`CreatedById`);

CREATE INDEX `IX_Genders_UpdatedById` ON `Genders` (`UpdatedById`);

CREATE UNIQUE INDEX `IX_Nationalities_CountryId` ON `Nationalities` (`CountryId`);

CREATE INDEX `IX_Nationalities_CreatedById` ON `Nationalities` (`CreatedById`);

CREATE INDEX `IX_Nationalities_UpdatedById` ON `Nationalities` (`UpdatedById`);

CREATE INDEX `IX_SchoolClassCourses_CourseId` ON `SchoolClassCourses` (`CourseId`);

CREATE INDEX `IX_SchoolClassCourses_CreatedById` ON `SchoolClassCourses` (`CreatedById`);

CREATE INDEX `IX_SchoolClassCourses_UpdatedById` ON `SchoolClassCourses` (`UpdatedById`);

CREATE INDEX `IX_SchoolClasses_CreatedById` ON `SchoolClasses` (`CreatedById`);

CREATE INDEX `IX_SchoolClasses_UpdatedById` ON `SchoolClasses` (`UpdatedById`);

CREATE INDEX `IX_SchoolClassStudents_CreatedById` ON `SchoolClassStudents` (`CreatedById`);

CREATE INDEX `IX_SchoolClassStudents_StudentId` ON `SchoolClassStudents` (`StudentId`);

CREATE INDEX `IX_SchoolClassStudents_UpdatedById` ON `SchoolClassStudents` (`UpdatedById`);

CREATE INDEX `IX_StudentCourses_CourseId` ON `StudentCourses` (`CourseId`);

CREATE INDEX `IX_StudentCourses_CreatedById` ON `StudentCourses` (`CreatedById`);

CREATE INDEX `IX_StudentCourses_UpdatedById` ON `StudentCourses` (`UpdatedById`);

CREATE INDEX `IX_Students_BirthplaceId` ON `Students` (`BirthplaceId`);

CREATE INDEX `IX_Students_CityId` ON `Students` (`CityId`);

CREATE INDEX `IX_Students_CountryId` ON `Students` (`CountryId`);

CREATE INDEX `IX_Students_CountryOfNationalityId` ON `Students` (`CountryOfNationalityId`);

CREATE INDEX `IX_Students_CreatedById` ON `Students` (`CreatedById`);

CREATE INDEX `IX_Students_GenderId` ON `Students` (`GenderId`);

CREATE INDEX `IX_Students_SchoolClassId` ON `Students` (`SchoolClassId`);

CREATE INDEX `IX_Students_UpdatedById` ON `Students` (`UpdatedById`);

CREATE INDEX `IX_Students_UserId` ON `Students` (`UserId`);

CREATE INDEX `IX_TeacherCourses_CourseId` ON `TeacherCourses` (`CourseId`);

CREATE INDEX `IX_TeacherCourses_CreatedById` ON `TeacherCourses` (`CreatedById`);

CREATE INDEX `IX_TeacherCourses_UpdatedById` ON `TeacherCourses` (`UpdatedById`);

CREATE INDEX `IX_Teachers_BirthplaceId` ON `Teachers` (`BirthplaceId`);

CREATE INDEX `IX_Teachers_CityId` ON `Teachers` (`CityId`);

CREATE INDEX `IX_Teachers_CountryId` ON `Teachers` (`CountryId`);

CREATE INDEX `IX_Teachers_CountryOfNationalityId` ON `Teachers` (`CountryOfNationalityId`);

CREATE INDEX `IX_Teachers_CreatedById` ON `Teachers` (`CreatedById`);

CREATE INDEX `IX_Teachers_GenderId` ON `Teachers` (`GenderId`);

CREATE INDEX `IX_Teachers_UpdatedById` ON `Teachers` (`UpdatedById`);

CREATE INDEX `IX_Teachers_UserId` ON `Teachers` (`UserId`);

INSERT INTO `_MyMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230906163156_InitDB', '7.0.10');

COMMIT;

