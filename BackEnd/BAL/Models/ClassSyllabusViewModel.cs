using System;
using System.Linq;
using DAL.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BAL.Models
{
  public class ClassSyllabusViewModel
  {
    public long? Id { get; set; }
    public string? Name { get; set; }
    public float? Version { get; set; }
    public string? Status { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public IEnumerable<ClassSessionViewModel>? ClassSession { get; set; }
  }
}


