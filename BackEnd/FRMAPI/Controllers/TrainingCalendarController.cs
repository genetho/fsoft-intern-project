using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using System.Data;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using BAL.Authorization;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TrainingCalendarController : ControllerBase
    {
        private readonly IFsucontactPointService _fsucontactPointService;
        private readonly IClassSelectedDateService _classSelectedDateService;
        private readonly IClassAdminService _classAdminService;
        private readonly IClassMentorService _classMentorService;
        private readonly IClassTraineeService _classTraineeService;
        private readonly IClassService _classService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFsoftUnitService _fSUService;
        private readonly ILocationService _locationService;
        private readonly IClassStatusService _classStatusService;
        private readonly IAttendeeTypeService _attendeeTypeService;

        public TrainingCalendarController(IFsucontactPointService fsucontactPointService, IUnitOfWork unitOfWork, IUserService userService,
            IClassSelectedDateService classSelectedDateService, IClassAdminService classAdminService, IClassMentorService classMentorService, IClassTraineeService classTraineeService,
            IClassService classService, IRoleService roleService, IFsoftUnitService fsoftUnitService, ILocationService locationService, IClassStatusService classStatusService, IAttendeeTypeService attendeeTypeService)
        {
            _fsucontactPointService = fsucontactPointService;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _classSelectedDateService = classSelectedDateService;
            _classAdminService = classAdminService;
            _classMentorService = classMentorService;
            _classTraineeService = classTraineeService;
            _roleService = roleService;
            _classService = classService;
            _fSUService = fsoftUnitService;
            _locationService = locationService;
            _classStatusService = classStatusService;
            _attendeeTypeService = attendeeTypeService;
        }


        #region GetCalendarsByDate
        /// <summary>
        /// Get list of calendars by date of the specific user
        /// </summary>
        /// <param name="userId">This is a user's ID</param>
        /// <param name="date">The date is selected by User</param>
        /// <param name="trainingCalendarFilter">The choosing filters that are used to search the wanted class calendar</param>
        /// <returns>A list of calendars on the selected date by specific user's role</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     "usetId": 1
        ///     "KeyWord": HCM22_FR_BA_02,
        ///     "Locations":["Ho Chi Minh", "Da Nang"],
        ///     "StartTime": 19/04/2001,
        ///     "EndTime": 14/10/2022,
        ///     "ClassTimes": ["Morning, "Noon"],
        ///     "Statuses: ["Planning"],
        ///     "Attendees": ["Intern", "Fresher"],
        ///     "IdFSU": 1,
        ///     "IdTrainer": 1
        /// </remarks>
        /// <response code="200">Returns a list of calendar</response>
        /// <response code="404">If the list is null</response>
        [HttpGet("CalendarsByDate/{userId}")]
        [ProducesResponseType(typeof(List<ClassCalenderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("View", "Full Access")]
        public async Task<IActionResult> GetCalendarsByDate([FromRoute] long userId, [FromQuery] string? date,
                [FromQuery] TrainingCalendarViewModel? trainingCalendarFilter)
        {
            try
            {
                Regex checkStr = new Regex("^\\d+$");
                if (checkStr.IsMatch(userId.ToString()) == false)
                {
                    throw new Exception("The UserId must be a positive number.");
                }
                if (trainingCalendarFilter.IdFSU != null && checkStr.IsMatch(trainingCalendarFilter.IdFSU.ToString()) == false)
                {
                    throw new Exception("The IdFSU must be a positive number.");
                }
                if (trainingCalendarFilter.IdTrainer != null && checkStr.IsMatch(trainingCalendarFilter.IdTrainer.ToString()) == false)
                {
                    throw new Exception("The IdTrainer must be a positive number.");
                }
                if (trainingCalendarFilter.Statuses.IsNullOrEmpty() == false && trainingCalendarFilter.Statuses.Any(x => checkStr.IsMatch(x.ToString()) == false))
                {
                    throw new Exception("There are invalid elements in the statuses. Please ensure that all elements are positive numbers.");
                }
                if (trainingCalendarFilter.Attendees.IsNullOrEmpty() == false && trainingCalendarFilter.Attendees.Any(x => checkStr.IsMatch(x.ToString()) == false))
                {
                    throw new Exception("There are invalid elements in the Attendees. Please ensure that all elements are positive numbers.");
                }
                Regex regex = new Regex("^\\d{4}-|/((0[1-9])|(1[012]))-|/((0[1-9]|[12]\\d)|3[01])$");
                if (date == null)
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    if (regex.IsMatch(date))
                    {
                        DateTime dateTime = DateTime.Parse(date);
                        date = dateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        throw new Exception("The date format is not suitable. Please input the following in the correct format: yyyy-MM-dd or yyyy/MM/dd");
                    }
                }
                if (trainingCalendarFilter.StartTime.IsNullOrEmpty() == false)
                {
                    if (regex.IsMatch(trainingCalendarFilter.StartTime))
                    {
                        DateTime dateTime;
                        DateTime.TryParse(trainingCalendarFilter.StartTime, out dateTime);
                    }
                    else
                    {
                        throw new Exception("The date format is not suitable. Please input the following in the correct format: yyyy-MM-dd or yyyy/MM/dd");
                    }
                }
                if (trainingCalendarFilter.EndTime.IsNullOrEmpty() == false)
                {
                    if (regex.IsMatch(trainingCalendarFilter.EndTime))
                    {
                        DateTime dateTime;
                        DateTime.TryParse(trainingCalendarFilter.EndTime, out dateTime);
                    }
                    else
                    {
                        throw new Exception("The date format is not suitable. Please input the following in the correct format: yyyy-MM-dd or yyyy/MM/dd");
                    }
                }

                if (trainingCalendarFilter.StartTime.IsNullOrEmpty() == false && trainingCalendarFilter.EndTime.IsNullOrEmpty() == false)
                {
                    if (DateTime.Parse(trainingCalendarFilter.StartTime).CompareTo(DateTime.Parse(trainingCalendarFilter.EndTime)) > 0)
                    {
                        throw new Exception("The start date or end date is not suitable. Please ensure that the start time precedes the end time.");
                    }
                }
                if (trainingCalendarFilter.KeyWord.IsNullOrEmpty()
                    && trainingCalendarFilter.Locations.IsNullOrEmpty()
                    && trainingCalendarFilter.TimeClasses.IsNullOrEmpty()
                    && trainingCalendarFilter.Statuses.IsNullOrEmpty()
                    && trainingCalendarFilter.Attendees.IsNullOrEmpty()
                    && trainingCalendarFilter.IdFSU == null
                    && trainingCalendarFilter.IdTrainer == null
                    && trainingCalendarFilter.StartTime.IsNullOrEmpty()
                    && trainingCalendarFilter.EndTime.IsNullOrEmpty())
                {
                    trainingCalendarFilter = null;
                }
                List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendars(userId, DateTime.Parse(date), trainingCalendarFilter);
                if (classCalenders.IsNullOrEmpty() == false)
                {
                    return Ok(new
                    {
                        Success = true,
                        Fiters = trainingCalendarFilter,
                        Data = classCalenders
                    });
                }
                return NotFound(new
                {
                    Success = false,
                    Filters = trainingCalendarFilter,
                    Message = "There are not any results."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("was not recognized as a valid DateTime"))
                {
                    return BadRequest(new
                    {
                        Sucess = false,
                        Filters = trainingCalendarFilter,
                        Message = "Date value is not valid date format."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Sucess = false,
                        Filters = trainingCalendarFilter,
                        Message = ex.Message
                    });
                }
            }
        }
        #endregion

        #region GetCalendarsByWeek
        /// <summary>
        /// Get list of calendars by date of a week by the specific user
        /// </summary>
        /// <param name="userId">This is a user's Id</param>
        /// <param name="date">The date of the week is selected by User</param>
        /// <param name="trainingCalendarFilter">The choosing filters that are used to search the wanted class calendar</param>
        /// <returns>A list of calendars on the selected date of a week by specific user's role</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     "userId": 1
        ///     "KeyWord": HCM22_FR_BA_02,
        ///     "Locations":["Ho Chi Minh", "Da Nang"],
        ///     "StartTime": 19/04/2001,
        ///     "EndTime": 14/10/2022,
        ///     "ClassTimes": ["Morning, "Noon"],
        ///     "Statuses: ["Planning"],
        ///     "Attendees": ["Intern", "Fresher"],
        ///     "IdFSU": 1,
        ///     "IdTrainer": 1
        /// </remarks>
        /// <response code="200">Returns a list of calendar</response>
        /// <response code="404">If the list is null</response>
        [HttpGet("CalendarsByWeek/{userId}")]
        [ProducesResponseType(typeof(List<ClassCalenderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("View", "Full Access")]
        public async Task<IActionResult> GetCalendarsByWeek([FromRoute] long userId, [FromQuery] string date,
            [FromQuery] TrainingCalendarViewModel? trainingCalendarFilter)
        {
            try
            {
                IEnumerable<ClassCalenderViewModel>? result = null;
                var existedUser = _userService.GetUserViewModelById(userId);

                if (existedUser != null)
                {
                    Regex checkStr = new Regex("^\\d+$");
                    if (checkStr.IsMatch(userId.ToString()) == false)
                    {
                        throw new Exception("The UserId must be a positive number.");
                    }
                    if (trainingCalendarFilter.IdFSU != null && checkStr.IsMatch(trainingCalendarFilter.IdFSU.ToString()) == false)
                    {
                        throw new Exception("The IdFSU must be a positive number.");
                    }
                    if (trainingCalendarFilter.IdTrainer != null && checkStr.IsMatch(trainingCalendarFilter.IdTrainer.ToString()) == false)
                    {
                        throw new Exception("The IdTrainer must be a positive number.");
                    }
                    if (trainingCalendarFilter.Statuses.IsNullOrEmpty() == false && trainingCalendarFilter.Statuses.Any(x => checkStr.IsMatch(x.ToString()) == false))
                    {
                        throw new Exception("There are invalid elements in the statuses. Please ensure that all elements are positive numbers.");
                    }
                    if (trainingCalendarFilter.Attendees.IsNullOrEmpty() == false && trainingCalendarFilter.Attendees.Any(x => checkStr.IsMatch(x.ToString()) == false))
                    {
                        throw new Exception("There are invalid elements in the Attendees. Please ensure that all elements are positive numbers.");
                    }
                    Regex regex = new Regex("^\\d{4}-|/((0[1-9])|(1[012]))-|/((0[1-9]|[12]\\d)|3[01])$");
                    if (date == null)
                    {
                        date = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        if (regex.IsMatch(date))
                        {
                            DateTime dateTime = DateTime.Parse(date);
                            date = dateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            throw new Exception("The date format is not suitable. Please input the following in the correct format: yyyy-MM-dd or yyyy/MM/dd");
                        }
                    }
                    if (trainingCalendarFilter.StartTime.IsNullOrEmpty() == false)
                    {
                        if (regex.IsMatch(trainingCalendarFilter.StartTime))
                        {
                            DateTime dateTime;
                            DateTime.TryParse(trainingCalendarFilter.StartTime, out dateTime);
                        }
                        else
                        {
                            throw new Exception("The date format is not suitable. Please input the following in the correct format: yyyy-MM-dd or yyyy/MM/dd");
                        }
                    }
                    if (trainingCalendarFilter.EndTime.IsNullOrEmpty() == false)
                    {
                        if (regex.IsMatch(trainingCalendarFilter.EndTime))
                        {
                            DateTime dateTime;
                            DateTime.TryParse(trainingCalendarFilter.EndTime, out dateTime);
                        }
                        else
                        {
                            throw new Exception("The date format is not suitable. Please input the following in the correct format: yyyy-MM-dd or yyyy/MM/dd");
                        }
                    }

                    if (trainingCalendarFilter.StartTime.IsNullOrEmpty() == false && trainingCalendarFilter.EndTime.IsNullOrEmpty() == false)
                    {
                        if (DateTime.Parse(trainingCalendarFilter.StartTime).CompareTo(DateTime.Parse(trainingCalendarFilter.EndTime)) > 0)
                        {
                            throw new Exception("The start date or end date is not suitable. Please ensure that the start time precedes the end time.");
                        }
                    }
                    if (trainingCalendarFilter.KeyWord.IsNullOrEmpty()
                    && trainingCalendarFilter.Locations.IsNullOrEmpty()
                    && trainingCalendarFilter.TimeClasses.IsNullOrEmpty()
                    && trainingCalendarFilter.Statuses.IsNullOrEmpty()
                    && trainingCalendarFilter.Attendees.IsNullOrEmpty()
                    && trainingCalendarFilter.IdFSU == null
                    && trainingCalendarFilter.IdTrainer == null
                    && trainingCalendarFilter.StartTime.IsNullOrEmpty()
                    && trainingCalendarFilter.EndTime.IsNullOrEmpty())
                    {
                        trainingCalendarFilter = null;
                    }
                    var loginUserRole = _roleService.GetRoleById(existedUser.IdRole);
                    switch (loginUserRole.Name)
                    {
                        case "Super Admin":
                            result = await _classSelectedDateService.GetCalendarsByWeek(_classService.GetClasses().ToList(), date, trainingCalendarFilter);
                            break;
                        case "Class Admin":
                            result = await _classSelectedDateService.GetCalendarsByWeek(await _classAdminService.GetClassesById(userId), date, trainingCalendarFilter);
                            break;
                        case "Trainer":
                            result = await _classSelectedDateService.GetCalendarsByWeek(await _classMentorService.GetClassesById(userId), date, trainingCalendarFilter);
                            break;
                        case "Student":
                            result = await _classSelectedDateService.GetCalendarsByWeek(await _classTraineeService.GetClassesById(userId), date, trainingCalendarFilter);
                            break;
                    }
                    return Ok(new
                    {
                        Success = true,
                        Filter = trainingCalendarFilter,
                        Data = result,
                    });
                }
                return NotFound(new
                {
                    Success = false,
                    Filters = trainingCalendarFilter,
                    Message = "The User's ID does not exist in the system."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("was not recognized as a valid DateTime"))
                {
                    return BadRequest(new
                    {
                        Sucess = false,
                        Filters = trainingCalendarFilter,
                        Message = "Date value is not valid date format."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Sucess = false,
                        Filters = trainingCalendarFilter,
                        Message = ex.Message
                    });
                }
            }

        }
        #endregion

        #region GetCalendarsByKeyword
        /// <summary>
        /// Get list of calendars by Keyword
        /// </summary>
        /// <param name="userId">This is a user's Id</param>
        /// <param name="trainingCalendar">The choosing filters that are used to search the wanted class calendar</param>
        /// <returns>A list of calendars with keywords</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET
        ///     "userId": 1
        ///     "KeyWord": HCM22_FR_BA_02,
        ///     "Locations":["Ho Chi Minh", "Da Nang"],
        ///     "StartTime": 19/04/2001,
        ///     "EndTime": 14/10/2022,
        ///     "ClassTimes": ["Morning, "Noon"],
        ///     "Statuses: ["Planning"],
        ///     "Attendees": ["Intern", "Fresher"],
        ///     "IdFSU": 1,
        ///     "IdTrainer": 1
        ///
        /// </remarks>
        /// <response code="200">Returns a list of calendar</response>
        /// <response code="404">If the list is null</response>
        [HttpGet("CalendarsByKeyword/{userId}")]
        [PermissionAuthorize("View", "Full Access")]
        [ProducesResponseType(typeof(List<ClassCalenderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCalendarsByKeyword([FromRoute] long userId,
            [FromQuery] TrainingCalendarViewModel? trainingCalendarFilter)
        {
            try
            {
                Regex checkStr = new Regex("^\\d+$");
                if (checkStr.IsMatch(userId.ToString()) == false)
                {
                    throw new Exception("The UserId must be a positive number.");
                }
                if (trainingCalendarFilter.IdFSU != null && checkStr.IsMatch(trainingCalendarFilter.IdFSU.ToString()) == false)
                {
                    throw new Exception("The IdFSU must be a positive number.");
                }
                if (trainingCalendarFilter.IdTrainer != null && checkStr.IsMatch(trainingCalendarFilter.IdTrainer.ToString()) == false)
                {
                    throw new Exception("The IdTrainer must be a positive number.");
                }
                if (trainingCalendarFilter.Statuses.IsNullOrEmpty() == false && trainingCalendarFilter.Statuses.Any(x => checkStr.IsMatch(x.ToString()) == false))
                {
                    throw new Exception("There are invalid elements in the statuses. Please ensure that all elements are positive numbers.");
                }
                if (trainingCalendarFilter.Attendees.IsNullOrEmpty() == false && trainingCalendarFilter.Attendees.Any(x => checkStr.IsMatch(x.ToString()) == false))
                {
                    throw new Exception("There are invalid elements in the Attendees. Please ensure that all elements are positive numbers.");
                }
                Regex regex = new Regex("^\\d{4}-|/((0[1-9])|(1[012]))-|/((0[1-9]|[12]\\d)|3[01])$");
                if (trainingCalendarFilter.StartTime.IsNullOrEmpty() == false)
                {
                    if (regex.IsMatch(trainingCalendarFilter.StartTime))
                    {
                        DateTime dateTime;
                        DateTime.TryParse(trainingCalendarFilter.StartTime, out dateTime);
                    }
                    else
                    {
                        throw new Exception("The date format is not suitable. Please input the following in the correct format: yyyy-MM-dd or yyyy/MM/dd");
                    }
                }
                if (trainingCalendarFilter.EndTime.IsNullOrEmpty() == false)
                {
                    if (regex.IsMatch(trainingCalendarFilter.EndTime))
                    {
                        DateTime dateTime;
                        DateTime.TryParse(trainingCalendarFilter.EndTime, out dateTime);
                    }
                    else
                    {
                        throw new Exception("The date format is not suitable. Please input the following in the correct format: yyyy-MM-dd or yyyy/MM/dd");
                    }
                }

                if (trainingCalendarFilter.StartTime.IsNullOrEmpty() == false && trainingCalendarFilter.EndTime.IsNullOrEmpty() == false)
                {
                    if (DateTime.Parse(trainingCalendarFilter.StartTime).CompareTo(DateTime.Parse(trainingCalendarFilter.EndTime)) > 0)
                    {
                        throw new Exception("The start date or end date is not suitable. Please ensure that the start time precedes the end time.");
                    }
                }

                if (trainingCalendarFilter.KeyWord.IsNullOrEmpty()
                    && trainingCalendarFilter.Locations.IsNullOrEmpty()
                    && trainingCalendarFilter.TimeClasses.IsNullOrEmpty()
                    && trainingCalendarFilter.Statuses.IsNullOrEmpty()
                    && trainingCalendarFilter.Attendees.IsNullOrEmpty()
                    && trainingCalendarFilter.IdFSU == null
                    && trainingCalendarFilter.IdTrainer == null
                    && trainingCalendarFilter.StartTime.IsNullOrEmpty()
                    && trainingCalendarFilter.EndTime.IsNullOrEmpty())
                {
                    trainingCalendarFilter = null;
                }
                List<ClassCalenderViewModel> classCalenders = await _classSelectedDateService.GetClassCalendarsFilter(userId, trainingCalendarFilter);

                if (classCalenders.IsNullOrEmpty() == false)
                {
                    return Ok(new
                    {
                        Success = true,
                        Data = classCalenders,
                        Filters = trainingCalendarFilter
                    });
                }
                return NotFound(new
                {
                    Success = false,
                    Filters = trainingCalendarFilter,
                    Message = "There are not any results.",
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("was not recognized as a valid DateTime"))
                {
                    return BadRequest(new
                    {
                        Sucess = false,
                        Filters = trainingCalendarFilter,
                        Message = "Date value is not valid date format."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Sucess = false,
                        Filters = trainingCalendarFilter,
                        Message = ex.Message
                    });
                }
            }
        }
        #endregion

        #region GetLocationByKeyword
        /// <summary>
        /// Get list of Locations by keyword 
        /// </summary>
        /// <param name="keyWord">This is a string that the user wants to use to search any locations</param>
        /// <returns>A list of Locations</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     "keyword": "Ha"
        ///
        /// </remarks>
        /// <response code="200">Returns a list of Locations</response>
        /// <response code="404">If the list is null</response>
        [HttpGet("LocationByKeyword")]
        [ProducesResponseType(typeof(List<LocationViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("View", "Full Access")]
        public async Task<IActionResult> GetLocationByKeyword([FromQuery] string keyWord)
        {
            List<LocationViewModel> locations = await _locationService.GetLocationByKeyword(keyWord);
            return Ok(new
            {
                Success = true,
                Keyword = keyWord,
                Data = locations
            });
        }
        #endregion

        #region GetFSUs
        /// <summary>
        /// Get list of FSUs 
        /// </summary>
        /// <returns>A list of FSUs</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET
        ///
        /// </remarks>
        /// <response code="200">Returns a list of FSUs</response>
        /// <response code="404">If the list is null</response>
        [HttpGet("FSUs")]
        [ProducesResponseType(typeof(List<FSUViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("View", "Full Access")]
        public async Task<IActionResult> GetFSUs()
        {

            List<FSUViewModel> result = await _fSUService.GetFSUsAsync();

            return Ok(new
            {
                Success = true,
                Data = result
            });
        }
        #endregion

        #region GetTrainers
        /// <summary>
        /// Get list of Trainers 
        /// </summary>
        /// <returns>A list of Trainers</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET
        ///
        /// </remarks>
        /// <response code="200">Returns a list of Trainers</response>
        /// <response code="404">If the list is null</response>
        [HttpGet("Trainers")]
        [ProducesResponseType(typeof(IEnumerable<TrainerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("View", "Full Access")]
        public async Task<IActionResult> GetTrainers()
        {
            IEnumerable<TrainerViewModel> result = await _userService.GetTrainers();

            return Ok(new
            {
                Sucess = true,
                Data = result
            });
        }
        #endregion

        #region GetTrainingClass
        /// <summary>
        /// Get details of training class by class's id 
        /// </summary>
        /// <param name="ClassId">This is a class's id</param>
        /// <returns>Details of training class</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     "id": 1
        ///
        /// </remarks>
        /// <response code="200">Returns details of training class</response>
        /// <response code="404">If the training class is null</response>
        [HttpGet("TrainingClass/{ClassId}")]
        [ProducesResponseType(typeof(ClassCalenderViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("View", "Full Access")]
        public async Task<IActionResult> GetTrainingClass([FromRoute] long ClassId)
        {
            try
            {
                ClassCalenderViewModel classCalenderViewModel = await _classService.GetClassCalender(ClassId);
                return Ok(new
                {
                    Success = true,
                    Data = classCalenderViewModel
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        #endregion

        #region GetStudentClasses
        /// <summary>
        /// Get List of student classes by student's id 
        /// </summary>
        /// <param name="studentId">This is a student's id</param>
        /// <returns>List of student classes</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     "studentId": 1
        ///
        /// </remarks>
        /// <response code="200">Returns List of student classes by student's id</response>
        /// <response code="404">If the list is null</response>
        [HttpGet("StudentClasses/{studentId}")]
        [PermissionAuthorize("View", "Full Access")]
        [ProducesResponseType(typeof(List<ClassTraineeViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentClasses([FromRoute] long studentId)
        {
            // test case 1: check user id existed ?
            // test case 2: check user id is student role is ? 
            // test case 3: check student has any classes ?
            try
            {
                List<ClassTraineeViewModel> classTrainees = await _classTraineeService.GetTraineeClasses(studentId);
                if (classTrainees == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "There aren't any classes studied by this trainee."
                    });
                }
                return Ok(new
                {
                    Success = true,
                    Data = classTrainees
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        #endregion

        #region GetTrainerClasses
        /// <summary>
        /// Get List of trainer classes by trainer's id 
        /// </summary>
        /// <param name="trainerId">This is a trainer's id</param>
        /// <returns>List of trainer classes</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     "trainerId": 1
        ///
        /// </remarks>
        /// <response code="200">Returns List of trainer classes by trainer's id </response>
        /// <response code="404">If the list is null</response>
        [HttpGet("TrainerClasses/{trainerId}")]
        [ProducesResponseType(typeof(List<ClassMentorViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [PermissionAuthorize("View", "Full Access")]
        public async Task<IActionResult> GetTrainerClasses(long trainerId)
        {
            try
            {
                // 1. Get list of trainer's classes
                List<ClassMentorViewModel> classes = await _classMentorService.GetMentorClasses(trainerId);
                if (classes == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "There aren't any classes taught by this mentor."
                    });
                }
                return Ok(new
                {
                    Success = true,
                    Data = classes
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        #endregion

        #region GetStatus
        /// <summary>
        /// Get list of status about the class
        /// </summary>
        /// <returns>List of status</returns>
        /// <response code="200">Returns List of statuses about the class </response>
        [HttpGet("GetStatuses")]
        [PermissionAuthorize("View", "Full Access")]
        [ProducesResponseType(typeof(List<ClassStatusViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatuses()
        {
            List<ClassStatusViewModel> classStatuses = _classStatusService.GetAll();
            return Ok(new
            {
                Success = true,
                Data = classStatuses
            });
        }
        #endregion


        #region GetClassAttendees
        /// <summary>
        /// Get list of attendee about the class
        /// </summary>
        /// <returns>List of attendee</returns>
        /// <response code="200">Returns List of attendees about the class </response>
        [HttpGet("GetClassAttendees")]
        //[PermissionAuthorize("View", "Full Access")]
        [ProducesResponseType(typeof(List<AttendeeTypeViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClassAttendees()
        {
            List<AttendeeTypeViewModel> classAttendees = _attendeeTypeService.GetAll();
            return Ok(new
            {
                Success = true,
                Data = classAttendees
            });
        }
        #endregion

    }
}