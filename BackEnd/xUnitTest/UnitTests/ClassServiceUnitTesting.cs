using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xUnitTest.Attributes;
using static DAL.Entities.Class;

namespace xUnitTest.UnitTests
{
    [TestCaseOrderer("xUnitTest.PriorityOrderer", "xUnitTest")]
    [Collection("xUnitTest")]
    public class ClassServiceUnitTesting: IClassFixture<DependencyInjection>
    {
        private ServiceProvider _provider;
        private IClassSelectedDateService _classSelectedDateService;
        private IServiceScope _scope;
        private IClassService _classService;

        public ClassServiceUnitTesting(DependencyInjection injection)
        {
            _provider = injection.provider;
            _scope = _provider.CreateScope();
            _classSelectedDateService = _scope.ServiceProvider.GetService<IClassSelectedDateService>();
            _classService = _scope.ServiceProvider.GetService<IClassService>();
        }


        #region getClassesTest
        [Fact, TestPriority(0)]
        public void GetClasses_WithAllNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(null, null, null, null, null, null,
                null, null, 0, 0, 0, 0);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithKeyIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>() {""};
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022,11,24);
            DateTime classTimeTo = new DateTime(2022,12,30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, classTime,
                status, attendee, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }


        [Fact, TestPriority(0)]
        public void GetClasses_WithSortByIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>();
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, null, location, classTimeFrom, classTimeTo, classTime,
                status, attendee, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithLocationIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassCode DESC"};
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, null, classTimeFrom, classTimeTo, classTime,
                status, attendee, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithClassTimeFromIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassCode DESC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, null, classTimeTo, classTime,
                status, attendee, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithClassTimeToIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, null, classTime,
                status, attendee, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithClassTimeIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, null,
                status, attendee, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithStatusIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, classTime,
                null, attendee, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithAttendeeIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>() { "Class name" };
            List<string> sortBy = new List<string>() { "ClassCode ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, classTime,
                status, null, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithFSUIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, classTime,
                status, attendee, 0, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithTrainerIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, classTime,
                status, attendee, FSU, 0, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithPageNumberIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, classTime,
                status, attendee, FSU, trainer, 0, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithPageSizeIsNull()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 1;
            int pageNumber = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, classTime,
                status, attendee, FSU, trainer, pageNumber, 0);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithFSUIsNotExist()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 100;
            int trainer = 1;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, classTime,
                status, attendee, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }

        [Fact, TestPriority(0)]
        public void GetClasses_WithTrainerIsNotExist()
        {
            //Arrange: prepare the data to test
            List<string> key = new List<string>();
            List<string> sortBy = new List<string>() { "ClassName ASC" };
            List<long> location = new List<long>() { 1 };
            DateTime classTimeFrom = new DateTime(2022, 11, 24);
            DateTime classTimeTo = new DateTime(2022, 12, 30);
            List<long> classTime = new List<long>() { 1 };
            List<long> status = new List<long>() { 1 };
            List<long> attendee = new List<long>() { 1 };
            int FSU = 1;
            int trainer = 999;
            int pageNumber = 3;
            int pageSize = 3;
            //Act: test
            List<ClassModel> result = _classService.GetClassess(key, sortBy, location, classTimeFrom, classTimeTo, classTime,
                status, attendee, FSU, trainer, pageNumber, pageSize);
            //Assert: compare the actual result and expect result
            Assert.NotNull(result);
        }
        #endregion

        #region importClassTest
        [Fact, TestPriority(0)]
        public async Task ImportClassTest()
        {
            string path = "..//..//..//Resource//ImportClassTemplate.xlsx";
            using var s = new MemoryStream(File.ReadAllBytes(path).ToArray());
            var formFile = new FormFile(s, 0, s.Length, "streamFile", path.Split("//").Last());


            ImportClassRequest request = new ImportClassRequest()
            {
                File = formFile,
            };
            using (FileStream stream = new FileStream(request.File.FileName, FileMode.CreateNew))
            {
                await request.File.CopyToAsync(stream);
            }
            var response = new ImportClassResponse()
            {
                IsSuccess = false
            };
            try
            {
                response = await _classService.ImportCLasses(request, path);
            }
            catch (Exception ex)
            {
                Assert.False(response.IsSuccess);
            }
            File.Delete(Path.GetFullPath(Path.GetFileName(path)));
            Assert.True(response.IsSuccess);


        }
        #endregion

    }
}
