using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Models.Disciplines;
using SchoolProject.Web.Models.SchoolClasses;
using SchoolProject.Web.Models.Students;
using SchoolProject.Web.Models.Teachers;

namespace SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;

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


    // --- Discipline ---- //

    Discipline ToCourse(DisciplinesViewModel courseViewModel, string? filePath,
        Guid fileStorageId, bool isNew);

    DisciplinesViewModel ToCourseViewModel(Discipline course);


    // --- Discipline ---- //
    Course ToSchoolClass(SchoolClassViewModel schoolClassViewModel,
        string? filePath, Guid fileStorageId, bool isNew);

    SchoolClassViewModel ToSchoolClassViewModel(Course schoolClass);


    // --- Student ---- //
    Student ToStudent(StudentViewModel studentViewModel, string? filePath,
        Guid fileStorageId, bool isNew);

    StudentViewModel ToStudentViewModel(Student student);


    // --- Teacher ---- //
    Teacher ToTeacher(TeacherViewModel teacherViewModel,
        string? filePath, Guid fileStorageId, bool isNew);

    TeacherViewModel ToTeacherViewModel(Teacher teacher);
}