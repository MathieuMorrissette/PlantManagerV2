using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PlantManager_WPF
{
    public class ImagePlant
    {
        public ImagePlant(int imageId, int plantId, BitmapImage image)
        {
            ImageId = imageId;
            PlantId = plantId;
            Image = image;
        }

        public int ImageId { get; private set; }
        public int PlantId { get; private set; }
        public BitmapImage Image { get; private set; }

        public static ImagePlant[] GetImagesByPlantId(int id)
        {
            DataTable images = Db.Query("SELECT * FROM Images WHERE ImagePlantID = ?", id.ToString());

            if (images == null)
                return null;

            List<ImagePlant> lstImages = new List<ImagePlant>();
            foreach (DataRow image in images.Rows)
            {
                lstImages.Add
                    (
                    new ImagePlant(Convert.ToInt32(image["ImageID"]), Convert.ToInt32(image["ImagePlantID"]),
                                    byteArrayToImage(Convert.FromBase64String((string)image["ImageData"])))
                );
            }

            return lstImages.ToArray();
        }

        public static byte[] imageToByteArray(BitmapImage imageSource)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            encoder.Save(memStream);
            return memStream.GetBuffer();
        }

        public static BitmapImage byteArrayToImage(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public static void AddImage(int plantId, string imagePath)
        {
            try
            {
                BitmapImage img = new BitmapImage(new Uri(imagePath));
                byte[] imageData = imageToByteArray(img);
                Db.Execute("INSERT INTO Images (ImagePlantID, ImageData) VALUES (?,?)", plantId.ToString(), Convert.ToBase64String(imageData));
            }
            catch (Exception ex)
            {
                //LogManager.LogSilentError()
            }
        }

        public static void DeleteImageByPlantId(int plantId)
        {
            Db.Execute("DELETE FROM Images WHERE ImagePlantID = ?", plantId.ToString());
        }
    }
}