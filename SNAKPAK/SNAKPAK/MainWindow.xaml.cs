using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

namespace SNAKPAK {
    public partial class MainWindow : Window {

        private bool _isDown;
        private bool _isDragging;
        private Canvas canvas;
        private UIElement _originalElement;
        private double _originalLeft;
        private double _originalTop;
        private Point _startPoint;

        void OnPageLoad(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("hi");
            canvas = new Canvas();

            var computer = new Rectangle();
            computer.Height = 60;
            computer.Width = 140;
            computer.Fill = Brushes.Black;

            Canvas.SetTop(computer, 20);
            Canvas.SetLeft(computer, 20);

            canvas.Children.Add(computer);

            computerGrid.Children.Add(canvas);
        }
    }
}
