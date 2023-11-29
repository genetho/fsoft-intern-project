using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IAttendeeTypeService
    {
        List<AttendeeTypeViewModel> GetAll();
        void Save();
        void SaveAsync();
    }
}
