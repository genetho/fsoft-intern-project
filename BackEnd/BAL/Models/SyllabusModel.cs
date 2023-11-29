using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class SyllabusModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public int? Duration { get; set; }
        public string? OutputStandard { get; set; }
        public int? Status { get; set; }
    }
}
