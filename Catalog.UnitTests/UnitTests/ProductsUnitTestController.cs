using AutoMapper;
using Catalog.Api.Context;
using Catalog.Api.DTOs.Mappings;
using Catalog.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.UnitTests.UnitTests
{
    public class ProductsUnitTestController
    {
        public IUnitOfWork repository;
        public IMapper mapper;
        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost;DataBase=CatalogDB;Uid=root;Pwd=admin";

        static ProductsUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;
        }

        public ProductsUnitTestController()
        {
            var config = new MapperConfiguration(config =>
            {
                config.AddProfile(new ProductDTOMappingProfile());
            });

            mapper = config.CreateMapper();
            var context = new AppDbContext(dbContextOptions);
            repository = new UnitOfWork(context);
        }
    }
}
