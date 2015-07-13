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

            //LoadHardinessZonesCombo();

            //LoadSunLevelsCombo();

            //LoadShapesCombo();

            //LoadPlantTypesCombo();

            //LoadSoilTypesCombo();

            //Desactiver le bouton pour sauvegarder les changements.
            butSaveChanges.IsEnabled = false;

            //Cacher le TabControl.
            tcPlant.Visibility = Visibility.Hidden;
        }

        private void LoadGenusCombo()
        {
            Genus[] genuses = Genus.GetAllGenus();

            cbGenus.ItemsSource = genuses;

            if (_mCurrentPlant != null)
                cbGenus.SelectedValue = _mCurrentPlant.Genus.Id;
        }

        private void btAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            if (_mCurrentPlant == null) return;

            ImagePlant.AddImage(_mCurrentPlant.Id, dlg.FileName);

            ShowPlantImage();

             // CheckControlsStates();
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
            //pbImage.Image = null;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { txtDescription.Text = string.Empty; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { lblName.Content = string.Empty; }, null);

            //udHeight.Value = 0;
            //udWidth.Value = 0;
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { lblName.Content = _mCurrentPlant.Name; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { txtCultivar.Text = _mCurrentPlant.Cultivar; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { txtSpecies.Text = _mCurrentPlant.Species; }, null);

            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { txtDescription.Text = _mCurrentPlant.Description; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { cbGenus.SelectedValue = _mCurrentPlant.Genus.Id; }, null);

            //cbHardinessZones.SelectedValue = _mCurrentPlant.HardZone.Id;
            //cbSunLevels.SelectedValue = _mCurrentPlant.SunLvl.Id;
            //cbShapes.SelectedValue = _mCurrentPlant.Shape.Id;
            //cbPlantTypes.SelectedValue = _mCurrentPlant.PlantType.Id;
            //cbSoilTypes.SelectedValue = _mCurrentPlant.SoilType.Id;

            //udHeight.Value = _mCurrentPlant.Height;
            //udWidth.Value = _mCurrentPlant.Width;

            ShowPlantImage();

            // this.CheckControlsStates();
            Thread.Sleep(100);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { this.pbLoading.IsIndeterminate = false ; }, null);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate { tcPlant.Visibility = Visibility.Visible; }, null);

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
