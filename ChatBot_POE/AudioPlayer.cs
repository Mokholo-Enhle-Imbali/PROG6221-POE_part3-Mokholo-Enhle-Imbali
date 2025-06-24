using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot_POE
{
     class AudioPlayer
    {
        public void Play()
        {
            string filePath = "C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\prog6221-poe-Mokholo-Enhle-Imbali\\PROG6221_POE\\files\\Chatbot.wav";

            using (SoundPlayer player = new SoundPlayer(filePath))
            {
                player.Play();
            }
        }

    }
}
