using HotelBookingBlazor.Data;
using HotelBookingBlazor.Data.Entites;
using HotelBookingBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingBlazor.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public RoomTypeService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<MethodResult<short>> CreateRoomTypeAsync(RoomTypeCreateModel model, string userId)
    {
        using var context = _contextFactory.CreateDbContext();
        if (await context.RoomTypes.AnyAsync(rt => rt.Name == model.Name))
        {
            return $"Room type with the same name {model.Name} already exists";
        }

        var roomType = new RoomType
        {
            Name = model.Name,
            AddedBy = userId,
            AddedOn = DateTime.Now,
            Description = model.Description,
            Image = model.Image,
            IsActive = model.IsActive,
            MaxAdults = model.MaxAdults,
            MaxChildren = model.MaxChildren,
            Price = model.Price
        };

        await context.RoomTypes.AddAsync(roomType);
        await context.SaveChangesAsync();

        if (model.Amenities.Length > 0)
        {
            var roomTypeAmenities = model.Amenities.Select(a => new RoomTypeAmenity
            {
                AmenityId = a.Id,
                RoomTypeId = roomType.Id,
                Unit = a.Unit
            });
            await context.RoomTypeAmenities.AddRangeAsync(roomTypeAmenities);
            await context.SaveChangesAsync();
        }

        return roomType.Id;
    }
}