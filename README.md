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

# Changes during final part

# NLP Update
the chatbot can now naturally handle conversations by picking up on certain keywords within the user's input (Natural Language Processing)

# Log update
the chat bot can now also save what the user is asking, along with a summary of what the question is about and the timestamp

# Quiz update
there is now a feature where the chatbot will quiz you on cybersecurity. you just have to ask it to quiz you

# wpf conversion
the application is now a Windows Presentation Foundation application, making the interation between the user an chatbot smoother.