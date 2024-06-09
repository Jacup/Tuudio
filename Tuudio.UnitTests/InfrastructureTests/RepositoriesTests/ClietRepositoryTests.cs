using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tuudio.Domain.Entities;
using Tuudio.Domain.Entities.People;
using Tuudio.Domain.Exceptions;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Repositories;

namespace Tuudio.Tests.Repositories
{
    public class GenericRepositoryTests
    {
        private TuudioDbContext _context;
        private GenericRepository<Client> _repository;
        private Guid _defaultGuid = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            // Konfiguracja DbContext w pamięci
            var options = new DbContextOptionsBuilder<TuudioDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TuudioDbContext(options);
            _repository = new GenericRepository<Client>(_context);

            // Dodanie przykładowych danych do bazy w pamięci
            var clients = new List<Client>
            {
                new Client { Id = _defaultGuid, FirstName = "John", LastName = "Doe" },
                new Client { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" },
                new Client { Id = Guid.NewGuid(), FirstName = "Mike", LastName = "Johnson" }
            };

            _context.AddRange(clients);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task DeleteAsync_WhenEntityExists_ShouldDeleteEntity()
        {
            // Arrange
            var entityId = _context.Clients.First().Id;

            // Act
            await _repository.DeleteAsync(entityId);
            await _repository.SaveAsync();

            // Assert
            var deletedEntity = await _context.Clients.FindAsync(entityId);
            deletedEntity.ShouldBeNull();
        }

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

        [Test]
        public async Task GetByIdAsync_WhenEntityExists_ShouldReturnEntity()
        {
            // Arrange
            var entityId = _context.Clients.First().Id;

            // Act
            var result = await _repository.GetByIdAsync(entityId);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBeEquivalentTo(entityId);
        }

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

        [Test]
        public async Task InsertAsync_ShouldInsertEntity()
        {
            // Arrange
            var entity = new Client { Id = Guid.NewGuid(), FirstName = "Anna", LastName = "White" };

            // Act
            await _repository.InsertAsync(entity);

            // Assert
            var insertedEntity = await _context.Clients.FindAsync(entity.Id);

            insertedEntity.ShouldNotBeNull();
            insertedEntity.ShouldBeEquivalentTo(entity);
        }

        [Test]
        public async Task InsertAsync_WhenEntityWithSameIdExists_ShouldThrowDbUpdateException()
        {
            // Arrange
            var entity = new Client { Id = _defaultGuid, FirstName = "Anna", LastName = "White" };

            // Act + Assert
            var ex = await Should.ThrowAsync<InvalidOperationException>(() => _repository.InsertAsync(entity));
        }

        [Test]
        public async Task UpdateAsync_WhenEntityExists_ShouldUpdateEntity()
        {
            // Arrange
            var entityId = _context.Clients.First().Id;
            var updatedEntity = new Client { Id = entityId, FirstName = "Updated John", LastName = "Doe" };

            // Act
            await _repository.UpdateAsync(updatedEntity);

            // Assert
            var entity = await _context.Clients.FindAsync(entityId);

            entity.ShouldNotBeNull();
            entity.FirstName.ShouldBeEquivalentTo(updatedEntity.FirstName);
        }

        [Test]
        public async Task UpdateAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
        {
            var ex = await Should.ThrowAsync<ArgumentNullException>(() => _repository.UpdateAsync(null as Client));
        }

        [Test]
        public void UpdateAsync_WhenEntityNotExists_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            var entityId = Guid.NewGuid(); // Random non-existent Id
            var entity = new Client { Id = entityId, FirstName = "John", LastName = "Doe" };

            // Act + Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException<Client>>(async () => await _repository.UpdateAsync(entity));
            Assert.That(ex.EntityId, Is.EqualTo(entityId));
        }
    }
}
