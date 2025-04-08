# Görev Takibi Modülü için Komut İşleyicileri

Görev takibi modülü için komut işleyicilerini geliştireceğim. Öncelikle gerekli dosyaları oluşturup, komut işleyicilerini ve ilgili sınıfları yazacağım.

İlk olarak, görev takibi için gerekli entity ve repository interface'ini oluşturalım.


```c#:src/FocusLifePlus.Domain/Entities/Task.cs
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Domain.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid UserId { get; set; }
        public int ProgressPercentage { get; set; } = 0;
        public TimeSpan? EstimatedDuration { get; set; }
        public TimeSpan? ActualDuration { get; set; }

        public bool IsCompleted => Status == TaskStatus.Done;

        public Task()
        {
            CreatedAt = DateTime.UtcNow;
            Status = TaskStatus.Todo;
            ProgressPercentage = 0;
        }

        public void Complete()
        {
            Status = TaskStatus.Done;
            CompletedAt = DateTime.UtcNow;
            ProgressPercentage = 100;
        }

        public void Update(string title, string description, DateTime dueDate, TaskPriority priority)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
        }

        public void UpdateProgress(int progressPercentage)
        {
            if (progressPercentage < 0 || progressPercentage > 100)
                throw new ArgumentException("Progress percentage must be between 0 and 100");

            ProgressPercentage = progressPercentage;
            
            if (progressPercentage == 100)
                Complete();
            else if (progressPercentage > 0)
                Status = TaskStatus.InProgress;
        }
    }
}
```
Şimdi TaskStatus ve TaskPriority enum'larını oluşturalım:


```c#:src/FocusLifePlus.Domain/Common/Enums/TaskStatus.cs
namespace FocusLifePlus.Domain.Common.Enums
{
    public enum TaskStatus
    {
        Todo,
        InProgress,
        Done,
        Cancelled
    }
}
```

```c#:src/FocusLifePlus.Domain/Common/Enums/TaskPriority.cs
namespace FocusLifePlus.Domain.Common.Enums
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Urgent
    }
}
```
Şimdi Task repository interface'ini oluşturalım:



```c#:src/FocusLifePlus.Domain/Repositories/ITaskRepository.cs
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Domain.Repositories
{
    public interface ITaskRepository : IRepository<Entities.Task>
    {
        Task<IEnumerable<Entities.Task>> GetTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<Entities.Task>> GetTasksByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Entities.Task>> GetTasksByStatusAsync(TaskStatus status);
        Task<IEnumerable<Entities.Task>> GetTasksByPriorityAsync(TaskPriority priority);
        Task<IEnumerable<Entities.Task>> GetOverdueTasksAsync();
        Task<IEnumerable<Entities.Task>> GetTasksDueTodayAsync();
        Task<IEnumerable<Entities.Task>> GetTasksDueThisWeekAsync();
        Task<IEnumerable<Entities.Task>> GetTasksDueThisMonthAsync();
    }
}
```
Linter hatalarını düzeltelim:



```c#:src/FocusLifePlus.Domain/Repositories/ITaskRepository.cs
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Domain.Repositories
{
    public interface ITaskRepository : IRepository<Entities.Task>
    {
        Task<IEnumerable<Entities.Task>> GetTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<Entities.Task>> GetTasksByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Entities.Task>> GetTasksByStatusAsync(TaskStatus status);
        Task<IEnumerable<Entities.Task>> GetTasksByPriorityAsync(TaskPriority priority);
        Task<IEnumerable<Entities.Task>> GetOverdueTasksAsync();
        Task<IEnumerable<Entities.Task>> GetTasksDueTodayAsync();
        Task<IEnumerable<Entities.Task>> GetTasksDueThisWeekAsync();
        Task<IEnumerable<Entities.Task>> GetTasksDueThisMonthAsync();
    }
}
```
Şimdi IRepository interface'ini oluşturalım ve TaskStatus için tam yolu belirtelim:




```c#:src/FocusLifePlus.Domain/Common/IRepository.cs
namespace FocusLifePlus.Domain.Common
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Guid id);
    }
}
```

```c#:src/FocusLifePlus.Domain/Repositories/ITaskRepository.cs
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Domain.Repositories
{
    public interface ITaskRepository : IRepository<Entities.Task>
    {
        Task<IEnumerable<Entities.Task>> GetTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<Entities.Task>> GetTasksByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Entities.Task>> GetTasksByStatusAsync(Domain.Common.Enums.TaskStatus status);
        Task<IEnumerable<Entities.Task>> GetTasksByPriorityAsync(TaskPriority priority);
        Task<IEnumerable<Entities.Task>> GetOverdueTasksAsync();
        Task<IEnumerable<Entities.Task>> GetTasksDueTodayAsync();
        Task<IEnumerable<Entities.Task>> GetTasksDueThisWeekAsync();
        Task<IEnumerable<Entities.Task>> GetTasksDueThisMonthAsync();
    }
}
```
Şimdi komut sınıflarını oluşturalım:



```c#:src/FocusLifePlus.Application/Tasks/Commands/CreateTask/CreateTaskCommand.cs
using MediatR;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid UserId { get; set; }
        public TimeSpan? EstimatedDuration { get; set; }
    }
}
```

```c#:src/FocusLifePlus.Application/Tasks/Commands/UpdateTask/UpdateTaskCommand.cs
using MediatR;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid? CategoryId { get; set; }
        public int? ProgressPercentage { get; set; }
        public TimeSpan? EstimatedDuration { get; set; }
        public TimeSpan? ActualDuration { get; set; }
    }
}
```

```c#:src/FocusLifePlus.Application/Tasks/Commands/DeleteTask/DeleteTaskCommand.cs
using MediatR;

namespace FocusLifePlus.Application.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
```
Şimdi komut işleyicilerini oluşturalım:




```c#:src/FocusLifePlus.Application/Tasks/Commands/CreateTask/CreateTaskCommandHandler.cs
using MediatR;
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Domain.Repositories;
using FocusLifePlus.Application.Common.Exceptions;

namespace FocusLifePlus.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new Domain.Entities.Task
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = request.Priority,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
                EstimatedDuration = request.EstimatedDuration
            };

            var createdTask = await _taskRepository.AddAsync(task);
            return createdTask.Id;
        }
    }
}
```

```c#:src/FocusLifePlus.Application/Tasks/Commands/UpdateTask/UpdateTaskCommandHandler.cs
using MediatR;
using FocusLifePlus.Domain.Repositories;
using FocusLifePlus.Application.Common.Exceptions;

namespace FocusLifePlus.Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);
            
            if (task == null)
                throw new NotFoundException(nameof(Domain.Entities.Task), request.Id);

            task.Update(request.Title, request.Description, request.DueDate, request.Priority);
            
            if (request.CategoryId.HasValue)
                task.CategoryId = request.CategoryId.Value;
                
            if (request.EstimatedDuration.HasValue)
                task.EstimatedDuration = request.EstimatedDuration.Value;
                
            if (request.ActualDuration.HasValue)
                task.ActualDuration = request.ActualDuration.Value;
                
            if (request.ProgressPercentage.HasValue)
                task.UpdateProgress(request.ProgressPercentage.Value);

            await _taskRepository.UpdateAsync(task);
            
            return Unit.Value;
        }
    }
}
```

```c#:src/FocusLifePlus.Application/Tasks/Commands/DeleteTask/DeleteTaskCommandHandler.cs
using MediatR;
using FocusLifePlus.Domain.Repositories;
using FocusLifePlus.Application.Common.Exceptions;

namespace FocusLifePlus.Application.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);
            
            if (task == null)
                throw new NotFoundException(nameof(Domain.Entities.Task), request.Id);

            await _taskRepository.DeleteAsync(task);
            
            return Unit.Value;
        }
    }
}
```
Şimdi NotFoundException sınıfını oluşturalım:




```c#:src/FocusLifePlus.Application/Common/Exceptions/NotFoundException.cs
namespace FocusLifePlus.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
```
Şimdi de ValidationException sınıfını oluşturalım:





```c#:src/FocusLifePlus.Application/Common/Exceptions/ValidationException.cs
namespace FocusLifePlus.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors)
            : this()
        {
            Errors = errors;
        }
    }
}
```
Şimdi komut doğrulama sınıflarını oluşturalım:






```c#:src/FocusLifePlus.Application/Tasks/Commands/CreateTask/CreateTaskCommandValidator.cs
using FluentValidation;
using FocusLifePlus.Application.Common.Exceptions;

namespace FocusLifePlus.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(v => v.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(v => v.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");

            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
```

```c#:src/FocusLifePlus.Application/Tasks/Commands/UpdateTask/UpdateTaskCommandValidator.cs
using FluentValidation;

namespace FocusLifePlus.Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(v => v.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(v => v.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");

            RuleFor(v => v.ProgressPercentage)
                .InclusiveBetween(0, 100).WithMessage("Progress percentage must be between 0 and 100.")
                .When(v => v.ProgressPercentage.HasValue);
        }
    }
}
```

```c#:src/FocusLifePlus.Application/Tasks/Commands/DeleteTask/DeleteTaskCommandValidator.cs
using FluentValidation;

namespace FocusLifePlus.Application.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
```
Şimdi de Behavior sınıflarını oluşturalım:






```c#:src/FocusLifePlus.Application/Common/Behaviors/ValidationBehavior.cs
using FluentValidation;
using MediatR;
using ValidationException = FocusLifePlus.Application.Common.Exceptions.ValidationException;

namespace FocusLifePlus.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    var errors = failures
                        .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                        .ToDictionary(g => g.Key, g => g.ToArray());

                    throw new ValidationException(errors);
                }
            }

            return await next();
        }
    }
}
```

```c#:src/FocusLifePlus.Application/Common/Behaviors/LoggingBehavior.cs
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FocusLifePlus.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling {RequestType}", typeof(TRequest).Name);

            var requestName = typeof(TRequest).Name;
            var uniqueId = Guid.NewGuid().ToString();
            var requestNameWithGuid = $"{requestName} [{uniqueId}]";

            _logger.LogInformation(
                "Begin Request {RequestNameWithGuid}",
                requestNameWithGuid);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var response = await next();

                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                _logger.LogInformation(
                    "End Request {RequestNameWithGuid} ({ElapsedMilliseconds}ms)",
                    requestNameWithGuid,
                    elapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                _logger.LogError(
                    ex,
                    "Request {RequestNameWithGuid} failed ({ElapsedMilliseconds}ms)",
                    requestNameWithGuid,
                    elapsedMilliseconds);

                throw;
            }
        }
    }
}
```

```c#:src/FocusLifePlus.Application/Common/Behaviors/UnhandledExceptionBehavior.cs
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FocusLifePlus.Application.Common.Behaviors
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnhandledExceptionBehavior(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "FocusLifePlus Request: Unhandled Exception for Request {Name} {@Request}",
                    requestName, request);

                var response = _httpContextAccessor.HttpContext.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = JsonSerializer.Serialize(new
                {
                    error = "An error occurred while processing your request.",
                    requestName = requestName,
                    timestamp = DateTime.UtcNow
                });

                await response.WriteAsync(result);
                throw;
            }
        }
    }
}
```
Linter hatalarını düzeltelim:






```c#:src/FocusLifePlus.Application/Common/Behaviors/UnhandledExceptionBehavior.cs
using System.Net;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FocusLifePlus.Application.Common.Behaviors
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnhandledExceptionBehavior(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "FocusLifePlus Request: Unhandled Exception for Request {Name} {@Request}",
                    requestName, request);

                var response = _httpContextAccessor.HttpContext.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = JsonSerializer.Serialize(new
                {
                    error = "An error occurred while processing your request.",
                    requestName = requestName,
                    timestamp = DateTime.UtcNow
                });

                await response.WriteAsync(result);
                throw;
            }
        }
    }
}
```
Şimdi de DependencyInjection sınıfını güncelleyelim:








```c#:src/FocusLifePlus.Application/DependencyInjection.cs
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FocusLifePlus.Application.Common.Behaviors;

namespace FocusLifePlus.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }
    }
}
```
# Görev Takibi Modülü Komut İşleyicileri

Görev takibi modülü için komut işleyicilerini başarıyla geliştirdim. Aşağıda oluşturduğum yapının detaylı bir açıklamasını bulabilirsiniz.

## Oluşturulan Yapı

### 1. Domain Katmanı

#### Entity
- **Task**: Görev entity'si, temel özellikleri ve davranışları içerir.
  - Temel özellikler: Id, Title, Description, DueDate, Priority, Status, CategoryId, UserId
  - Ek özellikler: ProgressPercentage, EstimatedDuration, ActualDuration
  - Davranışlar: Complete(), Update(), UpdateProgress()

#### Enum'lar
- **TaskStatus**: Todo, InProgress, Done, Cancelled
- **TaskPriority**: Low, Medium, High, Urgent

#### Repository Interface
- **ITaskRepository**: Görev verilerine erişim için temel CRUD işlemleri ve özel sorgular içerir.
  - Temel CRUD işlemleri: GetByIdAsync, GetAllAsync, AddAsync, UpdateAsync, DeleteAsync
  - Özel sorgular: GetTasksByUserIdAsync, GetTasksByCategoryIdAsync, GetTasksByStatusAsync, GetTasksByPriorityAsync, GetOverdueTasksAsync, GetTasksDueTodayAsync, GetTasksDueThisWeekAsync, GetTasksDueThisMonthAsync

### 2. Application Katmanı

#### Komutlar
- **CreateTaskCommand**: Yeni görev oluşturma komutu
- **UpdateTaskCommand**: Görev güncelleme komutu
- **DeleteTaskCommand**: Görev silme komutu

#### Komut İşleyicileri
- **CreateTaskCommandHandler**: Yeni görev oluşturma işlemini gerçekleştirir
- **UpdateTaskCommandHandler**: Görev güncelleme işlemini gerçekleştirir
- **DeleteTaskCommandHandler**: Görev silme işlemini gerçekleştirir

#### Doğrulama
- **CreateTaskCommandValidator**: Yeni görev oluşturma komutunun doğrulanması
- **UpdateTaskCommandValidator**: Görev güncelleme komutunun doğrulanması
- **DeleteTaskCommandValidator**: Görev silme komutunun doğrulanması

#### Davranışlar (Behaviors)
- **ValidationBehavior**: Komut doğrulama işlemini gerçekleştirir
- **LoggingBehavior**: Komut işleme sürecini loglar
- **UnhandledExceptionBehavior**: İşlenmeyen hataları yakalar ve uygun şekilde raporlar

#### Özel İstisnalar
- **NotFoundException**: Kaynak bulunamadığında fırlatılan istisna
- **ValidationException**: Doğrulama hataları olduğunda fırlatılan istisna

## Kullanım Örnekleri

### Yeni Görev Oluşturma

```csharp
// Controller veya başka bir sınıfta
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateTaskCommand command)
    {
        var taskId = await _mediator.Send(command);
        return Ok(taskId);
    }
}

// Kullanım
var command = new CreateTaskCommand
{
    Title = "Yeni Görev",
    Description = "Bu bir test görevidir",
    DueDate = DateTime.UtcNow.AddDays(7),
    Priority = TaskPriority.Medium,
    UserId = currentUserId
};

var taskId = await _mediator.Send(command);
```

### Görev Güncelleme

```csharp
// Controller veya başka bir sınıfta
[HttpPut("{id}")]
public async Task<ActionResult> Update(Guid id, UpdateTaskCommand command)
{
    if (id != command.Id)
        return BadRequest();

    await _mediator.Send(command);
    return NoContent();
}

// Kullanım
var command = new UpdateTaskCommand
{
    Id = taskId,
    Title = "Güncellenmiş Görev",
    Description = "Bu görev güncellenmiştir",
    DueDate = DateTime.UtcNow.AddDays(5),
    Priority = TaskPriority.High,
    ProgressPercentage = 50
};

await _mediator.Send(command);
```

### Görev Silme

```csharp
// Controller veya başka bir sınıfta
[HttpDelete("{id}")]
public async Task<ActionResult> Delete(Guid id)
{
    await _mediator.Send(new DeleteTaskCommand { Id = id });
    return NoContent();
}

// Kullanım
await _mediator.Send(new DeleteTaskCommand { Id = taskId });
```

## Avantajlar

1. **CQRS Mimarisi**: Komut ve sorgu sorumluluklarının ayrılması, daha temiz ve bakımı kolay bir kod tabanı sağlar.

2. **Doğrulama**: FluentValidation kullanarak komut doğrulama işlemleri merkezi bir şekilde yönetilir.

3. **Hata Yönetimi**: Özel istisna sınıfları ve davranışlar sayesinde hatalar tutarlı bir şekilde ele alınır.

4. **Loglama**: LoggingBehavior sayesinde tüm komut işlemleri otomatik olarak loglanır.

5. **Genişletilebilirlik**: Yeni komutlar ve işleyiciler kolayca eklenebilir.

6. **Test Edilebilirlik**: Komut ve işleyiciler ayrı ayrı test edilebilir.

Bu yapı, görev takibi modülünün temel işlevlerini sağlar ve gelecekteki genişletmeler için sağlam bir temel oluşturur.
