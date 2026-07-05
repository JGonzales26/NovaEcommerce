namespace EcommerceMVC.DTOs;

public sealed record ProductQueryDto(string? Search, int? CategoryId, int Page = 1, int PageSize = 8);
