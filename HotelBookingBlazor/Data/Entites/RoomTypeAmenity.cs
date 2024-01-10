namespace HotelBookingBlazor.Data.Entites;

public class RoomTypeAmenity
{
    public short RoomTypeId { get; set; }
    public int AmenityId { get; set; }
    public int? Unit { get; set; }

    public virtual RoomType RoomType { get; set; }
    public virtual Amenity Amenity { get; set; }
}

