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
    public static class ExtensionMethods {
        public static int RoundOff(int num, int snap) {
            return (int)((int)Math.Round(num / (double)snap) * snap);
        }
    }

    public partial class MainWindow : Window {
        //Settings Controls
        readonly RoutedEventHandler ToggleSnapToGrid = (sender, args) => {
            if (Properties.Settings.Default.SnapToGrid == true) {
                Properties.Settings.Default.SnapToGrid = false;
            } else {
                Properties.Settings.Default.SnapToGrid = true;
            }
        };
        readonly TextChangedEventHandler ChangeSnapAmount = (sender, args) => {
            var element = (TextBox)sender;
            if (int.TryParse(element.Text, out int n)) {
                Properties.Settings.Default.SnapAmount = n;
            }
        };

        //Settings Setup
        void SettingsBindings() {
            SnapToGrid.Checked += ToggleSnapToGrid;
            SnapToGrid.Unchecked += ToggleSnapToGrid;
            SnapAmount.TextChanged += ChangeSnapAmount;
        }
        void SettingsDefaults() {
            SnapToGrid.IsChecked = Properties.Settings.Default.SnapToGrid;
            SnapAmount.Text = Properties.Settings.Default.SnapAmount.ToString();

            SettingsBindings();
        }

        void CreateComputers(int count) {
            Nullable<Point> dragStart = null;

            //Left mouse down
            MouseButtonEventHandler mouseDown = (mouseSender, args) => {
                var element = (FrameworkElement)mouseSender;
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };
            //Left mouse up
            MouseButtonEventHandler mouseUp = (mouseSender, args) => {
                var element = (FrameworkElement)mouseSender;
                dragStart = null;
                element.ReleaseMouseCapture();
            };
            //On mouse move
            MouseEventHandler mouseMove = (mouseSender, args) => {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed) {
                    var element = (FrameworkElement)mouseSender;
                    var p2 = args.GetPosition(computerCanvas);

                    if ((p2.X - dragStart.Value.X) > 0 && (p2.X - dragStart.Value.X + element.ActualWidth) < computerCanvas.ActualWidth) {
                        double newX = p2.X - dragStart.Value.X;
                        if (Properties.Settings.Default.SnapToGrid == true) {
                            newX = ExtensionMethods.RoundOff((int)newX, Properties.Settings.Default.SnapAmount);

                            if ((newX + element.ActualWidth) > computerCanvas.ActualWidth) {
                                newX = newX - Properties.Settings.Default.SnapAmount;
                            }
                        }
                        Canvas.SetLeft(element, newX);
                    }
                    if ((p2.Y - dragStart.Value.Y) > 0 && (p2.Y - dragStart.Value.Y + element.ActualHeight) < computerCanvas.ActualHeight) {
                        double newY = p2.Y - dragStart.Value.Y;
                        if (Properties.Settings.Default.SnapToGrid == true) {
                            newY = ExtensionMethods.RoundOff((int)newY, Properties.Settings.Default.SnapAmount);

                            if ((newY + element.ActualHeight) > computerCanvas.ActualHeight) {
                                newY = newY - Properties.Settings.Default.SnapAmount;
                            }
                        }

                        Canvas.SetTop(element, newY);
                    }
                }
            };
            //Sets the bindings for the mouse movements
            Action<UIElement> enableDrag = (element) => {
                element.PreviewMouseDown += mouseDown;
                element.PreviewMouseMove += mouseMove;
                element.PreviewMouseUp += mouseUp;
            };

            //Creates the computer boxes on the screen
            for (int i = 1; i < count + 1; i++) {
                Button newComputer = new Button();
                newComputer.Name = "Computer" + i.ToString();
                //Apply style to remove button outline
                newComputer.Style = Resources["TransparentStyle"] as Style;

                Grid grid = new Grid();

                Rectangle display = new Rectangle();
                display.Width = 100;
                display.Height = 50;
                display.Fill = Brushes.Beige;
                display.Stroke = Brushes.Black;
                display.StrokeThickness = 1;

                Border border = new Border();
                TextBlock text = new TextBlock();
                text.Text = newComputer.Name;
                text.FontSize = 16;
                text.TextAlignment = TextAlignment.Center;
                text.Foreground = Brushes.Black;
                text.VerticalAlignment = VerticalAlignment.Center;
                border.Child = text;

                grid.Children.Add(display);
                grid.Children.Add(border);
                newComputer.Content = grid;

                enableDrag(newComputer);
                computerCanvas.Children.Add(newComputer);
            }
        }

        void OnPageLoad(object sender, RoutedEventArgs e) {
            SettingsDefaults();
            CreateComputers(20);
        }
    }
}
