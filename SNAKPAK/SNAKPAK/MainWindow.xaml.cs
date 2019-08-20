using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections;
using System.IO;
using Google.Protobuf;
using static View.Types;

namespace SNAKPAK {
    public static class ExtensionMethods {
        public static int RoundOff(int num, int snap) {
            return (int)((int)Math.Round(num / (double)snap) * snap);
        }
    }


    /*
      _____ _______ ______ __  __ 
     |_   _|__   __|  ____|  \/  |
       | |    | |  | |__  | \  / |
       | |    | |  |  __| | |\/| |
      _| |_   | |  | |____| |  | |
     |_____|  |_|  |______|_|  |_|                        
    */
    public partial class Item {
        public string Name;
        public Button Display;

        public View View;
        public Computer Computer;

        static Nullable<Point> dragStart = null;
        static readonly MainWindow mw = (MainWindow)Application.Current.MainWindow;

        //Left mouse down
        static MouseButtonEventHandler mouseDown = (mouseSender, args) => {
            var element = (FrameworkElement)mouseSender;
            dragStart = args.GetPosition(element);
            element.CaptureMouse();
        };
        //Left mouse up
        static MouseButtonEventHandler mouseUp = (mouseSender, args) => {
            var element = (FrameworkElement)mouseSender;
            dragStart = null;
            element.ReleaseMouseCapture();
        };
        //On mouse move
        static MouseEventHandler mouseMove = (mouseSender, args) => {
            if (dragStart != null && args.LeftButton == MouseButtonState.Pressed) {
                var element = (FrameworkElement)mouseSender;
                var p2 = args.GetPosition(mw.computerCanvas);

                if ((p2.X - dragStart.Value.X) > 0 && (p2.X - dragStart.Value.X + element.ActualWidth) < (mw.computerCanvas.ActualWidth)) {
                    double newX = p2.X - dragStart.Value.X;
                    if (Properties.Settings.Default.SnapToGrid == true) {
                        newX = ExtensionMethods.RoundOff((int)newX, Properties.Settings.Default.SnapAmount);

                        if ((newX + element.ActualWidth) > (mw.computerCanvas.ActualWidth)) {
                            newX = newX - Properties.Settings.Default.SnapAmount;
                        }
                    }
                    Canvas.SetLeft(element, newX);
                }
                if ((p2.Y - dragStart.Value.Y) > 0 && (p2.Y - dragStart.Value.Y + element.ActualHeight) < (mw.computerCanvas.ActualHeight)) {
                    double newY = p2.Y - dragStart.Value.Y;
                    if (Properties.Settings.Default.SnapToGrid == true) {
                        newY = ExtensionMethods.RoundOff((int)newY, Properties.Settings.Default.SnapAmount);

                        if ((newY + element.ActualHeight) > (mw.computerCanvas.ActualHeight)) {
                            newY = newY - Properties.Settings.Default.SnapAmount;
                        }
                    }

                    Canvas.SetTop(element, newY);
                }
            }
        };

        //Sets the bindings for the mouse movements
        static Action<UIElement> enableDrag = (element) => {
            element.PreviewMouseLeftButtonDown += mouseDown;
            element.PreviewMouseMove += mouseMove;
            element.PreviewMouseLeftButtonUp += mouseUp;
        };

        void CreateDisplay() {
            this.Display = new Button();
            this.Display.Style = mw.Resources["TransparentStyle"] as Style;

            //Containg grid inside of the button
            Grid grid = new Grid();

            //Right click menu
            ContextMenu contextMenu = new ContextMenu();
            for (int j = 0; j < 5; j++) {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = "Test";
                contextMenu.Items.Add(menuItem);
            }

            //Drawn representation of the item
            Rectangle display = new Rectangle();
            display.Width = 120;
            display.Height = 50;
            display.Fill = Brushes.Beige;
            display.Stroke = Brushes.Black;
            display.StrokeThickness = 1;

            //Text
            Border border = new Border();
            TextBlock text = new TextBlock();
            text.Text = this.Name;
            text.Width = 120;
            text.FontSize = 14;
            text.TextAlignment = TextAlignment.Center;
            text.Foreground = Brushes.Black;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.TextWrapping = TextWrapping.Wrap;
            border.Child = text;

            grid.Children.Add(display);
            grid.Children.Add(border);

            this.Display.Content = grid;
            this.Display.ContextMenu = contextMenu;

            enableDrag(this.Display);
            mw.computerCanvas.Children.Add(this.Display);
        }

        public Item(string name, View view) {
            Name = name;
            View = view;
            CreateDisplay();
        }
        public Item(string name, Computer computer) {
            Name = name;
            Computer = computer;
            CreateDisplay();
        }
    }


    /*
      __  __          _____ _   _  __          _______ _   _ _____   ______          __
     |  \/  |   /\   |_   _| \ | | \ \        / |_   _| \ | |  __ \ / __ \ \        / /
     | \  / |  /  \    | | |  \| |  \ \  /\  / /  | | |  \| | |  | | |  | \ \  /\  / / 
     | |\/| | / /\ \   | | | . ` |   \ \/  \/ /   | | | . ` | |  | | |  | |\ \/  \/ /  
     | |  | |/ ____ \ _| |_| |\  |    \  /\  /   _| |_| |\  | |__| | |__| | \  /\  /   
     |_|  |_/_/    \_|_____|_| \_|     \/  \/   |_____|_| \_|_____/ \____/   \/  \/                                                                             
    */
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

        void OnPageLoad(object sender, RoutedEventArgs e) {
            SettingsDefaults();
            View v;
            using (var input = File.OpenRead("test.snak")) {
                v = View.Parser.ParseFrom(input);
            }
            View sub = new View {
                ViewName = "JSRACS",
            };
            sub.Computers.Add(new Computer {
                ComputerName = "test",
            });
            v.Subviews.Add(sub);
            Debug.WriteLine(v.ViewName);

            using (var output = File.Create("test.snak")) {
                v.WriteTo(output);
            }
        }
    }
}