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
using System.DirectoryServices;
using System.Net;

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
        readonly RoutedPropertyChangedEventHandler<double> ChangeSnapAmount = (sender, args) => {
            var element = (Slider)sender;
            Properties.Settings.Default.SnapAmount = (int)element.Value;
        };

        //Settings Setup
        void SettingsBindings() {
            SnapToGrid.Checked += ToggleSnapToGrid;
            SnapToGrid.Unchecked += ToggleSnapToGrid;
            SnapAmount.ValueChanged += ChangeSnapAmount;
        }
        void SettingsDefaults() {
            SnapToGrid.IsChecked = Properties.Settings.Default.SnapToGrid;
            SnapAmount.Value = Properties.Settings.Default.SnapAmount;

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
                element.PreviewMouseLeftButtonDown += mouseDown;
                element.PreviewMouseMove += mouseMove;
                element.PreviewMouseLeftButtonUp += mouseUp;
            };

            //Creates the computer boxes on the screen
            for (int i = 1; i < count + 1; i++) {
                Button newComputer = new Button();
                newComputer.Name = "Computer" + i.ToString();
                //Apply style to remove button outline
                newComputer.Style = Resources["TransparentStyle"] as Style;

                Grid grid = new Grid();

                ContextMenu contextMenu = new ContextMenu();
                
                for (int j = 0; j < 5; j++) {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = "Test";
                    contextMenu.Items.Add(menuItem);
                }

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
                newComputer.ContextMenu = contextMenu;

                enableDrag(newComputer);
                computerCanvas.Children.Add(newComputer);
            }
        }

        void OnPageLoad(object sender, RoutedEventArgs e) {
            SettingsDefaults();
            DirectoryEntry root = new DirectoryEntry("WinNT:");
            foreach (DirectoryEntry computers in root.Children) {
                foreach (DirectoryEntry computer in computers.Children) {
                    if (computer.SchemaClassName == "Computer") {
                        var ipadd = Dns.GetHostAddresses(computer.Name);
                        try {
                            ComputerLog.Text += "\n" + computer.Name + ": " + ipadd[1];
                        } catch {
                            ComputerLog.Text += "\n" + computer.Name + ": " + ipadd[0];
                        }
                        
                    }  
                }
            }
            CreateComputers(20);
        }
    }
}
