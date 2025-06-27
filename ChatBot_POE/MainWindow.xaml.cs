using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Media3D;
using static System.Net.Mime.MediaTypeNames;

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
        private string filePathLogger = "C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\ChatLogs.txt";
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
        private bool waitForQuiz=false;
        int answerCounter = 0;
        int currentQuestionIndex = 0;

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


            AppendToUser(userName,userInput);

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

            if (waitForQuiz == true)
            {
                ProcessQuiz(userInput);
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
                Logger(userInput, "user wanted chatbot to answer questions about cybersecurity");
                HandleQuestion();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(concerned|worried|stressed|anxious|uneasy)\b", RegexOptions.IgnoreCase))
            {
                Logger(userInput, "user wanted chatbot to ease worries/concerns about a topic in cybersecurity");
                HandleConcern();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(topic|subject)\b", RegexOptions.IgnoreCase))
            {
                Logger(userInput,"User asked chatbot to save a topic that they like");
                HandleTopic();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(open|pod|bay|door)\b", RegexOptions.IgnoreCase))
            {
                Logger(userInput, "user waned chatbot to tell a funny joke");
                AppendToChat("I'm afraid I cannot do that hal...");
                ContinueQuestion();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(what|funcionality|purpose)\b", RegexOptions.IgnoreCase))
            {
                Logger(userInput, "user asked chatbot about what it can do");
                AppendToChat("My purpose is to answer any questions you have about cybersecurity. " +
                           "For now, you can ask me about phishing, password safety, and safe browsing.");
                ContinueQuestion();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(task|job|chore)\b", RegexOptions.IgnoreCase))
            {
                Logger(userInput, "user wanted to create a task for the user");
                HandleTask();
                return;
            }

            if(Regex.IsMatch(userInput, @"\b(show my tasks|show a list of tasks|list tasks|show tasks)\b", RegexOptions.IgnoreCase))
            {
                Logger(userInput, "user wanted chatbot to show a list of all the tasks they need to do");
                AppendToChat("Here is a list of all your tasks");
                string fileContent = File.ReadAllText(filePathTask);
                AppendToChat($"{fileContent}");
                ContinueQuestion();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(quiz|quizzes)\b", RegexOptions.IgnoreCase))
            {
                Logger(userInput, "user wanted the chatbot to quiz them on a cybersecurity topics");
                HandleQuiz();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(preferences| user preferences)\b", RegexOptions.IgnoreCase))
            {
                Logger(userInput,"user wanted chatbot to list all their topics they like");
                AppendToChat("Here is a list of all your preferences");
                string fileContent = File.ReadAllText(filePathPreferences);
                AppendToChat($"{fileContent}");
                ContinueQuestion();
                return;
            }

            if (Regex.IsMatch(userInput, @"\b(logs|previous chats)\b", RegexOptions.IgnoreCase))
            {
                ShowLogs();
                ContinueQuestion();
                return;
            }


            if (Regex.IsMatch(userInput, @"\b(exit|goodbye|leaving|bye|nothing)\b", RegexOptions.IgnoreCase))
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
                HandleTopic();
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





        //quiz creation
        private void HandleQuiz()
        {
            
            AppendToChat("Ok lets go!");
            AppendToChat("First question:\n how can one make their password safer?\nA: adding more characters (symbols and numbers) \nB: expose the password to a random person \nC: re use paswords");
            currentQuestionIndex = 0;
            answerCounter = 0;
            waitForQuiz = true;
        }


        private void ProcessQuiz(string userInput)
        {

            string answer = userInput.ToLower();
            switch (currentQuestionIndex)
            {
                case 0:
                    switch (answer)
                    {
                        case "a":
                            AppendToChat("Thats correct! Many attackers cycle through generic passwords. By adding symbols and numbers to your password, it makes the password more complex.");
                            answerCounter++;
                            break;

                        case "b":
                        case "c":
                            AppendToChat($"I afraid that is incorrect. the right answer is A. \nBy adding symbols and numbers to your password, it makes the password more complex.");
                            break;

                        default:
                            AppendToChat("Please answer the question");
                            return;
                    }
                    AppendToChat("Second question: \nhow can one browse the web safely? \nA: click on an http//: website \nB: use a public wifi \nC: click on websites with https//: at the start");
                    currentQuestionIndex++;
                    break;

                case 1:


                    switch (answer)
                    {
                        case "a":
                        case "b":
                            AppendToChat($"I afraid that is incorrect. the right answer is C. clicking on websites that have https at the start are more safer than the ones with http only");
                            break;
                        case "c":
                            AppendToChat("Thats correct! clicking on websites that have https at the start are more safer than the ones with http only.");
                            answerCounter++;
                            break;

                        default:
                            AppendToChat("Please answer the question");
                            return;
                    }
                    AppendToChat("Third Question: \nWhat is a great way to identify a phishing scam? \nA: clicking on the links from unsolicited emails \nB: checking the email address of the sender \nC: Believing what the email says");
                    currentQuestionIndex++;
                    break;

                    case 2:

                    switch (answer)
                    {
                        case "a":
                        case "c":
                            AppendToChat($"I afraid that is incorrect. the right answer is B. always make sure to see who is sending an email to you");
                            break;
                            case "b":
                            AppendToChat("thats correct! always make sure to see who is sending an email to you");
                            answerCounter++;
                            break;

                        default:
                            AppendToChat("Please answer the question");
                            return;
                    }
                    AppendToChat("Fourth Question: \nTrue or false:  you should always share your password with anyone who asks");
                    currentQuestionIndex++;
                    break;

                    case 3:
                    switch (answer)
                    {
                        case "true":
                            AppendToChat("I'm afraid that is incorect. you should never share your password with anyone, especially online. only people you know");
                            break;
                        case "false":
                            AppendToChat("Thats correct! you should never share your password with anyone, especially online. only people you know");
                            answerCounter++;
                            break;

                        default:
                            AppendToChat("Please answer the question");
                            return;
                    }
                    AppendToChat("Fifth Question: A random number calls you claiming it is your bank and asks for your card information. Do you give it to them? (answer yes/no)");
                    currentQuestionIndex++;
                    break;

                    case 4:

                    switch (answer)
                    {
                        case "yes":
                            AppendToChat("I'm affraid that is incorrect. you must never share any details with random numbers. remember, the bank would never call you for personal information");
                            break;

                            case "no":
                            AppendToChat("that is correct. you must never share any details with random numbers. remember, the bank would never call you for personal information");
                            answerCounter++;
                            break;

                        default:
                            AppendToChat("Please answer the question");
                            return;
                    }
                    AppendToChat("Sixth Question: True or false. The bank will call you about suspicious activity on your card");
                    currentQuestionIndex++;
                    break;

                    case 5:

                    switch (answer)
                    {
                        case "true":
                            AppendToChat("Correct! If the bank notices any strange purchases or even an exessive amount of money being spent, then they will contact you about it");
                            answerCounter++;
                            break;

                            case "false":
                            AppendToChat("Sorry, thats not correct. If the bank notices any strange purchases or even an exessive amount of money being spent, then they will contact you about it");
                            break;

                        default:
                            AppendToChat("Please answer the question");
                            return;
                    }
                    AppendToChat("Seventh Question: what is another way to ensure your safety even after someone steals your password? \nA: use the same username and password each time \nB: enable two-factor authentication \nC: give up and let them acces your personal info");
                    currentQuestionIndex++;
                    break;
                    
                    case 6:

                    switch (answer)
                    {
                        case "a":
                        case "c":
                            AppendToChat("not correct. using 2FA makes it harder for attackers to acess your information.");
                            break;

                            case "b":
                            AppendToChat("Thats right! using 2FA makes it harder for attackers to acess your information.");
                            answerCounter++;
                            break;

                        default:
                            AppendToChat("Please answer the question");
                            return;
                    }

                    AppendToChat("Eighth Question: what is the purpose of antivirus software? \nA: it is used to protect against malicious software on your device \nB: cure to a bad case of the cold \nC: a waste of space");
                    currentQuestionIndex++;
                    break;

                    case 7:

                    switch (answer)
                    {
                        case "a":
                            AppendToChat("yea! you got it! an antivirus software is used to protect you from malicious software and purge it if there is already malicious software on your device");
                            answerCounter++;
                            break;

                            case "b":
                            case "c":
                            AppendToChat("Not quite! an antivirus software is used to protect you from malicious software and purge it if there is already malicious software on your device");
                            break;

                        default:
                            AppendToChat("Please answer the question");
                            return;
                    }

                    AppendToChat("Ninth Question: why is public wifi bad? \nA: cost to much \nB: there is barely any security, making it vulnerable. \nC: drains your battery ");
                    currentQuestionIndex++;
                    break;

                case 8:

                    switch (answer)
                    {
                        case "a":
                        case "c":
                            AppendToChat("im afraid that is incorrect. public Wi-Fis are known to be quite vulnerable, making it easy for attackers to ");
                            break;

                            case "b":
                            AppendToChat("Thats right! public Wi-Fis are known to be quite vulnerable, making it easy for attackers to");
                            answerCounter++;
                            break;
                    }

                    AppendToChat("Final Question: How can you securely store a password? \nA: on a sticky note in the office \nB: on a random text file on your desktop \nC: with a password manager, such as google password manager");
                    currentQuestionIndex++;
                    break;

                    case 9:

                    switch (answer)
                    {
                        case "a":
                        case "b":
                            AppendToChat("Nope! password managers (like the google one mentioned) are known for being secure, as they encrypt your passwords using an encryption standards such as AES-256");
                            break;

                            case "c":
                            AppendToChat("Thats right! password managers (like the google one mentioned) are known for being secure, as they encrypt your passwords using an encryption standards such as AES-256");
                            answerCounter++;
                            break;
                    }

                    currentQuestionIndex++;
                    waitForQuiz = false;
                    AppendToChat($"Your total score is {answerCounter}/10");

                    if (answerCounter < 5)
                    {
                        AppendToChat("Don't worry you'll get it next time!");
                    }
                    else
                    {
                        AppendToChat("Great job, you are truly a cybersecurity expert");
                    }

                    ContinueQuestion();
                    break;
            }

           



        }


        //used to display to the textbox

        private void AppendToChat(string text)
        {
            chatbotoutput.Text += "Chat: "+text + "\n\n";
        }

        private void AppendToUser(string userName, string userInput)
        {
            chatbotoutput.Text += userName +": " +userInput+ "\n\n";
        }

        private void Logger(string userInput, string summary)
        {
            string logger = $"user asked: {userInput}\nSummary: {summary}\nTime Asked:{DateTime.Now}\n\n";
            File.AppendAllText(filePathLogger, logger);
        }

        private void ShowLogs()
        {
            AppendToChat("Heres a log of all the chats: \n");
            string fileContent = File.ReadAllText(filePathLogger);
            AppendToChat($"{fileContent}");
        }


    }
}