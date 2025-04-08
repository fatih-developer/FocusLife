using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Domain.Repositories;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Application.Features.FocusTasks.Commands.CreateFocusTask;
using Moq;

namespace FocusLifePlus.Application.Tests.Features.Tasks
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<IFocusTaskRepository> _repositoryMock;
        private readonly CreateFocusTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _repositoryMock = new Mock<IFocusTaskRepository>();
            _handler = new CreateFocusTaskCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesTask()
        {
            // Arrange
            var command = new CreateFocusTaskCommand 
            { 
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                UserId = Guid.NewGuid()
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<FocusTask>()))
                .ReturnsAsync((FocusTask task) => task);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBe(Guid.Empty);
            _repositoryMock.Verify(r => r.AddAsync(It.Is<FocusTask>(t => 
                t.Title == command.Title && 
                t.Description == command.Description && 
                t.DueDate == command.DueDate)), 
                Times.Once);
        }

        [Fact]
        public async Task Handle_EmptyTitle_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateFocusTaskCommand 
            { 
                Title = "",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                UserId = Guid.NewGuid()
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<FocusTask>()), Times.Never);
        }
    }
} 