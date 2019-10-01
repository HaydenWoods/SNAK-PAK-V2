using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace SNAKPAK {
    public partial class NewSubview : Window {
        public NewSubview() {
            InitializeComponent();
        }

        public void CloseWindow() {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button source = e.Source as Button;
            switch (source.Name) {
                case "Create":
                    ((MainWindow)this.Owner).CreateUISubview(SubviewName.Text);
                    CloseWindow();
                    break;
                case "Cancel":
                    CloseWindow();
                    break;;
                default:
                    break;
            }
        }
    }
}
