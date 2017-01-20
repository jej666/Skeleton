using Skeleton.Tests.Common;

namespace Skeleton.Tests.Benchmarks
{
    public static class DbPostSeeder
    {
        public static void SeedPosts()
        {
            var connection = new SqlConnectionHelper();
            using (var cnn = connection.OpenConnection())
            {
                var cmd = cnn.CreateCommand();
                cmd.CommandText = @"
                if (OBJECT_ID('Post') is null)
                    begin
	                    create table Post
	                    (
		                    PostId int identity primary key,
		                    [Text] varchar(max) not null,
		                    CreationDate datetime not null,
		                    LastChangeDate datetime not null,
		                    Counter1 int,
		                    Counter2 int,
		                    Counter3 int,
		                    Counter4 int,
		                    Counter5 int,
		                    Counter6 int,
		                    Counter7 int,
		                    Counter8 int,
		                    Counter9 int
	                    )
	                    set nocount on
	                    declare @i int
	                    declare @id int
	                    set @i = 0
	                    while @i <= 5001
	                    begin
		                    insert Post ([Text],CreationDate, LastChangeDate) values (replicate('x', 2000), GETDATE(), GETDATE())
		                    set @id = @@IDENTITY
		                    set @i = @i + 1
	                    end
                    end";

                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
