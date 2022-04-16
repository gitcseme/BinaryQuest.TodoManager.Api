using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoManager.NotificationChannel.Services;

namespace TodoManager.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILoggerManager _logger;

        public NotificationsController(INotificationService notificationService, ILoggerManager logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var notificationResponses = await _notificationService.GetAllAsync(trackChanges: false);
                return Ok(notificationResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error getting all notification");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:long}/read")]
        public async Task<IActionResult> MarkSeen(long id)
        {
            try
            {
                await _notificationService.MarkSeenAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error marking notification seen");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
