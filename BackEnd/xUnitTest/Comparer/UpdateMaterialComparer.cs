using BAL.Models;
using System.Diagnostics.CodeAnalysis;

namespace xUnitTest.Comparer
{
    public class UpdateMaterialComparer : IEqualityComparer<MaterialViewModel>
    {
        public bool Equals(MaterialViewModel? x, MaterialViewModel? y)
        {
            if (x == null || y == null)
                return false;

            if (x.Name == y.Name && x.HyperLink == y.HyperLink) return true;
            if (x.Status != y.Status) return true;

            return false;
        }

        public int GetHashCode([DisallowNull] MaterialViewModel obj)
        {
            int hCode = obj.Id.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}
