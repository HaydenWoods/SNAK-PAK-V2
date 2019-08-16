using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;

namespace SNAKPAK {
    public partial class MainWindow : Window {
        void createComputers(int count) {
            Nullable<Point> dragStart = null;

            MouseButtonEventHandler mouseDown = (mouseSender, args) => {
                var element = (UIElement)mouseSender;
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };
            MouseButtonEventHandler mouseUp = (mouseSender, args) => {
                var element = (UIElement)mouseSender;
                dragStart = null;
                element.ReleaseMouseCapture();
            };
            MouseEventHandler mouseMove = (mouseSender, args) => {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                {
                    var element = (UIElement)mouseSender;
                    var p2 = args.GetPosition(computerCanvas);
                    
                    if ((p2.X - dragStart.Value.X) > 0 && (p2.X + dragStart.Value.X) < computerCanvas.ActualWidth)
                    {
                        Canvas.SetLeft(element, p2.X - dragStart.Value.X);
                    }
                    if ((p2.Y - dragStart.Value.Y) > 0 && (p2.Y + dragStart.Value.Y) < computerCanvas.ActualHeight)
                    {
                        Canvas.SetTop(element, p2.Y - dragStart.Value.Y);
                    }
                }
            };
            Action<UIElement> enableDrag = (element) => {
                element.PreviewMouseDown += mouseDown;
                element.PreviewMouseMove += mouseMove;
                element.PreviewMouseUp += mouseUp;
            };

            //Creates the computer boxes on the screen
            /*
            <Buttons>
                <Rectangle>

                </Rectangle>
            </Button>
            */
            for (int i = 0; i < count; i++) {
                Button newComputer = new Button();
                newComputer.Name = "Computer" + i.ToString();
                newComputer.BorderThickness = new Thickness(0);
                Canvas.SetTop(newComputer, 0);
                Canvas.SetLeft(newComputer, (140 * i));

                Rectangle display = new Rectangle();
                display.Width = 120;
                display.Height = 50;
                display.Fill = Brushes.Black;

                newComputer.Content = display;

                enableDrag(newComputer);
                computerCanvas.Children.Add(newComputer);
            }
        }

        void OnPageLoad(object sender, RoutedEventArgs e) {
            createComputers(20);
        }
    }
}
