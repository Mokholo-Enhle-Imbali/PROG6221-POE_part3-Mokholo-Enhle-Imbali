using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatBot_POE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string customMemoryPath = "C:\\Users\\enhle\\OneDrive\\Desktop\\GitHub\\PROG6221-POE_part3-Mokholo-Enhle-Imbali\\ChatBot_POE";
        public MainWindow()
        {
            InitializeComponent();
            AudioPlayer player = new AudioPlayer();
            player.Play();
            Image image = new Image();
            string art = image.Show();
            string welcome = "Chat: Welcome to CyberBot! your one stop bot for all things cybersecurity! Before we get started, could you please tell me your name?";

            chatbotoutput.Text = $"{art}\n{welcome}";
        }

        private string userName;
        private bool confirmName = false;
        private bool confirmGreeting = false;
        private bool confirmAskingQuestion = false;
        private bool anyMoreQuestions = false;
        private bool confirmMemory = false;
        private void send_Click(object sender, RoutedEventArgs e)
        {
            Library lib = new Library(); //content for short questions
            lib.LoadData(); //loading the data

            string input = txtchat.Text.Trim();
            txtchat.Clear();



            if (string.IsNullOrEmpty(input))
            {
                chatbotoutput.Text += "\nPlease enter your name.";
                return;
            }

            if (!confirmName)
            {
                userName = input;
                chatbotoutput.Text += $"\nWelcome {userName}, it is nice to meet you!\n\nPlease say 'hello' to continue.";
                confirmName = true;
                return;
            }

            if (!confirmGreeting)
            {
                if (input.Equals("hello", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("hi", StringComparison.OrdinalIgnoreCase))
                {
                    chatbotoutput.Text += "\n\nWhat would you like to do today? \n- Ask a question \n- Exit \n- remember a topic i like";
                    confirmGreeting = true;
                }
                else
                {
                    chatbotoutput.Text += "\nPlease say 'hello' first!";
                }
                return;
            }

            if (input.Equals("ask a question", StringComparison.OrdinalIgnoreCase))
            {
                chatbotoutput.Text += "\n\nChat: What's your question about cybersecurity?";
                confirmAskingQuestion = true;
                return;
            }
            else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Close();
                return;
            }

            if (input.Equals("remember a topic i like", StringComparison.OrdinalIgnoreCase))
            {
                chatbotoutput.Text += "\n\nWhat would you like me to remember?";
                confirmMemory = true;
                return;
            }



            if (confirmAskingQuestion)
            {
                var questionArray = input.ToLower().Split(' ');
                var matchingRecords = lib.data
                    .Where(x => questionArray.Contains(x.Subject.ToLower()) ||
                                 questionArray.Intersect(x.Tags.ToLower().Split(',')).Any())
                    .OrderBy(x => Guid.NewGuid())
                    .ToList();

                if (matchingRecords.Any())
                {
                    chatbotoutput.Text += $"\n\n{matchingRecords.First().Content}";
                }
                else
                {
                    chatbotoutput.Text += "\n\nI don't have information on that topic. Try asking about something else.";
                }

                chatbotoutput.Text += "\n\nDo you have any other questions? (yes/no)";
                anyMoreQuestions = true;
                confirmAskingQuestion = false;
                return;
            }

            if (anyMoreQuestions)
            {
                if (input.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    chatbotoutput.Text += "\n\nWhat is your next question?";
                    confirmAskingQuestion = true;
                    anyMoreQuestions = false;
                }
                else if (input.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    chatbotoutput.Text += "\n\nOkay, let me know if you need anything else!\n\nWhat would you like to do today? \n- Ask a question \n- Exit \n- remember a topic i like";
                    anyMoreQuestions = false;
                    confirmGreeting = true; // Return to main menu
                }
                else
                {
                    chatbotoutput.Text += "\n\nPlease answer 'yes' or 'no'.";
                }
                return;
            }

            if (confirmMemory)
            {
                if (confirmMemory)
                {
                    try
                    {
                        // Get the directory path
                        string directory = System.IO.Path.GetDirectoryName(customMemoryPath);

                        // Create directory if it doesn't exist
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Write the file
                        File.WriteAllText(customMemoryPath, input);

                        // Verify
                        if (File.Exists(customMemoryPath))
                        {
                            string savedText = File.ReadAllText(customMemoryPath);
                            chatbotoutput.Text += $"\n\nI'll remember: {savedText}";

                            // Show confirmation (optional)
                            MessageBox.Show($"Successfully saved to:\n{customMemoryPath}");
                        }
                        else
                        {
                            chatbotoutput.Text += "\n\nError: File wasn't created!";
                        }
                    }
                    catch (Exception ex)
                    {
                        chatbotoutput.Text += $"\n\nSave failed: {ex.Message}";

                        // Fallback to AppData if needed
                        string fallbackPath = System.IO.Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            "ChatBot_POE",
                            "memory.txt");

                        File.WriteAllText(fallbackPath, input);
                        chatbotoutput.Text += $"\n(Saved to fallback location instead)";
                    }

                    confirmMemory = false;
                    return;
                }


                chatbotoutput.Text += "\n\nI didn't understand that. Please choose one of the options:\n- Ask a question \n- Exit \n- remeber a topic i like";
            }
        }

        private string LoadRememberedTopic()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = System.IO.Path.Combine(appDataPath, "ChatBot_POE", "memory.txt");

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return null;
        }
    }
}