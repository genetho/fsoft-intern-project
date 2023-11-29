using BAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTest.Comparer
{
    public class CreateSyllabusComparer : IEqualityComparer<SyllabusViewModel>
    {
        public bool Equals(SyllabusViewModel? x, SyllabusViewModel? y)
        {
            // Check Syllabus
            if (x == null || y == null)
                return false;
            if (!(x.Name.Equals(y.Name)) ||
                !(x.Code.Equals(y.Code)) ||
                x.AttendeeNumber != y.AttendeeNumber ||
                !(x.Technicalrequirement.Equals(y.Technicalrequirement)) ||
                !(x.CourseObjectives.Equals(y.CourseObjectives)) ||
                !(x.TrainingPrinciple.Equals(y.TrainingPrinciple)) ||
                x.IdLevel != y.IdLevel ||
                x.AssignmentSchema.PercentQuiz != y.AssignmentSchema.PercentQuiz ||
                x.AssignmentSchema.PercentAssigment != y.AssignmentSchema.PercentAssigment ||
                x.AssignmentSchema.PercentFinal != y.AssignmentSchema.PercentFinal ||
                x.AssignmentSchema.PercentTheory != y.AssignmentSchema.PercentTheory ||
                x.AssignmentSchema.PercentFinalPractice != y.AssignmentSchema.PercentFinalPractice ||
                x.AssignmentSchema.PassingCriterial != y.AssignmentSchema.PassingCriterial)
                return false;

            // Check Session
            if (x.Sessions.Count != y.Sessions.Count)
                return false;
            for (int i = 0; i < x.Sessions.Count; i++)
            {
                var xSession = x.Sessions[i];
                var ySession = y.Sessions[i];
                if (!(xSession.Name.Equals(ySession.Name)) || xSession.Index != ySession.Index)
                    return false;

                // Check Unit
                if (xSession.Units.Count != ySession.Units.Count)
                    return false;
                for (int j = 0; j < xSession.Units.Count; j++)
                {
                    var xUnit = xSession.Units[i];
                    var yUnit = ySession.Units[i];
                    if (!(xUnit.Name.Equals(yUnit.Name)) || xUnit.Index != yUnit.Index)
                        return false;

                    // Check lesson
                    if (xUnit.Lessons.Count != yUnit.Lessons.Count)
                        return false;
                    for (int k = 0; k < xUnit.Lessons.Count; k++)
                    {
                        var xLesson = xUnit.Lessons[i];
                        var yLesson = yUnit.Lessons[i];
                        if (!(xLesson.Name.Equals(yLesson.Name)) ||
                            xLesson.Duration != yLesson.Duration ||
                            xLesson.IdDeliveryType != yLesson.IdDeliveryType ||
                            xLesson.IdFormatType != yLesson.IdFormatType ||
                            xLesson.IdOutputStandard != yLesson.IdOutputStandard)
                            return false;

                        // Check material
                        if (xLesson.Materials.Count != yLesson.Materials.Count)
                            return false;
                        for (int l = 0; l < xLesson.Materials.Count; l++)
                        {
                            var xMaterial = xLesson.Materials[i];
                            var yMaterial = yLesson.Materials[i];
                            if (!(xMaterial.Name.Equals(yMaterial.Name)) || !(xMaterial.HyperLink.Equals(yMaterial.HyperLink)))
                                return false;
                        }
                    }
                }
            }


            // If every fields are equal
            return true;
        }

        public int GetHashCode([DisallowNull] SyllabusViewModel obj)
        {
            int hCode = obj.Id.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}
