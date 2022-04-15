using Microsoft.EntityFrameworkCore;
using TodoManager.NotificationChannel;
using TodoManager.NotificationChannel.Entities;

namespace TodoManager.BackgroundWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private DbContextOptions<NotificationContext> contextOptions = null;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");

            _logger = logger;
            contextOptions = new DbContextOptionsBuilder<NotificationContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var context = new NotificationContext(contextOptions))
            {
                
                while (!stoppingToken.IsCancellationRequested)
                {
                    context?.AddAsync(new Notification()
                    {
                        Message = "haha",
                        Date = DateTime.Now,
                        UserId = 1
                    });
                    await context?.SaveChangesAsync();

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
}