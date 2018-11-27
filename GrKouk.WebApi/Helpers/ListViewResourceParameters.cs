namespace GrKouk.WebApi.Helpers
{
    public class ListViewResourceParameters
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

       // public string Genre { get; set; }
        
        public string SearchQuery { get; set; }

        public string OrderBy { get; set; } = "Id";

        public string Fields { get; set; }
    }
}
