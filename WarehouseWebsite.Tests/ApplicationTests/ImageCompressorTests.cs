using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using WarehouseWebsite.Application.Helpers;

namespace WarehouseWebsite.Tests.ApplicationTests
{
    [TestFixture]
    public class ImageCompressorTests
    {
        private readonly int _maxCompressWidth = 800;
        private readonly int _maxCompressHeight = 800;

        [TestCase(1000, 1000)]
        [TestCase(700, 1000)]
        [TestCase(1000, 700)]
        [TestCase(500, 500)]
        [TestCase(10000, 20000)]
        public async Task ImageCompressorCompressImageInStreamAsyncCompressesImageProperly(
            int width, int height)
        {
            using var image = new Image<Rgba32>(width, height);
            using var stream = new MemoryStream();
            await image.SaveAsJpegAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            var compressor = new ImageCompressor();
            await compressor.CompressImageInStreamAsync(stream);

            using var compressedImage = await Image.LoadAsync<Rgba32>(stream);
            (int expectedWidth, int expectedHeight) = GetCompressedImageResolution(width, height);

            Assert.That(compressedImage.Width, Is.EqualTo(expectedWidth));
            Assert.That(compressedImage.Height, Is.EqualTo(expectedHeight));
        }

        [TestCase("jpg")]
        [TestCase("png")]
        [TestCase("gif")]
        [TestCase("bmp")]
        [TestCase("tiff")]
        [TestCase("webp")]
        public async Task ImageCompressorCompressImageInStreamAsyncCompressesDifferentImageTypes(string type)
        {
            int width = 1000;
            int height = 1000;
            using var image = new Image<Rgba32>(width, height);
            using var stream = new MemoryStream();

            await (type switch
            {
                "jpg" => image.SaveAsJpegAsync(stream),
                "png" => image.SaveAsPngAsync(stream),
                "gif" => image.SaveAsGifAsync(stream),
                "bmp" => image.SaveAsBmpAsync(stream),
                "tiff" => image.SaveAsTiffAsync(stream),
                "webp" => image.SaveAsWebpAsync(stream)
            });
            stream.Seek(0, SeekOrigin.Begin);

            var compressor = new ImageCompressor();
            await compressor.CompressImageInStreamAsync(stream);

            using var compressedImage = await Image.LoadAsync<Rgba32>(stream);
            (int expectedWidth, int expectedHeight) = GetCompressedImageResolution(width, height);

            Assert.That(compressedImage.Width, Is.EqualTo(expectedWidth));
            Assert.That(compressedImage.Height, Is.EqualTo(expectedHeight)); ;
        }

        private (int width, int height) GetCompressedImageResolution(int width, int height)
        {
            int compressedWidth;
            int compressedHeight;

            if (width > _maxCompressWidth || height > _maxCompressHeight)
            {
                if (width > height)
                {
                    compressedWidth = _maxCompressWidth;
                    compressedHeight = (int)((float)height / width * _maxCompressWidth);
                }
                else
                {
                    compressedHeight = _maxCompressHeight;
                    compressedWidth = (int)((float)width / height * _maxCompressHeight);
                }
            }
            else
            {
                compressedWidth = width;
                compressedHeight = height;
            }
            return (compressedWidth, compressedHeight);
        }
    }
}
