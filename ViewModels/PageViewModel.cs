using System;

namespace lavAspMvclast.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public PageViewModel(int count, int pagenumber, int pagesize)
        {
            PageNumber = pagenumber;
            TotalPages = (int)Math.Ceiling(count / (double)pagesize);
        }
        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }
        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }
    }
}
