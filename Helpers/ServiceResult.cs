namespace EcommerceMVC.Helpers;

public sealed class ServiceResult
{
    public bool Succeeded { get; init; }
    public string? Error { get; init; }

    public static ServiceResult Success() => new() { Succeeded = true };
    public static ServiceResult Failure(string error) => new() { Succeeded = false, Error = error };
}
