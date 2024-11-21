using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OT.Assessment.App.Models
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Data {get; set;}
        public int page {get; set;}
        public int PageSize {get; set;}
        public int Total {get; set;}
        public int TotalPages {get; set;}
    }
}