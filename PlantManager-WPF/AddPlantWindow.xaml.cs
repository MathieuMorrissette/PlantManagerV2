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
    /// Interaction logic for AddPlantWindow.xaml
    /// </summary>
    public partial class AddPlantWindow : MetroWindow
    {
        public AddPlantWindow()
        {
            InitializeComponent();

            this.LoadGenusCombo();
        }

        private void LoadGenusCombo()
        {
            Genus[] genuses = Genus.GetAllGenus();

            cbGenus.ItemsSource = genuses;

            cbGenus.SelectedIndex = 0;
            //Dont forget to check if theres no genus added they need to add before adding a plant
        }

        private void butCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtSpecies.Text.Length < 1)
            {
                MessageBox.Show(Constants.DialogNameCannotBeBlank);
                return;
            }

            Plant.AddPlant(((Genus)cbGenus.SelectedItem).Id, txtSpecies.Text, txtCultivar.Text, txtDescription.Text);
            Close();
        }
    }
}
