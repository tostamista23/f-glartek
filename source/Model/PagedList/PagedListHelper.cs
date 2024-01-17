using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Model.PagedList
{
    public class PagedListHelper
    {
        public static PagedListResult<IEnumerable<T>> CreatePagedResponse<T>(List<T> pagedData, int pageIndex, int pageSize, long totalRecords)
        {
            if (pageSize == 0) { pageSize = (int)totalRecords; }
            if (pageIndex == 0) { pageIndex = 1; }

            var totalPages = totalRecords == 0 || pageSize == 0 ? 0 : totalRecords / (double)pageSize;
            int roundedTotalPages = totalPages == 0 ? 0 : Convert.ToInt32(Math.Ceiling(totalPages));
            var response = new PagedListResult<IEnumerable<T>>(pagedData, pageIndex, pageSize)
            {
                TotalPages = roundedTotalPages,
                TotalRecords = totalRecords,
            };
            return response;
        }

        public static PagedListResult<T> CreatePagedResponse<T>(T pagedData, int pageIndex, int pageSize, long totalRecords)
        {
            if (pageSize == 0) { pageSize = (int)totalRecords; }
            if (pageIndex == 0) { pageIndex = 1; }

            var totalPages = totalRecords == 0 || pageSize == 0 ? 0 : totalRecords / (double)pageSize;
            int roundedTotalPages = totalPages == 0 ? 0 : Convert.ToInt32(Math.Ceiling(totalPages));
            var response = new PagedListResult<T>(pagedData, pageIndex, pageSize)
            {
                TotalPages = roundedTotalPages,
                TotalRecords = totalRecords,
            };
            return response;
        }
    }
}
