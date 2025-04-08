using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using FocusLifePlus.API.Controllers;
using FocusLifePlus.Application.Features.FocusTasks.Commands.CreateFocusTask;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTasks;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById;
using Microsoft.Extensions.Logging;

namespace FocusLifePlus.API.Tests.Controllers
{
    public class FocusTaskControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly FocusTaskController _controller;
        private readonly ILogger<FocusTaskController> _logger;

        public FocusTaskControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new FocusTaskController(_mediatorMock.Object,_logger);
        }

        [Fact]
        public async Task GetFocusTasks_ReturnsOkResult()
        {
            // Arrange
            var query = new GetFocusTasksQuery();
            var expectedResult = new List<FocusTaskDto>();
            _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetFocusTasks(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<FocusTaskDto>>(okResult.Value);
            Assert.Same(expectedResult, returnValue);
        }

        [Fact]
        public async Task CreateFocusTask_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var command = new CreateFocusTaskCommand { Title = "Test Task" };
            var expectedId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _controller.CreateFocusTask(command);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(FocusTaskController.GetFocusTaskById), createdAtResult.ActionName);
            Assert.Equal(expectedId, createdAtResult.RouteValues["id"]);
        }
    }
} 