using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Repositories
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            // Uwaga: host.docker.internal działa tylko na Windows/Mac
            optionsBuilder.UseSqlServer(
                "Server=host.docker.internal,1433;Database=MyDatabase;User Id=sa;Password=MyPass123$;Encrypt=False;TrustServerCertificate=True");

            return new DataContext(optionsBuilder.Options);
        }
    }
}
