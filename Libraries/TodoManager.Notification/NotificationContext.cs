using Microsoft.EntityFrameworkCore;

namespace TodoManager.Notification.Entities;

public class NotificationContext : DbContext
{
    public NotificationContext(DbContextOptions<NotificationContext> options) 
        : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
}
