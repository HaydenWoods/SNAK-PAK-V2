/*
         _____ _______ ______ __  __ 
        |_   _|__   __|  ____|  \/  |
          | |    | |  | |__  | \  / |
          | |    | |  |  __| | |\/| |
         _| |_   | |  | |____| |  | |
        |_____|  |_|  |______|_|  |_|                
        */
        public class Item {
            public Button Display;
            public int ID;
            static ComputerUIElement ComputerParent;
            static ViewUIElement ViewParent;

            static Nullable<Point> dragStart = null;
            static readonly MainWindow mw = (MainWindow)Application.Current.MainWindow;

            //Left mouse down
            static MouseButtonEventHandler mouseDown = (mouseSender, args) => {
                var element = (FrameworkElement)mouseSender;
                if (args.ClickCount == 2) {  
                    if (ViewParent != null) {
                        CurrentView.HideView();
                        ViewParent.ShowView();
                    }
                } else {
                    dragStart = args.GetPosition(element);
                    element.CaptureMouse();
                }
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
            public Action<UIElement> enableDrag = (element) => {
                element.PreviewMouseLeftButtonDown += mouseDown;
                element.PreviewMouseMove += mouseMove;
                element.PreviewMouseLeftButtonUp += mouseUp;
            };

            public void ShowItem() {
                Display.Visibility = Visibility.Visible;
            }
            public void HideItem() {
                Display.Visibility = Visibility.Hidden;
            }

            public void CreateDisplay(string name) {
                Display = new Button();
                Display.Style = mw.Resources["TransparentStyle"] as Style;

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

                Display.Content = grid;
                Display.ContextMenu = contextMenu;

                enableDrag(Display); 
                mw.computerCanvas.Children.Add(Display);
                HideItem();
            }

            public Item(ComputerUIElement parent) {
                ComputerParent = parent;
                CreateDisplay(ComputerParent.Name);
            }
            public Item(ViewUIElement parent) {
                ViewParent = parent;
                CreateDisplay(ViewParent.Name);
            }
        }

        void SaveFile() {
            /*
            using (var output = File.Create("test.snak")) {
                finalView.WriteTo(output);
            }
            */
        }