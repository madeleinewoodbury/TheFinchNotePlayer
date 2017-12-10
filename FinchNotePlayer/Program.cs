using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FinchAPI;
using HidSharp;

namespace FinchNotePlayer
{
    class Program
    {
        /// <summary>
        /// enum type for notes and length of notes
        /// </summary>
        public enum FinchNote
        {
            C4,
            D4,
            E4,
            F4,
            G4,
            A4,
            B4,
            C5,
            D5,
            E5,
            F5,
            G5,
            A5,
            B5,
            C6,
            D6,
            E6,
            F6,
            G6,
            A6,
            B6,
            EIGHT,
            QUARTER,
            HALF,
            WHOLE
        }

        static void Main(string[] args)
        {
            // ***********************************************************
            //
            // Title: The Finch Note Player: Capstone Project CIT 110
            // Description: This is a Finch Application where the user can enter
            //              in a series of notes and the length for each note. 
            //              The user can also insert musical rests and asjust the tempo.
            //              The Finch will play the notes back to the user, and the
            //              list of notes can be saved to a text file. Files with notes
            //              can also be retrieved from within the application.
            // Application Type: Console
            // Author: Madeleine Woodbury
            // Dated Created: 11/26/17
            // Last Modified: 12/10/17
            //
            // ***********************************************************

            DisplayWelcomeScreen();
            DisplayMenu();
        }

        /// <summary>
        /// display menu
        /// </summary>
        static void DisplayMenu()
        {
            //
            // Initialize a new Finch object
            //
            Finch myFinch = new Finch();

            //
            // instantiate list of notes
            //
            List<FinchNote> notes = new List<FinchNote>();

            //
            // local variables
            //
            bool exiting = false;
            string userResponse;
            int menuChoice = 0;
            int quarterNote = 600; //default quarter note in milliseconds = 100bpm

            //
            // The menu will loop until the user chooses to exit
            //
            while (!exiting)
            {
                DisplayHeader("Main Menu");

                Console.WriteLine();
                Console.WriteLine("\t1) Instructions");
                Console.WriteLine("\t2) Add New Notes");
                Console.WriteLine("\t3) Add Notes to Existing List");
                Console.WriteLine("\t4) Adjust Beats per Minute");
                Console.WriteLine("\t5) Display Notes");
                Console.WriteLine("\t6) Play Notes");
                Console.WriteLine("\t7) Save Notes");
                Console.WriteLine("\t8) Retrieve Notes");
                Console.WriteLine("\t9) Display Song Files");
                Console.WriteLine("\t10) Exit");
                Console.WriteLine();
                Console.Write("Enter your menu choice:");
                userResponse = Console.ReadLine();

                //
                // validate user's response and parse to int menuChoice
                //
                while (!int.TryParse(userResponse, out menuChoice))
                {
                    Console.WriteLine("Please enter the corresponding number for your menu choice:");
                    userResponse = Console.ReadLine();
                }

                //
                // the switch statement calls the method based on menuChoice
                //
                switch (menuChoice)
                {
                    case 1:
                        DisplayInstructions(quarterNote);
                        break;

                    case 2:
                        notes = DisplayAddNewNotes();
                        break;

                    case 3:
                        notes = DisplayContinueAddingNotes(notes);
                        break;

                    case 4:
                        quarterNote = GetQuarterNotesPerMinute(quarterNote);
                        break;

                    case 5:
                        DisplayNotes(notes);
                        break;

                    case 6:
                        ProcessFinchNotes(myFinch, notes, quarterNote);
                        break;

                    case 7:
                        DisplaySaveNotes(notes);
                        break;

                    case 8:
                        notes = DisplayRetrieveNotes();
                        break;

                    case 9:
                        DisplaySongFiles();
                        break;

                    case 10:
                        exiting = true;
                        DisplayClosingScreen(myFinch);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// dispaly instructions on how to add notes
        /// </summary>
        private static void DisplayInstructions(int quarterNote)
        {
            //
            // local variables
            //
            int bpm = 1000 / (quarterNote / 60); // calculates beats per minute

            DisplayHeader("Instructions");

            DisplayEnterNotesInstructions();

            Console.WriteLine("\nYou can change the tempo by selecting 'adjust beats per minute' in the menu.");
            Console.WriteLine($"The beats per minute is currently set to {bpm}.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// displays instructions specifir to entering notes
        /// </summary>
        private static void DisplayEnterNotesInstructions()
        {
            Console.WriteLine();
            Console.WriteLine("You will enter the notes you want followed by the length for each note.");
            Console.WriteLine("The notes you can enter range C4 - B6.");
            Console.Write("Type in your desired note: ");
            Console.WriteLine("C - D - E - F - G - A - B");
            Console.Write("Then type in the number to determine if you want a low, middle, or high tone: ");
            Console.WriteLine("4 = low, 5 = middle, 6 = high");

            Console.WriteLine("\nThe lengths you can enter for each note are:");
            Console.WriteLine("1/8 - 1/4 - 1/2 - 1/1.");

            Console.WriteLine("\nHere is an example of how to add a note:");
            Console.WriteLine("Note: G5");
            Console.WriteLine("Length: 1/4");

            Console.WriteLine("\nBy entering REST when you add notes, you can set the rest time to be either: ");
            Console.WriteLine("Whole = 4 beats");
            Console.WriteLine("Half = 2 beats");
            Console.WriteLine("Quarter = 1 beat");
            Console.WriteLine("Eight = 1/2 beat");

            Console.WriteLine("\nWhen you are finished adding your notes, simply write STOP.");
            Console.WriteLine("You will have the ability to go back and continue adding notes.");
            Console.WriteLine();
        }

        /// <summary>
        /// display get beats per minute and return quarter note length
        /// the quarterNote will be used to calculated the note lengths
        /// </summary>
        /// <returns></returns>
        private static int GetQuarterNotesPerMinute(int quarterNote)
        {
            //
            // local variables
            //
            string userResponse;
            double beats = 0;
            int bpm = 1000 / (quarterNote / 60); // calculates beats per minute

            DisplayHeader("Beats per Minute");

            //
            // display the current beats per minute
            //
            Console.WriteLine($"The current beats per minute is set to {bpm}.");
            Console.WriteLine();

            //
            // prompt the user to enter a new value for beats per minute
            //
            Console.WriteLine("Enter new beats per minute: ");
            userResponse = Console.ReadLine();

            //
            // validates respond and parse it to beats
            //
            while (!double.TryParse(userResponse, out beats))
            {
                Console.WriteLine("Please enter a valid number: ");
                userResponse = Console.ReadLine();
            }

            //
            // calculate to note frequency in milliseconds
            //
            double quarterBeatsDouble = 1000 / (beats / 60);

            //
            // convert to int and add new value to quarterNote
            //
            quarterNote = Convert.ToInt32(quarterBeatsDouble);

            DisplayContinuePrompt();

            return quarterNote;
        }

        /// <summary>
        /// display add new notes
        /// </summary>
        /// <returns></returns>
        private static List<FinchNote> DisplayAddNewNotes()
        {

            DisplayHeader("Add New Notes");

            //
            // instantiate new list
            //
            List<FinchNote> notes = new List<FinchNote>();

            //
            // execute method to add notes
            //
            notes = DisplayAddNotes(notes);

            DisplayContinuePrompt();

            return notes;
        }

        /// <summary>
        /// display continue adding notes to existing list
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        private static List<FinchNote> DisplayContinueAddingNotes(List<FinchNote> notes)
        {
            DisplayHeader("Continue Adding Notes");

            //
            // execute method to add notes
            //
            notes = DisplayAddNotes(notes);

            DisplayContinuePrompt();

            return notes;
        }

        /// <summary>
        /// display enter notes to the list
        /// </summary>
        private static List<FinchNote> DisplayAddNotes(List<FinchNote> notes)
        {
            //
            // local variables
            //
            FinchNote userNote = FinchNote.A5;
            bool keepAdding = true;
            bool validResponse = false;
            string userResponse;

            //
            // the user can enter in help to display the instructions
            //
            Console.WriteLine("Enter 'HELP' to see instructions.");
            Console.WriteLine();
            Console.WriteLine();

            //
            // loop runs until user enters 'STOP'
            //
            while (keepAdding)
            {
                //
                // loop runs until the respones are valid
                //
                while (!validResponse)
                {
                    //
                    // prompt user for a note
                    //
                    Console.Write("Note: ");
                    userResponse = Console.ReadLine().ToUpper();

                    //
                    // parse note to enum and add to notes list
                    //
                    if (Enum.TryParse<FinchNote>(userResponse, out userNote))
                    {
                        notes.Add(userNote);

                        //
                        // enter loop for the note length, runs until the response is valid
                        //
                        while (!validResponse)
                        {
                            //
                            // prompts the user for the length
                            //
                            Console.Write("Length: ");
                            userResponse = Console.ReadLine();

                            //
                            // validates response
                            //
                            if (userResponse != "1/8" && userResponse != "1/4" && userResponse != "1/2" && userResponse != "1/1" && userResponse != "HELP")
                            {
                                //
                                // message displayed if length is invalid
                                //
                                Console.WriteLine();
                                Console.WriteLine("You have entered an invalid length for the note.");
                                Console.WriteLine("Please try agian: ");
                            }

                            //
                            // displays the instructions if the user enter help
                            //
                            else if (userResponse == "HELP")
                            {
                                DisplayEnterNotesInstructions();
                            }

                            else
                            {
                                //
                                // add length to notes list
                                //
                                switch (userResponse)
                                {
                                    case "1/8":
                                        notes.Add(FinchNote.EIGHT);
                                        break;
                                    case "1/4":
                                        notes.Add(FinchNote.QUARTER);
                                        break;
                                    case "1/2":
                                        notes.Add(FinchNote.HALF);
                                        break;
                                    case "1/1":
                                        notes.Add(FinchNote.WHOLE);
                                        break;
                                    default:
                                        break;
                                }

                                validResponse = true;
                            }
                        }
                    }

                    //
                    // display instructions for the user
                    //
                    else if (userResponse == "HELP")
                    {
                        DisplayEnterNotesInstructions();
                    }

                    //
                    // executed if user enters 'rest'
                    //
                    else if (userResponse == "REST")
                    {
                        Console.Write("Enter type of rest: ");
                        userResponse = Console.ReadLine().ToUpper();

                        //
                        // validates response
                        //
                        if (userResponse != "WHOLE" && userResponse != "HALF" && userResponse != "QUARTER" && userResponse != "EIGHT")
                        {
                            //
                            // message displayed if rest  is invalid
                            //
                            Console.WriteLine();
                            Console.WriteLine("You have entered an invalid rest.");
                            Console.WriteLine("Values allowed are: whole, half, quarter, and eight.");
                            Console.WriteLine("Please try agian: ");
                        }

                        else
                        {
                            //
                            // add length to notes list
                            //
                            switch (userResponse)
                            {
                                case "WHOLE":
                                    notes.Add(FinchNote.WHOLE);
                                    break;
                                case "HALF":
                                    notes.Add(FinchNote.QUARTER);
                                    break;
                                case "QUARTER":
                                    notes.Add(FinchNote.HALF);
                                    break;
                                case "EIGHT":
                                    notes.Add(FinchNote.EIGHT);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    //
                    // loop ends if user enters 'stop'
                    //
                    else if (userResponse == "STOP")
                    {
                        keepAdding = false;
                        validResponse = true;
                    }

                    //
                    // message displays if note entered is invalid
                    //
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("You have entered an invalid note.");
                        Console.WriteLine("Please try agian: ");
                    }
                }
                validResponse = false;
            }

            //
            // return the list of notes
            //
            return notes;
        }

        /// <summary>
        /// the finch plays the notes in the list notes
        /// </summary>
        /// <param name="myFinch"></param>
        /// <param name="notes"></param>
        /// <param name="quarterNote"></param>
        private static void ProcessFinchNotes(Finch myFinch, List<FinchNote> notes, int quarterNote)
        {
            //
            // local variable
            //
            bool isConnect = false;

            DisplayHeader("Play Song");

            //
            // checks to see if there are any notes in the list
            //
            if (notes.Count() < 1)
            {
                Console.WriteLine("There are no notes to play.");
                Console.WriteLine("Please return to menu to add or retrieve notes.");
            }

            //
            // executed there are notes in the list
            //
            else
            {
                //
                // connects to the Finch
                //
                isConnect = GetConnectToFinch(myFinch);

                //
                // checks to see if Finch is connected
                //
                if (isConnect)
                {
                    Console.WriteLine("The Finch is ready to play your notes.");
                    Console.WriteLine("Press Enter to play the song.");
                    Console.ReadLine();

                    //
                    // loops through each note in the list
                    // plays the frequency for each note and holds it for the desired length
                    // for each not the finch's nose will light up
                    //
                    foreach (FinchNote note in notes)
                    {
                        switch (note)
                        {
                            case FinchNote.C4:
                                myFinch.noteOn(262);
                                myFinch.setLED(85, 0, 0);
                                break;
                            case FinchNote.D4:
                                myFinch.noteOn(294);
                                myFinch.setLED(85, 85, 0);
                                break;
                            case FinchNote.E4:
                                myFinch.noteOn(330);
                                myFinch.setLED(0, 85, 0);
                                break;
                            case FinchNote.F4:
                                myFinch.noteOn(349);
                                myFinch.setLED(85, 0, 85);
                                break;
                            case FinchNote.G4:
                                myFinch.noteOn(392);
                                myFinch.setLED(0, 0, 85);
                                break;
                            case FinchNote.A4:
                                myFinch.noteOn(440);
                                myFinch.setLED(0, 85, 85);
                                break;
                            case FinchNote.B4:
                                myFinch.noteOn(494);
                                myFinch.setLED(85, 85, 85);
                                break;

                            case FinchNote.C5:
                                myFinch.noteOn(523);
                                myFinch.setLED(170, 0, 0);
                                break;
                            case FinchNote.D5:
                                myFinch.noteOn(587);
                                myFinch.setLED(170, 170, 0);
                                break;
                            case FinchNote.E5:
                                myFinch.noteOn(659);
                                myFinch.setLED(0, 170, 0);
                                break;
                            case FinchNote.F5:
                                myFinch.noteOn(698);
                                myFinch.setLED(170, 0, 170);
                                break;
                            case FinchNote.G5:
                                myFinch.noteOn(784);
                                myFinch.setLED(0, 0, 170);
                                break;
                            case FinchNote.A5:
                                myFinch.noteOn(880);
                                myFinch.setLED(0, 170, 170);
                                break;
                            case FinchNote.B5:
                                myFinch.noteOn(988);
                                myFinch.setLED(170, 170, 170);
                                break;

                            case FinchNote.C6:
                                myFinch.noteOn(1047);
                                myFinch.setLED(255, 0, 0);
                                break;
                            case FinchNote.D6:
                                myFinch.noteOn(1175);
                                myFinch.setLED(255, 255, 0);
                                break;
                            case FinchNote.E6:
                                myFinch.noteOn(1319);
                                myFinch.setLED(0, 255, 0);
                                break;
                            case FinchNote.F6:
                                myFinch.noteOn(1397);
                                myFinch.setLED(25, 0, 255);
                                break;
                            case FinchNote.G6:
                                myFinch.noteOn(1568);
                                myFinch.setLED(0, 0, 255);
                                break;
                            case FinchNote.A6:
                                myFinch.noteOn(1760);
                                myFinch.setLED(0, 255, 255);
                                break;
                            case FinchNote.B6:
                                myFinch.noteOn(1976);
                                myFinch.setLED(170, 170, 170);
                                break;

                            //
                            // the quarter note value is used to calculate
                            // the length of the notes and the rest time
                            //
                            case FinchNote.EIGHT:
                                myFinch.wait(quarterNote / 2);
                                myFinch.noteOff();
                                myFinch.setLED(0, 0, 0);
                                break;
                            case FinchNote.QUARTER:
                                myFinch.wait(quarterNote);
                                myFinch.noteOff();
                                myFinch.setLED(0, 0, 0);
                                break;
                            case FinchNote.HALF:
                                myFinch.wait(quarterNote * 2);
                                myFinch.noteOff();
                                myFinch.setLED(0, 0, 0);
                                break;
                            case FinchNote.WHOLE:
                                myFinch.wait(quarterNote * 4);
                                myFinch.noteOff();
                                myFinch.setLED(0, 0, 0);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    //
                    // message displays if the finch can't be connected
                    //
                    Console.WriteLine("The Finch is not connected and can't play your notes.");
                    Console.WriteLine("Please check the wiring and try again.");
                }
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display current notes
        /// </summary>
        /// <param name="notes"></param>
        private static void DisplayNotes(List<FinchNote> notes)
        {
            DisplayHeader("Here are your Notes");

            //
            // checks to see if notes exist
            //
            if (notes.Count < 1)
            {
                Console.WriteLine("There are no notes to display.");
                Console.WriteLine("Please return to the menu to add or retieve notes.");
            }

            //
            // executed if notes exists
            //
            else
            {
                foreach (FinchNote note in notes)
                {
                    switch (note)
                    {
                        case FinchNote.C4:
                            Console.Write($"Note: {FinchNote.C4}");
                            break;
                        case FinchNote.D4:
                            Console.Write($"Note: {FinchNote.D4}");
                            break;
                        case FinchNote.E4:
                            Console.Write($"Note: {FinchNote.E4}");
                            break;
                        case FinchNote.F4:
                            Console.Write($"Note: {FinchNote.F4}");
                            break;
                        case FinchNote.G4:
                            Console.Write($"Note: {FinchNote.G4}");
                            break;
                        case FinchNote.A4:
                            Console.Write($"Note: {FinchNote.A4}");
                            break;
                        case FinchNote.B4:
                            Console.Write($"Note: {FinchNote.B4}");
                            break;
                        case FinchNote.C5:
                            Console.Write($"Note: {FinchNote.C5}");
                            break;
                        case FinchNote.D5:
                            Console.Write($"Note: {FinchNote.D5}");
                            break;
                        case FinchNote.E5:
                            Console.Write($"Note: {FinchNote.E5}");
                            break;
                        case FinchNote.F5:
                            Console.Write($"Note: {FinchNote.F5}");
                            break;
                        case FinchNote.G5:
                            Console.Write($"Note: {FinchNote.G5}");
                            break;
                        case FinchNote.A5:
                            Console.Write($"Note: {FinchNote.A5}");
                            break;
                        case FinchNote.B5:
                            Console.Write($"Note: {FinchNote.B5}");
                            break;
                        case FinchNote.C6:
                            Console.Write($"Note: {FinchNote.C6}");
                            break;
                        case FinchNote.D6:
                            Console.Write($"Note: {FinchNote.D6}");
                            break;
                        case FinchNote.E6:
                            Console.Write($"Note: {FinchNote.E6}");
                            break;
                        case FinchNote.F6:
                            Console.Write($"Note: {FinchNote.F6}");
                            break;
                        case FinchNote.G6:
                            Console.Write($"Note: {FinchNote.G6}");
                            break;
                        case FinchNote.A6:
                            Console.Write($"Note: {FinchNote.A6}");
                            break;
                        case FinchNote.B6:
                            Console.Write($"Note: {FinchNote.B6}");
                            break;
                        case FinchNote.EIGHT:
                            Console.WriteLine($" 1/8");
                            break;
                        case FinchNote.QUARTER:
                            Console.WriteLine($" 1/4");
                            break;
                        case FinchNote.HALF:
                            Console.WriteLine($" 1/2");
                            break;
                        case FinchNote.WHOLE:
                            Console.WriteLine($" 1/1");
                            break;
                        default:
                            break;
                    }
                }
            }


            DisplayContinuePrompt();
        }

        /// <summary>
        /// get data path file name from user
        /// </summary>
        /// <returns></returns>
        private static string GetDataPath()
        {
            //
            // local variables
            //
            string dataPath;
            string songTitle;

            //
            // prompt the user for a file name
            //
            Console.Write("Enter the name for your song file: ");
            songTitle = Console.ReadLine().ToUpper();

            //
            // instantiate a new dataPath
            //
            dataPath = ($@"Songs\{songTitle}.txt");

            return dataPath;
        }

        /// <summary>
        /// display save notes to text file
        /// </summary>
        /// <param name="notes"></param>
        private static void DisplaySaveNotes(List<FinchNote> notes)
        {
            //
            // local variables
            //
            string dataPath = "";

            DisplayHeader("Save Notes");

            if (notes.Count() < 1)
            {
                Console.WriteLine("There are no notes to save.");
                Console.WriteLine("Please return to the menu to add notes.");
            }
            else
            {
                //
                // get the dataPath file name from the user
                //
                dataPath = GetDataPath();

                //
                // instantiate a list of strings
                //
                List<string> notesAsString = new List<string>();

                //
                // add notes from notes list to string list
                //
                foreach (FinchNote note in notes)
                {
                    notesAsString.Add(note.ToString());
                }

                //
                // pause for the user
                //
                Console.WriteLine("Press Enter to save notes");
                Console.ReadLine();

                //
                // save notes to file
                //
                try
                {
                    File.WriteAllLines(dataPath, notesAsString);
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory("Songs");
                    File.WriteAllLines(dataPath, notesAsString);
                }

                Console.WriteLine($"The notes have been saved");
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display retrieve notes from text file and instantiate new notes lis
        /// </summary>
        private static List<FinchNote> DisplayRetrieveNotes()
        {
            //
            // instantiate a new list (prevents the possibility of adding to existing list)
            List<FinchNote> notes = new List<FinchNote>();

            //
            // local variables
            //
            FinchNote userNote = FinchNote.A5;
            string dataPath;

            DisplayHeader("Retrieve Notes");

            //
            // get the file from the user
            //
            dataPath = GetDataPath();

            //
            // checks if the file path exist
            //
            if (File.Exists(dataPath))
            {
                //
                // pause for user to initiate retrieving data
                //
                Console.WriteLine("Press Enter to retrieve data from the text file.");
                Console.ReadLine();

                //
                // retrieve and data form  the file
                //
                List<string> lines = File.ReadLines(dataPath).ToList();
                foreach (string line in lines)
                {
                    if (Enum.TryParse<FinchNote>(line, out userNote))
                    {
                        notes.Add(userNote);
                    }
                }

                Console.WriteLine("The notes have been retrieved.");
            }
            else
            {
                //
                // message displayed if file not found
                //
                Console.WriteLine("The file you have entered does not exist.");
            }


            DisplayContinuePrompt();

            return notes;
        }

        /// <summary>
        /// display all the files in the folder 'Songs'
        /// </summary>
        private static void DisplaySongFiles()
        {
            DisplayHeader("Here are the list of available song files");

            //
            // creates an array of all the file in the Songs directory
            string[] filePaths = Directory.GetFiles(@"Songs\");

            //
            // displays each file in the array filePaths
            foreach (string file in filePaths)
            {
                Console.WriteLine(file);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// connects to the the finch and displays a connection confirmation, Returns bool isConnect
        /// </summary>
        /// <param name="myFinch"></param>
        private static bool GetConnectToFinch(Finch myFinch)
        {
            bool isConnected;

            //
            // connect to the finch
            //
            if (myFinch.connect())
            {
                //
                // the finch displays lights
                //
                myFinch.setLED(255, 0, 0);
                myFinch.wait(300);
                myFinch.setLED(0, 255, 0);
                myFinch.wait(300);
                myFinch.setLED(0, 0, 255);
                myFinch.wait(300);
                myFinch.setLED(255, 255, 255);
                myFinch.wait(300);
                myFinch.setLED(0, 0, 0);

                isConnected = true;
            }
            else
            {
                isConnected = false;
            }

            return isConnected;
        }

        /// <summary>
        /// display disconect the Finch
        /// </summary>
        /// <param name="myFinch"></param>
        private static void DisconnectTheFinch(Finch myFinch)
        {
            for (int i = 0; i < 3; i++)
            {
                myFinch.setLED(255, 0, 0);
                myFinch.wait(250);
                myFinch.setLED(0, 0, 0);
                myFinch.wait(250);
            }

            myFinch.disConnect();

            Console.WriteLine("The Finch is disconnected");
        }

        /// <summary>
        /// setup console
        /// </summary>
        private static void SetupConsole()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Display a welcome screen including the purpose of the application
        /// </summary>
        private static void DisplayWelcomeScreen()
        {
            //
            // Changes the background color to white and text to black
            //
            SetupConsole();

            DisplayHeader("The Finch Note Player");

            Console.WriteLine();
            Console.WriteLine("Author: Madeleine Woodbury");
            Console.WriteLine("Type: Finch Application");

            Console.WriteLine("\nIn this application you can type in notes for the Finch to play back.");
            Console.WriteLine("The Finch can play any note you enter between C4 - B6.");

            Console.WriteLine("\nYou can save your notes to a text file, and you can retrieve notes from text files.");
            Console.WriteLine("Feel free to add as many notes as you like!");

            Console.WriteLine("\nRemember, you will need your finch to play the notes.");

            Console.WriteLine();
            DisplayContinuePrompt();
        }

        /// <summary>
        /// Turn the cursor off and display a continue prompt to the user
        /// </summary>
        private static void DisplayContinuePrompt()
        {
            Console.WriteLine();

            Console.CursorVisible = false;
            Console.Write("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// displays a header
        /// </summary>
        /// <param name="headerTitle"></param>
        private static void DisplayHeader(string headerTitle)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerTitle);
            Console.WriteLine();
        }

        /// <summary>
        /// Display a closing screen
        /// </summary>
        private static void DisplayClosingScreen(Finch myFinch)
        {
            Console.Clear();

            DisconnectTheFinch(myFinch);

            Console.WriteLine();
            Console.WriteLine("Thank you for using the application.");
            Console.WriteLine();

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

    }
}
