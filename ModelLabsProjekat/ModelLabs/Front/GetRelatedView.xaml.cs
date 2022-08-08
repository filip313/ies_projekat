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
    /// Interaction logic for GetRelatedView.xaml
    /// </summary>
    public partial class GetRelatedView : Window
    {
        public DataParser parser;
        public GetRelatedView(DataParser parser)
        {
            this.parser = parser;
            InitializeComponent();
            this.DataContext = this;
            Gids = parser.GetAllGIDs();
            DMSTypes = parser.GetDMSTypes();
        }

        public List<string> Gids { get; set; }
        public string SelectedGid { get; set; }
        public List<string> DMSTypes { get; set; }
        public string SelectedDms { get; set; }
        public List<string> RefProps { get; set; }
        public string SelectedRef { get; set; }
        public List<string> Props { get; set; }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedGid != string.Empty)
            {
                long gid = parser.ParseGIDFromString(SelectedGid);
                RefProps = parser.GetReferenceProps(gid);
                this.refs.ItemsSource = RefProps;
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedDms == "0 - No filter")
            {
                Props = parser.GetAllProps(DMSTypes);

            }
            else
            {
                ModelCode mc = parser.ModelCodeFromDMSType(parser.ParseDMSTypeFromString(SelectedDms));
                Props = parser.GetModelProperties(mc);
            }

            this.props.ItemsSource = Props;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(true == (sender as CheckBox).IsChecked)
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
            if(SelectedGid != string.Empty && SelectedDms != string.Empty &&
                SelectedRef != string.Empty && this.props.SelectedItems.Count > 0)
            {
                Association assoc = new Association();
                ModelCode propId = parser.ParseModelCodeFromString(SelectedRef);
                var assocType = SelectedDms == "0 - No filter" ? "0" : SelectedDms;
                ModelCode type = assocType != "0" ? parser.ModelCodeFromDMSType(parser.ParseDMSTypeFromString(assocType)) : (ModelCode)(long.Parse(assocType));

                assoc.PropertyId = propId;
                assoc.Type = type;

                long gid = parser.ParseGIDFromString(SelectedGid);

                List<string> list = new List<string>();
                foreach (string item in this.props.SelectedItems)
                {
                    list.Add(item);
                }

                var codes = (from x in list select (ModelCode)Enum.Parse(typeof(ModelCode), x)).ToList();

                this.tekst.Text = parser.GetRelatedValues(gid, assoc, codes);
            }
        }
    }
}
