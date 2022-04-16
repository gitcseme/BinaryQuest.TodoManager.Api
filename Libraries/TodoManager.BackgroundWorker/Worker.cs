using Microsoft.EntityFrameworkCore;
using TodoManager.Data;
using TodoManager.NotificationChannel;
using TodoManager.NotificationChannel.Entities;
using TodoManager.Data.Entities;
using TodoManager.Shared.Enums;

namespace TodoManager.BackgroundWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private DbContextOptions<NotificationContext> notificationDbContextOptions;
        private DbContextOptions<TodosDbContext> todoDbContextOptions;
        private string connectionString;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("SqlConnection");
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            notificationDbContextOptions = new DbContextOptionsBuilder<NotificationContext>()
                .UseSqlServer(connectionString)
                .Options;

            todoDbContextOptions = new DbContextOptionsBuilder<TodosDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            //todosDbContext = new TodosDbContext(todoDbContextOptions);
            //notificationContext = new NotificationContext(notificationDbContextOptions);


            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckTodosToNotifyAsync();

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task CheckTodosToNotifyAsync()
        {
            var todos = new List<Todo>();

            using (var todoContext = new TodosDbContext(todoDbContextOptions))
            {
                todos = await todoContext.Todos.ToListAsync();
            }

            using (var notificationContext = new NotificationContext(notificationDbContextOptions))
            {
                var notifications = await notificationContext.Notifications.ToListAsync();

                // Filter todos that are not notified already
                var todoList = (from todo in todos
                                where (notifications.FirstOrDefault(n => n.TodoId == todo.Id)) == null
                                select todo)
                                .ToList();

                // Find deadline crossed todos that are not completed
                var notificationList = todoList
                    .Where(todo => !todo.IsDone
                            && todo.Deadline != null
                            && todo.Deadline < DateTime.Now
                        )
                    .Select(todo => new Notification()
                    {
                        TodoId = todo.Id,
                        TodoCreatorId = todo.CreatorId,
                        Message = "Deadline over",
                        IsSeen = false,
                        Date = DateTime.Now,
                        Type = NotificationType.DeadlineCrossed
                    })
                    .ToList();

                if (notificationList.Any())
                {
                    await notificationContext.Notifications.AddRangeAsync(notificationList);
                    await notificationContext.SaveChangesAsync();
                }
            }
        }
    }
}