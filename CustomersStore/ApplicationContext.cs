using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CustomersStore
{
    public class ApplicationContext : DbContext
    {
        private readonly string connectionString = @"";
        public DbSet<Customer> Customers { get; set; } = null!;

        public ApplicationContext()
        {
            if (Database.EnsureCreated())
                _ = AddFilterStoredProcedure();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                    new Customer { Id = 1, Name = "First", CompanyName = "Amazon", Phone = "123435", Email = "first@test.com" },
                    new Customer { Id = 2, Name = "Second", CompanyName = "Microsoft", Phone = "2354645", Email = "second@test.com" },
                    new Customer { Id = 3, Name = "Third", CompanyName = "Netflix", Phone = "345463", Email = "third@test.com" },
                    new Customer { Id = 4, Name = "Fourth", CompanyName = "Apple", Phone = "3637346", Email = "fourth@test.com" });
        }


        private async Task AddFilterStoredProcedure()
        {
            SqlConnection connection = new(connectionString);
            await connection.OpenAsync();
            string addFilterCommand = @"CREATE PROCEDURE [dbo].[Sorting]
                                            @filterParameter NVARCHAR(100),
                                            @sortingDirection NVARCHAR(10)

                                            AS

                                            SELECT [Id]
	                                              ,[Name]
                                                  ,[CompanyName]
                                                  ,[Phone]
                                                  ,[Email]
                                              FROM [CustomersDB].[dbo].[Customers]
                                              ORDER BY 
	                                            CASE WHEN @sortingDirection = 'A' THEN
		                                            CASE 
			                                            WHEN @filterParameter = 'Name' THEN Name
			                                            WHEN @filterParameter = 'Company' THEN CompanyName
			                                            WHEN @filterParameter = 'Phone' THEN Phone
			                                            WHEN @filterParameter = 'Email' THEN Email
		                                            END
	                                            END ASC,

	                                            CASE WHEN @sortingDirection = 'D' THEN
		                                            CASE 
			                                            WHEN @filterParameter = 'Name' THEN Name
			                                            WHEN @filterParameter = 'Company' THEN CompanyName
			                                            WHEN @filterParameter = 'Phone' THEN Phone
			                                            WHEN @filterParameter = 'Email' THEN Email
		                                            END
	                                            END DESC;";
            SqlCommand command = new SqlCommand(addFilterCommand, connection);
            await command.ExecuteNonQueryAsync();
        }
    }

}
