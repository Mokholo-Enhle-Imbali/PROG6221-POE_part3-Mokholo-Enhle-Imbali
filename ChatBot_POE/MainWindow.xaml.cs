using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Media3D;

namespace ChatBot_POE
{
    public partial class MainWindow : Window
    {
        private Library lib; // Content for short questions
        private LibraryLong libraryLong; // Content for longer explanations
        private SentimentLibrary sentimentLibrary; // Sentiment detection
        private string userName;
        private string filePathPreferences = "C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\UserPreferences.txt";
        private string filePathTask = "C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\Tasks.txt";
        Image image = new Image();
        AudioPlayer audioPlayer = new AudioPlayer();
        private bool waitingForQuestion = false;
        private bool waitingForResponse = false;
        private bool waitingForConcern = false;
        private bool waitingForConcernResponse = false;
        bool waitForTopic = false;
        bool waitForMoreTopic=false;
        private bool waitForTaskTitle = false;
        private bool waitForTaskDescription = false;
        private bool waitForTaskReminder = false;
        private bool waitForTaskReminderTime = false;
        private string taskTitle = "";
        private string taskDescription = "";
        private string taskTime = "";
        private bool waitForMoreTasks = false;
        public MainWindow()
        {
            InitializeComponent();
            audioPlayer.Play();
            chatbotoutput.Text = image.Show();
            InitializeChatbot();
        }

        private void InitializeChatbot()
        {
            lib = new Library();
            lib.LoadData();

            libraryLong = new LibraryLong();
            libraryLong.LoadData();

            sentimentLibrary = new SentimentLibrary();
            sentimentLibrary.Responses();

            AppendToChat("Welcome to CyberBot! Your one stop bot for all things cybersecurity! Before we get started, could you please tell me your name?");
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            string userInput = txtchat.Text.Trim();
            txtchat.Clear();

            if (string.IsNullOrEmpty(userInput))
                return;

            AppendToChat($"{userInput}");

            if (waitingForQuestion==true)
            {
                ProcessQuestion(userInput);
                return;
            }

            if (waitingForResponse==true)
            {
                ProcessResponse(userInput);
                return;
            }

            if (waitingForConcern == true)
            {
                ProcessConcern(userInput);
                return;
            }

            if (waitingForConcernResponse == true)
            {
                ProcessConcernResponse(userInput);
                return;
            }

            if (waitForTopic == true)
            {
                ProcessTopic(userInput);
                return;
            }

            if (waitForMoreTopic == true) 
            {
                ProcessMoreTopic(userInput);
                return;
            }

            if (waitForTaskTitle || waitForTaskDescription || waitForTaskReminder || waitForTaskReminderTime)
            {
                ProcessTask(userInput);
                return;
            }

            if (waitForMoreTasks==true)
            {
                ProcessMoreTasks(userInput);
                return;
            }

            if (string.IsNullOrEmpty(userName))
            {
                HandleUserNameInput(userInput);
            }
            else
            {
                HandleUserInput(userInput.ToLower());
            }
        }

        private void HandleUserNameInput(string input)
        {
            userName = input;
            AppendToChat($"Hello {userName} I am chatbot!");
            ShowMainOptions();
        }

        private void ShowMainOptions()
        {
            AppendToChat($"What would you like to do today {userName}?\n");
        }

        private void ContinueQuestion()
        {
            AppendToChat($"What else would you like to talk about {userName}?\n");
        }

        private void HandleUserInput(string userInput)
        {


            if (Regex.IsMatch(userInput, @"\b(ask|question|query)\b", RegexOptions.IgnoreCase))
            {
                HandleQuestion();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(concerned|worried|stressed|anxious|uneasy)\b", RegexOptions.IgnoreCase))
            {
                HandleConcern();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(topic|subject)\b", RegexOptions.IgnoreCase))
            {
                HandleTopic();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(open|pod|bay|door)\b", RegexOptions.IgnoreCase))
            {
                AppendToChat("I'm afraid I cannot do that hal...");
                ContinueQuestion();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(what|funcionality|purpose)\b", RegexOptions.IgnoreCase))
            {
                AppendToChat("My purpose is to answer any questions you have about cybersecurity. " +
                           "For now, you can ask me about phishing, password safety, and safe browsing.");
                ContinueQuestion();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(task|job|chore)\b", RegexOptions.IgnoreCase))
            {
                HandleTask();
                return;
            }

            if(Regex.IsMatch(userInput, @"\b(show my tasks)\b", RegexOptions.IgnoreCase))
            {
                AppendToChat("Here is a list of all your tasks");
                string fileContent = File.ReadAllText(filePathTask);
                AppendToChat($"{fileContent}");
                ContinueQuestion();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(show my preferences|show my user preferences)\b", RegexOptions.IgnoreCase))
            {
                AppendToChat("Here is a list of all your preferences");
                string fileContent = File.ReadAllText(filePathPreferences);
                AppendToChat($"{fileContent}");
                ContinueQuestion();
                return;
            }



            if (Regex.IsMatch(userInput, @"\b(exit|goodbye|leaving|bye)\b", RegexOptions.IgnoreCase))
            {
                AppendToChat($"Goodbye {userName}!");
                Close();
                return;
            }


            else
            {
                AppendToChat("can you repeat that? i dont think i understood");
            }

        }
        

        //questions about cybersecurity
        private void HandleQuestion()
        {
            waitingForQuestion = true;
            AppendToChat("What question would you like to ask me?");
        }
         
       

        private void ProcessQuestion(string userInput)
        {
            waitingForQuestion = false;
            AppendToChat($"{userName}: {userInput}");

            bool contentFound = false;
            var questionArray = userInput.ToLower().Split(' ');

            // Check short questions
            var matchingRecords = lib.data.Where(x => questionArray.Contains(x.Subject.ToLower()) || questionArray.Intersect(x.Tags.ToLower().Split(',')).Count() > 0).OrderBy(x => Guid.NewGuid());

            // Check for remembered preferences
            if (File.Exists(filePathPreferences))
            {
                using (StreamReader readFile = new StreamReader(filePathPreferences))
                {
                    string read = readFile.ReadLine();
                    if (!string.IsNullOrEmpty(read))
                    {
                        string[] safeBrowsingOptions = read.Split(',');
                        foreach (var record in matchingRecords)
                        {
                            if (safeBrowsingOptions.Any(item =>
                                record.Tags.ToLower().Contains(item.ToLower()) ||
                                record.Subject.ToLower().Contains(item.ToLower())))
                            {
                                AppendToChat("I remember you said you like this topic!");
                                break;
                            }
                        }
                    }
                }
            }

            // Display matching short answers
            foreach (var record in matchingRecords)
            {
                AppendToChat($"{record.Content}");
                contentFound = true;
                break;
            }

            // Check long questions
            var matchingLongRecords = libraryLong.dataLong.Where(x =>
                questionArray.Contains(x.SubjectLong.ToLower()) ||
                questionArray.Intersect(x.TagsLong.ToLower().Split(',')).Any());

            foreach (var record in matchingLongRecords)
            {
                AppendToChat($"{record.ContentLong}");
                contentFound = true;
            }

            if (!contentFound)
            {
                AppendToChat("Sorry, I couldn't find anything related to your question.");
            }

            AskForMoreQuestions();
        }

        private void AskForMoreQuestions()
        {
            waitingForResponse = true;
            AppendToChat($"\nDo you have any other questions?");
        }

        private void ProcessResponse(string response)
        {
            waitingForResponse = false;
            response = response.ToLower();

            if (Regex.IsMatch(response, @"\b(nah|nope|no|negative)\b", RegexOptions.IgnoreCase))
            {
                ContinueQuestion();
            }
            else if (Regex.IsMatch(response, @"\b(yep|yea|affirmative|aye|yes)\b", RegexOptions.IgnoreCase))
            {
                HandleQuestion();
            }
            else
            {
                AppendToChat("I didn't understand that");
                AskForMoreQuestions();
            }
        }




        //sentiment feature
        private void HandleConcern()
        {
            waitingForConcern = true;
            AppendToChat("What specifically are you worried about?");
        }

        private void ProcessConcern(string userInput)
        {
            waitingForConcern = false;
            AppendToChat($"{userInput}");

            bool content = false;
            var sentimentData = sentimentLibrary.DataSentiment;
            var sentimentArray = userInput.ToLower().Split(' ');
            var records = sentimentData.Where(x => sentimentArray.Contains(x.sentimentSubject.ToLower()) || sentimentArray.Intersect(x.sentimentTags.ToLower().Split(',')).Count() > 0);
            foreach (var recordsentiment in records)
            {
                if (recordsentiment != null)
                {

                    AppendToChat($"{recordsentiment.sentimentContent}");
                    content = true;
                   
                }
            }
            if (!content)
            {
                AppendToChat("sorry, I couldn't find anything related to your question.");
            }

            AskForMoreConcerns();

        }

        private void AskForMoreConcerns()
        {
            waitingForConcernResponse = true;
            AppendToChat($"\nDo you have any other concerns?");
            
        }

        private void ProcessConcernResponse(string response)
        {
            waitingForConcernResponse = false;
            response = response.ToLower();

            if (Regex.IsMatch(response, @"\b(nah|nope|no|negative)\b", RegexOptions.IgnoreCase))
            {   
                ContinueQuestion();
            }
            else if (Regex.IsMatch(response, @"\b(yep|yea|affirmative|aye|yes)\b", RegexOptions.IgnoreCase))
            {
                HandleConcern();
            }
            else
            {
                AppendToChat("I didn't understand that.");
                AskForMoreConcerns();
            }
        }






        //remember topic i like
        private void HandleTopic()
        {
            waitForTopic=true;
            AppendToChat("What topic do you want me to remember?");
        }

        private void ProcessTopic(string topic)
        {
            waitForTopic = false;
            File.AppendAllText(filePathPreferences, $"{topic},");
            AppendToChat($"Thanks for letting me know {userName}! I'll remember that for next time");
            AskForMoreTopics();
        }

        private void AskForMoreTopics()
        {
            waitForMoreTopic=true;
            AppendToChat("Do you have any more topics i should remember?");
        }

        private void ProcessMoreTopic(string response)
        {
            waitForMoreTopic = false;
            response = response.ToLower();

            if (Regex.IsMatch(response, @"\b(nah|nope|no|negative)\b", RegexOptions.IgnoreCase))
            {
                ContinueQuestion();
            }
            else if (Regex.IsMatch(response, @"\b(yep|yea|affirmative|aye|yes)\b", RegexOptions.IgnoreCase))
            {
                HandleConcern();
            }
            else
            {
                AppendToChat("I didn't understand that. Please answer with 'yes' or 'no'.");
                AskForMoreTopics();
            }
        }







        // task creation
        private void HandleTask()
        {
            waitForTaskTitle=true;
            AppendToChat("what is the title of the task?");
        }


        private void ProcessTask(string userInput)
        {

            

            if (waitForTaskTitle==true)
            {
                
                taskTitle = userInput;
                waitForTaskTitle = false;
                waitForTaskDescription = true;
                AppendToChat("What is the description of the task?");
                return;
            }
            if (waitForTaskDescription==true)
            {
                taskDescription = userInput;
                waitForTaskDescription = false;
                waitForTaskReminder = true;  
                AppendToChat("Would you like a reminder?");
                return;
            }
            if (waitForTaskReminder==true)
            {
                if (Regex.IsMatch(userInput, @"\b(yep|yea|affirmative|aye|yes)\b", RegexOptions.IgnoreCase))
                {
                    waitForTaskReminder = false;   
                    waitForTaskReminderTime = true;
                    AppendToChat("when should i remind you?");
                    return;
     
                }
                else if (Regex.IsMatch(userInput, @"\b(nah|nope|no|negative)\b", RegexOptions.IgnoreCase))
                {
                    File.AppendAllText(filePathTask, $"Title: {taskTitle}\nDescription: {taskDescription}\nReminder: No\n\n");
                    waitForTaskReminder = false;
                    AppendToChat("Task has been set without a time");
                    ResetTaskState();
                    AskForMoreTasks();
                    return;
                }    
            }

            if (waitForTaskReminderTime == true)
            {
                taskTime = userInput;
                File.AppendAllText(filePathTask, $"Title: {taskTitle}\nDescription: {taskDescription}\nReminder: yes \nReminder Time:{taskTime}\n\n");
                waitForTaskReminder = false;
                AppendToChat($"Task has been set with a time: {taskTime}");
                ResetTaskState();
                AskForMoreTasks();
                return;
            }

            
        }

        private void AskForMoreTasks()
        {
            waitForMoreTasks = true;
            AppendToChat("Do you have any more tasks you want to save?");
        }

        private void ProcessMoreTasks(string response)
        {
            waitForMoreTasks = false;
            response = response.ToLower();

            if (Regex.IsMatch(response, @"\b(nah|nope|no|negative)\b", RegexOptions.IgnoreCase))
            {
                ContinueQuestion();
            }
            else if (Regex.IsMatch(response, @"\b(yep|yea|affirmative|aye|yes)\b", RegexOptions.IgnoreCase))
            {
                HandleTask();
            }
            else
            {
                AppendToChat("I didn't understand that.");
                AskForMoreTasks();
            }
        }

        private void ResetTaskState()
        {
            waitForTaskTitle = false;
            waitForTaskDescription = false;
            waitForTaskReminder = false;
            waitForTaskReminderTime = false;
            taskTitle = "";
            taskDescription = "";
            taskTime = "";
        }


        //used to display to the textbox

        private void AppendToChat(string text)
        {
            chatbotoutput.Text += text + "\n\n";
        }
    }
}