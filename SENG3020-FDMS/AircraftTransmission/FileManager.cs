using System;
using System.IO;

namespace AircraftTransmission
{
    /// <summary>
    /// This class loads aircraft and reads its data in. 
    /// </summary>
    class FileManager
    {
        internal Parser parser;
        private TimeManager timeManager;

        private string projectPath = "";    // path where the data files reside
        private string fileName = "";       // aircraft tail number with file extension

        /// <summary>
        /// This method reads aircraft data and sends the data to <see cref="Parser"></see>
        /// every second counted by <see cref="TimeManager"></see>.
        /// </summary>
        /// <param name="tailNum">Tail number of the aircraft to load</param>
        internal void ReadData(string tailNum)
        {
            if ((parser == null) || (timeManager == null))
            {
                Setup();
            }

            try
            {
                // open data file
                LoadAircraft(tailNum);

                using (StreamReader reader = new StreamReader(projectPath + fileName))
                {
                    string line;
                    // read line by line until the end of file
                    while ((line = reader.ReadLine()) != null)
                    {                        
                        parser.Parse(line);
                        timeManager.WaitForSeconds(1);
                    }
                    reader.Close();
                    EndFlight();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Failed to read data");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// This method wraps up the flight data in this class.
        /// </summary>
        internal void EndFlight()
        {
            projectPath = "";
            fileName = "";
            parser.EndFlight();
        }

        /// <summary>
        /// This method loads the requested aircraft.
        /// </summary>
        /// <param name="tailNum">Tail number of the aircraft to load</param>
        private void LoadAircraft(string tailNum)
        {
            projectPath = Directory.GetParent(Path.GetDirectoryName(Environment.CurrentDirectory)) + "\\";
            fileName = tailNum + ".txt";
        }

        /// <summary>
        /// This method sets up the connected classes.
        /// </summary>
        private void Setup()
        {
            parser = new Parser();
            timeManager = new TimeManager();
        }
    }
}
