using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Infrastructure.Persistence;
using FocusLifePlus.Infrastructure.Persistence.Repositories;

namespace FocusLifePlus.Infrastructure.Tests.Persistence
{
    public class TaskRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly ApplicationDbContext _context;
        private readonly FocusTaskRepository _repository;

        public TaskRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);
            _repository = new FocusTaskRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ValidTask_AddsToDatabase()
        {
            // Arrange
            var task = new FocusTask
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1)
            };

            // Act
            await _repository.AddAsync(task);
            await _context.SaveChangesAsync();

            // Assert
            var savedTask = await _context.FocusTasks.FirstOrDefaultAsync(t => t.Id == task.Id);
            Assert.NotNull(savedTask);
            Assert.Equal(task.Title, savedTask.Title);
            Assert.Equal(task.Description, savedTask.Description);
            Assert.Equal(task.DueDate, savedTask.DueDate);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingTask_ReturnsTask()
        {
            // Arrange
            var task = new FocusTask
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1)
            };
            await _context.FocusTasks.AddAsync(task);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(task.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Id, result.Id);
            Assert.Equal(task.Title, result.Title);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllTasks()
        {
            // Arrange
            var tasks = new[]
            {
                new FocusTask { Title = "Task 1", Description = "Description 1", DueDate = DateTime.Now.AddDays(1) },
                new FocusTask { Title = "Task 2", Description = "Description 2", DueDate = DateTime.Now.AddDays(2) }
            };
            await _context.FocusTasks.AddRangeAsync(tasks);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Title == "Task 1");
            Assert.Contains(result, t => t.Title == "Task 2");
        }
    }
} 