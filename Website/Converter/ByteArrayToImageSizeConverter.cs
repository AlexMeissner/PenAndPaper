namespace Website.Converter;

public static class ByteArrayToImageSizeConverter
{
    public static (int Width, int Height) Convert(byte[] imageData)
    {
        // PNG: (89 50 4E 47)
        if (imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47)
        {
            var widthBytes = new[] { imageData[19], imageData[18], imageData[17], imageData[16] };
            var heightBytes = new[] { imageData[23], imageData[22], imageData[21], imageData[20] };
            var width = BitConverter.ToInt32(widthBytes, 0);
            var height = BitConverter.ToInt32(heightBytes, 0);
            return (width, height);
        }

        // JPEG: (FF D8)
        if (imageData[0] == 0xFF && imageData[1] == 0xD8)
        {
            var i = 2;
            while (i < imageData.Length)
            {
                if (imageData[i] == 0xFF && imageData[i + 1] >= 0xC0 && imageData[i + 1] <= 0xC3)
                {
                    var height = (imageData[i + 5] << 8) + imageData[i + 6];
                    var width = (imageData[i + 7] << 8) + imageData[i + 8];
                    return (width, height);
                }

                i += 2 + (imageData[i + 2] << 8) + imageData[i + 3];
            }

            throw new Exception("Failed to get width and height from jpeg");
        }

        // BMP: (42 4D)
        if (imageData[0] == 0x42 && imageData[1] == 0x4D)
        {
            var width = BitConverter.ToInt32(imageData, 18);
            var height = BitConverter.ToInt32(imageData, 22);
            return (width, height);
        }

        throw new Exception("Unsupported image format");
    }
}