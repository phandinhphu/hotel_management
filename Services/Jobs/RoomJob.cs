using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services.Jobs
{
    public enum RoomStatus
    {
        Available,
        Occupied,
        Maintenance,
    }

    public class RoomJob
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RoomJob> _logger;

        public RoomJob(IServiceScopeFactory scopeFactory, ILogger<RoomJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task UpdateRoomStatusAsync(CancellationToken stoppingToken)
        {
            var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<HotelManagementContext>();

                var today = DateOnly.FromDateTime(now);
                var yesterday = today.AddDays(-1);

                var checkOutRoomIds = await dbContext.BookingsRoomDetails
                    .Where(b => b.CheckOut == yesterday)
                    .Select(b => b.RoomId)
                    .Distinct()
                    .ToListAsync(stoppingToken);

                var roomsToUpdate = await dbContext.Rooms
                    .Where(r => checkOutRoomIds.Contains(r.Id) && r.Status == RoomStatus.Occupied.ToString())
                    .ToListAsync(stoppingToken);

                foreach (var room in roomsToUpdate)
                {
                    room.Status = RoomStatus.Available.ToString();
                    _logger.LogInformation($"Updating room {room.Id} status to Available.");
                }

                if (roomsToUpdate.Any())
                {
                    await dbContext.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation($"Updated {roomsToUpdate.Count} rooms to Available status.");
                }
                else
                {
                    _logger.LogInformation("No rooms to update.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating room statuses.");
            }
        }
    }
}
