using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Client.Windows
{
    public abstract class CustomWindow : Window
    {
        private bool ResizeInProcess = false;

        public CustomWindow()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            AllowsTransparency = true;

            Loaded += WindowLoaded;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var grid = new Grid();

            var content = Content;
            Content = grid;
            grid.Children.Add((UIElement)content);

            var style = CreateResizeStyle();

            grid.Children.Add(CreateResizeBorder("leftSizeGrip", HorizontalAlignment.Left, VerticalAlignment.Stretch, Cursors.SizeWE, style));
            grid.Children.Add(CreateResizeBorder("rightSizeGrip", HorizontalAlignment.Right, VerticalAlignment.Stretch, Cursors.SizeWE, style));
            grid.Children.Add(CreateResizeBorder("topSizeGrip", HorizontalAlignment.Stretch, VerticalAlignment.Top, Cursors.SizeNS, style));
            grid.Children.Add(CreateResizeBorder("bottomSizeGrip", HorizontalAlignment.Stretch, VerticalAlignment.Bottom, Cursors.SizeNS, style));
            grid.Children.Add(CreateResizeBorder("topLeftSizeGrip", HorizontalAlignment.Left, VerticalAlignment.Top, Cursors.SizeNWSE, style));
            grid.Children.Add(CreateResizeBorder("bottomRightSizeGrip", HorizontalAlignment.Right, VerticalAlignment.Bottom, Cursors.SizeNWSE, style));
            grid.Children.Add(CreateResizeBorder("topRightSizeGrip", HorizontalAlignment.Right, VerticalAlignment.Top, Cursors.SizeNESW, style));
            grid.Children.Add(CreateResizeBorder("bottomLeftSizeGrip", HorizontalAlignment.Left, VerticalAlignment.Bottom, Cursors.SizeNESW, style));
        }

        protected void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private static Rectangle CreateResizeBorder(string name, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Cursor cursor, Style style)
        {
            const double gripSize = 7;

            var border = new Rectangle
            {
                Name = name,
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = verticalAlignment,
                Cursor = cursor,
                Style = style
            };

            if (cursor != Cursors.SizeNS)
            {
                border.Width = gripSize;
            }

            if (cursor != Cursors.SizeWE)
            {
                border.Height = gripSize;
            }

            return border;
        }

        private Style CreateResizeStyle()
        {
            var style = new Style(typeof(Rectangle));

            style.Setters.Add(new Setter(FocusableProperty, false));
            style.Setters.Add(new Setter(Shape.FillProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(TagProperty, this));

            style.Setters.Add(new EventSetter(MouseLeftButtonDownEvent, new MouseButtonEventHandler(ResizeStart)));
            style.Setters.Add(new EventSetter(MouseLeftButtonUpEvent, new MouseButtonEventHandler(ResizeEnd)));
            style.Setters.Add(new EventSetter(MouseMoveEvent, new MouseEventHandler(Resizing)));

            return style;
        }

        protected void Drag(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        protected void Maximize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        protected void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ResizeEnd(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle senderRect)
            {
                ResizeInProcess = false;
                senderRect.ReleaseMouseCapture();
            }
        }

        private void Resizing(object sender, MouseEventArgs e)
        {
            if (ResizeInProcess)
            {
                if (sender is Rectangle senderRect &&
                    senderRect.Tag is Window window) // IST 'window' nicht einfach 'this'? -> Dann auch tag style entfernen
                {
                    double width = e.GetPosition(window).X;
                    double height = e.GetPosition(window).Y;
                    senderRect.CaptureMouse();
                    if (senderRect.Name.ToLower().Contains("right"))
                    {
                        width += 5;
                        if (width > 0)
                        {
                            window.Width = width;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("left"))
                    {
                        width -= 5;
                        window.Left += width;
                        width = window.Width - width;
                        if (width > 0)
                        {
                            window.Width = width;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("bottom"))
                    {
                        height += 5;
                        if (height > 0)
                        {
                            window.Height = height;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("top"))
                    {
                        height -= 5;
                        window.Top += height;
                        height = window.Height - height;
                        if (height > 0)
                        {
                            window.Height = height;
                        }
                    }
                }
            }
        }

        private void ResizeStart(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle senderRect)
            {
                ResizeInProcess = true;
                senderRect.CaptureMouse();
            }
        }
    }
}
