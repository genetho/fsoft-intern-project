using DAL;
using Xunit;
using System;
using BAL.Models;
using System.Linq;
using System.Text;
using DAL.Entities;
using DAL.Infrastructure;
using Docker.DotNet.Models;
using BAL.AutoMapperProfile;
using System.Threading.Tasks;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using System.Collections.Generic;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace xUnitTest
{
    public class DependencyInjection
    {
        private ServiceCollection _services;
        public ServiceProvider provider { get; private set; }
        public DependencyInjection()
        {
            _services = new ServiceCollection();

            string connectionString = $"Server=frmtestdb;Database=myDB;"
                                           + $"User ID=sa;"
                                           + $"Password=YourStrong@Passw0rd;";

            //string connectionString = "Server=(local);Database=fff12343faavfeea;Trusted_Connection=True;MultipleActiveResultSets=true";

            //string connectionString = "Server=(local);Database=FRMManagementV22;Trusted_Connection=True;MultipleActiveResultSets=true";
            //string connectionString = "Server=LAPTOP-MIPD1N33\\CONG;Database=FRMManagement;User Id=Khalid;Password=Colorful.1601;Trusted_Connection=True;MultipleActiveResultSets=true";

            _services.AddDbContext<FRMDbContext>(options
          => options.UseSqlServer(connectionString));

            _services.AddAutoMapper(typeof(User));
            _services.AddAutoMapper(typeof(TrainingProgramViewModel));
            _services.AddAutoMapper(typeof(ClassProfile));
            _services.AddAutoMapper(typeof(Program));
            _services.AddAutoMapper(typeof(TrainingProgramProfile));
            _services.AddAutoMapper(typeof(HistorySyllabusProfile));
            _services.AddAutoMapper(
                typeof(SyllabusProfile), typeof(AssignmentSchemaProfile), typeof(SessionProfile),
                typeof(UnitProfile), typeof(LessonProfile), typeof(MaterialProfile)
                );

            _services.AddScoped<IDbFactory, DbFactory>();
            _services.AddScoped<IUnitOfWork, UnitOfWork>();
            _services.AddScoped<ITrainingProgramService, TrainingProgramService>();
            _services.AddScoped<IPermissionRightRepository, PermissionRightRepository>();
            _services.AddScoped<IPermissionRightService, PermissionRightService>();
            _services.AddScoped<IPermissionRepository, PermissionRepository>();
            _services.AddScoped<IPermissionService, PermissionService>();
            _services.AddScoped<IRightRepository, RightRepository>();
            _services.AddScoped<IRightService, RightService>();
            _services.AddScoped<IAssignmentSchemaRepository, AssignmentSchemaRepository>();
            _services.AddScoped<IHistoryMaterialRepository, HistoryMaterialRepository>();
            _services.AddScoped<IHistoryMaterialService, HistoryMaterialService>();
            _services.AddScoped<IMaterialRepository, MaterialRepository>();
            _services.AddScoped<IOutputStandardRepository, OutputStandardRepository>();
            _services.AddScoped<IClassRepository, ClassRepository>();
            _services.AddScoped<IClassService, ClassService>();
            _services.AddScoped<ICurriculumService, CurriculumService>();
            _services.AddScoped<ISessionRepository, SessionRepository>();
            _services.AddScoped<ISessionService, SessionService>();
            _services.AddScoped<IMaterialService, MaterialService>();
            _services.AddScoped<ISyllabusRepository, SyllabusRepository>();
            _services.AddScoped<ISyllabusService, SyllabusService>();
            _services.AddScoped<ILessonRepository, LessonRepository>();
            _services.AddScoped<ILessonService, LessonService>();
            _services.AddScoped<IClassSelectedDateRepository, ClassSelectedDateRepository>();
            _services.AddScoped<IClassTraineeRepository, ClassTraineeRepository>();
            _services.AddScoped<IClassAdminReporitory, ClassAdminRepository>();
            _services.AddScoped<IClassMentorRepository, ClassMentorRepository>();
            _services.AddScoped<ICurriculumRepository, CurriculumRepository>();
            _services.AddScoped<IRoleService, RoleService>();
            _services.AddScoped<IRoleRepository, RoleRepository>();
            _services.AddScoped<ITrainingProgramRepository, TrainingProgramRepository>();
            _services.AddScoped<ITrainingProgramService, TrainingProgramService>();
            _services.AddScoped<IHistoryTrainingProgramRepository, HistoryTrainingProgramRepository>();
            _services.AddScoped<IHistoryTrainingProgramService, HistoryTrainingProgramService>();
            _services.AddScoped<IUserRepository, UserRepository>();
            _services.AddScoped<IUserService, UserService>();
            _services.AddScoped<IHistorySyllabusRepository, HistorySyllabusRepository>();
            _services.AddScoped<IHistorySyllabusService, HistorySyllabusService>();
            _services.AddScoped<IUnitRepository, UnitRepository>();
            _services.AddScoped<IUnitService, UnitService>();
            _services.AddScoped<IClassSiteRepository, ClassSiteRepository>();
            _services.AddScoped<IAttendeeTypeRepository, AttendeeTypeRepository>();
            _services.AddScoped<IClassProgramCodeRepository, ClassProgramCodeRepository>();
            _services.AddScoped<IFsucontactPointService, FsucontactPointService>();
            _services.AddScoped<IFSUContactPointRepository, FSUContactPointRepository>();
            _services.AddScoped<IClassSelectedDateService, ClassSelectedDateService>();
            _services.AddScoped<ILocationService, LocationService>();
            _services.AddScoped<ILocationRepository, LocationRepository>();
            _services.AddScoped<IClassLocationService, ClassLocationService>();
            _services.AddScoped<IClassLocationRepository, ClassLocationRepository>();
            _services.AddScoped<IClassStatusRepository, ClassStatusRepository>();
            _services.AddScoped<IClassStatusService, ClassStatusService>();
            _services.AddScoped<IAttendeeTypeRepository, AttendeeTypeRepository>();
            _services.AddScoped<IAttendeeTypeService, AttendeeTypeService>();
            _services.AddScoped<IClassAdminService, ClassAdminService>();
            _services.AddScoped<IClassMentorService, ClassMentorService>();
            _services.AddScoped<IClassTraineeService, ClassTraineeService>();
            _services.AddScoped<IFsoftUnitService, FsoftUnitService>();
            _services.AddScoped<IFsoftUnitRepository, FsoftUnitRepository>();
            _services.AddScoped<IClassStatusRepository, ClassStatusRepository>();
            _services.AddScoped<IFsoftUnitRepository, FsoftUnitRepository>();
            _services.AddScoped<IClassUniversityCodeRepository, ClassUniversityCodeRepository>();
            _services.AddScoped<IClassTechnicalGroupRepository, ClassTechnicalGroupRepository>();
            _services.AddScoped<IClassSiteRepository, ClassSiteRepository>();
            _services.AddScoped<IClassFormatTypeRepository, ClassFormatTypeRepository>();
            _services.AddScoped<IClassProgramCodeRepository, ClassProgramCodeRepository>();
            _services.AddScoped<IAttendeeTypeRepository, AttendeeTypeRepository>();
            _services.AddScoped<ILevelRepository, LevelRepository>();
            _services.AddScoped<IDeliveryTypeRepository, DeliveryTypeRepository>();
            _services.AddScoped<IFormatTypeRepository, FormatTypeRepository>();
            _services.AddScoped<ILessonRepository, LessonRepository>();


            provider = _services.BuildServiceProvider();

            var context = provider.GetService<FRMDbContext>();

            context.Database.Migrate();
        }

    }

}

