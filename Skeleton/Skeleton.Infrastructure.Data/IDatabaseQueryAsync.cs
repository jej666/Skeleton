namespace Skeleton.Infrastructure.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    public interface IDatabaseQueryAsync
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<IEnumerable<TResult>> FindAsync<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class;

        Task<TResult> FirstOrDefaultAsync<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class;
    }
}