using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Data.Repositories.SchoolClasses;

/// <inheritdoc />
public interface ISchoolClassStudentRepository
    : IGenericRepository<CourseStudents>
{
}