using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xUnitTest.Attributes;

namespace xUnitTest.UnitTests
{
    [TestCaseOrderer("xUnitTest.PriorityOrderer", "xUnitTest")]
    [Collection("xUnitTest")]
    public class ClassSelectedDateUnitTest : IClassFixture<DependencyInjection>
    {
        private ServiceProvider _provider;
        private IClassSelectedDateService _classSelectedDateService;
        private IClassService _classService;
        private IClassAdminService _classAdminService;
        private IClassMentorService _classMentorService;
        private IClassTraineeService _classTraineeService;
        private ILocationService _locationService;
        private IUserService _userService;
        private IClassStatusService _classStatusService;
        private IAttendeeTypeService _attendeeTypeService;
        private IServiceScope _scope;
        private IClassRepository _classRepository;


        public ClassSelectedDateUnitTest(DependencyInjection dependencyInjection)
        {
            _provider = dependencyInjection.provider;
            _scope = _provider.CreateScope();
            _classSelectedDateService = _scope.ServiceProvider.GetService<IClassSelectedDateService>();
            _classAdminService = _scope.ServiceProvider.GetService<IClassAdminService>();
            _classService = _scope.ServiceProvider.GetService<IClassService>();
            _classMentorService = _scope.ServiceProvider.GetService<IClassMentorService>();
            _classTraineeService = _scope.ServiceProvider.GetService<IClassTraineeService>();
            _locationService = _scope.ServiceProvider.GetService<ILocationService>();
            _userService = _scope.ServiceProvider.GetService<IUserService>();
            _classStatusService = _scope.ServiceProvider.GetService<IClassStatusService>();
            _attendeeTypeService = _scope.ServiceProvider.GetService<IAttendeeTypeService>();
            _classRepository = _scope.ServiceProvider.GetService<IClassRepository>();

        }

        #region Prepare Data
        // data class
        [Theory, TestPriority(-4)]
        [CreateClassJsonFileData("..//..//..//TestSamples//Create_Class_Example.json")]
        public void PrepareData_ForClassTable(Class @class)
        {
            foreach (var item in @class.ClassSelectedDates)
            {
                if(item.ActiveDate.CompareTo(DateTime.Parse("2022/12/1")) == 0)
                {
                    item.ActiveDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                }
            }
            _classRepository.CreateClassForImport(@class);
        }
        #endregion

        #region GetClassCalendarByDate _ SuperAdmin Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithRightStatuses_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithWrongStatuses_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] classStatuses = { 1, 2, 3, 9, 15 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = await Assert.ThrowsAsync<Exception>(() => _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter));
            Assert.Equal("Status fields contains the id status that does not exist in the system.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithRightAttendees_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithWrongAttendees_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] attendees = { 1, 2, 3, 9 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = await Assert.ThrowsAsync<Exception>(() => _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter));
            Assert.Equal("Attendee fields contains the id Attendee that does not exist in the system.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithRightIdFSU_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithWrongIdFSU_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long fsuId = -1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = await Assert.ThrowsAsync<Exception>(() => _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter));
            Assert.Equal("Id FSU does not exist in the system.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithRightIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithWrongIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = -1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = await Assert.ThrowsAsync<Exception>(() => _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter));
            Assert.Equal("Id Trainer does not exist in the system.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithIdTrainerWithoutTrainerRole_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 4;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = await Assert.ThrowsAsync<Exception>(() => _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter));

            Assert.Equal("Id Trainer is not suitable for trainer role.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetClassCalendarByDate _ AdminClass Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_ByClassAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetClassCalendarByDate _ Trainer Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetClassCalendarByDate _ Trainee Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByDate_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(todayStr), trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetCalendarByWeek _ SuperAdmin Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] locations = { "Ftown 1" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ftown 1" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetCalendarByWeek _ AdminClass Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] locations = { "Ftown 1" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 12, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 31).ToString("yyyy-MM-dd");
            string[] locations = { "Ftown 1" };
            long[] classStatuses = { 1 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.True(classCalenders.Count > 0);
        }
        #endregion

        #region GetCalendarByWeek _ Trainer Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] locations = { "Ftown 1" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ftown 1" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetCalendarByWeek _ Trainee Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] locations = { "Ftown 1" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByWeek_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ftown 1" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), todayStr, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetCalendarsByKeyword _ SuperAdmin Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_BySuperAdmin()
        {
            //Arrange: prepare the data to test
            long userId = 1;
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetCalendarsByKeyword _ AdminClass Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_ByAdminClass()
        {
            //Arrange: prepare the data to test
            long userId = 2;
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetCalendarsByKeyword _ Trainer Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainer()
        {
            //Arrange: prepare the data to test
            long userId = 3;
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetCalendarsByKeyword _ Trainee Role
        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_LackOfFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, null);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithKeyword_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = "HCM",
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithLocations_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Locations = locations,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithStartTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                StartTime = startTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithEndTime_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                EndTime = endTimeStr,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithTimeClass_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            string[] timeClasses = { "Morning", "Noon" };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                TimeClasses = timeClasses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }


        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithStatuses_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Statuses = classStatuses,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithAttendees_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            long[] attendees = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                Attendees = attendees,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithIdFSU_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            long fsuId = 1;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdFSU = fsuId,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFiltersWithIdTrainer_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            long trainerID = 3;
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                IdTrainer = trainerID,
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }

        [Fact, TestPriority(0)]
        public async Task GetClassCalendarsByKeyword_HasFilters_ShouldReturnListClassCalendar_WhenDataFound_ByTrainee()
        {
            //Arrange: prepare the data to test
            long userId = 4;
            long trainerID = 3;
            string keyword = "HCM";
            long fsuId = 1;
            long[] attendees = { 1, 2 };
            string[] timeClasses = { "Morning", "Noon" };
            string endTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string startTimeStr = new DateTime(2022, 10, 30).ToString("yyyy-MM-dd");
            string[] locations = { "Ha noi", "Ho Chi Minh" };
            long[] classStatuses = { 1, 2 };
            TrainingCalendarViewModel trainingCalendarFilter = new TrainingCalendarViewModel()
            {
                KeyWord = keyword,
                Locations = locations,
                StartTime = startTimeStr,
                EndTime = endTimeStr,
                Attendees = attendees,
                TimeClasses = timeClasses,
                Statuses = classStatuses,
                IdFSU = fsuId,
                IdTrainer = trainerID
            };
            //Act: test
            List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);
            //Assert: compare the actual result and expect result
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetLocationByKeyword
        [Fact, TestPriority(0)]
        public async Task GetLocationByKeyword_ShouldReturnLocations_WhenDataFound()
        {
            //Arrange
            string keyword = "Ha Noi";
            //Act
            List<LocationViewModel> classCalenders = await _locationService.GetLocationByKeyword(keyword);
            //Assert
            Assert.NotNull(classCalenders);
        }
        #endregion

        #region GetTrainers
        [Fact, TestPriority(0)]
        public async Task GetTrainers_ShouldReturnClassCalendars_WhenDataFound()
        {
            //Arrange
            //Act
            IEnumerable<TrainerViewModel> classAdmins = await _userService.GetTrainers();
            //Assert
            Assert.NotNull(classAdmins);
        }
        #endregion

        #region GetTrainingClass
        [Fact, TestPriority(0)]
        public async Task GetTrainingClass_ShouldReturnClassCalendars_WhenDataFound()
        {
            //Arrange
            long classId = 1;
            //Act
            ClassCalenderViewModel classCalenderViewModel = await _classService.GetClassCalender(classId);
            //Assert
            Assert.NotNull(classCalenderViewModel);
        }

        [Fact, TestPriority(0)]
        public async Task GetTrainingClass_ShouldReturnException_WhenClassIdNotExist()
        {
            //Arrange
            long classId = -1;
            //Act
            //ClassCalenderViewModel classCalenderViewModel = await _classService.GetClassCalender(classId);
            //Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _classService.GetClassCalender(classId));
            Assert.Equal("The class's Id does not exist in the system.", ex.Message);
        }
        #endregion

        #region GetStudentClasses
        [Fact, TestPriority(0)]
        public async Task GetStudentClasses_WithStudentIdNotExist_ShouldReturnException()
        {
            //Arrange
            long traineeId = -1;
            //Act
            //Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _classTraineeService.GetTraineeClasses(traineeId));
            Assert.Equal("The User's ID does not exist in the system.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task GetStudentClasses_WithtraineeIdNotTraineeRole_ShouldReturnException()
        {
            //Arrange
            long traineeId = 3;
            //Act
            //Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _classTraineeService.GetTraineeClasses(traineeId));
            Assert.Equal("The User's ID is not suitable trainee role.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task GetStudentClasses_ShouldReturnException_WhenDataFound()
        {
            //Arrange
            long traineeId = 4;
            //Act
            List<ClassTraineeViewModel> traineeClasses = await _classTraineeService.GetTraineeClasses(traineeId);
            //Assert
            Assert.NotNull(traineeClasses);
        }
        #endregion

        #region GetTrainerClasses
        [Fact, TestPriority(0)]
        public async Task GetTrainerClasses_ShouldReturnTrainerClasses_WhenDataFound()
        {
            //Arrange
            long trainerId = 3;
            //Act
            List<ClassMentorViewModel> classes = await _classMentorService.GetMentorClasses(trainerId);
            //Assert
            Assert.NotNull(classes);
        }

        [Fact, TestPriority(0)]
        public async Task GetTrainerClasses_WithTrainerIdNotExist_ShouldReturnException_()
        {
            //Arrange
            long trainerId = -1;
            //Act
            //Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _classMentorService.GetMentorClasses(trainerId));
            Assert.Equal("The User's ID does not exist in the system.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task GetTrainerClasses_WithTrainerIdNotTrainerRole_ShouldReturnException_()
        {
            //Arrange
            long trainerId = 2;
            //Act
            //Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _classMentorService.GetMentorClasses(trainerId));
            Assert.Equal("The User's ID is not suitable trainer role.", ex.Message);
        }
        #endregion

        #region GetStatuses
        [Fact, TestPriority(0)]
        public void GetStatuses_ShouldReturnClassStatus_WhenDataFound()
        {
            //Arrange
            //Act
            List<ClassStatusViewModel> classStatuses = _classStatusService.GetAll();
            //Assert
            Assert.NotNull(classStatuses);
        }
        #endregion

        #region GetClassAttendees
        [Fact, TestPriority(0)]
        public void GetClassAttendees_ShouldReturnClassStatus_WhenDataFound()
        {
            //Arrange
            //Act
            List<AttendeeTypeViewModel> classAttendees = _attendeeTypeService.GetAll();
            //Assert
            Assert.NotNull(classAttendees);
        }
        #endregion
    }
}