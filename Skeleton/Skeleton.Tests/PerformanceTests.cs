using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Tests.Infrastructure;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Skeleton.Tests
{
    [TestClass]
    public class PerformanceTests : TestBase
    {
        [TestMethod]
        public void Run()
        {
            PostSeeder.Seed();

            var benchmarks = new Benchmarks();

            using (var connection = new SqlConnectionHelper().OpenConnection())
            {
                // HAND CODED
                var postCommand = new SqlCommand();
                postCommand.Connection = connection;
                postCommand.CommandText = @"select postId, [Text], [CreationDate], LastChangeDate,
                Counter1,Counter2,Counter3,Counter4,Counter5,Counter6,Counter7,Counter8,Counter9 from Post";

                benchmarks.Add(() =>
                    {
                        var list = new List<Post>();
                        using (var reader = postCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var post = new Post();
                                post.PostId = reader.GetInt32(0);
                                post.Text = reader.GetNullableString(1);
                                post.CreationDate = reader.GetDateTime(2);
                                post.LastChangeDate = reader.GetDateTime(3);

                                post.Counter1 = reader.GetNullableValue<int>(4);
                                post.Counter2 = reader.GetNullableValue<int>(5);
                                post.Counter3 = reader.GetNullableValue<int>(6);
                                post.Counter4 = reader.GetNullableValue<int>(7);
                                post.Counter5 = reader.GetNullableValue<int>(8);
                                post.Counter6 = reader.GetNullableValue<int>(9);
                                post.Counter7 = reader.GetNullableValue<int>(10);
                                post.Counter8 = reader.GetNullableValue<int>(11);
                                post.Counter9 = reader.GetNullableValue<int>(12);
                                list.Add(post);
                            }
                        }
                    }, "HandCoded");

                // With Skeleton.ORM
                var accessorCache = Container.Resolve<ITypeAccessorCache>();
                var database = Container.Resolve<IDatabase>();
                var repository = new PostRepository(accessorCache, database);

                benchmarks.Add(() =>
                    {
                        var list = repository.GetAll();
                    }, "Skeleton.Orm => Cold start");

                benchmarks.Add(() =>
                {
                    var list = repository.GetAll();
                }, "Skeleton.Orm => Hot start (benefits of TypeCacheAccessor)");

                var databaseAsync = Container.Resolve<IDatabaseAsync>();
                var repositoryAsync = new PostRepositoryAsync(accessorCache, databaseAsync);

                benchmarks.Add(() =>
                {
                    var list = repositoryAsync.GetAllAsync().Result;
                }, "Skeleton.Orm.Async => Hot start");

                benchmarks.Run();
            }
        }
    }
}