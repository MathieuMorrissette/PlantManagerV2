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
using MahApps.Metro.Controls;

namespace PlantManager_WPF
{
    /// <summary>
    /// Interaction logic for ManageShapesWindow.xaml
    /// </summary>
    public partial class ManageHardinessZonesWindow : MetroWindow
    {
        public ManageHardinessZonesWindow()
        {
            InitializeComponent();

            butDelete.IsEnabled = false;

            RefreshList();
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
                HardinessZone.AddHardinessZone(txtName.Text);

            txtName.Text = string.Empty;

            RefreshList();
        }

        private void RefreshList()
        {
            HardinessZone[] hardinessZones = HardinessZone.GetAllHardinessZones();

            lstItems.ItemsSource = hardinessZones;
        }

        private void butDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstItems.SelectedItems.Count <= 0) return;

            HardinessZone.DeleteHardinessZoneById(((HardinessZone)lstItems.SelectedItem).Id);

            RefreshList();
        }

        private void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            butDelete.IsEnabled = false;

            if (lstItems.SelectedItems.Count <= 0) return;

            butDelete.IsEnabled = Convert.ToInt32(((HardinessZone)lstItems.SelectedItem).Id) != -1;
        }
    }
}
