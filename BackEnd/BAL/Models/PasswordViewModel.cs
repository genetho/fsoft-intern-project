using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class PasswordViewModel
    {
        public string Otp { get; set; }
        public string NewPassword { get; set; }
        public string ComrfirmPassword { get; set; }
    }
}
