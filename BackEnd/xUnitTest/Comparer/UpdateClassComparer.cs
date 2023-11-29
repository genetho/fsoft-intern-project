using BAL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTest.Comparer
{
    public class UpdateClassComparer : IEqualityComparer<UpdateClassViewModel>
    {
        public bool Equals(UpdateClassViewModel? x, UpdateClassViewModel? y)
        {
            if (x == null || y == null)
            { return false; }
            if (x.Id != y.Id ||
                !(x.Name.Equals(y.Name)) ||
                      !(x.ClassCode.Equals(y.ClassCode)) ||
                      x.Status != y.Status ||
                      x.StartTimeLearning != y.StartTimeLearning ||
                      x.EndTimeLearing != y.EndTimeLearing ||
                      x.ReviewedBy != y.ReviewedBy ||
                      x.ReviewedOn != y.ReviewedOn ||
                      x.CreatedBy != y.CreatedBy ||
                      //x.CreatedOn != y.CreatedOn ||
                      x.ApprovedBy != y.ApprovedBy ||
                      x.ApprovedOn != y.ApprovedOn ||
                      x.PlannedAtendee != y.PlannedAtendee ||
                      x.ActualAttendee != y.ActualAttendee ||
                      x.AcceptedAttendee != y.AcceptedAttendee ||
                      x.CurrentSession != y.CurrentSession ||
                      x.CurrentUnit != y.CurrentUnit ||
                      x.StartYear != y.StartYear ||
                      x.StartDate != y.StartDate ||
                      x.EndDate != y.EndDate ||
                      x.ClassNumber != y.ClassNumber ||
                      x.IdProgram != y.IdProgram ||
                      x.IdTechnicalGroup != y.IdTechnicalGroup ||
                      x.IdFSU != y.IdFSU ||
                      x.IdFSUContact != y.IdFSUContact ||
                      x.IdStatus == y.IdStatus ||
                      x.IdSite != y.IdSite ||
                      x.IdUniversity != y.IdUniversity ||
                      x.IdFormatType != y.IdFormatType ||
                      x.IdProgramContent != y.IdProgramContent ||
                      x.IdAttendeeType != y.IdAttendeeType ||
                      x.IdLocation.Count != y.IdLocation.Count ||
                      x.ActiveDate.Count != y.ActiveDate.Count ||
                      x.IdTrainee.Count != y.IdTrainee.Count ||
                      x.IdAdmin.Count != y.IdAdmin.Count ||
                      x.IdMentor.Count != y.IdMentor.Count ||
                      x.Syllabi.Count != y.Syllabi.Count

                      )
            { return false; }


            for (int i = 0; i < x.IdLocation.Count; i++)
            {
                var xLocation = x.IdLocation[i];
                var yLocation = y.IdLocation[i];
                if (xLocation != yLocation)
                    return false;
            }

            for (int i = 0; i < x.ActiveDate.Count; i++)
            {
                var xActiveDate = x.ActiveDate[i];
                var yActiveDate = y.ActiveDate[i];
                if (xActiveDate != yActiveDate)
                    return false;
            }

            for (int i = 0; i < x.IdTrainee.Count; i++)
            {
                var xIdTrainee = x.IdTrainee[i];
                var yIdTrainee = y.IdTrainee[i];
                if (xIdTrainee != yIdTrainee)
                    return false;
            }

            for (int i = 0; i < x.IdAdmin.Count; i++)
            {
                var xIdAdmin = x.IdAdmin[i];
                var yIdAdmin = y.IdAdmin[i];
                if (xIdAdmin != yIdAdmin)
                    return false;
            }

            for (int i = 0; i < x.IdMentor.Count; i++)
            {
                var xIdMentor = x.IdMentor[i];
                var yIdMentor = y.IdMentor[i];
                if (xIdMentor != yIdMentor)
                    return false;
            }

            for (int i = 0; i < x.Syllabi.Count; i++)
            {
                var xSyllabi = x.Syllabi[i];
                var ySyllabi = y.Syllabi[i];
                if (xSyllabi.idSyllabus == ySyllabi.idSyllabus && xSyllabi.numberOrder != ySyllabi.numberOrder)
                    return false;
            }

            return true;

        }
        public int GetHashCode([DisallowNull] UpdateClassViewModel obj)
        {
            int hCode = obj.Id.GetHashCode();
            return hCode.GetHashCode();
        }


    }
}