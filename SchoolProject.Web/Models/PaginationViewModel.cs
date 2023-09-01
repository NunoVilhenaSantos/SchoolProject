using System.Collections;

namespace SchoolProject.Web.Models
{
    public class PaginationViewModel<T> 
    {
        public List<T> Records { get; set; }

        
        public int PageNumber { get; set; }

        
        public int PageSize { get; set; }
        

        public int TotalCount { get; set; }



        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);



    }

}
