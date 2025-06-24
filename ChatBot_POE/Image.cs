using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChatBot_POE
{
     class Image
    {
        public string Show()
        {
            string imagePath = "C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\prog6221-poe-Mokholo-Enhle-Imbali\\PROG6221_POE\\files\\Chatbot Image.png"; // Change to your image path
            int width = 30;
            int height = 15;

           return ConvertImageToAscii(imagePath, width, height);  
         
        }


        private string ConvertImageToAscii(string imagePath, int newWidth, int newHeight)
        {
            Bitmap image = new Bitmap(imagePath);
            image = new Bitmap(image, new Size(newWidth, newHeight));

            // ASCII characters ordered by brightness
            StringBuilder art = new StringBuilder();
            string asciiChars = "@%#*+=-:. ";

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int grayValue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3; // Convert to grayscale
                    int index = grayValue * (asciiChars.Length - 1) / 255; // Map to ASCII range
                    art.Append(asciiChars[index]);
                }
                art.AppendLine();
            }

            return art.ToString();
        }
    }
}
