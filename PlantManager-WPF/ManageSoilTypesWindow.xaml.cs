﻿using System;
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
    public partial class ManageSoilTypesWindow : MetroWindow
    {
        public ManageSoilTypesWindow()
        {
            InitializeComponent();

            butDelete.IsEnabled = false;

            RefreshList();
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
                SoilType.AddSoilType(txtName.Text);

            txtName.Text = string.Empty;

            RefreshList();
        }

        private void RefreshList()
        {
            SoilType[] soilTypes = SoilType.GetAllSoilTypes();

            dtgItems.ItemsSource = soilTypes;
        }

        private void butDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dtgItems.SelectedItems.Count <= 0) return;

            SoilType.DeleteSoilTypeById(((SoilType)dtgItems.SelectedItem).Id);

            RefreshList();
        }

        private void dtgItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            butDelete.IsEnabled = false;

            if (dtgItems.SelectedItems.Count <= 0) return;

            butDelete.IsEnabled = Convert.ToInt32(((SoilType)dtgItems.SelectedItem).Id) != -1;
        }
    }
}
