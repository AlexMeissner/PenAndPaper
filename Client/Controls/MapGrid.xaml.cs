using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.Controls
{
    public partial class MapGrid : UserControl
    {
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size), typeof(int), typeof(MapGrid), new PropertyMetadata(10));
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(nameof(LineThickness), typeof(double), typeof(MapGrid), new PropertyMetadata(1.0));
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(SolidColorBrush), typeof(MapGrid), new PropertyMetadata(Brushes.Black));

        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set
            {
                SetValue(SizeProperty, value);
                Redraw();
            }
        }

        public double LineThickness
        {
            get => (double)GetValue(LineThicknessProperty);
            set
            {
                SetValue(LineThicknessProperty, value);
                Redraw();
            }
        }

        public SolidColorBrush Color
        {
            get => (SolidColorBrush)GetValue(ColorProperty);
            set
            {
                SetValue(ColorProperty, value);
                Redraw();
            }
        }

        public MapGrid()
        {
            InitializeComponent();

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Redraw();
        }

        private void Redraw()
        {
            Clear();
            Draw();
        }

        private void Clear()
        {
            foreach (UIElement element in GridCanvas.Children)
            {
                if (element is Image)
                {
                    GridCanvas.Children.Remove(element);
                    break;
                }
            }
        }

        private void Draw()
        {
            if (double.IsNaN(Width) || double.IsNaN(Height) || double.IsNaN(Size))
            {
                return;
            }

            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();

            var pen = new Pen(Color, LineThickness);
            pen.Freeze();

            DrawHorizontalLines(drawingContext, pen);
            DrawVerticalLines(drawingContext, pen);

            drawingContext.Close();

            var gitmap = new RenderTargetBitmap((int)Width, (int)Height, 96, 96, PixelFormats.Pbgra32);
            gitmap.Render(drawingVisual);
            gitmap.Freeze();

            var gridImage = new Image
            {
                Source = gitmap
            };

            GridCanvas.Children.Add(gridImage);
        }

        private void DrawHorizontalLines(DrawingContext drawingContext, Pen pen)
        {
            int rows = (int)Math.Floor(Height / Size) - 1;

            var x = new Point(0, Size);
            var y = new Point(Width, Size);

            for (int i = 0; i <= rows; i++)
            {
                drawingContext.DrawLine(pen, x, y);
                x.Offset(0, Size);
                y.Offset(0, Size);
            }
        }

        private void DrawVerticalLines(DrawingContext drawingContext, Pen pen)
        {
            int columns = (int)Math.Floor(Width / Size) - 1;

            var x = new Point(Size, 0);
            var y = new Point(Size, Height);

            for (int i = 0; i <= columns; i++)
            {
                drawingContext.DrawLine(pen, x, y);
                x.Offset(Size, 0);
                y.Offset(Size, 0);
            }
        }
    }
}
