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
    /*
      __  __          _____ _   _  __          _______ _   _ _____   ______          __
     |  \/  |   /\   |_   _| \ | | \ \        / |_   _| \ | |  __ \ / __ \ \        / /
     | \  / |  /  \    | | |  \| |  \ \  /\  / /  | | |  \| | |  | | |  | \ \  /\  / / 
     | |\/| | / /\ \   | | | . ` |   \ \/  \/ /   | | | . ` | |  | | |  | |\ \/  \/ /  
     | |  | |/ ____ \ _| |_| |\  |    \  /\  /   _| |_| |\  | |__| | |__| | \  /\  /   
     |_|  |_/_/    \_|_____|_| \_|     \/  \/   |_____|_| \_|_____/ \____/   \/  \/                                                                             
    */
    public partial class MainWindow : Window {
        public ViewUIElement masterView;
        
        public static class ExtensionMethods {
            public static int RoundOff(int num, int snap) {
                return (int)((int)Math.Round(num / (double)snap) * snap);
            }
            public static void GenerateRandomViews(View parentView, int recursions, int maxRecursions) {
                Random r = new Random();

                for (int j = 0; j < r.Next(1, 10); j++) {
                    Computer computer = new Computer();
                    computer.ComputerName = parentView.ViewName + " - Computer (" + j + ")";
                    parentView.Computers.Add(computer);
                }

                for (int i = 0; i < r.Next(1, 4); i++) {
                    View subView = new View();
                    subView.ViewName = parentView.ViewName + " - SubView(" + i + ")";

                    parentView.Subviews.Add(subView);

                    recursions = recursions + 1;
                    if ( recursions < maxRecursions) {
                        GenerateRandomViews(subView, recursions, maxRecursions);
                    }
                }
            }
            public static void GenerateRandomFile(string fileName) {
                Random r = new Random();
                View masterView = new View();
                masterView.ViewName = "Master View";

                GenerateRandomViews(masterView, 0, r.Next(3,10));

                using (var output = File.Create("test.snak")) {
                    masterView.WriteTo(output);
                }
            }
        }

        public class Item {
            public Button Display;

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
            public static Action<UIElement> enableDrag = (element) => {
                element.PreviewMouseLeftButtonDown += mouseDown;
                element.PreviewMouseMove += mouseMove;
                element.PreviewMouseLeftButtonUp += mouseUp;
            };

            public void CreateDisplay(string name) {
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
                text.Text = name;
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

            public Item(string name) {
                CreateDisplay(name);
            }
        }

        public class ComputerUIElement {
            public String Name;
            public Item Display;

            static readonly MainWindow mw = (MainWindow)Application.Current.MainWindow;

            public ComputerUIElement(Computer computer) {
                Name = computer.ComputerName;
            }
        }

        public class ViewUIElement {
            public string Name;
            public Item Display;

            public List<ViewUIElement> SubViews = new List<ViewUIElement>();
            public List<ComputerUIElement> Computers = new List<ComputerUIElement>();

            static readonly MainWindow mw = (MainWindow)Application.Current.MainWindow;

            public void LoadSubViews(View parentView) {
                Debug.WriteLine("\nStarting Loading Views for " + parentView.ViewName);
                if (parentView.Subviews.Count > 0) {
                    for (int i = 0; i < parentView.Subviews.Count; i++) {
                        ViewUIElement subView = new ViewUIElement(parentView.Subviews[i]);
                        Debug.WriteLine("Starting Loading Computers for " + parentView.Subviews[i].ViewName);
                        for (int j = 0; j < parentView.Subviews[i].Computers.Count; j++) {
                            ComputerUIElement computer = new ComputerUIElement(parentView.Subviews[i].Computers[j]);
                            subView.Computers.Add(computer);
                            Debug.WriteLine("Loaded Computer " + parentView.Subviews[i].Computers[j].ComputerName);
                        }
                        Debug.WriteLine("Finished Loading Computers for " + parentView.Subviews[i].ViewName);
                        SubViews.Add(subView);
                    }
                } else {
                    Debug.WriteLine("Finished Loading Views for " + parentView.ViewName + "\n");
                }
            }

            public void DisplayView() {
                mw.currentViewName.Text = Name;
                for (int i = 0; i < SubViews.Count; i++) {
                    SubViews[i].Display = new Item(SubViews[i].Name);
                }
                for (int i = 0; i < Computers.Count; i++) {
                    Computers[i].Display = new Item(Computers[i].Name);
                }
            }

            public ViewUIElement(View parentView) {
                Name = parentView.ViewName;
                LoadSubViews(parentView);
            }
        }

        void LoadFile(string fileName) {
            View view;
            using (var input = File.OpenRead(fileName)) {
                view = View.Parser.ParseFrom(input);
            }
            masterView = new ViewUIElement(view);
            masterView.SubViews[0].DisplayView();
        }
        void SaveFile(string fileName) {
            /*
            using (var output = File.Create("test.snak")) {
                finalView.WriteTo(output);
            }
            */
        }

        void OnPageLoad(object sender, RoutedEventArgs e) {
            ExtensionMethods.GenerateRandomFile("test.snak");
            LoadFile("test.snak");       
        }
    }
}

