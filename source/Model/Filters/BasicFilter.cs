using API.Model.PagedList;

namespace API.Model.Filters
{
    public class BasicFilter : PagedListFilter
    {
        public BasicFilter(BasicFilter dto)
        {
            PageIndex = dto.PageIndex < 1 ? 1 : dto.PageIndex;
            PageSize = dto.PageSize;
            SearchTerm = dto.SearchTerm;
        }

        public BasicFilter() { }

    }
}
