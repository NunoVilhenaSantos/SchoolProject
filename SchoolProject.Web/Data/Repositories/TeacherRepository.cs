﻿using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    protected TeacherRepository(DataContextMSSQL dataContext) :
        base(dataContext)
    {
    }
}