using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using Castle.Core.Internal;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using System.Text.RegularExpressions;

namespace BAL.Services.Implements
{
    public class ClassSelectedDateService : IClassSelectedDateService
    {
        private readonly IClassSelectedDateRepository _classSelectedDateRepository;
        private readonly IClassRepository _classRepository;
        private readonly IClassMentorRepository _classMentorRepository;
        private readonly IClassLocationRepository _classLocationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IClassStatusRepository _classStatusRepository;
        private readonly IAttendeeTypeRepository _attendeeTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private List<ClassCalenderViewModel> listClassCalenderViewModels = new();
        private IEnumerable<Class> listClass;
        private Class @class;
        private ClassLocation classLocation;
        private ClassStatus classStatus;
        private Location location;
        private AttendeeType attendeeType;
        private IUserRepository _userRepository;
        private IFsoftUnitRepository _fsoftUnitRepository;

        public ClassSelectedDateService
            (
            IClassSelectedDateRepository classSelectedDateRepository,
            IClassRepository classRepository,
            IClassMentorRepository classMentorRepository,
            IClassLocationRepository classLocationRepository,
            ILocationRepository locationRepository,
            IClassStatusRepository classStatusRepository,
            IAttendeeTypeRepository attendeeTypeRepository,
            IUnitOfWork unitOfWork, IMapper mapper,
            IUserRepository userRepository,
            IFsoftUnitRepository fsoftUnitRepository
            )
        {
            _classSelectedDateRepository = classSelectedDateRepository;
            _classRepository = classRepository;
            _classMentorRepository = classMentorRepository;
            _classLocationRepository = classLocationRepository;
            _locationRepository = locationRepository;
            _classStatusRepository = classStatusRepository;
            _attendeeTypeRepository = attendeeTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
            _fsoftUnitRepository = fsoftUnitRepository;
        }

        #region bhhiep
        public async Task<List<ClassCalenderViewModel>> GetCalendarsByWeek(List<Class> classes, string date, TrainingCalendarViewModel? trainingCalendarFilter)
        {
            if (classes.Count > 0)
            {
                if (trainingCalendarFilter != null)
                {
                    if (trainingCalendarFilter.KeyWord != null && classes.Count > 0)
                    {
                        classes = classes.Where(c => c.ClassCode.ToLower().Contains(trainingCalendarFilter.KeyWord.ToLower())).ToList();
                    }
                    if (trainingCalendarFilter.Locations != null && classes.Count > 0)
                    {
                        classes = classes.Where(c => c.Locations.Any(cl => trainingCalendarFilter.Locations.Any(tl => tl.ToLower().Equals(cl.Location.Name.ToLower())))).ToList();
                    }
                    if (trainingCalendarFilter.TimeClasses != null && classes.Count > 0)
                    {
                        List<Class> classesTemp = new List<Class>();
                        if (trainingCalendarFilter.TimeClasses.Any(tc => tc.ToLower().Equals("Morning".ToLower())))
                        {
                            classesTemp.AddRange(classes.Where(c => c.StartTimeLearning <= TimeSpan.Parse("12:00:00")).ToList());
                        }
                        if (trainingCalendarFilter.TimeClasses.Any(tc => tc.ToLower().Equals("Noon".ToLower())))
                        {
                            classesTemp.AddRange(classes.Where(c => c.StartTimeLearning >= TimeSpan.Parse("13:00:00")
                                                                 && c.StartTimeLearning <= TimeSpan.Parse("17:00:00")).ToList());
                        }
                        if (trainingCalendarFilter.TimeClasses.Any(tc => tc.ToLower().Equals("Night".ToLower())))
                        {
                            classesTemp.AddRange(classes.Where(c => c.StartTimeLearning >= TimeSpan.Parse("18:00:00")).ToList());
                        }
                        classes = classesTemp;
                    }
                    if (trainingCalendarFilter.Statuses != null && classes.Count > 0)
                    {
                        foreach (var status in trainingCalendarFilter.Statuses)
                        {
                            if ((await _classStatusRepository.GetClassStatusById(status)) == null)
                            {
                                throw new Exception("Statuses fields contains the id status that does not exist in the system.");
                            }
                        }
                        classes = classes.Where(c => trainingCalendarFilter.Statuses.Any(s => c.ClassStatus.Id.Equals(s))).ToList();
                    }
                    if (trainingCalendarFilter.Attendees != null && classes.Count > 0)
                    {
                        foreach (var attendee in trainingCalendarFilter.Attendees)
                        {
                            if (_attendeeTypeRepository.GetById(attendee) == null)
                            {
                                throw new Exception("Attendee fields contains the id status that does not exist in the system.");
                            }
                        }
                        classes = classes.Where(c => trainingCalendarFilter.Attendees.Any(a => c.AttendeeType.Id.Equals(a))).ToList();
                    }
                    if (trainingCalendarFilter.IdFSU != null && classes.Count > 0)
                    {
                        if (_fsoftUnitRepository.GetById(trainingCalendarFilter.IdFSU) == null)
                        {
                            throw new Exception("Id FSU does not exist in the system.");
                        }
                        classes = classes.Where(c => c.IdFSU == trainingCalendarFilter.IdFSU).ToList();
                    }
                    if (trainingCalendarFilter.IdTrainer != null && classes.Count > 0)
                    {
                        if ((await _userRepository.GetById(trainingCalendarFilter.IdTrainer.Value)) == null)
                        {
                            throw new Exception("Id Trainer does not exist in the system.");
                        }
                        if ((await _userRepository.GetById(trainingCalendarFilter.IdTrainer.Value)).IdRole != 3)
                        {
                            throw new Exception("Id Trainer is not suitable for trainer role.");
                        }
                        classes = classes.Where(c => c.ClassMentors.Any(cm => cm.IdUser == trainingCalendarFilter.IdTrainer)).ToList();
                    }
                }

                if (classes.Any())
                {
                    //Get the week of the selected date
                    DateTime parseDate = DateTime.Parse(date);
                    int currentDayofWeek = (int)parseDate.DayOfWeek;
                    IEnumerable<DateTime> DaysOfWeek = Enumerable.Range(-currentDayofWeek, 7).Select(days => parseDate.AddDays(days));
                    //Get the week of the selected date

                    var filteredClasses = (await _classSelectedDateRepository.GetSelectedDatesQueryAsync())
                        .Where(csd => classes.Select(c => c.Id).Any(oci => oci == csd.IdClass) && DaysOfWeek.Any(ad => ad.Equals(csd.ActiveDate)))
                        .OrderBy(csd => csd.ActiveDate)
                        .ToList();

                    if (trainingCalendarFilter != null)
                    {
                        if (trainingCalendarFilter.StartTime != null && filteredClasses.Count > 0)
                        {
                            filteredClasses = filteredClasses.Where(csd => csd.ActiveDate.Date.CompareTo(DateTime.Parse(trainingCalendarFilter.StartTime)) >= 0).ToList();
                        }
                        if (trainingCalendarFilter.EndTime != null && filteredClasses.Count > 0)
                        {
                            filteredClasses = filteredClasses.Where(csd => csd.ActiveDate.Date.CompareTo(DateTime.Parse(trainingCalendarFilter.EndTime)) <= 0).ToList();
                        }
                    }

                    List<ClassCalenderViewModel> classCalenderViewModels = _mapper.Map<List<ClassCalenderViewModel>>(filteredClasses);
                    MapSelectedDateClass(filteredClasses, classCalenderViewModels);
                    return classCalenderViewModels;
                }
                else throw new Exception("There is no class matched with the conditions of the filter");
            }
            else throw new Exception("There is no class currently");
        }
        #endregion

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveAsync()
        {
            _unitOfWork.commitAsync();
        }

        #region Group 5 - GetCalendarsByDate
        public async Task<List<ClassCalenderViewModel>> GetClassCalendarsFilter(long userID, TrainingCalendarViewModel? trainingCalendarFilter)
        {
            User user = await _userRepository.GetUserAsync(userID);
            if (user != null)
            {
                List<ClassCalenderViewModel> classCalenders = null;
                List<ClassSelectedDate> classes = await _classSelectedDateRepository.GetSelectedDatesQueryAsync();
                if (trainingCalendarFilter != null)
                {
                    classes = await GetClassCalendarsByFilter(userID, trainingCalendarFilter, classes);
                }
                switch (user.Role.Name)
                {
                    case "Super Admin":
                        {
                            classCalenders = _mapper.Map<List<ClassCalenderViewModel>>(classes);
                            MapSelectedDateClass(classes, classCalenders);
                            break;
                        }
                    case "Class Admin":
                        {
                            List<ClassSelectedDate> cls = new List<ClassSelectedDate>();
                            foreach (var item in classes)
                            {
                                foreach (var ca in item.Class.ClassAdmins)
                                {
                                    if (ca.IdUser == userID)
                                    {
                                        cls.Add(item);
                                    }
                                }
                            }
                            classCalenders = _mapper.Map<List<ClassCalenderViewModel>>(cls);
                            MapSelectedDateClass(cls, classCalenders);
                            break;
                        }
                    case "Trainer":
                        {
                            List<ClassSelectedDate> cls = new List<ClassSelectedDate>();
                            foreach (var item in classes)
                            {
                                foreach (var ca in item.Class.ClassMentors)
                                {
                                    if (ca.IdUser == userID)
                                    {
                                        cls.Add(item);
                                    }
                                }
                            }
                            classCalenders = _mapper.Map<List<ClassCalenderViewModel>>(cls);
                            MapSelectedDateClass(cls, classCalenders);
                            break;
                        }
                    case "Student":
                        {
                            List<ClassSelectedDate> cls = new List<ClassSelectedDate>();
                            foreach (var item in classes)
                            {
                                foreach (var ca in item.Class.ClassTrainees)
                                {
                                    if (ca.IdUser == userID)
                                    {
                                        cls.Add(item);
                                    }
                                }
                            }
                            classCalenders = _mapper.Map<List<ClassCalenderViewModel>>(cls);
                            MapSelectedDateClass(cls, classCalenders);
                            break;
                        }
                }
                return classCalenders;
            }
            else
            {
                throw new Exception("The user does not exist in the system.");
            }
        }
        private async Task<List<ClassSelectedDate>> GetClassCalendarsByFilter(long userID, TrainingCalendarViewModel? trainingCalendarFilter, List<ClassSelectedDate> classes)
        {
            DateTime? startTime = null;
            DateTime? endTime = null;
            if (trainingCalendarFilter.StartTime.IsNullOrEmpty() == false)
            {
                startTime = DateTime.Parse(trainingCalendarFilter.StartTime);
                startTime = DateTime.Parse(startTime?.ToString("yyyy-MM-dd"));
            }
            if (trainingCalendarFilter.EndTime.IsNullOrEmpty() == false)
            {
                endTime = DateTime.Parse(trainingCalendarFilter.EndTime);
                endTime = DateTime.Parse(endTime?.ToString("yyyy-MM-dd"));
            }
            if (trainingCalendarFilter.StartTime.IsNullOrEmpty() == false && trainingCalendarFilter.EndTime.IsNullOrEmpty() == false)
            {
                if (DateTime.Parse(trainingCalendarFilter.EndTime).CompareTo(DateTime.Parse(trainingCalendarFilter.StartTime)) < 0)
                {
                    throw new Exception("Start and End Time are not suitable.");
                }
                classes = await _classSelectedDateRepository.GetSelectedDatesQueryAsync();
                classes = classes.Where(x => x.ActiveDate.Date.CompareTo(startTime) >= 0 && x.ActiveDate.Date.CompareTo(endTime) <= 0).ToList();
            }
            else if (trainingCalendarFilter.EndTime.IsNullOrEmpty() == false && trainingCalendarFilter.StartTime.IsNullOrEmpty() == true)
            {
                classes = await _classSelectedDateRepository.GetSelectedDatesQueryAsync();
                classes = classes.Where(x => x.ActiveDate.Date.CompareTo(endTime) <= 0).ToList();
            }
            else if (trainingCalendarFilter.EndTime.IsNullOrEmpty() == true && trainingCalendarFilter.StartTime.IsNullOrEmpty() == false)
            {
                classes = await _classSelectedDateRepository.GetSelectedDatesQueryAsync();
                classes = classes.Where(x => x.ActiveDate.Date.CompareTo(startTime) >= 0).ToList();
            }

            if (string.IsNullOrWhiteSpace(trainingCalendarFilter.KeyWord) == false)
            {
                classes = classes.Where(a => a.Class.ClassCode.Contains(trainingCalendarFilter.KeyWord)).ToList();
            }
            if (trainingCalendarFilter.Locations.IsNullOrEmpty() == false && trainingCalendarFilter.Locations.Length > 0)
            {
                classes = classes.Where(c => c.Class.Locations.Any(cl => trainingCalendarFilter.Locations.Any(tl => tl.ToLower().Equals(cl.Location.Name.ToLower())))).ToList();
            }
            if (trainingCalendarFilter.IdFSU != null)
            {
                if (_fsoftUnitRepository.GetById(trainingCalendarFilter.IdFSU) == null)
                {
                    throw new Exception("Id FSU does not exist in the system.");
                }
                classes = classes.Where(b => b.Class.IdFSU == trainingCalendarFilter.IdFSU).ToList();
            }
            if (trainingCalendarFilter.IdTrainer != null)
            {
                if ((await _userRepository.GetById(trainingCalendarFilter.IdTrainer.Value)) == null)
                {
                    throw new Exception("Id Trainer does not exist in the system.");
                }
                if ((await _userRepository.GetById(trainingCalendarFilter.IdTrainer.Value)).IdRole != 3)
                {
                    throw new Exception("Id Trainer is not suitable for trainer role.");
                }
                classes = classes.Where(c => c.Class.ClassMentors.Where(x => x.IdUser == trainingCalendarFilter.IdTrainer).IsNullOrEmpty() == false).ToList();
            }
            if (trainingCalendarFilter.TimeClasses.IsNullOrEmpty() == false && trainingCalendarFilter.TimeClasses.Length > 0)
            {
                List<ClassSelectedDate> classesTemp = new List<ClassSelectedDate>();
                if (trainingCalendarFilter.TimeClasses.Any(tc => tc.ToLower().Equals("Morning".ToLower())))
                {
                    classesTemp.AddRange(classes.Where(c => c.Class.StartTimeLearning >= TimeSpan.Parse("8:00:00") && c.Class.EndTimeLearing <= TimeSpan.Parse("12:00:00")).ToList());
                }
                if (trainingCalendarFilter.TimeClasses.Any(tc => tc.ToLower().Equals("Noon".ToLower())))
                {
                    classesTemp.AddRange(classes.Where(c => c.Class.StartTimeLearning >= TimeSpan.Parse("13:00:00")
                                                         && c.Class.EndTimeLearing <= TimeSpan.Parse("17:00:00")).ToList());
                }
                if (trainingCalendarFilter.TimeClasses.Any(tc => tc.ToLower().Equals("Night".ToLower())))
                {
                    classesTemp.AddRange(classes.Where(c => c.Class.StartTimeLearning >= TimeSpan.Parse("18:00:00") &&
                                                        c.Class.EndTimeLearing <= TimeSpan.Parse("22:00:00")).ToList());
                }
                classes = classesTemp;
            }

            if (trainingCalendarFilter.Statuses.IsNullOrEmpty() == false)
            {
                foreach (var status in trainingCalendarFilter.Statuses)
                {
                    if ((await _classStatusRepository.GetClassStatusById(status)) == null)
                    {
                        throw new Exception("Status fields contains the id status that does not exist in the system.");
                    }
                }
                classes = classes.Where(c => trainingCalendarFilter.Statuses.Any(s => c.Class.ClassStatus.Id.Equals(s))).ToList();
            }
            if (trainingCalendarFilter.Attendees.IsNullOrEmpty() == false)
            {

                foreach (var attendee in trainingCalendarFilter.Attendees)
                {
                    if (_attendeeTypeRepository.GetById(attendee) == null)
                    {
                        throw new Exception("Attendee fields contains the id Attendee that does not exist in the system.");
                    }
                }
                classes = classes.Where(c => trainingCalendarFilter.Attendees.Any(a => c.Class.AttendeeType.Id.Equals(a))).ToList();
            }
            return classes;
        }

        public async Task<List<ClassCalenderViewModel>> GetClassCalendars(long userID, DateTime date, TrainingCalendarViewModel? trainingCalendarFilter)
        {
            User user = await _userRepository.GetUserAsync(userID);
            if (user != null)
            {
                List<ClassCalenderViewModel> classCalenders = null;
                List<ClassSelectedDate> classes = await _classSelectedDateRepository.GetSelectedDatesQueryAsync();
                classes = classes.Where(x => x.ActiveDate.Date.Equals(date)).ToList();
                if (trainingCalendarFilter != null)
                {
                    classes = await GetClassCalendarsByFilter(userID, trainingCalendarFilter, classes);
                }

                switch (user.Role.Name)
                {
                    case "Super Admin":
                        {
                            classCalenders = _mapper.Map<List<ClassCalenderViewModel>>(classes);
                            MapSelectedDateClass(classes, classCalenders);
                            break;
                        }
                    case "Class Admin":
                        {
                            List<ClassSelectedDate> cls = new List<ClassSelectedDate>();
                            foreach (var item in classes)
                            {
                                foreach (var ca in item.Class.ClassAdmins)
                                {
                                    if (ca.IdUser == userID)
                                    {
                                        cls.Add(item);
                                    }
                                }
                            }
                            classCalenders = _mapper.Map<List<ClassCalenderViewModel>>(cls);
                            MapSelectedDateClass(cls, classCalenders);
                            break;
                        }
                    case "Trainer":
                        {
                            List<ClassSelectedDate> cls = new List<ClassSelectedDate>();
                            foreach (var item in classes)
                            {
                                foreach (var ca in item.Class.ClassMentors)
                                {
                                    if (ca.IdUser == userID)
                                    {
                                        cls.Add(item);
                                    }
                                }
                            }
                            classCalenders = _mapper.Map<List<ClassCalenderViewModel>>(cls);
                            MapSelectedDateClass(cls, classCalenders);
                            break;
                        }
                    case "Student":
                        {
                            List<ClassSelectedDate> cls = new List<ClassSelectedDate>();
                            foreach (var item in classes)
                            {
                                foreach (var ca in item.Class.ClassTrainees)
                                {
                                    if (ca.IdUser == userID)
                                    {
                                        cls.Add(item);
                                    }
                                }
                            }
                            classCalenders = _mapper.Map<List<ClassCalenderViewModel>>(cls);
                            MapSelectedDateClass(cls, classCalenders);
                            break;
                        }
                }
                return classCalenders;
            }
            else
            {
                throw new Exception("The user does not exist in the system.");
            }
        }

        private void MapSelectedDateClass(List<ClassSelectedDate> classes, List<ClassCalenderViewModel> classCalenders)
        {
            foreach (var cl in classes)
            {
                foreach (var clvm in classCalenders)
                {
                    if (cl.IdClass == clvm.Id)
                    {
                        clvm.Locations = _mapper.Map<IEnumerable<ClassLocationViewModel>>(cl.Class.Locations);
                        clvm.ClassMentors = _mapper.Map<IEnumerable<TrainerViewModel>>(cl.Class.ClassMentors);
                        clvm.ClassAdmins = _mapper.Map<IEnumerable<AdminViewModel>>(cl.Class.ClassAdmins);
                    }
                }
            }
        }
        #endregion

        public ClassCalenderViewModel GetByIdClass(long idClass, DateTime? date)
        {
            return _mapper.Map<ClassCalenderViewModel>(_classSelectedDateRepository.GetByIdClass(idClass, date));
        }

        public ClassCalenderViewModel GetByIdClassFilter(long idClass, long idClassFilter, DateTime? date)
        {
            return _mapper.Map<ClassCalenderViewModel>(_classSelectedDateRepository.GetByIdClassFilter(idClass, idClassFilter, date));
        }
    }
}