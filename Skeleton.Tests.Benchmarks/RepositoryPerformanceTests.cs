using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Skeleton.Tests.Benchmarks
{
    [TestClass]
    public class RepositoryPerformanceTests 
    {
        public RepositoryPerformanceTests()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(builder =>
                    builder.UsingConfigConnectionString("Default").Build());
        }

    public static IDependencyResolver Container => Bootstrapper.Resolver;

    [TestMethod]
        public void RunBenchmarks()
        {
            DbPostSeeder.SeedPosts();

            var benchmarks = new Benchmarks();

            using (var connection = new SqlConnectionHelper().OpenConnection())
            {
                // HAND CODED
                var postCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"select postId, [Text], [CreationDate], LastChangeDate,
                Counter1,Counter2,Counter3,Counter4,Counter5,Counter6,Counter7,Counter8,Counter9 from Post"
                };

                benchmarks.Add(() =>
                {
                    var list = new List<Post>();
                    using (var reader = postCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var post = new Post
                            {
                                PostId = reader.GetInt32(0),
                                Text = reader.GetNullableString(1),
                                CreationDate = reader.GetDateTime(2),
                                LastChangeDate = reader.GetDateTime(3),
                                Counter1 = reader.GetNullableValue<int>(4),
                                Counter2 = reader.GetNullableValue<int>(5),
                                Counter3 = reader.GetNullableValue<int>(6),
                                Counter4 = reader.GetNullableValue<int>(7),
                                Counter5 = reader.GetNullableValue<int>(8),
                                Counter6 = reader.GetNullableValue<int>(9),
                                Counter7 = reader.GetNullableValue<int>(10),
                                Counter8 = reader.GetNullableValue<int>(11),
                                Counter9 = reader.GetNullableValue<int>(12)
                            };

                            list.Add(post);
                        }
                    }
                }, "HandCoded");

                // With Skeleton.ORM
                var repository = Container.Resolve<IEntityReader<Post>>();

                benchmarks.Add(() =>
                {
                    repository.Find();
                }, "Skeleton.Orm => Cold start");

                benchmarks.Add(() =>
                {
                    repository.Find();
                }, "Skeleton.Orm => Hot start (benefits of TypeCacheAccessor)");

                var repositoryAsync = Container.Resolve<IAsyncEntityReader<Post>>();

                benchmarks.Add(() =>
                {
                    var list = repositoryAsync.FindAsync().Result;
                }, "Skeleton.Orm.Async => Hot start");

                benchmarks.Run();
            }
        }
    }
}