using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using MahApps.Metro.Controls;
using System.Diagnostics;

namespace PlantManager_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Plant _mCurrentPlant;

        public MainWindow()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(this.imgImage,BitmapScalingMode.HighQuality);

            //Afficher les plantes dans la liste.
            ReloadListView();

            //Charger les informations dans le combobox des genres.
            LoadGenusCombo();

            LoadHardinessZonesCombo();

            LoadSunLevelsCombo();

            LoadShapesCombo();

            LoadPlantTypesCombo();

            LoadSoilTypesCombo();

            //Desactiver le bouton pour sauvegarder les changements.
            butSaveChanges.IsEnabled = false;

            //Cacher le TabControl.
            tcPlant.Visibility = Visibility.Hidden;
        }

        private void LoadHardinessZonesCombo()
        {
            HardinessZone[] hardinessZones = HardinessZone.GetAllHardinessZones();

            cbHardinessZones.ItemsSource = hardinessZones;

            if (_mCurrentPlant != null)
            {
                cbHardinessZones.SelectedValue = _mCurrentPlant.HardZone.Id;
            }
            else
            {
                cbHardinessZones.SelectedValue = HardinessZone.GetDefaultHardinessZone().Id;
            }
        }

        private void LoadSunLevelsCombo()
        {
            SunLevel[] sunLevels = SunLevel.GetAllSunLevels();

            cbSunLevels.ItemsSource = sunLevels;

            if (_mCurrentPlant != null)
            {
                cbSunLevels.SelectedValue = _mCurrentPlant.SunLvl.Id;
            }
            else
            {
                cbSunLevels.SelectedValue = SunLevel.GetDefaultSunLevel().Id;
            }
        }

        private void LoadPlantTypesCombo()
        {
            PlantType[] plantTypes = PlantType.GetAllPlantTypes();

            cbPlantTypes.ItemsSource = plantTypes;

            if (_mCurrentPlant != null)
            {
                cbPlantTypes.SelectedValue = _mCurrentPlant.PlantType.Id;
            }
            else
            {
                cbPlantTypes.SelectedValue = PlantType.GetDefaultPlantType().Id;
            }
        }

        private void LoadShapesCombo()
        {
            Shape[] shapes = Shape.GetAllShapes();

            cbShapes.ItemsSource = shapes;


            if (_mCurrentPlant != null)
            {
                cbShapes.SelectedValue = _mCurrentPlant.Shape.Id;
            }
            else
            {
                cbShapes.SelectedValue = Shape.GetDefaultShape().Id;
            }
        }

        private void LoadSoilTypesCombo()
        {
            SoilType[] soilTypes = SoilType.GetAllSoilTypes();

            cbSoilTypes.ItemsSource = soilTypes;

            if (_mCurrentPlant != null)
            {
                cbSoilTypes.SelectedValue = _mCurrentPlant.SoilType.Id;
            }
            else
            {
                cbSoilTypes.SelectedValue = SoilType.GetDefaultSoilType().Id;
            }
        }

        private void LoadGenusCombo()
        {
            Genus[] genuses = Genus.GetAllGenus();

            cbGenus.ItemsSource = genuses;

            if (_mCurrentPlant != null)
            {
                cbGenus.SelectedValue = _mCurrentPlant.Genus.Id;
            }
            else
            {
                cbGenus.SelectedValue = Genus.GetDefaultGenus().Id;
            }
        }

        private void btAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            if (_mCurrentPlant == null) return;

            ImagePlant.AddImage(_mCurrentPlant.Id, dlg.FileName);

            ShowPlantImage();

            CheckControlsStates();
        }

        private void ReloadListView()
        {
            Plant[] plants = Plant.GetAllPlant();

            lstPlants.Items.Clear();

            lstPlants.ItemsSource = plants;
        }

        public void RefreshPlant()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { tcPlant.Visibility = Visibility.Hidden; }, null);

            Thread.Sleep(100);

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { txtDescription.Text = string.Empty; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { lblName.Content = string.Empty; }, null);

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { nudHeight.Value = 0; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { nudWidth.Value = 0; }, null);

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { lblName.Content = _mCurrentPlant.Name; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { txtCultivar.Text = _mCurrentPlant.Cultivar; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { txtSpecies.Text = _mCurrentPlant.Species; }, null);

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { txtDescription.Text = _mCurrentPlant.Description; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { cbGenus.SelectedValue = _mCurrentPlant.Genus.Id; }, null);

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { cbHardinessZones.SelectedValue = _mCurrentPlant.HardZone.Id; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { cbSunLevels.SelectedValue = _mCurrentPlant.SunLvl.Id; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { cbShapes.SelectedValue = _mCurrentPlant.Shape.Id; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { cbPlantTypes.SelectedValue = _mCurrentPlant.PlantType.Id; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { cbSoilTypes.SelectedValue = _mCurrentPlant.SoilType.Id; }, null);

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { nudHeight.Value = _mCurrentPlant.Height; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { nudWidth.Value = _mCurrentPlant.Width; }, null);

            ShowPlantImage();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { this.CheckControlsStates(); }, null);

            Thread.Sleep(100);

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { this.pbLoading.IsIndeterminate = false ; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { tcPlant.Visibility = Visibility.Visible; }, null);

        }

        private void CheckControlsStates()
        {
            if (_mCurrentPlant != null)
            {
                if (_mCurrentPlant.Images.Length > 1)
                {
                    btLeftImage.IsEnabled = true;
                    btRightImage.IsEnabled = true;
                }
                else
                {
                    btLeftImage.IsEnabled = false;
                    btRightImage.IsEnabled = false;
                }
            }
        }

        private void ShowPlantImage()
        {
            if (_mCurrentPlant == null) return;

                Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    if (_mCurrentPlant.Images.Length > 0)
                    {

                        imgImage.Source = _mCurrentPlant.Images[0].Image;
                        imgImage.Tag = 0;
                    }
                    else
                    {
                        imgImage.Source = null;
                        imgImage.Tag = null;
                    }
                }, null);
        }

        private void lstPlants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstPlants.SelectedItems.Count > 0)
            {
                Plant maPlante = (Plant)lstPlants.SelectedItem;

                //Afficher un message si les modifications n'ont pas ete enregistres.
                _mCurrentPlant = maPlante;

                Thread t;

                this.pbLoading.IsIndeterminate = true;
                t = new Thread(new ThreadStart(RefreshPlant));
                t.Start();

            }
            else
            {
                if (tcPlant.Visibility == Visibility.Hidden) return;

                tcPlant.Visibility = Visibility.Hidden;
            }
        }

        private void btRightImage_Click(object sender, RoutedEventArgs e)
        {
            if (_mCurrentPlant == null) return;

            if (_mCurrentPlant.Images.Length == 0) return;
            int currentImage = (int)imgImage.Tag;

            if (currentImage + 1 >= _mCurrentPlant.Images.Length)
            {
                currentImage = 0;
            }
            else
            {
                currentImage++;
            }

            imgImage.Source = _mCurrentPlant.Images[currentImage].Image;
            imgImage.Tag = currentImage;
        }

        private void btLeftImage_Click(object sender, RoutedEventArgs e)
        {
            if (_mCurrentPlant == null) return;


            if (_mCurrentPlant.Images.Length == 0) return;
            int currentImage = (int)imgImage.Tag;

            if (currentImage - 1 < 0)
            {
                currentImage = _mCurrentPlant.Images.Length - 1;
            }
            else
            {
                currentImage--;
            }

            imgImage.Source = _mCurrentPlant.Images[currentImage].Image;
            imgImage.Tag = currentImage;
        }

        private void imgImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                try
                {
                    ViewPicture view = new ViewPicture(_mCurrentPlant.Name, ((Image)sender).Source);
                    view.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void mnuQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void butSearch_Click(object sender, RoutedEventArgs e)
        {
            tcPlant.Visibility = Visibility.Hidden;
            Plant[] plants = Plant.GetAllPlantByNameContains(txtSearchField.Text);

            lstPlants.Items.Clear();

            foreach (Plant plant in plants)
            {
                lstPlants.Items.Add(plant);
            }
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            AddPlantWindow addPlantForm = new AddPlantWindow();
            addPlantForm.ShowDialog();
            ReloadListView();
        }

        private void butDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstPlants.SelectedItems.Count <= 0) return;

            Plant plant = (Plant)lstPlants.SelectedItem;

            if (plant.Id.ToString() == _mCurrentPlant.Id.ToString())
            {
                _mCurrentPlant = null;
                tcPlant.Visibility = Visibility.Hidden;
            }

            Plant.DeletePlantById(plant.Id);
            ReloadListView();
        }

        private void txtWidth_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = (int)e.Key >= 43 || (int)e.Key <= 34;
        }

        private void txtHeight_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = (int)e.Key >= 43 || (int)e.Key <= 34;
        }

        private void btDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            if (_mCurrentPlant == null) return;

            MessageBoxResult dialogResult = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette image?", "Attention", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if(_mCurrentPlant.Images.Length > 0)
                { 
                    ImagePlant.DeleteImage(_mCurrentPlant.Images[(int)imgImage.Tag].ImageId);
                    ShowPlantImage();
                }
            }
            
        }

        private void butSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_mCurrentPlant == null)
            {
                return;
            }
            if (txtSpecies.Text.Length < 1)
            {
                MessageBox.Show(Constants.DialogNameCannotBeBlank);
                return;
            }

            if (txtSpecies.Text != _mCurrentPlant.Name ||
                txtDescription.Text != _mCurrentPlant.Description || txtSpecies.Text != _mCurrentPlant.Species || (int)cbGenus.SelectedValue != _mCurrentPlant.Genus.Id)
            {
                Plant.UpdatePlantBase(_mCurrentPlant.Id, (int)cbGenus.SelectedValue, txtSpecies.Text, txtDescription.Text);
            }

            if (txtCultivar.Text != _mCurrentPlant.Cultivar)
                Plant.UpdatePlantCultivar(_mCurrentPlant.Id, txtCultivar.Text);

            if (nudHeight.Value != _mCurrentPlant.Height)
                Plant.UpdatePlantHeight(_mCurrentPlant.Id, (int)nudHeight.Value);

            if (nudWidth.Value != _mCurrentPlant.Width)
                Plant.UpdatePlantWidth(_mCurrentPlant.Id, (int)nudWidth.Value);

            if ((int)cbHardinessZones.SelectedValue != _mCurrentPlant.HardZone.Id)
                Plant.UpdatePlantHardinessZone(_mCurrentPlant.Id, (int)cbHardinessZones.SelectedValue);

            if ((int)cbSunLevels.SelectedValue != _mCurrentPlant.SunLvl.Id)
                Plant.UpdatePlantSunLevel(_mCurrentPlant.Id, (int)cbSunLevels.SelectedValue);

            if ((int)cbShapes.SelectedValue != _mCurrentPlant.Shape.Id)
                Plant.UpdatePlantShape(_mCurrentPlant.Id, (int)cbShapes.SelectedValue);

            if ((int)cbPlantTypes.SelectedValue != _mCurrentPlant.PlantType.Id)
                Plant.UpdatePlantPlantType(_mCurrentPlant.Id, (int)cbPlantTypes.SelectedValue);

            if ((int)cbSoilTypes.SelectedValue != _mCurrentPlant.SoilType.Id)
                Plant.UpdatePlantSoilType(_mCurrentPlant.Id, (int)cbSoilTypes.SelectedValue);

            butSaveChanges.IsEnabled = false;
        }

        private void butSearchImage_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://images.google.com/search?tbm=isch&q=" + _mCurrentPlant.Name);
        }

        private void RefreshName()
        {
            lblName.Content = cbGenus.SelectedItem.ToString() + " " + txtSpecies.Text + " " + txtCultivar.Text;
        }

        private void CheckChanges(object sender, EventArgs e)
        {
            if (tcPlant.Visibility == Visibility.Hidden)
                return;

            bool enableSave = false;

            if (_mCurrentPlant == null)
                return;

            if (sender is ComboBox && ((ComboBox)sender).SelectedValue == null)
            {
                return;
            }

            if (txtSpecies.Text != _mCurrentPlant.Species || (int)cbGenus.SelectedValue != _mCurrentPlant.Genus.Id || txtCultivar.Text != _mCurrentPlant.Cultivar)
            {
                RefreshName();
            }

            if (txtSpecies.Text != _mCurrentPlant.Species)
                enableSave = true;

            if (txtDescription.Text != _mCurrentPlant.Description)
                enableSave = true;

            if ((int)cbGenus.SelectedValue != _mCurrentPlant.Genus.Id)
                enableSave = true;

            if (txtCultivar.Text != _mCurrentPlant.Cultivar)
                enableSave = true;

            if (nudHeight.Value != _mCurrentPlant.Height)
                enableSave = true;

            if (nudWidth.Value != _mCurrentPlant.Width)
                enableSave = true;

            if ((int)cbHardinessZones.SelectedValue != _mCurrentPlant.HardZone.Id)
                enableSave = true;

            if ((int)cbSunLevels.SelectedValue != _mCurrentPlant.SunLvl.Id)
                enableSave = true;

            if ((int)cbShapes.SelectedValue != _mCurrentPlant.Shape.Id)
                enableSave = true;

            if ((int)cbPlantTypes.SelectedValue != _mCurrentPlant.PlantType.Id)
                enableSave = true;

            if ((int)cbSoilTypes.SelectedValue != _mCurrentPlant.SoilType.Id)
                enableSave = true;

            butSaveChanges.IsEnabled = enableSave;
        }

        private void mnuManageShapes_Click(object sender, RoutedEventArgs e)
        {
            ManageShapesWindow manageShapesWindow = new ManageShapesWindow();
            manageShapesWindow.ShowDialog();

            LoadShapesCombo();
        }

        private void mnuManageSunLevels_Click(object sender, RoutedEventArgs e)
        {
            ManageSunLevelsWindow manageSunLevelsWindow = new ManageSunLevelsWindow();
            manageSunLevelsWindow.ShowDialog();

            LoadSunLevelsCombo();
        }

        private void mnuManagePlantTypes_Click(object sender, RoutedEventArgs e)
        {
            ManagePlantTypesWindow managePlantTypesWindow = new ManagePlantTypesWindow();
            managePlantTypesWindow.ShowDialog();

            LoadPlantTypesCombo();
        }

        private void mnuManageSoilTypes_Click(object sender, RoutedEventArgs e)
        {
            ManageSoilTypesWindow manageSoilTypesWindow = new ManageSoilTypesWindow();
            manageSoilTypesWindow.ShowDialog();

            LoadSoilTypesCombo();
        }

        private void mnuManageHardinessZones_Click(object sender, RoutedEventArgs e)
        {
            ManageHardinessZonesWindow manageHardinessZonesWindow = new ManageHardinessZonesWindow();
            manageHardinessZonesWindow.ShowDialog();

            LoadHardinessZonesCombo();
        }

        private void mnuManageGenus_Click(object sender, RoutedEventArgs e)
        {
            ManageGenusWindow manageGenusWindow = new ManageGenusWindow();
            manageGenusWindow.ShowDialog();

            LoadGenusCombo();
        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

    }
}
