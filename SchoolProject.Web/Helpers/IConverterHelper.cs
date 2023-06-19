using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities;

namespace SchoolProject.Web.Helpers;

public interface IConverterHelper
{
    Owner ToOwner(OwnerViewModel ownerViewModel, string? filePath, Guid fileStorageId, bool isNew);

    OwnerViewModel ToOwnerViewModel(Owner owner);

    Lessee ToLessee(LesseeViewModel lesseeViewModel, string? filePath, Guid fileStorageId, bool isNew);

    LesseeViewModel ToLesseeViewModel(Lessee lessee);




    Course ToCourse(CourseViewModel courseViewModel, string? filePath, Guid fileStorageId, bool isNew);

    CourseViewModel ToCourseViewModel(Course course);



    SchoolClass ToSchoolClass(SchoolClassViewModel schoolClassViewModel, string? filePath, Guid fileStorageId, bool isNew);

    SchoolClassViewModel ToSchoolClassViewModel(SchoolClass schoolClass);

    public DbSet<SchoolClass> SchoolClasses { get; set; }


    Student ToStudent(StudentViewModel studentViewModel, string? filePath, Guid fileStorageId, bool isNew);

    StudentViewModel ToStudentViewModel(Student student);
    public DbSet<Student> Students { get; set; }


    Enrollment ToEnrollment(EnrollmentViewModel enrollmentViewModel, string? filePath, Guid fileStorageId, bool isNew);

    EnrollmentViewModel ToEnrollmentViewModel(Enrollment enrollment);
    public DbSet<Teacher> Teachers { get; set; }
}