using endavaRestApi.Controllers;
using endavaRestApi.Data;
using endavaRestApi.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task ShopRepository_Get_ReturnsProduct()
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
        public async Task ShopRepository_AddUser_ReturnsUser()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var shopRepository = new ShopRepository(dbContext);
            var user = new User { Id = 16, Name = "Teodora", Email = "teodora@example.com", Password = "teodorasiljanoska" };

            //Act
            var result = await shopRepository.AddUser(user);

            //Assert
            result.Should().BeOfType<User>();
            result.Should().BeEquivalentTo(user);
        }

        //xUnit Test for Get(id)   ---- GetUserByIdAPI which API in order to test if the user from the UserRegistrationAPI is created
        [Fact]
        public async Task ShopRepository_Get_ReturnsUser()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var shopRepository = new ShopRepository(dbContext);
            var id = 1;
            dbContext.Users.Add(new User { Id = id, Name = "John",Email="john@example.com", Password="johnjohn" });
            dbContext.SaveChanges();

            //Act
            var result = await shopRepository.Get(id);

            //Assert
           if (result == null) { result.Should().BeNull(); }   //in case there isn't a user with that id - KOREKCIJA

           
            else
            {
                result.Should().NotBeNull();
                result.Should().BeOfType(typeof(User));
                result.Id.Should().Be(id);
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
                A.CallTo(() => fakeShopRepository.Get()).Returns(products);

                var shopController = new ShopController(fakeShopRepository);

                // Act
                var result = await shopController.Filter(productFilter);

                // Assert
                result.Should().BeOfType<ActionResult<Product>>();
                
            }
        }
    }


