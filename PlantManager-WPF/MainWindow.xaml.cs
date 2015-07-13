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

            foreach (Plant plant in plants)
            {
                string customizeName = string.Empty;
                if (plant.Genus.Id != Genus.GetDefaultGenus().Id)
                    customizeName += plant.Genus.Name + " ";
                lstPlants.Items.Add(plant);
            }
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
    }
}
