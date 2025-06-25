# prog6221-poe-Mokholo-Enhle-Imbali (Cyber Security ChatBot)

# Project Description: 
This program is desined to answer any cybersecurity questions that the user may have. For now, the user can ask about phishing, safe browsing and password safety

# How to use the project: 
First clone the repository onto your local device (Ensure that you have .NET 9.0 installed on your device)

next, build the project using either an IDE, or the windows CMD (Command: dotnet build)

then run the project using either the play button on the IDE or using the windows CMD (command: dotnet run)


# Additional Notes: 
Ensure that all dependencies are installed using either the built in NuGet pakcage install: 
System.Drawing.Common 
System.Windows.Extentions

or using the Command: dotnet restore (this will add all the relevant dependencies needed to run the application) 
(juliakm, 2025)

# Changes Made during Part 2

# Updated menu options: 
when selecting a menu option, instead of choosing a number, the user can type in the words, to make it feel more natural.

# Error Handling changes: 
if the user does not type in one of the options of the menu, questions, or concerns the program will re-prompt the user for an answer.

# Sentimental Update: 
added a feature that allows the user to tell the chatbot about their concerns on a certain topic (password safety, phishing, safe browsing)

# Memory Update: 
The chatbot can now remember topics that the user likes and, when asking a question about that topic, the chatbot will state that it remembers that the user likes the topic

# Activation Update
To start using the chat bot, the user must first enter their name. Once they have entered their name, the chatbot will say: hello [name], to which the user must respond with either of the following responses: 
"hi"
"hello"
"hi chatbot"
"hello chatbot"