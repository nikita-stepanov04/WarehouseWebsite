using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WarehouseWebsite.Application.Helpers
{
    public class ImageCompressor
    {
        private readonly int _maxCompressWidth = 800;
        private readonly int _maxCompressHeight = 800;

        public async Task CompressImageInStreamAsync(Stream stream)
        {
            using (var image = await Image.LoadAsync<Rgba32>(stream))
            {
                if (image.Width > _maxCompressWidth || image.Height > _maxCompressHeight)
                {
                    var resizeOptions = new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(_maxCompressWidth, _maxCompressHeight)
                    };
                    image.Mutate(x => x.Resize(resizeOptions));
                }

                stream.SetLength(0);
                stream.Seek(0, SeekOrigin.Begin);

                await image.SaveAsJpegAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}
