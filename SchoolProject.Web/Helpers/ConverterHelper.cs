using SchoolProject.Web.Data.Entities;
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


    public Course ToCourse(CourseViewModel courseViewModel, string? filePath,
        Guid fileStorageId, bool isNew)
    {
        return new Course
        {
            Name = courseViewModel.Name,
            WorkLoad = courseViewModel.WorkLoad,
            Credits = courseViewModel.Credits,
            StudentsCount = courseViewModel.StudentsCount,
            Id = isNew ? 0 : courseViewModel.Id,
            WasDeleted = courseViewModel.WasDeleted
        };
    }


    public CourseViewModel ToCourseViewModel(Course course)
    {
        return new CourseViewModel
        {
            Name = course.Name,
            WorkLoad = course.WorkLoad,
            Credits = course.Credits,
            StudentsCount = course.StudentsCount,
            Id = course.Id,
            WasDeleted = course.WasDeleted,
            ImageFile = null
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