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
        private ContinentToCountryToCity _continentToCountryToCity;

        public MainWindow()
        {
            InitializeComponent();
            PopulateComboBox();

            _continentToCountryToCity = new ContinentToCountryToCity();
        }


        private async Task PopulateComboBox()
        {
            RootRegionObject regions = await Vultr.GetLocations();

            _continentToCountryToCity.Parse(regions);

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

        private void ChangeCountryComboBox(object sender, SelectionChangedEventArgs e)
        {
            CountryComboBox.Items.Clear();

            foreach (var lookup in _continentToCountryToCity.Lookup)
            {
                if (lookup.Key == ContinentsComboBox.SelectedValue.ToString())
                {
                    foreach (var country in lookup.Value)
                    {
                        CountryComboBox.Items.Add(country.Key);
                    }
                }
            }

            CountryComboBox.SelectedIndex = 0;
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityComboBox.Items.Clear();

            
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}