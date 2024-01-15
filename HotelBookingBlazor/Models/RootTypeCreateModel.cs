using System.ComponentModel.DataAnnotations;

namespace HotelBookingBlazor.Models;

public class RootTypeCreateModel
{
    public short Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Required, MaxLength(100)]
    public string Image { get; set; }

    [Required, Range(1, double.MaxValue)]
    public decimal Price { get; set; }

    [Required, MaxLength(200)]
    public string Description { get; set; }

    [Range(1, 10)]
    public int MaxAdults { get; set; }

    [Range(0, 10)]
    public int MaxChildren { get; set; }

    public bool IsActive { get; set; }

    public RoomTypeAmenityCreateModel[] Amenities { get; set; } = [];

    public class RoomTypeAmenityCreateModel
    {
        public int Id { get; set; }
        public int? Unit { get; set; }
    }
}

