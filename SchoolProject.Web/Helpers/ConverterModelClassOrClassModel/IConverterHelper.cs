using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Seeders;
using SchoolProject.Web.Models.Account;
using SchoolProject.Web.Models.Countries;
using SchoolProject.Web.Models.Courses;
using SchoolProject.Web.Models.Disciplines;
using SchoolProject.Web.Models.Students;
using SchoolProject.Web.Models.Teachers;

namespace SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;

/// <summary>
/// </summary>
public interface IConverterHelper
{
    // Owner ToOwner(OwnerViewModel ownerViewModel, string? filePath,
    //     Guid fileStorageId, bool isNew);
    //
    // OwnerViewModel ToOwnerViewModel(Owner owner);
    //
    // Lessee ToLessee(LesseeViewModel lesseeViewModel, string? filePath,
    //     Guid fileStorageId, bool isNew);
    //
    // LesseeViewModel ToLesseeViewModel(Lessee lessee);


    // --- Courses ---- //


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="filePath"></param>
    /// <param name="fileStorageId"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    Course ToCourse(CourseViewModel model, string? filePath,
        Guid fileStorageId, bool isNew);

    /// <summary>
    /// </summary>
    /// <param name="course"></param>
    /// <returns></returns>
    CourseViewModel ToCourseViewModel(Course course);


    // --- Disciplines ---- //
    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="filePath"></param>
    /// <param name="fileStorageId"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    Discipline ToDiscipline(DisciplinesViewModel model,
        string? filePath, Guid fileStorageId, bool isNew);


    /// <summary>
    /// </summary>
    /// <param name="discipline"></param>
    /// <returns></returns>
    DisciplinesViewModel ToDisciplineViewModel(Discipline discipline);


    // --- Student ---- //


    /// <summary>
    /// </summary>
    /// <param name="studentViewModel"></param>
    /// <param name="filePath"></param>
    /// <param name="fileStorageId"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    Student ToStudent(StudentViewModel studentViewModel, string? filePath,
        Guid fileStorageId, bool isNew);

    /// <summary>
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    StudentViewModel ToStudentViewModel(Student student);


    // --- Teacher ---- //

    /// <summary>
    /// </summary>
    /// <param name="teacherViewModel"></param>
    /// <param name="filePath"></param>
    /// <param name="fileStorageId"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    Teacher ToTeacher(TeacherViewModel teacherViewModel,
        string? filePath, Guid fileStorageId, bool isNew);


    /// <summary>
    /// </summary>
    /// <param name="teacher"></param>
    /// <returns></returns>
    TeacherViewModel ToTeacherViewModel(Teacher teacher);


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    AppUser ViewModelToUser(AppUserViewModel model, bool isNew);

    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="appUser"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    AppUser ViewModelToUser(AppUserViewModel model, AppUser appUser,
        bool isNew);

    /// <summary>
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    AppUserViewModel UserToViewModel(AppUser appUser, string role);

    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <param name="imageId"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    Country ToCountry(CountryViewModel model, Guid imageId, bool isNew);

    /// <summary>
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    CountryViewModel ToCountryViewModel(Country country);

    /// <summary>
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="address"></param>
    /// <param name="email"></param>
    /// <param name="cellPhone"></param>
    /// <param name="role"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    AppUser AddUser(
        string firstName, string lastName, string address,
        string email, string cellPhone, string role,
        string password = SeedDb.DefaultPassword);

    /// <summary>
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Teacher ToTeacherFromUser(AppUser user);


    /// <summary>
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Student ToStudentFromUser(AppUser user);
}