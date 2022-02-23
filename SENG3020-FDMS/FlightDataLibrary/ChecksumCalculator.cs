using System;

namespace FlightDataLibrary
{
    /// <summary>
    /// This class calculates checksum for telemetry packet.
    /// </summary>
    public class ChecksumCalculator
    {
        private const string INVALID_FLAG = "0";   // string to indicate any invalid flight data

        /// <summary>
        /// This method calculates checksum for <see cref="DataPacket"></see> trailer.
        /// If any data for checksum calculation is invalid, the checksum is set to -1.
        /// </summary>
        /// <param name="checksum">reference to the checksum storage</param>
        /// <param name="alt">reference to the altitude storage</param>
        /// <param name="pitch">reference to the pitch storage</param>
        /// <param name="bank">reference to the bank storage</param>
        public void CalculateChecksum(ref int checksum, ref string alt, ref string pitch, ref string bank)
        {
            try
            {
                checksum = Convert.ToInt32(Math.Round((double.Parse(alt) + double.Parse(pitch) + double.Parse(bank)) / 3));
            }
            catch (Exception e)
            {
                // if fail, check for data sanity and set checksum as -1
                CheckDataSanity(ref alt, ref pitch, ref bank);
                checksum = -1;
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// This method checks the data sanity of the alt, pitch and bank.
        /// Any data that is invalid is set as <see cref="INVALID_FLAG"></see>.
        /// </summary>
        /// <param name="alt">reference to the altitude storage</param>
        /// <param name="pitch">reference to the pitch storage</param>
        /// <param name="bank">reference to the bank storage</param>
        private void CheckDataSanity(ref string alt, ref string pitch, ref string bank)
        {
            var checklist = new [] { alt, pitch, bank };
            double checker;
            bool isInvalid = false;

            // check for each data sanity and set flag
            for (int i = 0; i < checklist.Length; i++)
            {
                if (!double.TryParse(checklist[i], out checker))
                {
                    checklist[0] = INVALID_FLAG;
                    isInvalid = true;
                }
            }

            // if invalid, report
            if (isInvalid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Invalid Aircraft Data Type");
                Console.ForegroundColor = ConsoleColor.White;
            }        
        }
    }
}
