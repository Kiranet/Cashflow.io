﻿using System;
using System.Linq;
using Cashflowio.Core.Entities;
using Cashflowio.Core.Interfaces;
using Cashflowio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Cashflowio.Tests.Integration.Data
{
    public class EfRepositoryShould
    {
        private AppDbContext _dbContext;

        private static DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("cashflowio")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public void AddItemAndSetId()
        {
            var repository = GetRepository();
            var item = new RawTransactionBuilder().Build();

            repository.Add(item);

            var newItem = repository.List<RawTransaction>().FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem?.Id > 0);
        }

        [Fact]
        public void UpdateItemAfterAddingIt()
        {
            // add an item
            var repository = GetRepository();
            var initialTitle = Guid.NewGuid().ToString();
            //var item = new RawTransactionBuilder().Title(initialTitle).Build();

            //repository.Add(item);

            //// detach the item so we get a different instance
            //_dbContext.Entry(item).State = EntityState.Detached;

            //// fetch the item and update its title
            //var newItem = repository.List<RawTransaction>()
            //    .FirstOrDefault(i => i.Title == initialTitle);
            //Assert.NotNull(newItem);
            //Assert.NotSame(item, newItem);
            //var newTitle = Guid.NewGuid().ToString();
            //newItem.Title = newTitle;

            //// Update the item
            //repository.Update(newItem);
            //var updatedItem = repository.List<RawTransaction>()
            //    .FirstOrDefault(i => i.Title == newTitle);

            //Assert.NotNull(updatedItem);
            //Assert.NotEqual(item.Title, updatedItem.Title);
            //Assert.Equal(newItem.Id, updatedItem.Id);
        }

        [Fact]
        public void DeleteItemAfterAddingIt()
        {
            //// add an item
            //var repository = GetRepository();
            //var initialTitle = Guid.NewGuid().ToString();
            //var item = new RawTransactionBuilder().Title(initialTitle).Build();
            //repository.Add(item);

            //// delete the item
            //repository.Delete(item);

            //// verify it's no longer there
            //Assert.DoesNotContain(repository.List<RawTransaction>(),
                //i => i.Title == initialTitle);
        }

        private EfRepository GetRepository()
        {
            var options = CreateNewContextOptions();
            var mockDispatcher = new Mock<IDomainEventDispatcher>();

            _dbContext = new AppDbContext(options, mockDispatcher.Object);
            return new EfRepository(_dbContext);
        }
    }
}
