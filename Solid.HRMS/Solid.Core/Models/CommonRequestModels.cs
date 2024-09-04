using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Core.Models
{
    internal class CommonRequestModels
    {
    }
    public class DateRangeRequestModels
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class CommonResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

    }
    public class PagedResponseModel
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
