﻿using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Countries;

namespace SchoolProject.Web.Models.Roles
{
    public class RolesPagesViewModel
    {

        public required IQueryable<IdentityRole> Records { get; set; }


        public required int PageNumber { get; set; }


        public required int PageSize { get; set; }


        public required int TotalCount { get; set; }

    }
}