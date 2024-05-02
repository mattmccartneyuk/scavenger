using System.ComponentModel;
using Scavenger.Core;
using System.Windows;
using System.Windows.Controls;
using Scavenger.Core.Config;

namespace Scavenger.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContinentToCountryToCity _continentToCountryToCity;
        private readonly Instance _InstanceDetails;

        public MainWindow()
        {
            InitializeComponent();
            PopulateComboBox();

            _InstanceDetails = new Instance();
            _InstanceDetails.Update("0.0.0.0", "root", "password");

            _continentToCountryToCity = new ContinentToCountryToCity();

            TextBlock.Text = Settings.Default.APIKEY;
            TextBlock.Text = ConfigManager.GetApiKey();
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

            foreach (var city in regions.Regions.GroupBy(x => x.City).Select(x => x.Key).OrderBy(x => x))
            {
                CityComboBox.Items.Add(city);
            }

            ContinentsComboBox.SelectedIndex = 0;
            CountryComboBox.SelectedIndex = 0;
        }

        private void ChangeCountryComboBox(object sender, SelectionChangedEventArgs e)
        {
            CountryComboBox.Items.Clear();
            CountryComboBox.SelectedIndex = 0;

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

            var selectedCountry = CountryComboBox.SelectedValue.ToString();
            CityComboBox.Items.Clear();

            foreach (var lookup in _continentToCountryToCity.Lookup)
            {
                if (lookup.Key == ContinentsComboBox.SelectedValue.ToString())
                {
                    foreach (var country in lookup.Value)
                    {
                        if (country.Key == selectedCountry)
                        {
                            foreach (var city in country.Value)
                            {
                                CityComboBox.Items.Add(city);
                            }
                        }
                    }

                }
            }
            CityComboBox.SelectedIndex = 0;
        }

        private void ChangeCityComboBox(object sender, SelectionChangedEventArgs e)
        {
            if (CountryComboBox.IsDropDownOpen)
            {
                var selectedCountry = CountryComboBox.SelectedValue.ToString();
                CityComboBox.Items.Clear();

                foreach (var lookup in _continentToCountryToCity.Lookup)
                {
                    if (lookup.Key == ContinentsComboBox.SelectedValue.ToString())
                    {
                        foreach (var country in lookup.Value)
                        {
                            if (country.Key == selectedCountry)
                            {
                                foreach (var city in country.Value)
                                {
                                    CityComboBox.Items.Add(city);
                                }
                            }
                        }

                    }
                }
                CityComboBox.SelectedIndex = 0;
            }

        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private async void Deploy_Instance(object sender, RoutedEventArgs e)
        {
            var response = await Vultr.CreateInstance(Settings.Default.APIKEY);

            _InstanceDetails.Update(response.Instance.IpAddress, response.Instance.DefaultUser, response.Instance.Password);

            TextBlock.Text = $"{_InstanceDetails.DefaultUser}, {_InstanceDetails.Password}";
        }

        private async void Get_Instances(object sender, RoutedEventArgs e)
        {
            InstancesTextBlock.Text = await Vultr.GetAllInstances(Settings.Default.APIKEY);
        }

        private async void Send_SSH(object sender, RoutedEventArgs e)
        {
            var result = await Vultr.SendSsh(_InstanceDetails);

            InstancesTextBlock.Text = result;
        }

        private void Get_InstanceDetails(object sender, RoutedEventArgs e)
        {
            UserBox.Text = _InstanceDetails.DefaultUser;
            PasswordBox.Text = _InstanceDetails.Password;
            IpBox.Text = _InstanceDetails.IpAddress;
        }
    }
}