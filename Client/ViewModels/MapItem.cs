using Client.Converter;
using System.Printing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Client.ViewModels
{
    public class MapTransformation : BaseViewModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public MatrixTransform Scaling { get; set; } = new();

        public void Move(Vector offset)
        {
            X += (int)offset.X;
            Y += (int)offset.Y;
        }

        public void Reset()
        {
            X = 0;
            Y = 0;
            Application.Current.Dispatcher.Invoke(() => Scaling.Matrix = Matrix.Identity);
        }

        public void Zoom(Point position, int delta)
        {
            const float _zoomFactor = 1.1f;
            var factor = (delta < 0) ? 1.0f / _zoomFactor : _zoomFactor;

            var matrix = Scaling.Matrix;
            matrix.ScaleAt(factor, factor, position.X, position.Y);
            Scaling.Matrix = matrix;
        }
    }

    public interface IMapItem
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex { get; }
    }

    public class BackgroundMapItem : BaseViewModel, IMapItem
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex => 0;
        public int Width { get; set; }
        public int Height { get; set; }
        public BitmapImage Image { get; set; }

        public BackgroundMapItem(byte[] imageData)
        {
            Image = Converter.Convert(imageData);
            Width = (int)Image.Width;
            Height = (int)Image.Height;
        }

        private static readonly ByteArrayToBitmapImageConverter Converter = new();
    }

    public class GridMapItem : BaseViewModel, IMapItem
    {
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
            Size = size;
            Width = width;
            Height = height;
            LineThickness = lineThickness;
            Color = color;
        }
    }

    public class LandmarkMapItem : BaseViewModel, IMapItem
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex => 2;
    }

    public class TokenMapItem : BaseViewModel, IMapItem
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ZIndex => 3;
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public BitmapImage Image { get; set; }

        public TokenMapItem(int x, int y, int id, int userId, string name, byte[] imageData)
        {
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
