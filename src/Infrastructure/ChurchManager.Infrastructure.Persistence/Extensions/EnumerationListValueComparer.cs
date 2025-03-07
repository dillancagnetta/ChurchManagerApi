using Codeboss.Types;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class EnumerationListValueComparer<TEnum> : ValueComparer<ICollection<TEnum>>
    where TEnum : Enumeration<TEnum, string>, new()
{
    public EnumerationListValueComparer() : base(
        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToList())
    {
    }
}