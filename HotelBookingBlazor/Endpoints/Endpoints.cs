using HotelBookingBlazor.Constants;
using HotelBookingBlazor.Services;

namespace HotelBookingBlazor.Endpoints;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapCustomEndpoints(this IEndpointRouteBuilder builder)
    {
        var staffAdminGroup = builder.MapGroup("/staff-admin")
                                    .RequireAuthorization(authPolicyBuilder => authPolicyBuilder.RequireRole(RoleType.Admin.ToString(), RoleType.Staff.ToString()));

        staffAdminGroup.MapPost("/manage-amenities/delete/{amenityId:int}",
            async (int amenityId, IAmenitiesService amenitiesService) =>
            {
                await amenitiesService.DeleteAmenityAsync(amenityId);
                return TypedResults.LocalRedirect("~/staff-admin/manage-amenities");
            });

        return builder;
    }
}
