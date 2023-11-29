using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface IClassSelectedDateService
    {
        ClassCalenderViewModel GetByIdClass(long idClass, DateTime? date);
        ClassCalenderViewModel GetByIdClassFilter(long idClass, long idClassFilter, DateTime? date);
        Task<List<ClassCalenderViewModel>> GetClassCalendars(long userID, DateTime date, TrainingCalendarViewModel? trainingCalendarFilter);
        Task<List<ClassCalenderViewModel>> GetClassCalendarsFilter(long userID, TrainingCalendarViewModel? trainingCalendarFilter);
        void Save();
        void SaveAsync();
        Task<List<ClassCalenderViewModel>> GetCalendarsByWeek(List<Class> classes, string date, TrainingCalendarViewModel? trainingCalendarFilter);
    }
}
