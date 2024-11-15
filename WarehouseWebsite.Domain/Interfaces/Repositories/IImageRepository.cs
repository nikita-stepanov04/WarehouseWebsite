namespace WarehouseWebsite.Domain.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task<Guid> UploadAsync(Stream stream, string contentType);
        Task DeleteAsync(Guid imageId);
        string GetImageUri(Guid imageId);
    }
}
