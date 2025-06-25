using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot_POE
{
    class Library
    {
        public List<ChatBotData> data = new List<ChatBotData>();

        public StreamReader passwordSReader, phishingReader, safeBrowsingReader, sentimentReader;
        public StreamWriter userPreferencesWriter;
        public string passwordSafety, phishing, safeBrowsing, sentiment, userPreferences;

        public void LoadData()
        {


            try //for password safety
            {
                passwordSReader = new StreamReader("C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\PasswordSafety.txt");
                passwordSafety = passwordSReader.ReadLine(); //this is the string containing the specifc data

                if (passwordSafety != null)
                {
                    string[] passwordSafetyOptions = passwordSafety.Split("---");

                    foreach (string option in passwordSafetyOptions)
                    {
                        data.Add(new ChatBotData
                        {
                            Content = option.Trim(),
                            Subject = "password Safety",
                            Tags = "password safety,safety,password"
                        });

                    }
                }

                passwordSReader.Close();

            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine(e.Message);
            }


            try //for phishing
            {
                phishingReader = new StreamReader("C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\Phishing.txt");
                phishing = phishingReader.ReadLine();


                if (phishing != null)
                {
                    string[] pishingOptions = phishing.Split("---");

                    foreach (string option in pishingOptions)
                    {
                        data.Add(new ChatBotData
                        {
                            Content = option.Trim(),
                            Subject = "pishing",
                            Tags = "phishing,scamming"
                        });

                        //print individually

                    }
                }

                phishingReader.Close();




            }

            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine(e.Message);
            }


            try //for safe browsing
            {
                safeBrowsingReader = new StreamReader("C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\SafeBrowsing.txt");
                safeBrowsing = safeBrowsingReader.ReadLine();


                if (safeBrowsing != null)
                {
                    string[] safeBrowsingOptions = safeBrowsing.Split("---");

                    foreach (string option in safeBrowsingOptions)
                    {
                        data.Add(new ChatBotData
                        {
                            Content = option.Trim(),
                            Subject = "safe browsing",
                            Tags = "safe browsing,safe,browsing"
                        });


                    }
                }


                safeBrowsingReader.Close();

            }

            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine(e.Message);
            }




        }

    }
}

