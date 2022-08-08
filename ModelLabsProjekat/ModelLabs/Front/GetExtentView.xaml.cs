using Front.DataTools;
using FTN.Common;
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

namespace Front
{
    /// <summary>
    /// Interaction logic for GetExtentView.xaml
    /// </summary>
    public partial class GetExtentView : Window
    {
        public DataParser parser;
        public GetExtentView(DataParser parser)
        {
            this.parser = parser;
            InitializeComponent();
            this.DataContext = this;
            ModelCodes = parser.GetAllConcreteModelCodes();
        }

        public List<string> ModelCodes { get; set; }
        public string SelectedCode { get; set; }
        public List<string> Props { get; set; }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedCode != string.Empty)
            {
                ModelCode modelCode = parser.ParseModelCodeFromString(SelectedCode);
                Props = parser.GetModelProperties(modelCode);
                this.props.ItemsSource = Props;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if((sender as CheckBox).IsChecked == true)
            {
                this.props.SelectAll();
            }
            else
            {
                this.props.UnselectAll();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedCode != string.Empty && this.props.SelectedItems.Count > 0)
            {
                ModelCode mc = parser.ParseModelCodeFromString(SelectedCode);

                List<string> list = new List<string>();
                foreach (string item in this.props.SelectedItems)
                {
                    list.Add(item);
                }

                var codes = (from x in list select (ModelCode)Enum.Parse(typeof(ModelCode), x)).ToList();

                this.text.Text = parser.GetExtentValues(mc, codes);
            }
        }
    }
}
