using Microsoft.EntityFrameworkCore;
using TodoManager.NotificationChannel.Entities;

namespace TodoManager.NotificationChannel;

public class NotificationContext : DbContext
{
    public NotificationContext(DbContextOptions<NotificationContext> options) 
        : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
}
