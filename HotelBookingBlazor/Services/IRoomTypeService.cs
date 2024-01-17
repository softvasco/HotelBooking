using HotelBookingBlazor.Models;

namespace HotelBookingBlazor.Services
{
    public interface IRoomTypeService
    {
        Task<MethodResult<short>> SaveRoomTypeAsync(RoomTypeSaveModel model, string userId);
        Task<RoomTypeListModel[]> GetRoomTypesForManagePageAsync();
        Task<RoomTypeSaveModel?> GetRoomTypeEditAsync(short id);
    }
}