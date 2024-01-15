using HotelBookingBlazor.Data.Entites;
using HotelBookingBlazor.Models;

namespace HotelBookingBlazor.Services
{
    public interface IAmenitiesService
    {
        Task<Amenity[]> GetAmenitiesAsync();
        Task<MethodResult<Amenity>> SaveAmenityAsync(Amenity amenity);
        Task<MethodResult<bool>> DeleteAmenityAsync(int id);
    }
}