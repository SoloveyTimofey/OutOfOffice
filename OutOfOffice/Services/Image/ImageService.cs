
namespace OutOfOffice.Services.Image
{
    public class ImageService : IImageService
    {
        public async Task<string> BytesToString64StringAsync(byte[] bytes)
        {
            return await Task.FromResult(Convert.ToBase64String(bytes));
        }

        public async Task<byte[]> ToByteArrayAsync(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}
