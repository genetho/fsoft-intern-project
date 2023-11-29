using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BAL.Models
{
  public class ClassAttendeeViewModel
  {
    public string? Role { get; set; }    
    public long? Id { get; set; }
    public string? FullName { get; set; }
    public byte[]? Image { get; set; }

  }
}