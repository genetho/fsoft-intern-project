using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BAL.Models
{
  public class ClassSessionViewModel
  {
    public long? Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<ClassUnitViewModel>? Unit { get; set; }
  }
}