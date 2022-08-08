using Front.DataTools;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Front
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataParser parser;

        public MainWindow()
        {
            parser = new DataParser();
            InitializeComponent();
        }

        private void GetValuesView(object sender, RoutedEventArgs e)
        {
            GetValuesView view = new GetValuesView(parser);
            view.ShowDialog();
        }

        private void GetExtentView(object sender, RoutedEventArgs e)
        {
            GetExtentView view = new GetExtentView(parser);
            view.ShowDialog();
        }

        private void GetRelatedView(object sender, RoutedEventArgs e)
        {
            GetRelatedView view = new GetRelatedView(parser);
            view.ShowDialog();
        }
    }
}
