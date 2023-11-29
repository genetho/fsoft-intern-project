using System;
using System;
using AutoMapper;
using BAL.Models;
using System.Linq;
using System.Text;
using System.Linq;
using System.Text;
using System.Linq;
using System.Text;
using DAL.Entities;
using DAL.Infrastructure;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Generic;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace BAL.Services.Implements
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private ITrainingProgramRepository _trainingProgramRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ICurriculumRepository _curriculumRepository;
        private IHistoryTrainingProgramRepository _historyTrainingProgramRepository;
        private IUserRepository _userRepository;
        private ISyllabusRepository _syllabusRepository;
        private ISyllabusService _syllabusService;
        private ISessionRepository _sessionRepository;
        private IUnitRepository _unitRepository;
        private ILessonRepository _lessonRepository;

        public TrainingProgramService(ITrainingProgramRepository trainingProgramRepository,
            IUnitOfWork unitOfWork, IHistoryTrainingProgramRepository historyTrainingProgramRepository,
            ICurriculumRepository curriculumRepository, IMapper mapper, IUserRepository userRepository, 
            ISyllabusRepository syllabusRepository, ISyllabusService syllabusService, ISessionRepository sessionRepository,
            IUnitRepository unitRepository, ILessonRepository lessonRepository)
        {
            _trainingProgramRepository = trainingProgramRepository;
            _unitOfWork = unitOfWork;
            _historyTrainingProgramRepository = historyTrainingProgramRepository;
            _curriculumRepository = curriculumRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _syllabusRepository = syllabusRepository;
            _syllabusService = syllabusService;
            this._sessionRepository = sessionRepository;
            this._unitRepository = unitRepository;
            this._lessonRepository = lessonRepository;
        }

        public TrainingProgram CreateTrainingProgram(ProgramViewModel newProgram)
        {
            var namePro = _trainingProgramRepository.GetTraingProgramAll();

            foreach (var name in namePro)
            {
                if (name.Name.Equals(newProgram.name))
                {
                    throw new Exception("Training program name is already exist in system.");
                }
            }
            if (newProgram.name.Length > 50)
            {
                throw new Exception("Training program name must less than or equal 50 character.");
            }
            if (newProgram.name.Trim().Equals(""))
            {
                throw new Exception("name field cannot be left blank.");
            }
            List<Curriculum> curriculums = new List<Curriculum>();
            TrainingProgram trainingProgram = new TrainingProgram

            {
                Name = newProgram.name,
                Status = 1,
            };


            foreach (var syllabus in newProgram.syllabi)
            {
                if (_syllabusRepository.GetById(syllabus.idSyllabus) == null)
                {
                    throw new Exception("Syllabus doesn't exist in the system.");
                }

                Curriculum curriculum = new Curriculum
                {
                    IdProgram = trainingProgram.Id,
                    IdSyllabus = syllabus.idSyllabus,
                    NumberOrder = syllabus.numberOrder,
                };
                foreach (var cur in curriculums)
                {

                    if (cur.IdSyllabus == syllabus.idSyllabus)
                    {
                        throw new Exception("Id syllabus is duplicated.");
                    }
                }
                curriculums.Add(curriculum);

            }
            trainingProgram.Curricula = curriculums;
            var user = _userRepository.GetUser(newProgram.createdBy);
            trainingProgram.HistoryTrainingPrograms = new List<HistoryTrainingProgram>()
            {
                new HistoryTrainingProgram
                {
                    IdUser = user.ID,
                    IdProgram = trainingProgram.Id,
                    ModifiedOn = DateTime.Now,
                }
            };
            TrainingProgram result = _trainingProgramRepository.CreateTrainingProgram(trainingProgram);
            return result;
        }




        public async Task<List<TrainingProgramViewModel>> GetAll(string? sortBy, int pagesize)
        {
            var trainingList = await _trainingProgramRepository.GetAll();
            int PageSize = pagesize;
            int PageNumber = 1;

            var result = trainingList.Select(X => new TrainingProgramViewModel
            {
                
                Createby = X.HistoryTrainingPrograms.First().User.UserName,
                Createon = X.HistoryTrainingPrograms.First().ModifiedOn,
                Duration = GetDuration(X.Curricula.Select(x => x.Syllabus).ToList()),
                Id = X.Id,
                Name = X.Name,
                Status = X.Status,
            }).ToList();
             var result1 = result.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
            result1.OrderBy(cl => cl.Name).ToList();
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc": result1 = result1.OrderByDescending(cl => cl.Name).ToList(); break;
                    case "name_asc": result1 = result1.OrderBy(cl => cl.Name).ToList(); break;
                    case "Id_desc": result1 = result1.OrderByDescending(cl => cl.Id).ToList(); break;
                    case "Id_asc": result1 = result1.OrderBy(cl => cl.Id).ToList(); break;
                    case "createby_desc": result1 = result1.OrderByDescending(cl => cl.Createby).ToList(); break;
                    case "createby_asc": result1 = result1.OrderBy(cl => cl.Createby).ToList(); break;
                    case "createon_desc": result1 = result1.OrderByDescending(cl => cl.Createon).ToList(); break;
                    case "createon_asc": result1 = result1.OrderBy(cl => cl.Createon).ToList(); break;
                    case "duration_desc": result1 = result1.OrderByDescending(cl => cl.Duration).ToList(); break;
                    case "duration_asc": result1 = result1.OrderBy(cl => cl.Duration).ToList(); break;
                    case "status_desc": result1 = result1.OrderByDescending(cl => cl.Status).ToList(); break;
                    case "status_asc": result1 = result1.OrderBy(cl => cl.Status).ToList(); break;


                }
            }

            return result1.ToList();
        }
        public async Task<bool> Delete(long id)
        {
            //getbyID
            var statusId = GetById(id);
            //check if status
           
            if(statusId==null)
            {
                throw new Exception("No Training Program has that id");
            }
            else
                 if (statusId.Status == 1)
            {
                throw new Exception("Training Program is Active");
            }
            //kep mess throw ex ("Mess")
            //Controller Try catch
            return await _trainingProgramRepository.Delete(id);
        }
        public TrainingProgram GetById(long Id)
        {
            TrainingProgram trainingprogram = null;
            trainingprogram = _trainingProgramRepository.GetbyId(Id);
            return trainingprogram;
        }

        public async Task<bool> Edit(long id, string name, int status)
        {

            return await _trainingProgramRepository.Edit(id, name, status);
        }
        public async Task<bool> DeActivate(long id)
        {

            return await _trainingProgramRepository.DeActivate(id);

        }
        public async Task<long> Duplicate(long id)
        {
            return await _trainingProgramRepository.Duplicate(id);

        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveAsync()
        {
            _unitOfWork.commitAsync();
        }

        public async Task<List<TrainingProgramViewModel>> GetByFilter(List<string> programNames)
        {
            var trainingList = await _trainingProgramRepository.GetByFilter(programNames);
            var result = trainingList.Select(X => new TrainingProgramViewModel
            {
                Createby = X.HistoryTrainingPrograms.First().User.UserName,
                Createon = X.HistoryTrainingPrograms.First().ModifiedOn,
                Duration = GetDuration(X.Curricula.Select(x => x.Syllabus).ToList()),
                Id = X.Id,
                Name = X.Name,
                Status = X.Status,
            }).ToList();
            return result;
        }

        private int GetDuration(List<Syllabus> syllabuses)
        {
            var result = 0;
            foreach (var syllabus in syllabuses)
            {
                if (syllabus != null)
                {
                    foreach (var ses in syllabus.Sessions)
                    {
                        if (ses != null)
                        {
                            foreach (var unit in ses.Units)
                            {
                                if (unit != null)
                                {
                                    foreach (var less in unit.Lessons)
                                    {
                                        result += less.Duration;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public int GetAllSession(Syllabus syl)
        {
            IEnumerable<Session> sessions = _sessionRepository.GetAllSessions();
            IEnumerable<Unit> units = _unitRepository.GetAllUnits();
            IEnumerable<Lesson> lessons = _lessonRepository.GetAllLessons();
            var sess = sessions.Where(s => s.IdSyllabus == syl.Id);
            return sess.Count();
        }

        public TrainingProgramDetailViewModel GetDetailTrainingProgram(long? id)
        {
            TrainingProgramDetailViewModel viewModel = new TrainingProgramDetailViewModel();
            if (id != null)
            {
                List<SearchSyllabusViewModel> syllabuses = new List<SearchSyllabusViewModel>();

                TrainingProgram trainingProgram = _trainingProgramRepository.GetDetailById(id);
                viewModel = _mapper.Map<TrainingProgramDetailViewModel>(trainingProgram);

                if (viewModel == null)
                {
                    throw new Exception("Training program is not found");
                }

                var curriculums = _curriculumRepository.GetCurriculumsQuery(id)
                                    .Include(x => x.Syllabus)
                                    .ThenInclude(x => x.HistorySyllabi)
                                    .ThenInclude(x => x.User).OrderBy(x => x.NumberOrder)
                                    .ToList();

                var histories = _historyTrainingProgramRepository.GetHistoryTrainingQuery(id)
                                     .Include(x => x.User).ToList();


                foreach (var c in curriculums)
                {


                    if (c.Syllabus.Status != 3)
                    {
                        Syllabus syllabi = _syllabusRepository.GetById(c.Syllabus.Id);
                        var syllabus = _mapper.Map<SearchSyllabusViewModel>(syllabi);
                        syllabus.CreateBy = c.Syllabus.HistorySyllabi.OrderBy(x => x.ModifiedOn).FirstOrDefault()?.User.FullName;
                        syllabus.ModifiedOn = c.Syllabus.HistorySyllabi.OrderBy(x => x.ModifiedOn).FirstOrDefault()?.ModifiedOn;
                        syllabus.Duration =  _syllabusService.GetDuration(c.Syllabus);
                        viewModel.Duration += _syllabusService.GetDuration(c.Syllabus);
                        syllabus.TotalSession = GetAllSession(c.Syllabus);
                        syllabus.Status =  GetStatus(syllabi.Status);
                        viewModel.totalSession += GetAllSession(c.Syllabus);
                        syllabuses.Add(syllabus);

                    }
                }

                viewModel.Createon = histories.OrderBy(x => x.ModifiedOn).FirstOrDefault()!.ModifiedOn;
                viewModel.Createby = histories.OrderBy(x => x.ModifiedOn).FirstOrDefault()!.User.FullName;
                viewModel.syllabuses = syllabuses;
                viewModel.Status = GetStatus(trainingProgram.Status);

                return viewModel;
            }
            throw new Exception("Id is null");
        }
        public void AddHistoryTrainingProgram(HistoryTrainingProgram @historyTrainingProgram)
        {
            HistoryTrainingProgram his = new HistoryTrainingProgram
            {
                IdProgram = @historyTrainingProgram.IdProgram,
                IdUser = @historyTrainingProgram.IdUser,
                ModifiedOn = DateTime.Now
            };
            _trainingProgramRepository.AddHistoryTrainingProgram(his);
        }

        public String GetStatus(int status)
        {
            switch (status)
            {
                case 0:
                    return "Inactive";
                case 1:
                    return "Active";
                case 2:
                    return "Draft";
                case 3:
                    return "Delete";
                default:
                    return "Undefine";

            }
        }
    }
}