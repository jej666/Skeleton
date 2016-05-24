﻿using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    [TestClass]
    public class PerformanceTests : TestBase
    {
        [TestMethod]
        public void Run()
        {
            Seeder.SeedPosts();

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

                //var databaseAsync = Container.Resolve<IDatabaseAsync>();
                //var repositoryAsync = new PostRepositoryAsync(accessorCache, databaseAsync);

                //benchmarks.Add(() =>
                //{
                //    var list = repositoryAsync.GetAllAsync().Result;
                //}, "Skeleton.Orm.Async => Hot start");

                benchmarks.Run();
            }
        }
    }
}