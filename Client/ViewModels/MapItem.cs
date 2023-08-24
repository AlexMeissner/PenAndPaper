using Client.Converter;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.ViewModels
{
    public interface IMapItem
    {
        public MatrixTransform Transformation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex { get; }
    }

    public class BackgroundMapItem : BaseViewModel, IMapItem
    {
        public MatrixTransform Transformation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex => 0;
        public int Width { get; set; }
        public int Height { get; set; }
        public BitmapImage Image { get; set; }

        public BackgroundMapItem(byte[] imageData)
        {
            Transformation = new();
            Image = Converter.Convert(imageData);
            Width = (int)Image.Width;
            Height = (int)Image.Height;
        }

        private static readonly ByteArrayToBitmapImageConverter Converter = new();
    }

    public class GridMapItem : BaseViewModel, IMapItem
    {
        public MatrixTransform Transformation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex => 1;
        public int Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int LineThickness { get; set; }
        public SolidColorBrush Color { get; set; }

        public GridMapItem(int size, int width, int height, int lineThickness, SolidColorBrush color)
        {
            Transformation = new();
            Size = size;
            Width = width;
            Height = height;
            LineThickness = lineThickness;
            Color = color;
        }
    }

    public class LandmarkMapItem : BaseViewModel, IMapItem
    {
        public MatrixTransform Transformation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex => 2;

        public LandmarkMapItem()
        {
            Transformation = new();
        }
    }

    public class TokenMapItem : BaseViewModel, IMapItem
    {
        public MatrixTransform Transformation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex => 3;
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public BitmapImage Image { get; set; }

        public TokenMapItem(int x, int y, int id, int userId, string name, byte[] imageData)
        {
            Transformation = new();
            X = x;
            Y = y;
            Id = id;
            UserId = userId;
            Name = name;
            Image = Converter.Convert(imageData);
        }

        private static readonly ByteArrayToBitmapImageConverter Converter = new();
    }
}
