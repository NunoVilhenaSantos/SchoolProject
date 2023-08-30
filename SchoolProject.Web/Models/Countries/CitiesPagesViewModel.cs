using SchoolProject.Web.Data.Entities.Countries;

namespace SchoolProject.Web.Models.Countries
{
    public class CitiesPagesViewModel
    {

        public required IQueryable<City> Records { get; set; }


        public required int PageNumber { get; set; }


        public required int PageSize { get; set; }


        public required int TotalCount { get; set; }

    }
}
