﻿using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Models.Disciplines;
using SchoolProject.Web.Models.SchoolClasses;
using SchoolProject.Web.Models.Students;
using SchoolProject.Web.Models.Teachers;

namespace SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;

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


    public Discipline ToCourse(
        DisciplinesViewModel courseViewModel, string? filePath,
        Guid fileStorageId, bool isNew)
    {
        return new Discipline
        {
            Code = courseViewModel.Code,
            Name = courseViewModel.Name,
            Hours = courseViewModel.Hours,
            CreditPoints = courseViewModel.CreditPoints,
            Enrollments = courseViewModel.Enrollments,
            Id = isNew
                ? 0
                : courseViewModel.Id,
            WasDeleted = courseViewModel.WasDeleted,
            CreatedBy = courseViewModel.CreatedBy,
            IdGuid = courseViewModel.IdGuid,
            CreatedAt = courseViewModel.CreatedAt,
            ProfilePhotoId = courseViewModel.ProfilePhotoId
        };
    }


    public DisciplinesViewModel ToCourseViewModel(Discipline course)
    {
        return new DisciplinesViewModel
        {
            Code = course.Code,
            Name = course.Name,
            Hours = course.Hours,
            CreditPoints = course.CreditPoints,
            Enrollments = course.Enrollments,
            Id = course.Id,
            WasDeleted = course.WasDeleted,
            ImageFile = null,
            CreatedBy = course.CreatedBy,
            IdGuid = course.IdGuid,
            CreatedAt = course.CreatedAt,
            ProfilePhotoId = course.ProfilePhotoId
        };
    }


    public Course ToSchoolClass(SchoolClassViewModel schoolClassViewModel,
        string? filePath, Guid fileStorageId, bool isNew)
    {
        throw new NotImplementedException();
    }


    public SchoolClassViewModel ToSchoolClassViewModel(Course schoolClass)
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