using System;
using System.Timers;
using System.Threading;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
//timers comparison
//https://web.archive.org/web/20150329101415/https://msdn.microsoft.com/en-us/magazine/cc164015.aspx

namespace PetrolStationAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            //Menu implementation
            RunMenu();
           
        }

        //Timer fields
        public static System.Timers.Timer timer1= new System.Timers.Timer();
        public static System.Timers.Timer timer2= new System.Timers.Timer();
        //Global fields
        public static string folderPathTransactions;
        public static int timesFirstMenuOptionChosen = 0;

        #region Menu
        /// <summary>
        /// Get user input, try to parse it, and implement menu functionality using switch statements
        /// </summary>
        static void RunMenu()
        {
            //Temporary variable will contain user input
            int userInput;

            //Boolean variable is FALSE as long as user input is not 4
            bool exit = false;

            //Looping until user decides to exit and presses 4
            do
            {
                //Clear the console
                Console.Clear();

                //Showing menu options
                Display.DrawMenu();

                //Getting and parsing user input
                Int32.TryParse(Console.ReadLine(), out userInput);

                //Switch cases depend on user input
                switch (userInput)
                {
                    //If user input is 1, run petrol station simulation
                    case 1:
                        if (timesFirstMenuOptionChosen == 0)
                        {
                            AskForFolderName();
                            Data.Initialise();
                            RunPetrolStation();
                        }
                        else
                        {
                            RunPetrolStation();
                        }
                        timesFirstMenuOptionChosen++;
                            break;
                    //If user imput is 2, read transcations from currently available csv files
                    case 2:
                        //Read transactions
                        //Reading from mutiple csv files
                        // https://stackoverflow.com/questions/45763209/how-to-read-multiple-csv-files-from-a-folder-in-c
                       // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file?view=netframework-4.8
                        ReadTransactions();
                        break;
                    //If user input is 3, read current statuses of all counters
                    case 3:
                        //Read Counters
                        Console.Clear();
                        Display.DrawCounters();
                        break;
                    //If user input is 4, exit the loop
                    case 4:
                        Console.Clear();
                        Console.WriteLine("Press any key to exit ...");
                        Console.ReadKey();
                        exit = true;
                        return;
                    //If user input parsing was unsuccessful, ask user to re-enter value
                    default:
                        Console.Clear();
                        Console.WriteLine("Please, enter valid input");
                        break;

                }
                //After breaking every case, user is asked to press any key to return to menu options
                Console.WriteLine();
                Console.WriteLine("Press any key to return to the Menu ...");
                Console.ReadKey();

            } while (!exit);
           
        }

        /// <summary>
        /// Start processing of petrol station simulation
        /// </summary>
        static void RunPetrolStation()
        {
            Console.Clear();
            Console.WriteLine("Loading...");
            //Create and start independent thread that creates and manages queue of vehicles
            //Thread Initiaization = new Thread(new ThreadStart(Data.Initialise));
            //Initiaization.Start();

            //Run pump assignment
            PumpProcessing();
            //Run user interface displaying
            UIProcessing();

            var input = Console.ReadKey();
            //If user entered something to the console, pause simulation displaying
            if (input != null)
            {
                timer1.Stop();//stop the timer that manages user interface
            }

        }

        /// <summary>
        /// Get user input folder directory path, check for its validity
        /// </summary>
        static void AskForFolderName()
        {
            //Local boolean variable to check if directory exists
            bool dirExists = false;
            //Local boolean variable to check if current PC user has the rigt to open and write to files/folders in directory specified
            bool hasWritePermission = false;
            //Loop until a valid directory is provided
            do
            {
                //Notify user to provide a folder directory
                Console.Clear();
                Console.WriteLine("First, please, enter valid folder path, where you want to save all transaction receipts:");
                var inputFolferPath = @"" + Console.ReadLine();//Get user input
                dirExists = Directory.Exists(inputFolferPath);//Assign boolean value of whether the given directory refers to an existing directory on disk
                CurrentPCUserSecurity cus = new CurrentPCUserSecurity();//Instantiate object that can check directory access control. Note: the checker meant to work for Windows users
                DirectoryInfo inputDir = new DirectoryInfo(inputFolferPath);//Instantiate DirectoryInfo class
                hasWritePermission = cus.HasAccess(inputDir, FileSystemRights.WriteData);//Assign boolean value of whether the given directory allows to open and write data to directory specified
                //Check if both of the above boolean variables are true simultaneously
                if (dirExists && hasWritePermission)
                {
                    //Save user input to the global variable
                    folderPathTransactions = inputFolferPath + @"\";
                    //Ask user to proceed
                    Console.WriteLine("Press any key to start...");
                }
                //Otherwise, notify user with invalid input and ask to try again
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please, input valid folder path that allows to write into it...");
                    Console.WriteLine("Press any key to try again...");
                }
                Console.ReadKey();
            } while (!dirExists || !hasWritePermission);//Keep asking for directory if at least one of the boolean checkers is false
        }



        /// <summary>
        /// Read transcations information and write them to the console
        /// </summary>
        static void ReadTransactions()
        {
            Console.Clear();
            Console.WriteLine("Vehicle ID | Vehicle type | Litres dispensed | Fuel type | Fuelling Time | Pump number | Date and Time ");//First line of the display
            Console.WriteLine();
            try
            {
                if (folderPathTransactions == null) { return; }
                else
                {
                    //Create files array of file paths
                    var files = Directory.EnumerateFiles(folderPathTransactions, "*.csv");
                    foreach (string file in files)
                    {
                        //Create an array of lines from each file
                        string[] lines = File.ReadAllLines(file);
                        //Search for the second line in csv file with transcation information, as we do not need first line
                        foreach (string line in lines)
                        {
                            //Define common string in every file. Would need to be changed very year.
                            string stringToSearch = "2019";
                            //Search for specified string
                            if (line.Contains(stringToSearch))
                            {
                                //Replace commas in line with string of spaces and save it to new variable
                                string newline = line.Replace(",", "            ");
                                //Write transformed line to the console
                                Console.WriteLine(newline);
                                Console.WriteLine();
                            }
                        }
                    }
                }

            }
            catch (IOException e)
            {
                //Handling exception
                Console.WriteLine("File could not be read...");
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region UIProcessing
        /// <summary>
        /// Continuously call elapsed method RunProgramLoop every 2050 miliseconds
        /// </summary>
        static void UIProcessing()
        {
            timer1.Interval = 450;//The higher this interval, the faster UI display shows
            timer1.AutoReset = true;//repeat
            timer1.Elapsed += RunProgramLoop;
            timer1.Enabled = true;
            timer1.Start();
        }
        /// <summary>
        /// Displaying petrol station interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void RunProgramLoop(object sender, ElapsedEventArgs e)
        {

            Console.Clear();
            Display.DrawVehicles();//display vehicles queue
            Console.WriteLine();
            Console.WriteLine();
            Display.DrawPumps();//display pumps statuses
            Console.WriteLine();
            Console.WriteLine();
            Display.DrawCounters();//display counters statuses
            Console.WriteLine();
            Console.WriteLine("Press any key to pause...");

        }
        #endregion

        #region Pump Processing
        /// <summary>
        /// Continuously call PunPumpProcessing method every 950 miliseconds
        /// </summary>
        static void PumpProcessing()
        {

            timer2.Interval = 500;//Effectiveness of pump processing and assignment depnds on this interval. The lower the interval, the faster assigning goes
            timer2.AutoReset = true;//repeat
            timer2.Elapsed += RunPumpProcessing;
            timer2.Enabled = true;
            timer2.Start();

        }
        /// <summary>
        /// Assign vehicles to pump on call
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void RunPumpProcessing(object sender, ElapsedEventArgs e)
        {
            Data.AssignVehicleToPump();
        }
        #endregion

        

    }
}
