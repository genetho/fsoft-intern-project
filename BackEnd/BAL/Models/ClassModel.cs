using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class ClassModel
    {
        public string? ClassName { get; set; }
        public string? ClassCode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? Attendee { get; set; }
        public string? Location { get; set; }
        public string? FSU { get; set; }
        public int? Duration { get; set; }

    }
}
