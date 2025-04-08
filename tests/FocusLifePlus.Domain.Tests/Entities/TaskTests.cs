using FocusLifePlus.Domain.Common.Enums;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Domain.Tests.Entities
{
    public class TaskTests
    {
        [Fact]
        public void Create_WithValidParameters_CreatesTask()
        {
            // Arrange
            var title = "Test Task";
            var description = "Test Description";
            var dueDate = DateTime.Now.AddDays(1);

            // Act
            var task = new FocusTask
            {
                Title = title,
                Description = description,
                DueDate = dueDate
            };

            // Assert
            Assert.Equal(title, task.Title);
            Assert.Equal(description, task.Description);
            Assert.Equal(dueDate, task.DueDate);
            Assert.False(task.IsCompleted);
            Assert.NotEqual(Guid.Empty, task.Id);
        }

        [Fact]
        public void Complete_SetsIsCompletedToTrue()
        {
            // Arrange
            var task = new FocusTask
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1)
            };

            // Act
            task.Complete();

            // Assert
            Assert.True(task.IsCompleted);
        }

        [Fact]
        public void Update_WithValidParameters_UpdatesTask()
        {
            // Arrange
            var task = new FocusTask
            {
                Title = "Original Title",
                Description = "Original Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = FocusTaskPriority.Medium
            };

            var newTitle = "Updated Title";
            var newDescription = "Updated Description";
            var newDueDate = DateTime.Now.AddDays(2);
            var newPriority = FocusTaskPriority.High;
            
            // Act
            task.Update(newTitle, newDescription, newDueDate, newPriority);

            // Assert
            Assert.Equal(newTitle, task.Title);
            Assert.Equal(newDescription, task.Description);
            Assert.Equal(newDueDate, task.DueDate);
            Assert.Equal(newPriority, task.Priority);
        }
    }
} 