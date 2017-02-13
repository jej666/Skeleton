using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction.Repository;
using Skeleton.Tests.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Tests.Infrastructure
{
    [TestClass]
    public class StoredProcedureExecutorTests : RepositoryTestBase
    {
        private const string ProcedureName = "ProcedureSelectCustomerByCategory";
        private readonly IDictionary<string, object> parameters = new Dictionary<string, object> { { "@CategoryId", 9 } };

        public StoredProcedureExecutorTests()
        {
            SqlLocalDbHelper.InstallProcStocIfNotExists();
        }

        [TestMethod]
        public void ExecuteStoredProcedure()
        {
            using (var executor = Container.Resolve<IStoredProcedureExecutor>())
            {
                var result = executor.Execute(ProcedureName, parameters);

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task AsyncExecuteStoredProcedure()
        {
            using (var executor = Container.Resolve<IAsyncStoredProcedureExecutor>())
            {
                var result = await executor.ExecuteAsync(ProcedureName, parameters);

                Assert.IsNotNull(result);
            }
        }
    }
}
