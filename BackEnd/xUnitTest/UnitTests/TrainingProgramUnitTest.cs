using BAL.Models;
using DAL.Entities;
using xUnitTest.Comparer;
using xUnitTest.Attributes;
using BAL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace xUnitTest.UnitTests
{
    [TestCaseOrderer("xUnitTest.PriorityOrderer", "xUnitTest")]
    [Collection("xUnitTest")]
    public class TrainingProgramUnitTest : IClassFixture<DependencyInjection>
    {
        private ServiceProvider _provider;
        private IServiceScope _scope;
        private ISyllabusService _syllabusService;
        private IMaterialService _materialService;
        private ITrainingProgramService _trainingProgramService;
        private IClassService _classService;

        public TrainingProgramUnitTest(DependencyInjection dependencyInjection)
        {
            _provider = dependencyInjection.provider;
            _scope = _provider.CreateScope();
            _syllabusService = _scope.ServiceProvider.GetService<ISyllabusService>();
            _materialService = _scope.ServiceProvider.GetService<IMaterialService>();
            _trainingProgramService = _scope.ServiceProvider.GetService<ITrainingProgramService>();
            _classService = _scope.ServiceProvider.GetService<IClassService>();
        }

        #region GetDetailTrainingProgramById
        [Fact, TestPriority(0)]
        public async Task GetDetailTrainingProgram_LackOfId_ShouldReturnTrainingProgram_WhenDataFound()
        {
            //Arrange: prepare the data to test
            long? trainingProgramId = null;
            //Act: test
            var ex = Assert.Throws<Exception>(() => _trainingProgramService.GetDetailTrainingProgram(trainingProgramId));
            //Assert: compare the actual result and expect result
            Assert.Equal("Id is null", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task GetDetailTrainingProgram_HasId_ShouldReturnTrainingProgram_WhenDataFound()
        {
            //Arrange: prepare the data to test
            long? trainingProgramId = 3;
            //Act: test
            TrainingProgramDetailViewModel trainingProgramDetai = _trainingProgramService.GetDetailTrainingProgram(trainingProgramId);
            //Assert: compare the actual result and expect result
            Assert.NotNull(trainingProgramDetai);
        }

        [Fact, TestPriority(0)]
        public async Task GetDetailTrainingProgram_LackIdInDatabase_ShouldReturnTrainingProgram_WhenDataFound()
        {
            //Arrange: prepare the data to test
            long? trainingProgramId = 100;
            //Act: test
            var ex = Assert.Throws<Exception>(() => _trainingProgramService.GetDetailTrainingProgram(trainingProgramId));
            //Assert: compare the actual result and expect result
            Assert.Equal("Training program is not found", ex.Message);
           // Assert.NotNull(model);

        }

        #endregion

        #region EditMaterial
        [Fact, TestPriority(0)]
        public async Task UpdateMaterial_HasId_ShouldReturnNewMaterial_WhenDataFound()
        {
            //Arrange: prepare the data to test
            MaterialViewModel materialViewModel = new MaterialViewModel()
            {
                Id = 50,
                Name = "HA",
                HyperLink = "123.COM",
                Status = 1
            };
            //Act: test
            _materialService.UpdateMaterial(materialViewModel);
            MaterialViewModel updatedMaterial = _materialService.GetMaterial(materialViewModel.Id.Value);
            UpdateMaterialComparer c = new UpdateMaterialComparer();
            //Assert: compare the actual result and expect result
            Assert.True(((IEqualityComparer<MaterialViewModel>)c).Equals(materialViewModel, updatedMaterial));
        }
        #endregion


        #region DeleteLessonMaterialById

        [Fact, TestPriority(0)]
        public async Task DeleteLessonMaterialById_LackOfId_ShouldReturnTrainingProgram_WhenDataFound()
        {
            //Arrange: prepare the data to test
            long? materialId = null;
            //Act: test
            _materialService.DeleteMaterial(materialId);
            List<MaterialViewModel> materialViewModelList = _materialService.GetMaterials(materialId);
            //Assert: compare the actual result and expect result
            Assert.NotNull(materialViewModelList);
        }


        [Fact, TestPriority(0)]
        public async Task DeleteLessonMaterialById_HasId_ShouldReturnTrainingProgram_WhenDataFound()
        {
            //Arrange: prepare the data to test
            long? materialId = 1;
            //Act: test
            _materialService.DeleteMaterial(materialId);
            List<MaterialViewModel> materialViewModelList = _materialService.GetMaterials(materialId);
            //Assert: compare the actual result and expect result
            Assert.NotNull(materialViewModelList);
        }
        #endregion

        #region SearchSyllabusByName
        [Fact, TestPriority(0)]
        public async Task SearchSyllabus_LackOfName_ShouldReturnSyllabus_WhenDataFound()
        {
            //Arrange: repare the data to test
            string name = null;
            //Act: test
            List<SearchSyllabusViewModel> syllabuses = _syllabusService.SearchSyllabusByName(name);
            //Assert: compare the actual result and expect result
            Assert.NotNull(syllabuses);
        }

        [Fact, TestPriority(0)]
        public async Task SearchSyllabus_HasName_ShouldReturnSyllabus_WhenDataFound()
        {
            //Arrange: repare the data to test
            string name = "Linux";
            //Act: test
            List<SearchSyllabusViewModel> syllabuses = _syllabusService.SearchSyllabusByName(name);
            //Assert: compare the actual result and expect result
            Assert.NotNull(syllabuses);
        }
        #endregion

        #region CreateTrainingProgram
        [Fact, TestPriority(0)]
        public async Task CreateTrainingProgram_FullInfo_ShouldReturnThatTrainingProgram()
        {
            //Arrange: repare the data to test
            List<CurriculumViewModel> curriculumViewModelList = new()
            {
                new CurriculumViewModel()
                {
                    idSyllabus = 3,
                    numberOrder = 1,
                },

            };
            ProgramViewModel model = new()
            {
                name = "Flutter and dart",
                createdBy = "superadmin@fsoft.com",
                syllabi = curriculumViewModelList,
            };
            //Act: test
            TrainingProgram trainingProgram = _trainingProgramService.CreateTrainingProgram(model);
            //Assert: compare the actual result and expect result
            Assert.NotNull(trainingProgram);
        }

        [Fact, TestPriority(0)]
        public async Task CreateTrainingProgram_WrongIdSyllabus_ShouldReturnException()
        {
            //Arrange: repare the data to test
            List<CurriculumViewModel> curriculumViewModelList = new()
            {
                new CurriculumViewModel()
                {
                    idSyllabus = 12,
                    numberOrder = 1,
                },

            };
            ProgramViewModel model = new()
            {
                name = "Operation system",
                createdBy = "superadmin@fsoft.com",
                syllabi = curriculumViewModelList,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = Assert.Throws<Exception>(() => _trainingProgramService.CreateTrainingProgram(model));
            Assert.Equal("Syllabus doesn't exist in the system.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task CreateTrainingProgram_NameIsAlreadyExist_ShouldReturnException()
        {
            //Arrange: repare the data to test
            List<CurriculumViewModel> curriculumViewModelList = new()
            {
                new CurriculumViewModel()
                {
                    idSyllabus = 3,
                    numberOrder = 1,
                },

            };
            ProgramViewModel model = new()
            {
                name = "Flutter and dart",
                createdBy = "superadmin@fsoft.com",
                syllabi = curriculumViewModelList,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = Assert.Throws<Exception>(() => _trainingProgramService.CreateTrainingProgram(model));
            Assert.Equal("Training program name is already exist in system.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task CreateTrainingProgram_NameIsGreaterThan50Characters_ShouldReturnException()
        {
            //Arrange: repare the data to test
            List<CurriculumViewModel> curriculumViewModelList = new()
            {
                new CurriculumViewModel()
                {
                    idSyllabus = 3,
                    numberOrder = 1,
                },

            };
            ProgramViewModel model = new()
            {
                name = "Lunar New Year Festival often falls between late January and early February; " +
                "it is among the most important holidays in Vietnam. Officially, the " +
                "festival includes the 1st, 2nd, and 3rd day in Lunar Calendar",
                createdBy = "superadmin@fsoft.com",
                syllabi = curriculumViewModelList,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = Assert.Throws<Exception>(() => _trainingProgramService.CreateTrainingProgram(model));
            Assert.Equal("Training program name must less than or equal 50 character.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task CreateTrainingProgram_NameFieldLeftBlank_ShouldReturnException()
        {
            //Arrange: repare the data to test
            List<CurriculumViewModel> curriculumViewModelList = new()
            {
                new CurriculumViewModel()
                {
                    idSyllabus = 3,
                    numberOrder = 1,
                },

            };
            ProgramViewModel model = new()
            {
                name = "",
                syllabi = curriculumViewModelList,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = Assert.Throws<Exception>(() => _trainingProgramService.CreateTrainingProgram(model));
            Assert.Equal("name field cannot be left blank.", ex.Message);
        }

        [Fact, TestPriority(0)]
        public async Task CreateTrainingProgram_IdSyllabusIsDuplicated_ShouldReturnException()
        {
            //Arrange: repare the data to test
            List<CurriculumViewModel> curriculumViewModelList = new()
            {
                new CurriculumViewModel()
                {
                    idSyllabus = 3,
                    numberOrder = 1,
                },
                new CurriculumViewModel()
                {
                    idSyllabus = 3,
                    numberOrder = 2,
                },

            };
            ProgramViewModel model = new()
            {
                name = "Java OOP",
                syllabi = curriculumViewModelList,
            };
            //Act: test
            //Assert: compare the actual result and expect result
            var ex = Assert.Throws<Exception>(() => _trainingProgramService.CreateTrainingProgram(model));
            Assert.Equal("Id syllabus is duplicated.", ex.Message);
        }

        #endregion
        #region GetTrainingProgrmList 
        [Fact, TestPriority(0)]
        public async Task GetTrainingProgramListTest()
        {
            string sortBy = "Id_asc";
            int pagesize = 5;


            var model = new List<TrainingProgramViewModel>();
            try
            {
                model = await _trainingProgramService.GetAll(sortBy, pagesize);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get List Training Program at pagesize {pagesize} with exception: {ex}");
            }
            Assert.NotNull(model);
        }
        #endregion
        #region DeleteTrainingProgram
        [Fact, TestPriority(5)]
        public async Task DeleteTrainingProgramAsync()
        {
            long id =2;
            try
            {
                await _trainingProgramService.Delete(id);
                var checkStatus=  _trainingProgramService.GetDetailTrainingProgram(id).Status;
                Assert.True(checkStatus == "Delete");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Delete Trainging Program at id {id} with exception: {ex}");
                
            }
        }
        #endregion
        #region DuplicateTrainingProgram
        [Fact, TestPriority(300)]
        public async Task DuplicateTrainingProgramTest()
        {
            long id = 3;
            try
            {
                await _trainingProgramService.Duplicate(id);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Duplicate training program at id {id} with exception: {ex}");
            }
        }
        #endregion
        #region DeActivateTrainingProgram
        [Fact, TestPriority(5)]
        public async Task DeActivateTrainingProgramTest()
        {
            long id = 2;
            try
            {
                await _trainingProgramService.DeActivate(id);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to DeActivate Training Program at id {id} with exception: {ex}");
            }
        }
        #endregion
        #region EditTrainingProgram
        [Fact, TestPriority(4)]
        public async Task EditTrainingTest()
        {
            long id = 1;
            string name = "C#";
            int status = 0;
            try
            {
                await _trainingProgramService.Edit(id, name, status);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Edit Training Program at id {id} with exception: {ex}");
            }
        }
        #endregion
        #region GetTrainingProgrmByFilter 
        [Fact, TestPriority(0)]
        public async Task GetTrainingProgrmByFilterTest()
        {
            var programName = new List<string>
            {
                 "H"
            };

            var model = new List<TrainingProgramViewModel>();
            try
            {
                model = await _trainingProgramService.GetByFilter(programName);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get List Training Program at programName {programName} with exception: {ex}");
            }
            Assert.NotNull(model);
        }
        #endregion
        #region Prepare Data
        // data class
        [Theory, TestPriority(-11)]
        [HistoryTrainingProgramJsonFileData("..//..//..//TestSamples//History_TrainingProgram_Example.json")]
        public void PrepareData_ForClassTable(HistoryTrainingProgram @historyTrainingProgram)
        {

            _trainingProgramService.AddHistoryTrainingProgram(@historyTrainingProgram);
        }
        #endregion
    }
}

