using HotelBookingBlazor.Data.Entites;

namespace HotelBookingBlazor.Services
{
    public interface IAmenitiesService
    {
        Task<Amenity[]> GetAmenitiesAsync();
        Task<Amenity> SaveAmenityAsync(Amenity amenity);
    }
}