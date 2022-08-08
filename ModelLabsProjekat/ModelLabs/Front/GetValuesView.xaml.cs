using Front.DataTools;
using FTN.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Front
{
    /// <summary>
    /// Interaction logic for GetValuesView.xaml
    /// </summary>
    public partial class GetValuesView : Window
    {
        public DataParser parser;
        public GetValuesView(DataParser parser)
        {
            this.parser = parser;
            InitializeComponent();
            DataContext = this;
            this.GIDs = parser.GetAllGIDs();
        }

        public List<string> GIDs { get; set; }
        public string SelectedGid { get; set; }

        public List<string> Props { get; set; }
        //public ObservableCollection<string> PropsList { get; set; }
        private List<string> GetProps(string val)
        {
            List<string> ret = new List<string>();
            if(val != string.Empty)
            {
                sve.IsChecked = false;
                if(Props != null)
                    Props.Clear();
                long gid = parser.ParseGIDFromString(val);
                ret = parser.GetAllProperties(gid);
            }

            return ret;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Props = this.GetProps(SelectedGid);
            

            this.PropListBox.ItemsSource = Props;
        }

        private void sve_Click(object sender, RoutedEventArgs e)
        {
            if (this.sve.IsChecked == true)
            {
                this.PropListBox.SelectAll();
            }
            else
            {
                this.PropListBox.UnselectAll();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedGid != string.Empty && this.PropListBox.SelectedItems.Count > 0)
            {
                long gid = parser.ParseGIDFromString(SelectedGid.ToString());
                List<string> list = new List<string>();
                foreach(string item in this.PropListBox.SelectedItems)
                {
                    list.Add(item);
                }

                var codes = (from x in list select (ModelCode)Enum.Parse(typeof(ModelCode), x)).ToList();

                if(codes.Count > 0)
                {
                    this.@out.Text = parser.GetValues(gid, codes);
                }
            }
        }
    }
}
