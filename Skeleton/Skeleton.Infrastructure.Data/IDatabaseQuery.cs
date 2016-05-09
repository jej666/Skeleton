namespace Skeleton.Infrastructure.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public interface IDatabaseQuery
    {
        IEnumerable<TResult> Find<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class;

        TResult FirstOrDefault<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class;
    }
}