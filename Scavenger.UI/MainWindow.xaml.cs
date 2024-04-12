using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Scavenger.Core;

namespace Scavenger.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContinentToCountry _continentToCountry;

        public MainWindow()
        {
            InitializeComponent();
            PopulateComboBox();

            _continentToCountry = new ContinentToCountry();
        }

        private async Task PopulateComboBox()
        {
            RootRegionObject regions = await Vultr.GetLocations();

            _continentToCountry.Parse(regions);

            string countries = "";

            foreach (var lookup in _continentToCountry.Lookup)
            {
                if (lookup.Key == "Europe")
                {
                    foreach (var country in lookup.Value)
                    {
                        countries += country + " ";
                    }
                }
            }

            TextBlock.Text = countries;

            foreach (var continent in regions.Regions.GroupBy(x => x.Continent).Select(x => x.Key).OrderBy(x => x))
            {
                ContinentsComboBox.Items.Add(continent);
            }

            foreach (var country in regions.Regions.GroupBy(x => x.Country).Select(x => x.Key).OrderBy(x => x))
            {
                CountryComboBox.Items.Add(country);
            }

            ContinentsComboBox.SelectedIndex = 0;
            CountryComboBox.SelectedIndex = 0;
        }
        
    }
}