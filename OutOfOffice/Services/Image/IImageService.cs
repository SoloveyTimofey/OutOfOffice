namespace OutOfOffice.Services.Image
{
    public interface IImageService
    {
        Task<byte[]> ToByteArrayAsync(IFormFile formFile);
        Task<string> BytesToString64StringAsync(byte[] bytes);
    }
}
