using BAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTest.Comparer
{
    public class UpdateSyllabusComparer : IEqualityComparer<SyllabusViewModel>
    {
        public bool Equals(SyllabusViewModel? x, SyllabusViewModel? y)
        {
            // Check Syllabus
            if (x == null || y == null)
                return false;

            if (x.Description == null || x.HyperLink == null)
            {
                if (x.Id != y.Id ||
                !(x.Name.Equals(y.Name)) ||
                !(x.Code.Equals(y.Code)) ||
                x.AttendeeNumber != y.AttendeeNumber ||
                !(x.Technicalrequirement.Equals(y.Technicalrequirement)) ||
                !(x.CourseObjectives.Equals(y.CourseObjectives)) ||
                x.Status != y.Status ||
                !(x.TrainingPrinciple.Equals(y.TrainingPrinciple)) ||
                x.IdLevel != y.IdLevel ||
                !(x.LevelName.Equals(y.LevelName)) ||
                x.AssignmentSchema.PercentQuiz != y.AssignmentSchema.PercentQuiz ||
                x.AssignmentSchema.PercentAssigment != y.AssignmentSchema.PercentAssigment ||
                x.AssignmentSchema.PercentFinal != y.AssignmentSchema.PercentFinal ||
                x.AssignmentSchema.PercentTheory != y.AssignmentSchema.PercentTheory ||
                x.AssignmentSchema.PercentFinalPractice != y.AssignmentSchema.PercentFinalPractice ||
                x.AssignmentSchema.PassingCriterial != y.AssignmentSchema.PassingCriterial)
                    return false;
            }
            else
            {
                if (x.Id != y.Id ||
                !(x.Name.Equals(y.Name)) ||
                !(x.Code.Equals(y.Code)) ||
                x.AttendeeNumber != y.AttendeeNumber ||
                !(x.Technicalrequirement.Equals(y.Technicalrequirement)) ||
                !(x.CourseObjectives.Equals(y.CourseObjectives)) ||
                x.Status != y.Status ||
                !(x.TrainingPrinciple.Equals(y.TrainingPrinciple)) ||
                !(x.Description.Equals(y.Description)) ||
                !(x.HyperLink.Equals(y.HyperLink)) ||
                x.IdLevel != y.IdLevel ||
                !(x.LevelName.Equals(y.LevelName)) ||
                x.AssignmentSchema.PercentQuiz != y.AssignmentSchema.PercentQuiz ||
                x.AssignmentSchema.PercentAssigment != y.AssignmentSchema.PercentAssigment ||
                x.AssignmentSchema.PercentFinal != y.AssignmentSchema.PercentFinal ||
                x.AssignmentSchema.PercentTheory != y.AssignmentSchema.PercentTheory ||
                x.AssignmentSchema.PercentFinalPractice != y.AssignmentSchema.PercentFinalPractice ||
                x.AssignmentSchema.PassingCriterial != y.AssignmentSchema.PassingCriterial)
                    return false;
            }
            
            // Check Session
            if (x.Sessions.Count != y.Sessions.Count)
                return false;
            for (int i = 0; i < x.Sessions.Count; i++)
            {
                var xSession = x.Sessions[i];
                var ySession = y.Sessions[i];
                if (xSession.Id != ySession.Id || !(xSession.Name.Equals(ySession.Name)) || xSession.Index != ySession.Index || xSession.Status != ySession.Status)
                    return false;

                // Check Unit
                if (xSession.Units.Count != ySession.Units.Count)
                    return false;
                for (int j = 0; j < xSession.Units.Count; j++)
                {
                    var xUnit = xSession.Units[i];
                    var yUnit = ySession.Units[i];
                    if (xUnit.Id != yUnit.Id || !(xUnit.Name.Equals(yUnit.Name)) || xUnit.Index != yUnit.Index || xUnit.Status != yUnit.Status)
                        return false;

                    // Check lesson
                    if (xUnit.Lessons.Count != yUnit.Lessons.Count)
                        return false;
                    for (int k = 0; k < xUnit.Lessons.Count; k++)
                    {
                        var xLesson = xUnit.Lessons[i];
                        var yLesson = yUnit.Lessons[i];
                        if (xLesson.Id != yLesson.Id ||
                            !(xLesson.Name.Equals(yLesson.Name)) ||
                            xLesson.Duration != yLesson.Duration ||
                            xLesson.IdDeliveryType != yLesson.IdDeliveryType ||
                            !(xLesson.DeliveryType.Equals(yLesson.DeliveryType)) ||
                            xLesson.IdFormatType != yLesson.IdFormatType ||
                            !(xLesson.FormatType.Equals(yLesson.FormatType)) ||
                            xLesson.IdOutputStandard != yLesson.IdOutputStandard ||
                            !(xLesson.OutputStandard.Equals(yLesson.OutputStandard)) ||
                            xLesson.Status != yLesson.Status)
                            return false;

                        // Check material
                        if (xLesson.Materials.Count != yLesson.Materials.Count)
                            return false;
                        for (int l = 0; l < xLesson.Materials.Count; l++)
                        {
                            var xMaterial = xLesson.Materials[i];
                            var yMaterial = yLesson.Materials[i];
                            if (xMaterial.Id != yMaterial.Id || !(xMaterial.Name.Equals(yMaterial.Name)) || !(xMaterial.HyperLink.Equals(yMaterial.HyperLink)) || xMaterial.Status != yMaterial.Status)
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
