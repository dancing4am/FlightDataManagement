using System;

namespace AircraftTransmission
{
    /// <summary>
    /// This class powers up the aircraft transmission system.
    /// </summary>
    public static class AtsManager
    {
        private static FileManager fileManager;

        /// <summary>
        ///     Tail number of the current aircraft
        /// </summary>
        internal static string TailNumber { get; private set; }

        /// <summary>
        ///     True if any aircraft is in flight
        /// </summary>
        internal static bool isInFlight { get; private set; }

        /// <summary>
        ///     List of available aircrafts
        /// </summary>
        internal static readonly string[] aircrafts = { "C-FGAX", "C-GEFC", "C-QWWT" };

        /// <summary>
        /// This is main entry point of the ATS.
        /// </summary>
        static void Main(string[] args)
        {
            SimpleLogger.Log.SetDefaultLogFileLocation(); 
            Boot();
        }

        /// <summary>
        /// This method passes the selected aircraft to <see cref="FileManager"></see> by prompting user
        /// </summary>
        /// <returns>int input: selected aircraft index</returns>
        private static int SelectAircraft()
        {
            int input = 0;

            // get user input until to quit or to choose an aircraft
            while (!isInFlight)
            {
                Console.Clear();
                DisplayMenu();               
                input = GetAircraftNumber();

                // if any valid input, proceed
                if (input >= 0)
                {
                    TailNumber = aircrafts[input];
                    isInFlight = true;
                }
            }
            return input;
        }

        /// <summary>
        /// This method displays the menu for aircraft transmission system
        /// </summary>
        private static void DisplayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("");
            Console.WriteLine("                  ______");
            Console.WriteLine("                  _\\ _~-\\___");
            Console.WriteLine("      =  = == (____AA____D");
            Console.WriteLine("                   \\_____\\___________________,-~~~~~~~`-.._");
            Console.WriteLine("                   / o O o o o o O O o o o o o o O o |\\_");
            Console.WriteLine("                   `~-.__        ___..----..                  )");
            Console.WriteLine("                         `---~~\\___________ / ------------`````");
            Console.WriteLine("                         =  === (_________D");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("      Press the Aircraft Trail Number from the followings:");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("      1. C-FGAX");
            Console.WriteLine("      2. C-GEFC");
            Console.WriteLine("      3. C-QWWT");
            Console.WriteLine("      4. Quit");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("      The Number is:       ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// This method prompts user to select the aircraft to fly
        /// </summary>
        /// <returns>int key: selected aircraft index</returns>
        private static int GetAircraftNumber()
        {
            ConsoleKey key = Console.ReadKey().Key;
            Console.WriteLine("");
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return 0;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    return 1;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    return 2;

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    return 3;

                default:
                    return -1;
            }
        }

        /// <summary>
        /// This method sets up the connected classes.
        /// </summary>
        private static void Setup()
        {
            fileManager = new FileManager();
        }

        /// <summary>
        /// This method boots up the ATS application.
        /// </summary>
        public static void Boot()
        {
            isInFlight = false;
            // get user input for aircraft to fly
            int input = SelectAircraft();

            // if to quit, exit application
            if (input == 3)
            {
                Environment.Exit(0);
            }
            else
            {
                // pass file manager the tail number to read data
                if (fileManager == null)
                {
                    Setup();
                }
                fileManager.ReadData(TailNumber);
            }

        }
    }
}
