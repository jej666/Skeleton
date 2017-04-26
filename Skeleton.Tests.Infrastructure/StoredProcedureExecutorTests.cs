using NUnit.Framework;
using Skeleton.Abstraction.Orm;
using Skeleton.Tests.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class StoredProcedureExecutorTests : OrmTestBase
    {
        private const string ProcedureName = "ProcedureSelectCustomerByCategory";
        private readonly IDictionary<string, object> parameters = new Dictionary<string, object> { { "@CategoryId", 9 } };

        public StoredProcedureExecutorTests()
        {
            SqlLocalDbHelper.InstallProcStocIfNotExists();
        }

        [Test]
        public void StoredProcedure_Execute()
        {
            using (var executor = Container.Resolve<IStoredProcedureExecutor>())
            {
                var result = executor.Execute(ProcedureName, parameters);

                Assert.IsNotNull(result);
            }
        }

        [Test]
        public async Task StoredProcedure_ExecuteAsync()
        {
            using (var executor = Container.Resolve<IAsyncStoredProcedureExecutor>())
            {
                var result = await executor.ExecuteAsync(ProcedureName, parameters);

                Assert.IsNotNull(result);
            }
        }
    }
}