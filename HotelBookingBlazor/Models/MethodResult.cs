namespace HotelBookingBlazor.Models;

public record MethodResult<TData>(bool IsSuccess, string? ErrorMessage, TData Data)
{
    public static MethodResult<TData> Success(TData data) => new(true, null, data);
    public static MethodResult<TData> Failure(string errorMessage) => new(false, errorMessage, default);

    public static implicit operator MethodResult<TData>(TData data) => Success(data);
    public static implicit operator MethodResult<TData>(string errorMessage) => Failure(errorMessage);
}
