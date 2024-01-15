using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingBlazor.Data.Entites;

public class Amenity
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(25), Unicode(false)]
    public string Name { get; set; }

    [Required, MaxLength(25), Unicode(false)]
    public string Icon { get; set; }

    public bool IsDeleted { get; set; }

    public Amenity Clone() => (MemberwiseClone() as Amenity)!;
}

