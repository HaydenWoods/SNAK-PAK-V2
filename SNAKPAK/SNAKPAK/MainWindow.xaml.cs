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
using Microsoft.Win32;

namespace SNAKPAK {
    public static class ExtensionMethods {
        public static int RoundOff(int num, int snap) {
            return (int)((int)Math.Round(num / (double)snap) * snap);
        }
        public static void GenerateRandomViews(View parentView, int recursions, int maxRecursions) {
            Random r = new Random();

            for (int i = 0; i < r.Next(1, 6); i++) {
                View subView = new View();
                subView.ViewName = parentView.ViewName + " - SubView(" + i + ")";

                for (int j = 0; j < r.Next(1, 10); j++) {
                    Computer computer = new Computer();
                    computer.ComputerName = subView.ViewName + " - Computer (" + j + ")";
                    subView.Computers.Add(computer);
                }

                parentView.Subviews.Add(subView);

                recursions = recursions + 1;
                if (recursions < maxRecursions) {
                    GenerateRandomViews(subView, recursions, maxRecursions);
                }
            }
        }
        public static void GenerateRandomFile(string fileName) {
            Random r = new Random();
            View masterView = new View();
            masterView.ViewName = "Master View";

            GenerateRandomViews(masterView, 0, r.Next(3, 10));

            using (var output = File.Create("test.snak")) {
                masterView.WriteTo(output);
            }
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
        public ViewUI MasterView;
        public ViewUI CurrentView {
            get {
                return _CurrentView;
            }
            set {
                _CurrentView = value;
                if (_CurrentView != null) {
                    DrawCanvas();
                }
            }
        }
        private ViewUI _CurrentView;
        public static MainWindow mw;
        public static ActiveDirectory activeDir;

        /*
          _____          _   ___      __      _____   ______ _      ______ __  __ ______ _   _ _______ 
         / ____|   /\   | \ | \ \    / /\    / ____| |  ____| |    |  ____|  \/  |  ____| \ | |__   __|
        | |       /  \  |  \| |\ \  / /  \  | (___   | |__  | |    | |__  | \  / | |__  |  \| |  | |   
        | |      / /\ \ | . ` | \ \/ / /\ \  \___ \  |  __| | |    |  __| | |\/| |  __| | . ` |  | |   
        | |____ / ____ \| |\  |  \  / ____ \ ____) | | |____| |____| |____| |  | | |____| |\  |  | |   
         \_____/_/    \_|_| \_|   \/_/    \_|_____/  |______|______|______|_|  |_|______|_| \_|  |_|                                                                                                
        */
        public class CanvasElement {
            public int id;
            public string name;   
        }

        /*
          _____ ____  __  __ _____  _    _ _______ ______ _____    _    _ _____ 
         / ____/ __ \|  \/  |  __ \| |  | |__   __|  ____|  __ \  | |  | |_   _|
        | |   | |  | | \  / | |__) | |  | |  | |  | |__  | |__) | | |  | | | |  
        | |   | |  | | |\/| |  ___/| |  | |  | |  |  __| |  _  /  | |  | | | |  
        | |___| |__| | |  | | |    | |__| |  | |  | |____| | \ \  | |__| |_| |_ 
         \_____\____/|_|  |_|_|     \____/   |_|  |______|_|  \_\  \____/|_____|                                                               
        */
        public class ComputerUI : CanvasElement {
            public ComputerUI(Computer computer) {
                name = computer.ComputerName;
            }
        }


        /*
        __      _______ ________          __  _    _ _____ 
        \ \    / |_   _|  ____\ \        / / | |  | |_   _|
         \ \  / /  | | | |__   \ \  /\  / /  | |  | | | |  
          \ \/ /   | | |  __|   \ \/  \/ /   | |  | | | |  
           \  /   _| |_| |____   \  /\  /    | |__| |_| |_ 
            \/   |_____|______|   \/  \/      \____/|_____|                                              
        */
        public class ViewUI : CanvasElement {
            public List<Object> children = new List<Object>();
            
            //Called on object construction
            public void LoadSubViews(View parentView) {
                for (int i = 0; i < parentView.Subviews.Count; i++) {
                    ViewUI subView = new ViewUI(parentView.Subviews[i]);
                    children.Add(subView);
                }
                for (int i = 0; i < parentView.Computers.Count; i++) {
                    ComputerUI computer = new ComputerUI(parentView.Computers[i]);
                    children.Add(computer);
                }
            }

            public ViewUI(View parentView) {
                name = parentView.ViewName;
                LoadSubViews(parentView);
            }
        }

        //Active Directory
        public class ActiveDirectory {
            public DirectoryEntry dir;

            public SearchResult SearchDirSingle() {
                DirectorySearcher search = new DirectorySearcher(dir);
                search.Filter = "(objectClass=Computer)";

                SearchResult searchResults = search.FindOne();
                return searchResults;
            }
            public SearchResultCollection SearchDirAll() {
                DirectorySearcher search = new DirectorySearcher(dir);
                search.Filter = "(objectClass=Computer)";

                SearchResultCollection searchResults = search.FindAll();
                return searchResults;
            }

            DirectoryEntry LoadActiveDir() {
                DirectoryEntry dir = new DirectoryEntry("WinNT:");
                return dir;
            }

            public ActiveDirectory() {
                dir = LoadActiveDir();
            }
        }
        
        //AD UI
        public void DisplayADResults(SearchResultCollection searchResults) {
            for (int i = 0; i < searchResults.Count; i++) {
                try {
                    SearchResult searchResult = searchResults[i];
                    ADResults.Text += searchResult.GetDirectoryEntry().Properties["name"].Value.ToString() + " - ";
                    ADResults.Text += searchResult.GetDirectoryEntry().Properties["dnshostname"].Value.ToString();
                    ADResults.Text += "\n";
                } catch {
                    Debug.WriteLine("error");
                }
            }      
        }

        //General UI
        public static Nullable<Point> dragStart = null;
        new void MouseDown(object mouseSender, MouseButtonEventArgs args) {
            var element = (FrameworkElement)mouseSender;
            if (args.ClickCount == 2) {
                for (int i = 0; i < CurrentView.children.Count; i++) {
                    if (CurrentView.children[i].GetType() == typeof(ViewUI)) {
                        ViewUI child = (ViewUI)CurrentView.children[i];
                        if (child.id == element.GetHashCode()) {
                            CurrentView = child;
                        }
                    }      
                }
            } else {
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            }
        }
        new void MouseUp(object mouseSender, MouseButtonEventArgs args) {
            var element = (FrameworkElement)mouseSender;
            dragStart = null;
            element.ReleaseMouseCapture();
        }
        new void MouseMove(object mouseSender, MouseEventArgs args) {
            var element = (FrameworkElement)mouseSender;
            if (dragStart != null && args.LeftButton == MouseButtonState.Pressed) {
                var p2 = args.GetPosition(mw.ViewCanvas);

                if ((p2.X - dragStart.Value.X) > 0 && (p2.X - dragStart.Value.X + element.ActualWidth) < (mw.ViewCanvas.ActualWidth)) {
                    double newX = p2.X - dragStart.Value.X;
                    if (Properties.Settings.Default.SnapToGrid == true) {
                        newX = ExtensionMethods.RoundOff((int)newX, Properties.Settings.Default.SnapAmount);

                        if ((newX + element.ActualWidth) > (mw.ViewCanvas.ActualWidth)) {
                            newX = newX - Properties.Settings.Default.SnapAmount;
                        }
                    }
                    Canvas.SetLeft(element, newX);
                }
                if ((p2.Y - dragStart.Value.Y) > 0 && (p2.Y - dragStart.Value.Y + element.ActualHeight) < (mw.ViewCanvas.ActualHeight)) {
                    double newY = p2.Y - dragStart.Value.Y;
                    if (Properties.Settings.Default.SnapToGrid == true) {
                        newY = ExtensionMethods.RoundOff((int)newY, Properties.Settings.Default.SnapAmount);

                        if ((newY + element.ActualHeight) > (mw.ViewCanvas.ActualHeight)) {
                            newY = newY - Properties.Settings.Default.SnapAmount;
                        }
                    }

                    Canvas.SetTop(element, newY);
                }
            }
        }
        void EnableDrag(UIElement element) {
            element.PreviewMouseLeftButtonDown += MouseDown;
            element.PreviewMouseMove += MouseMove;
            element.PreviewMouseLeftButtonUp += MouseUp;
        }

        //Canvas 
        void ClearCanvas() {
            ViewCanvas.Children.Clear();
        }
        void DrawCanvas() {
            ClearCanvas();
            CurrentViewName.Text = CurrentView.name;
            for (int i = 0; i < CurrentView.children.Count; i++) {
                Button Display = new Button();
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
                if (CurrentView.children[i].GetType() == typeof(ViewUI)) {
                    ViewUI child = (ViewUI)CurrentView.children[i];
                    text.Text = child.name;
                    child.id = Display.GetHashCode();
                } else if (CurrentView.children[i].GetType() == typeof(ComputerUI)) {
                    ComputerUI child = (ComputerUI)CurrentView.children[i];
                    text.Text = child.name;
                    child.id = Display.GetHashCode();
                }

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

                EnableDrag(Display);
                mw.ViewCanvas.Children.Add(Display);
            }
        }

        //Menu item functionality
        View LoadFile(string fileName) {
            View view;
            using (var input = File.OpenRead(fileName)) {
                view = View.Parser.ParseFrom(input);
            }
            return view;   
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            MenuItem source = e.Source as MenuItem;
            switch (source.Name) {
                case "Open":
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "SnakPak Files (*.snak|*.snak";
                    if (openFileDialog.ShowDialog() == true) {
                        MasterView = new ViewUI(LoadFile(openFileDialog.FileName));
                        CurrentView = MasterView;
                    }
                    break;
                default:
                    break;
            }
        }
        void OnPageLoad(object sender, RoutedEventArgs e) {
            mw = (MainWindow)Application.Current.MainWindow;
            activeDir = new ActiveDirectory();
            DisplayADResults(activeDir.SearchDirAll());
            //ExtensionMethods.GenerateRandomFile("test.snak");    
        }
    }
}

