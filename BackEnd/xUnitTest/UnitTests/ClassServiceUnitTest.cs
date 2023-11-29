using DAL;
using Xunit.Sdk;
using AutoMapper;
using BAL.Models;
using DAL.Entities;
using Newtonsoft.Json;
using DAL.Infrastructure;
using xUnitTest.Comparer;
using xUnitTest.Attributes;
using System.Security.Claims;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Implements;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace xUnitTest.UnitTests
{
    [TestCaseOrderer("xUnitTest.PriorityOrderer", "xUnitTest")]
    [Collection("xUnitTest")]
    public class ClassServiceUnitTest : IClassFixture<DependencyInjection>
    {
        private ServiceProvider _provider;
        private IClassService _classService;
        private IServiceScope _scope;
        private ISyllabusService _syllabusService;
        private IPermissionRightService _permissionRightService;
        private IClassRepository _classRepository;

        public ClassServiceUnitTest(DependencyInjection injection)
        {
            _provider = injection.provider;
            _scope = _provider.CreateScope();
#pragma warning disable CS8601 // Possible null reference assignment.
            _classService = _scope.ServiceProvider.GetService<IClassService>();
            _syllabusService = _scope.ServiceProvider.GetService<ISyllabusService>();
            _classRepository = _scope.ServiceProvider.GetService<IClassRepository>();

        }

        // data syllabus
        //[Theory, TestPriority(5)]
        //[SyllabusJsonFileData(".\\TestSamples\\Create_Syllabus_Example.json")]
        //public void PrepareData_ForSyllabusTable(SyllabusViewModel @syllabus)
        //{
        //    List<Claim> claims = new List<Claim>();
        //    claims.Add(new Claim(ClaimTypes.Name, "superadmin@fsoft.com"));

        //    _syllabusService.CreateSyllabus(@syllabus, claims);
        //    _syllabusService.Save();
        //}
        [Theory, TestPriority(115)]
        [ClassJsonFileData(@".//TestSamples//Update_Class_Test_Sample.json")]
        public async Task SaveAsDraftClassTestAsync(UpdateClassViewModel sampleClass)
        {

            try
            {
                await _classService.SaveAsDraft(sampleClass);
                _classService.Save();
            }
            catch (Exception ex)
            {
                // If caught exception
                Assert.Fail("Fail to Update Class with exception: " + ex);
            }
            var updatedSyllabus = _classService.GetIdClass((long)sampleClass.Id);

            // Assert
            UpdateClassComparer comparer = new UpdateClassComparer();
            Assert.True(comparer.Equals(sampleClass, updatedSyllabus));
            Assert.Equal(updatedSyllabus.IdStatus, 1);
        }

        [Theory, TestPriority(116)]
        [ClassJsonFileData(@".//TestSamples//Update_Class_Test_Sample.json")]
        public async Task UpdateClassTestAsync(UpdateClassViewModel sampleClass)
        {

            try
            {
                await _classService.UpdateClass(sampleClass);
                _classService.Save();
            }
            catch (Exception ex)
            {
                // If caught exception
                Assert.Fail("Fail to Update Class with exception: " + ex);
            }
            var updatedSyllabus = _classService.GetIdClass((long)sampleClass.Id);

            // Assert
            UpdateClassComparer comparer = new UpdateClassComparer();
            Assert.True(comparer.Equals(sampleClass, updatedSyllabus));

        }
        [Fact, TestPriority(117)]
        //get class
        public async Task GetClassCodeTest()
        {
            string classcode = "H";
            List<ClassSearchViewModel> search = await _classService.GetClassByCodeService(classcode);

            Assert.True(search.Count >= 0);

        }
        [Fact, TestPriority(118)]
        public async Task GetTrainingProgramTest()
        {
            string classcode = "H";
            List<ClassTrainingProgamViewModel> search = await _classService.GetClassTrainingProgams(classcode);

            Assert.True(search.Count >= 0);

        }
        //NHOM3
        #region GetClass 
        [Fact, TestPriority(120)]
        public async Task GetClassTest()
        {
            long id = 1;
            ClassDetailViewModel model = null;
            try
            {
                model = await _classService.GetDetail(id);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to get detail by {id} with exception: {ex}");
            }
            Assert.NotNull(model);
        }
        #endregion

        #region Get Class Attendee
        [Fact, TestPriority(121)]
        public async Task GetClassAttendeeTest()
        {
            long Id = 1;
            int PageNumber = 1, PageSize = 5;
            var model = new List<ClassAttendeeViewModel>();
            try
            {
                model = await _classService.GetClassAttendee(Id, PageNumber, PageSize);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get Class Attende at id {Id} with exception: {ex}");
            }
            Assert.NotNull(model);
        }
        #endregion
        #region Get Class TrainingProgram
        [Fact, TestPriority(122)]
        public async Task GetClassTrainingProgram()
        {
            long Id = 1;


            var model = new ClassDetailTrainingViewModel();
            try
            {
                model = await _classService.GetTrainingProgram(Id);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get Class Attende at id {Id} with exception: {ex}");
            }
            Assert.NotNull(model);
        }
        #endregion
        #region DeleteClass
        [Fact, TestPriority(301)]
        public async void DeleteClass()
        {
            long id = 2;
            try
            {
                await _classService.Delete(id);
                _classService.SaveAsync();
                 _classService.GetById(id);
            }
            catch (Exception ex)
            {
                // If caught exception

                if (ex.Message.Equals($"Unable to find Class {id}") || ex.Message.Equals("No Class has that Id"))
                {
                    Assert.True(1 == 1);
                    return;
                }


                Assert.Fail("Fail to Delete Class with exception: " + ex);
            }
        }
        #endregion
      
        #region Duplicate
        [Fact, TestPriority(128)]
        public async Task Duplicate()
        {
            long Id = 1;
            try
            {
                await _classService.Duplicate(Id);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Duplicate at id {Id} with exception: {ex}");
            }
        }
        #endregion
        #region DeActivateClass
        [Fact, TestPriority(128)]
        public async Task DeActivateClassTest()
        {
            long Id = 2 ;
            try
            {
                await _classService.DeActivate(Id);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get Class Attende at id {Id} with exception: {ex}");
            }
        }
        #endregion

        #region Prepare Data
        // data class
        [Theory, TestPriority(-10)]
        [CurriculumJsonFileData("..//..//..//TestSamples//Curiculum_Example.json")]
        public void PrepareData_ForClassTable(Curriculum @curriculum)
        {
            
            _classService.CreateCurricula(@curriculum);
        }
        #endregion
    }


}

