using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Helpers;

public class ConverterHelper : IConverterHelper
{
    // public Owner ToOwner(OwnerViewModel ownerViewModel,
    //     string? filePath, Guid fileStorageId, bool isNew)
    // {
    //     return new Owner
    //     {
    //         Id = isNew ? 0 : ownerViewModel.Id,
    //         Document = ownerViewModel.Document,
    //         FirstName = ownerViewModel.FirstName,
    //         LastName = ownerViewModel.LastName,
    //         ProfilePhotoUrl = filePath,
    //         ProfilePhotoId = fileStorageId,
    //         FixedPhone = ownerViewModel.FixedPhone,
    //         CellPhone = ownerViewModel.CellPhone,
    //         Address = ownerViewModel.Address,
    //         User = ownerViewModel.User
    //     };
    // }


    // public OwnerViewModel ToOwnerViewModel(Owner owner)
    // {
    //     return new OwnerViewModel
    //     {
    //         Id = owner.Id,
    //         Document = owner.Document,
    //         FirstName = owner.FirstName,
    //         LastName = owner.LastName,
    //         ProfilePhotoUrl = owner.ProfilePhotoUrl,
    //         ProfilePhotoId = owner.ProfilePhotoId,
    //         FixedPhone = owner.FixedPhone,
    //         CellPhone = owner.CellPhone,
    //         Address = owner.Address,
    //         User = owner.User
    //     };
    // }


    public Course ToCourse(
        CourseViewModel courseViewModel, string? filePath,
        Guid fileStorageId, bool isNew)
    {
        return new Course
        {
            Name = courseViewModel.Name,
            WorkLoad = courseViewModel.WorkLoad,
            Credits = courseViewModel.Credits,
            Enrollments = courseViewModel.Enrollments,
            Id = isNew
                ? 0
                : courseViewModel.Id,
            WasDeleted = courseViewModel.WasDeleted,
            CreatedBy = courseViewModel.CreatedBy,
            IdGuid = courseViewModel.IdGuid,
            CreatedAt = courseViewModel.CreatedAt
        };
    }


    public CourseViewModel ToCourseViewModel(Course course)
    {
        return new CourseViewModel
        {
            Name = course.Name,
            WorkLoad = course.WorkLoad,
            Credits = course.Credits,
            Enrollments = course.Enrollments,
            Id = course.Id,
            WasDeleted = course.WasDeleted,
            ImageFile = null,
            CreatedBy = course.CreatedBy,
            IdGuid = course.IdGuid,
            CreatedAt = course.CreatedAt,
        };
    }


    public SchoolClass ToSchoolClass(SchoolClassViewModel schoolClassViewModel,
        string? filePath, Guid fileStorageId, bool isNew)
    {
        throw new NotImplementedException();
    }


    public SchoolClassViewModel ToSchoolClassViewModel(SchoolClass schoolClass)
    {
        throw new NotImplementedException();
    }

    public Student ToStudent(StudentViewModel studentViewModel,
        string? filePath, Guid fileStorageId, bool isNew)
    {
        throw new NotImplementedException();
    }


    public StudentViewModel ToStudentViewModel(Student student)
    {
        throw new NotImplementedException();
    }


    public Teacher ToTeacher(TeacherViewModel teacherViewModel,
        string? filePath, Guid fileStorageId, bool isNew)
    {
        throw new NotImplementedException();
    }


    public TeacherViewModel ToTeacherViewModel(Teacher teacher)
    {
        throw new NotImplementedException();
    }
}