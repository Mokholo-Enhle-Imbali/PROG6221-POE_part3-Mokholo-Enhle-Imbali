using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot_POE
{
    class SentimentLibrary
    {
        public List<SentimentData> DataSentiment = new List<SentimentData>();
        public StreamReader passwordSafetyWorries, phishingWorries, safeBrowsingWorries;
        public string passwordSafety, phishing, safeBrowsing;
        public void Responses()
        {
            try
            {
                passwordSafetyWorries = new StreamReader("C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\PasswordWorries.txt");
                passwordSafety = passwordSafetyWorries.ReadLine();

                while (passwordSafety!=null)
                {
                    DataSentiment.Add(new SentimentData
                    {
                        sentimentContent = passwordSafety,
                        sentimentSubject="password safety",
                        sentimentTags="password safety,password,safety"

                    });
                    passwordSafety = passwordSafetyWorries.ReadLine();   
                }

                passwordSafetyWorries.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                phishingWorries = new StreamReader("C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\PhishingWorries.txt");
                phishing = phishingWorries.ReadLine();

                while (phishing!=null)
                {
                    DataSentiment.Add(new SentimentData
                    {
                        sentimentContent=phishing,
                        sentimentSubject="phishing",
                        sentimentTags="phishing,scamming"
                    });
                    phishing = phishingWorries.ReadLine();
                }
                phishingWorries.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            try
            {
                safeBrowsingWorries = new StreamReader("C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\SafeBrowsingWorries.txt");
                safeBrowsing = safeBrowsingWorries.ReadLine();

                while (safeBrowsing!=null)
                {
                    DataSentiment.Add(new SentimentData
                    {
                        sentimentContent= safeBrowsing,
                        sentimentSubject="safe browsing",
                        sentimentTags="safe browsing,safe,browsing"
                    });
                    safeBrowsing = safeBrowsingWorries.ReadLine();
                }

                safeBrowsingWorries.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

    }
}
