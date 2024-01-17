using HotelBookingBlazor.Data;
using HotelBookingBlazor.Data.Entites;
using HotelBookingBlazor.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace HotelBookingBlazor.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public RoomTypeService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<MethodResult<short>> SaveRoomTypeAsync(RoomTypeSaveModel model, string userId)
    {
        using var context = _contextFactory.CreateDbContext();

        RoomType? roomType;

        if (model.Id == 0)
        {
            if (await context.RoomTypes.AnyAsync(rt => rt.Name == model.Name))
            {
                return $"Room type with the same name {model.Name} already exists";
            }

            roomType = new RoomType
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
        }
        else
        {
            if (await context.RoomTypes.AnyAsync(rt => rt.Name == model.Name && rt.Id != model.Id))
            {
                return $"Room type with the same name {model.Name} already exists";
            }

            roomType = await context.RoomTypes
                                    .AsTracking()
                                    .FirstOrDefaultAsync(rt => rt.Id == model.Id);
            if (roomType is null)
            {
                return $"Invalid request";
            }

            roomType.Name = model.Name;
            roomType.Description = model.Description;
            roomType.Image = model.Image;
            roomType.IsActive = model.IsActive;
            roomType.MaxAdults = model.MaxAdults;
            roomType.MaxChildren = model.MaxChildren;
            roomType.Price = model.Price;

            roomType.LastUpdatedBy = userId;
            roomType.LastUpdatedOn = DateTime.Now;

            var existingRoomTypeAmenities = await context.RoomTypeAmenities
                            .Where(rta => rta.RoomTypeId == model.Id)
                            .ToListAsync();
            context.RoomTypeAmenities.RemoveRange(existingRoomTypeAmenities);
        }
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

    public async Task<RoomTypeListModel[]> GetRoomTypesForManagePageAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.RoomTypes
            .Select(r => new RoomTypeListModel(r.Id, r.Name, r.Image, r.Price))
            .ToArrayAsync();
    }

    public async Task<RoomTypeSaveModel?> GetRoomTypeEditAsync(short id)
    {
        using var context = _contextFactory.CreateDbContext();
        var roomType = await context.RoomTypes
                .Include(rt => rt.Amenities)
                .Where(rt => rt.Id == id)
                .Select(rt => new RoomTypeSaveModel
                {
                    Name = rt.Name,
                    Image = rt.Image,
                    Price = rt.Price,
                    Description = rt.Description,
                    IsActive = rt.IsActive,
                    Id = id,
                    MaxAdults = rt.MaxAdults,
                    MaxChildren = rt.MaxChildren,
                    Amenities = rt.Amenities.Select(a => new RoomTypeSaveModel.RoomTypeAmenitySaveModel(a.AmenityId, a.Unit)).ToArray(),
                })
                .FirstOrDefaultAsync();

        return roomType;
    }

}