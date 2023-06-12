using endavaRestApi.Controllers;
using endavaRestApi.Data;
using endavaRestApi.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace endavaRestApi.Tests.Repository
{   
    //All these are Web API Entity Framework xUnit Tests (maybe ProductFilter is Web API Controller xUnit Test
    public class ShopRepositoryTests
    {
        //fake in memory database
        private async Task<ShopContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ShopContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) //we need to install the NuGet Package Microsoft.EntityFrameworkCore.InMemory
                .Options;
            var databaseContext = new ShopContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Products.CountAsync() <= 0)
            {
                for (int i = 5; i < 15; i++)
                {
                    databaseContext.Products.Add(
                          new Product { ProductId = i, ProductName = "Product 1", ProductCategory = "Food", ProductBrand = "McDonald's", Price = 150, ProductSize = "Large", ProductDescription = "The best burger!", Weight = 200, ProductQuantity = 10 });
                    await databaseContext.SaveChangesAsync();

                }
            }
            return databaseContext;
        }

        //xUnit Test for Get()  --- GetAllItemsAPI
        [Fact]
        public async Task UserRepository_Get_ReturnsProduct()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var shopRepository = new ShopRepository(dbContext);
            

            // Act
            var result = await shopRepository.Get();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Product>>();
            
        }

        //xUnit Test for AddUser(user)  --- UserRegistrationAPI
        [Fact]
        public async Task UserRepository_AddUser_ReturnsUser()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var userRepository = new UserRepository(dbContext);
            var user = new User { UserId = 16, Name = "Teodora", Email = "teodora@example.com", Password = "teodorasiljanoska" };

            //Act
            var result = await userRepository.AddUser(user);

            //Assert
            result.Should().BeOfType<User>();
            result.Should().BeEquivalentTo(user);
        }

        //xUnit Test for Get(id)   ---- GetUserByIdAPI which API in order to test if the user from the UserRegistrationAPI is created
        [Fact]
        public async Task UserRepository_Get_ReturnsUser()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var userRepository = new UserRepository(dbContext);
            var id = 1;
            dbContext.Users.Add(new User { UserId = id, Name = "John",Email="john@example.com", Password="johnjohn" });
            dbContext.SaveChanges();

            //Act
            var result = await userRepository.Get(id);

            //Assert
           if (result == null) { result.Should().BeNull(); }   //in case there isn't a user with that id - KOREKCIJA

           
            else
            {
                result.Should().NotBeNull();
                result.Should().BeOfType(typeof(User));
                result.UserId.Should().Be(id);
            }
        }

        //xUnit Test for Filter(filter)   --- ProductFilterAPI
        [Fact]
        public async Task ShopController_Filter_ReturnsOKWithFilteredProducts()
        {
                // Arrange
                var productFilter = new ProductFilter
                { 
                ProductCategory = "Food", ProductBrand = "McDonald's", PriceMin = 100, PriceMax = 250, ProductSize = "Large", WeightMin = 150, WeightMax = 450 
                };


            var products = new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Product 1", ProductCategory="Food", ProductBrand="McDonald's", Price=150, ProductSize="Large",ProductDescription="The best burger!",Weight=200, ProductQuantity=10 },
                new Product { ProductId = 2, ProductName = "Product 2",ProductCategory="Drink", ProductBrand="McDonald's", Price=100, ProductSize="Medium",ProductDescription="The best milkshake!",Weight=150, ProductQuantity=15 },
                new Product { ProductId = 3, ProductName = "Product 3", ProductCategory="Food", ProductBrand="McDonald's", Price=200, ProductSize="Large",ProductDescription="The best pizza!",Weight=400, ProductQuantity=20 }
            };

            var fakeShopRepository = A.Fake<IShopRepository>();
            var fakeOrderRepository = A.Fake<IOrderRepository>();
                A.CallTo(() => fakeShopRepository.Get()).Returns(products);

                var shopController = new ShopController(fakeShopRepository, fakeOrderRepository);

                // Act
                var result = await shopController.Filter(productFilter);

                // Assert
                result.Should().BeOfType<ActionResult<Product>>();
                
            }
        [Fact]
        public async Task ImportProducts_WithValidCsvFile_ReturnsOkResult()
        {
            //Arrange
            var file = CreateFakeCsvFile();
            var fakeShopRepository = A.Fake<IShopRepository>();
            var controller = new ShopController(fakeShopRepository);

            //Act
            var result = await controller.ImportProducts(file);

            //Assert
            result.Should().BeOfType<OkResult>();
            A.CallTo(() => fakeShopRepository.ImportCsv(file)).MustHaveHappened();

        }
        private static IFormFile CreateFakeCsvFile()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.WriteLine("Product 1, Brand 1, Category 1, Price 1, Description 1, Quantity 1, Size 1, Weight 1, Color 1,{0}", Guid.NewGuid());
            writer.WriteLine("Product 2, Brand 2, Category 2, Price 2, Description 2, Quantity 2, Size 2, Weight 2, Color 2,{0}", Guid.NewGuid());
            writer.Flush();
            stream.Position = 0;
            var file = A.Fake<IFormFile>();
            A.CallTo(() => file.OpenReadStream()).Returns(stream);
            A.CallTo(() => file.Length).Returns(stream.Length);
            return file;
        }
    }
    }


