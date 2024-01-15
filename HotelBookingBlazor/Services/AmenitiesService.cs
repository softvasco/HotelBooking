using HotelBookingBlazor.Data;
using HotelBookingBlazor.Data.Entites;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingBlazor.Services;

public class AmenitiesService : IAmenitiesService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public AmenitiesService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Amenity[]> GetAmenitiesAsync()
    {
        using var context = _contextFactory.CreateDbContext();

        return await context.Amenities.ToArrayAsync();
    }

    public async Task<Amenity> SaveAmenityAsync(Amenity amenity)
    {
        using var context = _contextFactory.CreateDbContext();
        if (amenity.Id == 0)
        {
            //Create a new Amenity
            await context.Amenities.AddAsync(amenity);
        }
        else
        {
            //Update existing Amenity
            var dbAmenity = await context.Amenities
                                    .AsTracking()
                                    .FirstOrDefaultAsync(a => a.Id == amenity.Id)
                                    ?? throw new InvalidOperationException("Amenity does not exist");

            dbAmenity.Name = amenity.Name;
            dbAmenity.Icon = amenity.Icon;
        }

        await context.SaveChangesAsync();
        return amenity;
    }
}

public class RoomService
{

}