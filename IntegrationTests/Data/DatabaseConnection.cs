using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Data
{
    public class DatabaseConnection
    {
        private static ApplicationDbContext dbContext;
        public static ApplicationDbContext DbContext { get {
                if(dbContext == null)
                {
                    DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlServer("ConnectionStrings__DefaultConnection=Server=db;Database=master;User=sa;Password=Dexcelence!1",
                            sqlOptions => sqlOptions.EnableRetryOnFailure(50, TimeSpan.FromSeconds(30), null))
                        .Options;

                    dbContext = new ApplicationDbContext(options);
                }
                return dbContext;
            }
        }
    }
}
