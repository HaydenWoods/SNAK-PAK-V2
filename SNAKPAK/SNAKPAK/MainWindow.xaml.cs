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
using static View;
using Microsoft.Win32;
using System.Windows.Threading;

namespace SNAKPAK {
    public static class ExtensionMethods {
        public static int RoundOff(int num, int snap) {
            return (int)((int)Math.Round(num / (double)snap) * snap);
        }
        public static void GenerateRandomViews(View parentView, int recursions, int maxRecursions) {
            Random r = new Random();

            for (int i = 0; i < r.Next(1, 6); i++) {
                View subView = new View();
                subView.Name = parentView.Name + " " + i;

                for (int j = 0; j < r.Next(1, 10); j++) {
                    Computer computer = new Computer();
                    computer.Name = "Computer " + j;
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
            masterView.Name = "Master View";

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

        public string CurrentFilePath;

        public static MainWindow mw;
        public static ComputerListings cl;

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
            public CanvasElement parent;
            public int posX;
            public int posY;
            public int posZ;

            internal object TransformToVisual(object root) {
                throw new NotImplementedException();
            }
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
            public string hostname;

            public ComputerUI(Computer computer) {
                if (computer.Name != null && computer.Name != "") {
                    name = computer.Name;
                } else {
                    name = computer.HostName;
                }
                hostname = computer.HostName;
            }

            public ComputerUI(string _name, string _hostname) {
                name = _name;
                hostname = _hostname;
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

            public void LoadSubViews(View view) {
                for (int i = 0; i < view.Subviews.Count; i++) {
                    View child = (View)view.Subviews[i];

                    ViewUI subView = new ViewUI(child);

                    subView.parent = this;
                    children.Add(subView);
                }
                for (int i = 0; i < view.Computers.Count; i++) {
                    Computer child = (Computer)view.Computers[i];

                    ComputerUI computer = new ComputerUI(child);

                    computer.parent = this;
                    children.Add(computer);
                }
            }

            public List<View> SaveSubViews(View parentView) {
                List<View> views = new List<View>();
                
                for (int i = 0; i < children.Count; i++) {
                    if (children[i].GetType() == typeof(ViewUI)) {
                        ViewUI child = (ViewUI)children[i];

                        View view = new View();
                        view.Name = child.name;
                        view.PosX = child.posX;
                        view.PosY = child.posY;
                        view.PosZ = child.posZ;
                        view.Subviews.AddRange(child.SaveSubViews(view));

                        views.Add(view);
                    }
                    if (children[i].GetType() == typeof(ComputerUI)) {
                        ComputerUI child = (ComputerUI)children[i];

                        Computer computer = new Computer();
                        computer.Name = child.name;
                        computer.PosX = child.posX;
                        computer.PosY = child.posY;
                        computer.PosZ = child.posZ;

                        parentView.Computers.Add(computer);
                    }
                }

                return views;
            }

            public ViewUI(string _name) {
                name = _name;
            }

            public ViewUI(View view) {
                name = view.Name;
                posX = view.PosX;
                posY = view.PosY;
                posZ = view.PosZ;
                LoadSubViews(view);
            }
        }

        //
        // Computer Listings
        //
        public class ComputerListings {
            public List<string> results = new List<string>();

            public void LocalNetworkResults() {
                DirectoryEntry root = new DirectoryEntry("WinNT:");

                foreach (DirectoryEntry container in root.Children) {
                    foreach (DirectoryEntry result in container.Children) {
                        /*
                        if (result.SchemaClassName == "Computer") {
                            results.Add(result);
                        }
                        */
                        Debug.WriteLine(result.Properties["Name"]);
                    }
                }
            }
            
            public void ADResults() {
                using (DirectoryEntry entry = new DirectoryEntry("LDAP://")) {
                    using (DirectorySearcher mySearcher = new DirectorySearcher(entry)) {
                        mySearcher.Filter = ("(objectClass=computer)");

                        // No size limit, reads all objects
                        mySearcher.SizeLimit = 0;

                        // Read data in pages of 250 objects. Make sure this value is below the limit configured in your AD domain (if there is a limit)
                        mySearcher.PageSize = 250;

                        // Let searcher know which properties are going to be used, and only load those
                        mySearcher.PropertiesToLoad.Add("name");

                        foreach (SearchResult resEnt in mySearcher.FindAll())
                        {
                            // Note: Properties can contain multiple values.
                            if (resEnt.Properties["name"].Count > 0)
                            {
                                string computerName = (string)resEnt.Properties["name"][0];
                                results.Add(computerName);
                            }
                        }
                    }
                }
            }

            public void ClearListings() {
                mw.ComputerListingsBox.Text = "";
            }

            public void RefreshListings() {
                ClearListings();
                for (int i = 0; i < results.Count; i++) {
                    mw.ComputerListingsBox.Text += results[i] + "\n";
                }
            }

            public ComputerListings() {
                //LocalNetworkResults();
                //ADResults();
            }
        }

        //General UI
        public static Nullable<Point> dragStart = null;
        new void MouseDown(object mouseSender, MouseButtonEventArgs args) {
            FrameworkElement element = (FrameworkElement)mouseSender;
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
            FrameworkElement element = (FrameworkElement)mouseSender;
            dragStart = null;
            element.ReleaseMouseCapture();
        }
        new void MouseMove(object mouseSender, MouseEventArgs args) {
            FrameworkElement element = (FrameworkElement)mouseSender;
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

                CanvasElement cElement = FindCanvasElementFromID(CurrentView, element.GetHashCode());
                Point pos = element.TranslatePoint(new Point(0.0, 0.0), ViewCanvas);
                cElement.posX = (int)pos.X;
                cElement.posY = (int)pos.Y;
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
            CurrentViewName.Text = "";
        }
        void DrawCanvas() {
            ClearCanvas();
            CurrentViewName.Text = CurrentView.name;
            for (int i = 0; i < CurrentView.children.Count; i++) {
                Button Display = new Button();
                Display.Style = mw.Resources["TransparentStyle"] as Style;

                //Give the Canvas Element an ID that links to the Display Element
                CanvasElement element = (CanvasElement)CurrentView.children[i];
                element.id = Display.GetHashCode();

                Canvas.SetTop(Display, element.posY);
                Canvas.SetLeft(Display, element.posX);
                Canvas.SetZIndex(Display, element.posZ);

                //Containg grid inside of the button
                Grid grid = new Grid();

                //Drawn representation of the item
                Rectangle rect = new Rectangle();
                rect.Width = 120;
                rect.Height = 50;
                rect.Stroke = Brushes.Black;
                rect.StrokeThickness = 1;

                if (element.GetType() == typeof(ViewUI)) {
                    rect.Fill = Brushes.Beige;
                } else if (element.GetType() == typeof(ComputerUI)) {
                    rect.Fill = Brushes.AliceBlue;
                }

                //Text
                Border border = new Border();
                TextBlock text = new TextBlock();
                
                text.Text = element.name;
                text.Width = 120;
                text.FontSize = 14;
                text.TextAlignment = TextAlignment.Center;
                text.Foreground = Brushes.Black;
                text.VerticalAlignment = VerticalAlignment.Center;
                text.TextWrapping = TextWrapping.Wrap;
                border.Child = text;

                grid.Children.Add(rect);
                grid.Children.Add(border);

                //Right click menu
                ContextMenu contextMenu = new ContextMenu();
                List<Object> menuItems = new List<Object>();

                //Delete
                MenuItem deleteItem = new MenuItem();
                deleteItem.Header = "Delete";
                deleteItem.DataContext = element.id;
                deleteItem.Click += new RoutedEventHandler(DeleteUIElement);
                menuItems.Add(deleteItem);

                menuItems.Add(new Separator());

                //Bring to Front
                MenuItem frontItem = new MenuItem();
                frontItem.Header = "Bring to Front";
                frontItem.DataContext = element.id;
                frontItem.Click += new RoutedEventHandler(FrontUIElement);
                menuItems.Add(frontItem);

                //Send to Back
                MenuItem backItem = new MenuItem();
                backItem.Header = "Send to Back";
                backItem.DataContext = element.id;
                backItem.Click += new RoutedEventHandler(BackUIElement);
                menuItems.Add(backItem);

                //Add all to the context menu
                for (int j = 0; j < menuItems.Count; j++) {
                    contextMenu.Items.Add(menuItems[j]);
                }

                Display.Content = grid;
                Display.ContextMenu = contextMenu;

                EnableDrag(Display);
                mw.ViewCanvas.Children.Add(Display);
            }
        }

        //Menu item functionality
        void NewFile() {
            if (MasterView == null && CurrentFilePath == null && CurrentView == null) {
                MasterView = new ViewUI("Master View");
                CurrentView = MasterView;
            } else {
                CloseFile();
                NewFile();
            }
        }

        void OpenFile() {
            CloseFile();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SnakPak Files (*.snak)|*.snak";
            if (openFileDialog.ShowDialog() == true) {
                MasterView = new ViewUI(LoadFile(openFileDialog.FileName));
                CurrentView = MasterView;
            }
        }
        View LoadFile(string filepath) {  
            View view;
            using (var input = File.OpenRead(filepath)) {
                view = View.Parser.ParseFrom(input);
            }
            CurrentFilePath = filepath;
            return view;   
        }

        void SaveFile() {
            if (MasterView != null && CurrentFilePath != null) {
                View view = new View();
                view.Name = MasterView.name;
                view.PosX = MasterView.posX;
                view.PosY = MasterView.posY;
                view.Subviews.Add(MasterView.SaveSubViews(view));
                using (var output = File.Create(CurrentFilePath))
                {
                    view.WriteTo(output);
                }
            } else if (CurrentFilePath == null) {
                SaveFileAs();
            }
        }

        void SaveFileAs() {
            if (MasterView != null) {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "SnakPak Files (*.snak)|*.snak";
                if (saveFileDialog.ShowDialog() == true) {
                    CurrentFilePath = saveFileDialog.FileName;
                    SaveFile();
                }
            }
        }

        void CloseFile() {
            if (MasterView != null) {
                MessageBoxResult result = MessageBox.Show("Would you like to save changes to the document?", "Save Changes", MessageBoxButton.YesNoCancel);
                switch (result) {
                    case MessageBoxResult.Yes:
                        if (CurrentFilePath != null) {
                            SaveFile();
                        } else if (CurrentFilePath == null) {
                            SaveFileAs();
                        }
                        ClearCanvas();
                        break;
                    case MessageBoxResult.No:
                        ClearCanvas();
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
                CurrentFilePath = null;
                MasterView = null;
                CurrentView = null;
            }
        }

        public void MenuBarClick(object sender, ExecutedRoutedEventArgs e) {
            switch (e.Parameter) {
                case "New":
                    NewFile();
                    return;
                case "Open":
                    OpenFile();
                    return;
                case "Save":
                    SaveFile();
                    return;
                case "SaveAs":
                    SaveFileAs();
                    return;
                case "Close":
                    CloseFile();
                    return;
                default:
                    return;
            }
        }

        public void CreateUISubview(string name) {
            if (name != null && name != "") {
                if (CurrentView == null && MasterView == null) {
                    NewFile();
                }

                ViewUI view = new ViewUI(name);
                view.parent = CurrentView;
                CurrentView.children.Add(view);
                DrawCanvas();
            } 
        }

        public void CreateUIComputer(string name, string hostname) {
            if (name != null && name != "" && hostname != null && hostname != "") {
                if (CurrentView == null && MasterView == null) {
                    NewFile();
                }

                ComputerUI computer = new ComputerUI(name, hostname);
                computer.parent = CurrentView;
                CurrentView.children.Add(computer);
                DrawCanvas();
            }
        }

        public CanvasElement FindCanvasElementFromID(ViewUI view, int id) {
            for (int i = 0; i < view.children.Count; i++) {
                CanvasElement child = (CanvasElement)CurrentView.children[i];
                if (child.id == id) {
                    return child;
                }
            }
            return null;    
        }

        public void DeleteUIElement(object sender, RoutedEventArgs e) {
            MenuItem source = e.Source as MenuItem;
            CurrentView.children.Remove(FindCanvasElementFromID(CurrentView, (int)source.DataContext));
            DrawCanvas();
        }

        public void FrontUIElement(object sender, RoutedEventArgs e) {
            MenuItem source = e.Source as MenuItem;
            CanvasElement cElement = FindCanvasElementFromID(CurrentView, (int)source.DataContext);

            int maxZ = 0;
            for (int i = 0; i < CurrentView.children.Count; i++) {
                CanvasElement child = (CanvasElement)CurrentView.children[i];
                if (child.posZ > maxZ) {
                    maxZ = child.posZ;
                }
            }
            cElement.posZ = maxZ + 1;

            DrawCanvas();
        }

        public void BackUIElement(object sender, RoutedEventArgs e) {
            MenuItem source = e.Source as MenuItem;
            CanvasElement cElement = FindCanvasElementFromID(CurrentView, (int)source.DataContext);

            int minZ = 0;
            for (int i = 0; i < CurrentView.children.Count; i++) {
                CanvasElement child = (CanvasElement)CurrentView.children[i];
                if (child.posZ < minZ) {
                    minZ = child.posZ;
                }
            }
            cElement.posZ = minZ - 1;

            DrawCanvas();
        }

        public void RefreshListingsUIElement(object sender, RoutedEventArgs e) {
            cl.RefreshListings();
        }

        public void PreviousSubview(object sender, RoutedEventArgs e) {
            if (CurrentView != null) {
                if (CurrentView.parent != null) {
                    CurrentView = (ViewUI)CurrentView.parent;
                }
            }
        }

        void OpenNewSubviewWindow(object sender, RoutedEventArgs e) {
            NewSubview win = new NewSubview();
            win.Owner = this;
            win.Show();
        }
        void OpenNewComputerWindow(object sender, RoutedEventArgs e) {
            NewComputer win = new NewComputer();
            win.Owner = this;
            win.Show();
        }
        void OpenSettingsWindow(object sender, RoutedEventArgs e) {
            Settings win = new Settings();
            win.Owner = this;
            win.Show();
        }

        void OnPageLoad(object sender, RoutedEventArgs e) {
            mw = (MainWindow)Application.Current.MainWindow;
            cl = new ComputerListings();
            //ExtensionMethods.GenerateRandomFile("test.snak");

            //Save Ticker
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += SaveTimer;
            dispatcherTimer.Interval = new TimeSpan(0,0,30);
            dispatcherTimer.Start();
        }

        private void SaveTimer(object sender, EventArgs e) {
            if (CurrentFilePath != null && MasterView != null) {
                SaveFile();
            }
        }
    }
}

