using System;

namespace AircraftTransmission
{
    /// <summary>
    /// This class parses the aircraft data read in.
    /// </summary>
    class Parser
    {
        public delegate void ParserCalled(object sender, EventArgs e);
        public event EventHandler ParserCalledHandler;

        private const int PARSED_LEN = 8;   // numbers of parsed words of proper flight data format
        internal Packetizer packetizer;

        /// <summary>
        /// This method parses the aircraft data passed in from <see cref="FileManager"></see>
        /// and sends the result to <see cref="Packetizer"></see>.
        /// </summary>
        /// <param name="line">aircraft data to parse passed in from <see cref="FileManager"></see></param>
        internal void Parse(string line)
        {
            ParserCalledHandler?.Invoke(this, EventArgs.Empty);

            string[] separatingStrings = { ", ", "," };

            string[] parsed = line.Split(separatingStrings, StringSplitOptions.RemoveEmptyEntries);

            if (packetizer == null)
            {
                Setup();
            }

            if (parsed.Length == PARSED_LEN)
            {
                packetizer.Packetize(parsed);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Failed to parse data");
            }
        }

        /// <summary>
        /// This method wraps up the flight data in this class.
        /// </summary>
        internal void EndFlight()
        {
            packetizer.EndFlight();
        }

        /// <summary>
        /// This method sets up the connected classes.
        /// </summary>
        private void Setup()
        {
            packetizer = new Packetizer();
        }
    }
}
