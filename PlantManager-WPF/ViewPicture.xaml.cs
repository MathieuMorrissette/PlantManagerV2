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
    /// Interaction logic for ViewPicture.xaml
    /// </summary>
    public partial class ViewPicture : MetroWindow
    {
        public ViewPicture(string name, ImageSource picture)
        {
            InitializeComponent();
            this.Title = name;
            image.Source = picture;

            RenderOptions.SetBitmapScalingMode(this.image, BitmapScalingMode.HighQuality);
        }
    }
}
