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
        public MainWindow()
        {
            InitializeComponent();
            InitializeTextAsync();
            PopulateContinentsComboBox();
        }

        private async void InitializeTextAsync()
        {

            string text = "";
            RootRegionObject regions = await Vultr.GetLocations();

            foreach (var region in regions.Regions)
            {
                text += region.Continent + " ";
            }

            FirstTextBlock.Text = text;
        }

        private void PopulateContinentsComboBox()
        {
            for (int i = 0; i < 5; i++)
            {
                ContinentsComboBox.Items.Add("Place" + i);
            }

            ContinentsComboBox.SelectedIndex = 1;
        }
    }
}