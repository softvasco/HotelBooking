using HotelBookingBlazor.Models;

namespace HotelBookingBlazor.Services
{
    public interface IRoomTypeService
    {
        Task<MethodResult<short>> CreateRoomTypeAsync(RoomTypeCreateModel model, string userId);
    }
}