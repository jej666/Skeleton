﻿namespace Skeleton.Tests.Common
{
    public static class SqlDbSeeder
    {
        public static void SeedCustomers()
        {
            var connection = new SqlConnectionHelper();
            using (var cnn = connection.OpenConnection())
            {
                var cmd = cnn.CreateCommand();
                cmd.CommandText = @"

                if (OBJECT_ID('CustomerCategory') is null)
                    begin
	                    CREATE TABLE [dbo].[CustomerCategory] (
                        [CustomerCategoryId] INT        IDENTITY (1, 1) NOT NULL,
                        [Name]               NCHAR (50) NULL,
                        PRIMARY KEY CLUSTERED ([CustomerCategoryId] ASC)
                        )
                        set nocount on
	                    declare @i int
	                    set @i = 1
	                    while @i <= 10
	                    begin
		                    insert CustomerCategory ([Name]) values ('Category' + CAST(@i as varchar(2)))
		                    set @i = @i + 1
	                    end
                    end

                if (OBJECT_ID('Customer') is null)
                    begin
	                    CREATE TABLE [dbo].[Customer] (
                        [CustomerId]         INT           IDENTITY (1, 1) NOT NULL,
                        [Name]               NVARCHAR (50) NULL,
                        [CustomerCategoryId] INT           NULL,
                        PRIMARY KEY CLUSTERED ([CustomerId] ASC)
                        )
                        set nocount on
	                    declare @j int
	                    set @j = 1
	                    while @j <= 500
	                    begin
		                    insert Customer ([Name], [CustomerCategoryId]) values ('Customer' + CAST(@j as varchar(3)), ROUND( RAND() * 9 ,0) + 1)
		                    set @j = @j + 1
	                    end
                    end";
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();
            }
        }
    }
}