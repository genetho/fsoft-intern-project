using System;
using Xunit.Sdk;
using BAL.Models;
using System.Linq;
using System.Text;
using DAL.Entities;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace xUnitTest.Attributes
{
    public class CurriculumJsonFileDataAttribute : DataAttribute
    {
        private readonly string _filePath;

        /// <summary>
        /// Load data from a JSON file as the data source for a theory
        /// </summary>
        /// <param name="filePath">The absolute or relative path to the JSON file to load</param>
        public CurriculumJsonFileDataAttribute(string filePath)
        {
            _filePath = filePath;
        }


        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) { throw new ArgumentNullException(nameof(testMethod)); }

            // Get the absolute path to the JSON file
            var path = Path.IsPathRooted(_filePath)
                ? _filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), _filePath);

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Could not find file at path: {path}");
            }

            // Load the file
            var fileData = File.ReadAllText(_filePath);
            List<Curriculum> classes = JsonConvert.DeserializeObject<List<Curriculum>>(fileData);
            

            var objects = new List<object[]>();
            foreach (var obj in classes)
            {
                objects.Add(new object[] { obj });
            }

            return objects;

        }
    }
}
