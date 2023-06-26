using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Helpers;

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


    // --- Course ---- //

    Course ToCourse(CourseViewModel courseViewModel, string? filePath,
        Guid fileStorageId, bool isNew);

    CourseViewModel ToCourseViewModel(Course course);


    // --- SchoolClass ---- //
    SchoolClass ToSchoolClass(SchoolClassViewModel schoolClassViewModel,
        string? filePath, Guid fileStorageId, bool isNew);

    SchoolClassViewModel ToSchoolClassViewModel(SchoolClass schoolClass);


    // --- Student ---- //
    Student ToStudent(StudentViewModel studentViewModel, string? filePath,
        Guid fileStorageId, bool isNew);

    StudentViewModel ToStudentViewModel(Student student);


    // --- Teacher ---- //
    Teacher ToTeacher(TeacherViewModel teacherViewModel,
        string? filePath, Guid fileStorageId, bool isNew);

    TeacherViewModel ToTeacherViewModel(Teacher teacher);
}