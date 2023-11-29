using AutoMapper;
using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using DAL;
using Newtonsoft.Json;
using System.Security.Claims;
using DAL.Repositories.Implements;
using Microsoft.Extensions.DependencyInjection;
using xUnitTest.Attributes;
using Xunit.Sdk;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using xUnitTest.Comparer;
using Microsoft.AspNetCore.Http;
using static DAL.Entities.Syllabus;

namespace xUnitTest.UnitTest
{
    [TestCaseOrderer("xUnitTest.PriorityOrderer", "xUnitTest")]
    [Collection("xUnitTest")]
    public class SyllabusServiceUnitTest : IClassFixture<DependencyInjection>
    {
        private ServiceProvider _provider;
        private ISyllabusService _syllabusService;
        private IServiceScope _scope;
        //Duong dan cua Linux, ai thich test trong Windows thi tu sua lai
        public SyllabusServiceUnitTest(DependencyInjection injection)
        {
            _provider = injection.provider;
            _scope = _provider.CreateScope();
            _syllabusService = _scope.ServiceProvider.GetService<ISyllabusService>();
        }

        #region syllabusBassic
        [Fact, TestPriority(12)]
        public void GetSyllabus_Basic()
        {
            //Arrange: repare the data to test
            int page_Size = 3;
            List<string> key = new List<string>() { "C#" };
            DateTime endTime = new DateTime(2022, 11, 15);
            DateTime startTime = new DateTime(2022, 11, 15);
            List<string> sortBy = new List<string>() { "Name ASC" };
            int page = 1;
            //Act: test
            List<SyllabusModel> result = _syllabusService.GetAll(key, page_Size, startTime, endTime, sortBy, page);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        #endregion
        #region syllabusnull
        [Fact, TestPriority(18)]
        public void GetSyllabus_null()
        {
            //Arrange: repare the data to test
            int page_Size = 0;
            List<string> key = new List<string>() { "" };
            DateTime endTime = new DateTime();
            DateTime startTime = new DateTime();
            List<string> sortBy = new List<string>() { "" };
            int page = 0;
            //Act: test
            List<SyllabusModel> result = _syllabusService.GetAll(null, 0, null, null, null, 0);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        #endregion
        #region syllabus_withoutkey
        [Fact, TestPriority(13)]
        public void GetSyllabus_WithoutKey()
        {
            //Arrange: repare the data to test
            int page_Size = 3;
            List<string> key = new List<string>() { "" };
            DateTime endTime = new DateTime(2022, 11, 15);
            DateTime startTime = new DateTime(2022, 11, 15);
            List<string> sortBy = new List<string>() { "Name ASC" };
            int page = 1;
            //Act: test
            List<SyllabusModel> result = _syllabusService.GetAll(null, page_Size, startTime, endTime, sortBy, page);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        #endregion
        #region syllabus_withoutDay
        [Fact, TestPriority(14)]
        public void GetSyllabus_WithoutDay()
        {
            //Arrange: repare the data to test
            int page_Size = 3;
            List<string> key = new List<string>() { "NET" };
            DateTime endTime = new DateTime();
            DateTime startTime = new DateTime();
            List<string> sortBy = new List<string>() { "Name ASC" };
            int page = 1;
            //Act: test
            List<SyllabusModel> result = _syllabusService.GetAll(key, page_Size, null, null, sortBy, page);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        #endregion
        #region syllabus_WithoutSort
        [Fact, TestPriority(15)]
        public void GetSyllabus_WithoutSortBy()
        {
            //Arrange: repare the data to test
            int page_Size = 3;
            List<string> key = new List<string>() { "NET" };
            DateTime endTime = new DateTime(2022, 11, 15);
            DateTime startTime = new DateTime(2022, 11, 15);
            List<string> sortBy = new List<string>() { "" };
            int page = 1;
            //Act: test
            List<SyllabusModel> result = _syllabusService.GetAll(key, page_Size, startTime, endTime, null, page);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        #endregion
        #region syllabus_WithoutPage
        [Fact, TestPriority(16)]
        public void GetSyllabus_WithoutPage()
        {
            //Arrange: repare the data to test
            int page_Size = 3;
            List<string> key = new List<string>() { "NET" };
            DateTime endTime = new DateTime(2022, 11, 15);
            DateTime startTime = new DateTime(2022, 11, 15);
            List<string> sortBy = new List<string>() { "Name ASC" };
            int page = 0;
            //Act: test
            List<SyllabusModel> result = _syllabusService.GetAll(key, page_Size, startTime, endTime, sortBy, 0);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        #endregion
        #region syllabus_WithoutPageSize
        [Fact, TestPriority(17)]
        public void GetSyllabus_WithoutPageSize()
        {
            //Arrange: repare the data to test
            int page_Size = 0;
            List<string> key = new List<string>() { "NET" };
            DateTime endTime = new DateTime(2022, 11, 15);
            DateTime startTime = new DateTime(2022, 11, 15);
            List<string> sortBy = new List<string>() { "Name ASC" };
            int page = 1;
            //Act: test
            List<SyllabusModel> result = _syllabusService.GetAll(key, 0, startTime, endTime, sortBy, page);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        #endregion
        #region Import
        [Fact, TestPriority(19)]
        public async Task ImportSyllabusTest()
        {
            string path = "..//..//..//Resource//ImportSyllabus-Edit.xlsx";
            using var s = new MemoryStream(File.ReadAllBytes(path).ToArray());

            var formFile = new FormFile(s, 0, s.Length, "streamFile", path.Split("//").Last());


            UpLoadExcelFileRequest request = new UpLoadExcelFileRequest()
            {
                File = formFile
            };
            using (FileStream stream = new FileStream(request.File.FileName, FileMode.CreateNew))
            {
                await request.File.CopyToAsync(stream);
            }
            var response = new UpLoadExcelFileResponse()
            {
                IsSuccess = false
            };
            try
            {
                response = await _syllabusService.UploadExcelFile(request, path);
            }
            catch (Exception ex)
            {
                Assert.False(response.IsSuccess);
            }
            File.Delete(Path.GetFullPath(Path.GetFileName(path)));
            Assert.True(response.IsSuccess);


        }
        #endregion

        #region Deactivate Syllabys Test
        [Fact, TestPriority(8)]
        public void DeactivateSyllabusTest()
        {
            List<Claim> claims = new List<Claim>();
            var id = 1;
            claims.Add(new Claim(ClaimTypes.Name, "superadmin@fsoft.com"));

            try
            {
                _syllabusService.DeactivateSyllabus(id, claims);
                _syllabusService.Save();
            }
            catch (Exception ex)
            {
                // If caught exception
                Assert.Fail("Fail to Deactivate Syllabus with exception: " + ex);
            }
            var deactivateSyllabus = _syllabusService.GetById(id);
            // If no exception was caught
            Assert.True(Equals(deactivateSyllabus.Status, 0));
        }
        #endregion

        #region Activate Syllabus Test
        [Fact, TestPriority(9)]
        public void ActivateSyllabusTest()
        {
            List<Claim> claims = new List<Claim>();
            var id = 1;
            claims.Add(new Claim(ClaimTypes.Name, "superadmin@fsoft.com"));

            try
            {
                _syllabusService.ActivateSyllabus(id, claims);
                _syllabusService.Save();
            }
            catch (Exception ex)
            {
                // If caught exception
                Assert.Fail("Fail to Activate Syllabus with exception: " + ex);
            }
            var activateSyllabus = _syllabusService.GetById(id);
            // If no exception was caught
            Assert.True(Equals(activateSyllabus.Status, 1));
        }
        #endregion

        #region Get By ID Test
        [Fact, TestPriority(11)]
        public void GetByIdTest()
        {

            List<Claim> claims = new List<Claim>();
            var id = 0;
            claims.Add(new Claim(ClaimTypes.Name, "superadmin@fsoft.com"));
            SyllabusViewModel syllabus = null;
            try
            {
                _syllabusService.GetById(id);
                syllabus = _syllabusService.GetById(id);

            }
            catch (Exception ex)
            {
                // If caught exception

                if (ex.Message.Equals("No syllabus with that id!"))
                {
                    Assert.True(1 == 1);
                    return;
                }


                Assert.Fail("Fail to Delete Syllabus with exception: " + ex);
            }

            // If no exception was caught
            Assert.NotNull(syllabus);
        }
        #endregion

        #region Duplicate Syllabus Test
        [Fact, TestPriority(-5)]
        public void DuplicateSyllabusTest()
        {
            List<Claim> claims = new List<Claim>();
            var id = 1;
            claims.Add(new Claim(ClaimTypes.Name, "superadmin@fsoft.com"));

            try
            {
                _syllabusService.DuplicateSyllabus(id, claims);
                _syllabusService.Save();
            }
            catch (Exception ex)
            {
                // If caught exception
                Assert.Fail("Fail to Duplicate Syllabus with exception: " + ex);
            }
            var duplicatedSyllabus = _syllabusService.GetById(id + 2);
            // If no exception was caught
            Assert.True(duplicatedSyllabus.Name.Contains("(Copy)"));
        }
        #endregion

        #region Create Syllabus Test
        [Theory, TestPriority(-6)]
        [SyllabusJsonFileData(".//TestSamples//Create_Syllabus_Example.json")]
        public void CreateSyllabusTest(SyllabusViewModel sampleSyllabus)
        {
            // Arrage
            // Load Claim List with only Username
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "superadmin@fsoft.com"));

            // Act
            try
            {
                _syllabusService.CreateSyllabus(sampleSyllabus, claims);
                _syllabusService.Save();
            }
            catch (Exception ex)
            {
                // If caught exception
                Assert.Fail("Fail to Create Syllabus with exception: " + ex);
            }
            // Get the syllabusId from input name
            var syllabusId = long.Parse(sampleSyllabus.Name.Substring(sampleSyllabus.Name.Length - 1));
            var createdSyllabus = _syllabusService.GetById(syllabusId);

            // Assert
            CreateSyllabusComparer comparer = new CreateSyllabusComparer();
            Assert.True(comparer.Equals(sampleSyllabus, createdSyllabus));

        }
        #endregion

        #region Update Syllabus Test
        [Theory, TestPriority(5)]
        [SyllabusJsonFileData(@"./TestSamples/Update_Syllabus_Test_Sample.json")]
        public void UpdateSyllabusTest(SyllabusViewModel sampleSyllabus)
        {
            // Arrage
            // Load Claim List with only Username
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "superadmin@fsoft.com"));

            try
            {
                _syllabusService.UpdateSyllabus(sampleSyllabus, claims);
                _syllabusService.Save();
            }
            catch (Exception ex)
            {
                // If caught exception
                Assert.Fail("Fail to Update Syllabus with exception: " + ex);
            }
            var updatedSyllabus = _syllabusService.GetById((long)sampleSyllabus.Id);

            // Assert
            UpdateSyllabusComparer comparer = new UpdateSyllabusComparer();
            Assert.True(comparer.Equals(sampleSyllabus, updatedSyllabus));
        }
        #endregion

        #region Delete Syllabus Test
        [Fact, TestPriority(10)]
        public void DeleteSyllabusTest()
        {

            List<Claim> claims = new List<Claim>();
            var id = 1;
            claims.Add(new Claim(ClaimTypes.Name, "superadmin@fsoft.com"));

            try
            {
                _syllabusService.DeleteSyllabus(id, claims);
                _syllabusService.Save();
                _syllabusService.GetById(id);
            }
            catch (Exception ex)
            {
                // If caught exception

                if (ex.Message.Equals("No syllabus found!") || ex.Message.Equals("No syllabus with that id!"))
                {
                    Assert.True(1 == 1);
                    return;
                }


                Assert.Fail("Fail to Delete Syllabus with exception: " + ex);
            }

        }
        #endregion

        #region Save Syllabus as Draft Test
        [Theory, TestPriority(-5)]
        [SyllabusJsonFileData(".//TestSamples//Create_Syllabus_Example.json")]
        public void SaveAsDraftTest(SyllabusViewModel sampleSyllabus)
        {
            // Arrage
            // Load Claim List with only Username
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "superadmin@fsoft.com"));

            // Act and Assert
            try
            {
                _syllabusService.SaveAsDraft(sampleSyllabus, claims);
                _syllabusService.Save();
            }
            catch (Exception ex)
            {
                // If caught exception
                Assert.Fail("Fail to Save Syllabus as Draft with exception: " + ex);
            }
            // Get the syllabusId from input name
            var syllabusId = long.Parse(sampleSyllabus.Name.Substring(sampleSyllabus.Name.Length - 1)) + 3; // +2 as save as draft test run after create test
            var createdSyllabus = _syllabusService.GetById(syllabusId);

            // Assert
            CreateSyllabusComparer comparer = new CreateSyllabusComparer();
            Assert.True(comparer.Equals(sampleSyllabus, createdSyllabus));
            Assert.Equal(createdSyllabus.Status, 2); // check if Created Syllabus with status as Draft
        }
        #endregion

    }
}