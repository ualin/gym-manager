using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Gym_Manager
{
    //Design by Pongsakorn Poosankam
    class Helper
    {

        public static void SaveImageCapture(System.Drawing.Image image, string imageName)
        {
            //SaveFileDialog s = new SaveFileDialog();
            //s.FileName = imageName;// Default file name
            //s.DefaultExt = ".Jpg";// Default file extension
            //s.Filter = "Image (.jpg)|*.jpg"; // Filter files by extension
            //s.OverwritePrompt = false;

            // Show save file dialog box
            // Process save file dialog box results
            //if (s.ShowDialog() == DialogResult.OK)
            //{
                // Save Image
                //string filename = s.FileName;
                //FileStream fstream = new FileStream("pics/"+filename, FileMode.Create);
                image.Save("pics/" + imageName+".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //fstream.Close();
            //}
           

            //SaveFileDialog s = new SaveFileDialog();
            //s.FileName = "Image";// Default file name
            //s.DefaultExt = ".Jpg";// Default file extension
            //s.Filter = "Image (.jpg)|*.jpg"; // Filter files by extension

            //// Show save file dialog box
            //// Process save file dialog box results
            //if (s.ShowDialog()==DialogResult.OK)
            //{
            //    // Save Image
            //    string filename = s.FileName;
            //    FileStream fstream = new FileStream(filename, FileMode.Create);
            //    image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    fstream.Close();

            //}


        }
    }
}
