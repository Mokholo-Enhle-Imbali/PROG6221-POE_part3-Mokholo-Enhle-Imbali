using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot_POE
{
     class LibraryLong
    {
        public List<ChatBotDataLong> dataLong = new List<ChatBotDataLong>();

        public StreamReader passwordSReader, phishingReader, safeBrowsingReader;
        public string passwordSafety, phishing, safeBrowsing;
        
        public void LoadData()
        {

            
            try //for password safety
            {
                passwordSReader = new StreamReader("C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\PasswordSafetyFull.txt");
                passwordSafety = passwordSReader.ReadLine(); //this is the string containing the specifc data

                
                while (passwordSafety != null)
                {
                    dataLong.Add(new ChatBotDataLong
                    {
                        ContentLong = passwordSafety.Trim(),
                        SubjectLong = "explain further",
                        TagsLong = "explain,further,explain further"
                    });

                    passwordSafety = passwordSReader.ReadLine();
                }



                passwordSReader.Close();



            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


            try //for phishing
            {
                phishingReader = new StreamReader("C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\PhishingFull.txt");
                phishing = phishingReader.ReadLine();


                while (phishing != null)
                {
                    dataLong.Add(new ChatBotDataLong
                    {
                        ContentLong = phishing,
                        SubjectLong = "elaborate",
                        TagsLong = "elaborate"
                    });

                    phishing = phishingReader.ReadLine();
                }

                phishingReader.Close();


            }

            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


            try //for safe browsing
            {
                safeBrowsingReader = new StreamReader("C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\SafeBrowsingFull.txt");
                safeBrowsing = safeBrowsingReader.ReadLine();



                while (safeBrowsing != null)
                {

                    dataLong.Add(new ChatBotDataLong
                    {
                        ContentLong = safeBrowsing,
                        SubjectLong = "tell me more",
                        TagsLong = "tell me more"
                    });

                    safeBrowsing = safeBrowsingReader.ReadLine();
                }



                safeBrowsingReader.Close();

            }

            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


          
        }



    }
}
