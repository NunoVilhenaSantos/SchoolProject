using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Users
{
    public class UsersPagesViewModel
    {

        public required IQueryable<User> Records { get; set; }


        public required int PageNumber { get; set; }


        public required int PageSize { get; set; }


        public required int TotalCount { get; set; }

    }
}
