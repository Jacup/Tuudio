﻿using Microsoft.EntityFrameworkCore;
using Tuudio.Domain.Entities.People;
using Tuudio.Domain.Exceptions;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Repositories;

namespace Tuudio.UnitTests.InfrastructureTests.Services.Repositories
{
    public class ClientGenericRepositoryTests
    {
        private TuudioDbContext _context;
        private GenericRepository<Client> _repository;
        private readonly Guid _defaultGuid = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TuudioDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TuudioDbContext(options);
            _repository = new GenericRepository<Client>(_context);

            var clients = new List<Client>
            {
                new() { Id = _defaultGuid, FirstName = "John", LastName = "Doe" },
                new() { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" },
                new() { Id = Guid.NewGuid(), FirstName = "Mike", LastName = "Johnson" }
            };

            _context.AddRange(clients);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        //[Test]
        //public async Task DeleteAsync_WhenEntityExists_ShouldDeleteEntity()
        //{
        //    // Arrange
        //    var entityId = _context.Clients.First().Id;

        //    // Act
        //    await _repository.DeleteAsync(entityId);
        //    await _repository.SaveAsync();

        //    // Assert
        //    var deletedEntity = await _context.Clients.FindAsync(entityId);
        //    deletedEntity.ShouldBeNull();
        //}

        [Test]
        public async Task DeleteAsync_WhenEntityNotExists_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            var entityId = Guid.NewGuid();

            // Act + Assert
            var ex = await Should.ThrowAsync<EntityNotFoundException<Client>>(_repository.DeleteAsync(entityId));
            ex.EntityId.ShouldBeEquivalentTo(entityId);
        }

        [Test]
        public async Task GetAllAsync_WhenEntitiesExists_ShouldReturnAllEntities()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert

            result.Count().ShouldBeEquivalentTo(3);
        }

        //[Test]
        //public async Task GetByIdAsync_WhenEntityExists_ShouldReturnEntity()
        //{
        //    // Arrange
        //    var entityId = _context.Clients.First().Id;

        //    // Act
        //    var result = await _repository.GetByIdAsync(entityId);

        //    // Assert
        //    result.ShouldNotBeNull();
        //    result.Id.ShouldBeEquivalentTo(entityId);
        //}

        [Test]
        public async Task GetByIdAsync_WhenEntityNotFound_ShouldReturnNull()
        {
            // Arrange
            var entityId = Guid.NewGuid(); // Random non-existent Id

            // Act
            var result = await _repository.GetByIdAsync(entityId);

            // Assert
            result.ShouldBeNull();
        }

        //[Test]
        //public async Task InsertAsync_ShouldInsertEntity()
        //{
        //    // Arrange
        //    var entity = new Client { Id = Guid.NewGuid(), FirstName = "Anna", LastName = "White" };

        //    // Act
        //    await _repository.InsertAsync(entity);

        //    // Assert
        //    var insertedEntity = await _context.Clients.FindAsync(entity.Id);

        //    insertedEntity.ShouldNotBeNull();
        //    insertedEntity.ShouldBeEquivalentTo(entity);
        //}

        [Test]
        public async Task InsertAsync_WhenEntityWithSameIdExists_ShouldThrowDbUpdateException()
        {
            // Arrange
            var entity = new Client { Id = _defaultGuid, FirstName = "Anna", LastName = "White" };

            // Act + Assert
            var ex = await Should.ThrowAsync<InvalidOperationException>(() => _repository.InsertAsync(entity));
        }

        //[Test]
        //public async Task UpdateAsync_WhenEntityExists_ShouldUpdateEntity()
        //{
        //    // Arrange
        //    var entityId = _context.Clients.First().Id;
        //    var updatedEntity = new Client { Id = entityId, FirstName = "Updated John", LastName = "Doe" };

        //    // Act
        //    await _repository.UpdateAsync(updatedEntity);

        //    // Assert
        //    var entity = await _context.Clients.FindAsync(entityId);

        //    entity.ShouldNotBeNull();
        //    entity.FirstName.ShouldBeEquivalentTo(updatedEntity.FirstName);
        //}

        [Test]
        public async Task UpdateAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var ex = await Should.ThrowAsync<ArgumentNullException>(() => _repository.UpdateAsync(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        [Test]
        public void UpdateAsync_WhenEntityNotExists_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var entity = new Client { Id = entityId, FirstName = "John", LastName = "Doe" };

            // Act + Assert
            Should.ThrowAsync<EntityNotFoundException<Client>>(async () => await _repository.UpdateAsync(entity));
        }
    }
}
